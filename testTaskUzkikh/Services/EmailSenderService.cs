using testTaskUzkikh.DbRepository.Interfaces;
using testTaskUzkikh.Models;

namespace testTaskUzkikh.Services
{
    public class EmailSenderService : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;
        private Timer _timer;

        public EmailSenderService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            TimeSpan interval = TimeSpan.FromHours(24);
            var nextRunTime = DateTime.Today.AddDays(1).AddHours(7);
            var curTime = DateTime.Now;
            var firstInterval = nextRunTime.Subtract(curTime);

            Action action = () =>
            {
                var t1 = Task.Delay(firstInterval);
                t1.Wait();
                SendEmailWithUnpStatusAsync(null);
                _timer = new Timer(
                    SendEmailWithUnpStatusAsync,
                    null,
                    TimeSpan.Zero,
                    interval
                );
            };

            Task.Run(action);
            return Task.CompletedTask;

        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        private async void SendEmailWithUnpStatusAsync(object? state)
        {
            using (IServiceScope scope = _serviceProvider.CreateScope())
            {
                IUnpRepository unpRepository = scope.ServiceProvider.GetRequiredService<IUnpRepository>();
                IMailService mailService = scope.ServiceProvider.GetRequiredService<IMailService>();

                var unps = await unpRepository.GetAllWithAssignedUsersAsync();

                if (unps.Count > 100)
                {
                    await SendEmailAboutStatusOfUnpsAsync(unps.Take(100).ToArray(), mailService);

                    await Task.Delay(TimeSpan.FromMinutes(5));

                    await SendEmailAboutStatusOfUnpsAsync(unps.Skip(100).ToArray(), mailService);
                }
                else
                {
                    await SendEmailAboutStatusOfUnpsAsync(unps.ToArray(), mailService);
                }
            }
        }

        private async Task SendEmailAboutStatusOfUnpsAsync(UNP[] unps, IMailService mailService)
        {
            for(int i = 0; i < unps.Length; i++)
            {
                string adding = string.Empty;

                if (unps[i].DLIKV is not null)
                {
                    adding = $"<p><strong>Дата изменения состояния плательщика: </strong>{unps[i].DLIKV.Value.ToShortDateString()}</p><p><strong>Пичина изменения состояния плательщика: </strong>{unps[i].VLIKV}</p>";
                }
                else
                {
                    adding = "<p><strong>Состояние плательщика не менялось.</strong></p>";
                }

                await mailService.SendEmailAsync(new Email()
                {
                    ToEmail = unps[i].User.Email,
                    Subject = "Статус УНП",
                    Body = $"<h2>Статус Вашего УНП:</h2> <p><strong>УНП:</strong> {unps[i].VUNP}</p><p><strong>Код статуса:</strong> {unps[i].CKODSOST}</p><p><strong>Статус:</strong> {unps[i].VKODS}.</p>" + adding
                });
            }
        }
    }
}

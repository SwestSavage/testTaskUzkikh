﻿using testTaskUzkikh.DbRepository.Interfaces;
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

            _timer = new Timer(
                SendEmailWithUnpStatusAsync,
                null,
                TimeSpan.Zero,
                TimeSpan.FromMinutes(1)
                );

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

                int i = 0;
                foreach (var unp in unps)
                {
                    mailService.SendEmailAsync(new Email()
                    {
                        ToEmail = unp.User.Email,
                        Subject = "UNP status",
                        Body = $"Your UNP status: \nUNP: {unp.VUNP} \nStatus code: {unp.CKODSOST}\nStatus: {unp.VKODS}."
                    });
                }
            }
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using testTaskUzkikh.DbRepository.Interfaces;
using testTaskUzkikh.Models;
using testTaskUzkikh.Helpers;
using testTaskUzkikh.Services;

namespace testTaskUzkikh.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [FormatFilter]
    public class UnpController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnpRepository _unpRepository;
        private readonly IMailService _mailService;

        public UnpController(IUserRepository userRepository,
            IUnpRepository unpRepository, 
            IMailService mailService)
        {
            _userRepository = userRepository;
            _unpRepository = unpRepository;
            _mailService = mailService;
        }

        [HttpGet("getData")]
        public async Task<IActionResult> GetData(long unp, string? charset, string? type)
        {
            UNP unpFromDb;

            if (charset is null) charset = "UTF-8";
            if (type is null) type = "xml";

            try
            {
                unpFromDb = await _unpRepository.GetAsync(unp);
            }
            catch (NullReferenceException e)
            {
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            var unpViewModel = new UnpViewModel
            {
                VUNP = unpFromDb.VUNP.ToString(),
                VNAIMP = unpFromDb.VNAIMP,
                VNAIMK = unpFromDb.VNAIMK,
                DREG = unpFromDb.DREG.ToShortDateString(),
                NMNS = unpFromDb.NMNS.ToString(),
                VMNS = unpFromDb.VMNS,
                CKODSOST = unpFromDb.CKODSOST.ToString(),
                VKODS = unpFromDb.VKODS,
                DLIKV = unpFromDb.DLIKV is null ? "" : unpFromDb.DLIKV.Value.ToShortDateString(),
                VLIKV = unpFromDb.VLIKV ?? ""
            };

            if (type == "json")
            {
                var jsonResult = new JsonResult(unpViewModel);
                jsonResult.ContentType = $"application/json; charset={charset}";

                return jsonResult;
            }

            var result = new ContentResult();

            result.Content = XmlSerializerHelper.SerializeInXmlString(unpViewModel);
            result.ContentType = $"application/xml; charset={charset}";

            return result;
        }

        [HttpGet("checkUnp")]
        public async Task<IActionResult> CheckUnp(long unp)
        {
            var exist = await _unpRepository.CheckIfExistAsync(unp);

            return new JsonResult(new { UnpExist = exist});
        }

        [HttpPost("PostData")]
        public async Task<IActionResult> SubscribeToEmail([FromBody] UnpSubscribeViewModel model)
        {
            if (!string.IsNullOrEmpty(model.Email) && model.Unps.Count > 0)
            {

                if (!await _userRepository.CheckIfExist(model.Email))
                {
                    await _userRepository.AddNewAsync(model.Email);
                }
                try
                {                    
                    foreach (var u in model.Unps)
                    {
                        await _unpRepository.UpdateUserIdAsync(u, model.Email);
                    }
                }
                catch (Exception)
                {
                    return NoContent();
                }

                return Ok();
            }

            if (string.IsNullOrEmpty(model.Email)) return BadRequest(model.Email);

            return BadRequest(model.Unps);
        }        
    }
}

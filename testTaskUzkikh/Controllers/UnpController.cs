using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using testTaskUzkikh.DbRepository.Interfaces;
using testTaskUzkikh.Models;
using testTaskUzkikh.Helpers;

namespace testTaskUzkikh.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [FormatFilter]
    public class UnpController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnpRepository _unpRepository;

        public UnpController(IUserRepository userRepository,
            IUnpRepository unpRepository)
        {
            _userRepository = userRepository;
            _unpRepository = unpRepository;
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
    }
}

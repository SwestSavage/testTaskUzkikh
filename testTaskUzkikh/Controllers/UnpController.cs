using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using testTaskUzkikh.DbRepository.Interfaces;
using testTaskUzkikh.Models;

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

        [HttpGet]
        [Produces("application/xml")]
        public async Task<IActionResult> GetData(long unp, string charset = "UTF-8", string type = "xml")
        {
            UNP unpFromDb;

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

            if (type == "json")
            {
                return new JsonResult(unpFromDb);
            }

            return Ok(unpFromDb);
        }
      
    }
}

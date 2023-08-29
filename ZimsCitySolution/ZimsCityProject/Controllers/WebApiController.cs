using Microsoft.AspNetCore.Mvc;
using ZimsCityProject.Exceptions;
using ZimsCityProject.Services.Interfaces;

namespace ZimsCityProject.Controllers
{
    [ApiController]
    [Route("api/")]
    public class WebApiController : ControllerBase
    {
        readonly IWebApiService _webApiService;

        public WebApiController(IWebApiService webApiService)
        {
            _webApiService = webApiService;
        }

        [HttpPatch]
        [Route("save-movings")]
        public IActionResult SaveMovings(int[] idArray)
        {
            return RunApiFunction(() => _webApiService.ReorderHouses(idArray), successsMessage: "House order saved successfully");
        }

        [HttpDelete("delete/{id}")]
        public IActionResult Delete(int id)
        {
            return RunApiFunction(() => _webApiService.DeleteHouse(id), successsMessage: "House deleted successfully");
        }


        [HttpDelete("DeleteAll")]
        public IActionResult DeleteAllHouses()
        {
            return RunApiFunction(() => _webApiService.DeleteAllHouses(), successsMessage: "All user houses deleted successfully");
        }

        private IActionResult RunApiFunction(Action action, string successsMessage)
        {
            try
            {
                action();
                return Ok(new { message = successsMessage });
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred");
            }
        }

    }
}

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
            string successMessage = "House order saved successfully";
            int statusCode = StatusCodes.Status204NoContent;
            return RunApiFunction(() => _webApiService.ReorderHouses(idArray), successMessage, statusCode);
        }

        [HttpDelete("delete/{id}")]
        public IActionResult Delete(int id)
        {
            string successMessage = "House deleted successfully";
            int statusCode = StatusCodes.Status204NoContent;
            return RunApiFunction(() => _webApiService.DeleteHouse(id), successMessage, statusCode);
        }

        [HttpDelete("DeleteAll")]
        public IActionResult DeleteAllHouses()
        {
            string successMessage = "All user houses deleted successfully";
            int statusCode = StatusCodes.Status204NoContent;
            return RunApiFunction(() => _webApiService.DeleteAllHouses(), successMessage, statusCode);
        }

        private IActionResult RunApiFunction(Action action, string successMessage, int statusCode)
        {
            try
            {
                action();
                return StatusCode(statusCode, new { message = successMessage });
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred");
            }

        }
    }
}

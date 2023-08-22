using Microsoft.AspNetCore.Mvc;
using YousifsProject.Exceptions;
using YousifsProject.Services.Interfaces;

namespace YousifsProject.Controllers
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
        [ValidateAntiForgeryToken]
        public IActionResult SaveMovings(int[] idArray)
        {
            return RunApiFunction(() => _webApiService.ReorderHouses(idArray), successsMessage: "House order saved successfully");
        }

        [HttpDelete("delete/{id}")]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            return RunApiFunction(() => _webApiService.DeleteHouse(id), successsMessage: "House deleted successfully");
        }


        [HttpGet("DeleteAll")]
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

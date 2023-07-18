using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YousifsProject.Services.Interfaces;
using YousifsProject.Views.Houses;

namespace YousifsProject.Controllers
{
    [Authorize]
    public class HousesController : Controller
    {

        IHouseService service;
        public HousesController(IHouseService service)
        {
            this.service = service;
        }

        [HttpGet("")]
        [HttpGet("index")]
        public async Task<IActionResult> Index() => View();

        [HttpGet("indexpartial/")]
        public async Task<IActionResult> IndexPartialAsync(string sort, bool isAscending, string roofs, int minFloor, int MaxFloor)
        {
            IndexVM[] model = await service.GetIndexVMAsync(sort, isAscending, roofs, minFloor, MaxFloor);

            return PartialView("_IndexPartial", model);
        }

        [HttpGet("Build")]
        public IActionResult BuildHouse()
        {
            BuildHouseVM model = service.getBuildVM();
            return View(model);
        }

        [HttpPost("Build")]
        public IActionResult BuildHouse(BuildHouseVM model)
        {

            if (!ModelState.IsValid)
            {
                return View(service.getBuildVM());
            }
            if (!service.IsAddressAvailable(model.Address, -1))
            {
                ModelState.AddModelError(nameof(model.Address), "The Address is already taken");
                return View(service.getBuildVM());
            }
            service.AddHouse(model);
            return RedirectToAction(nameof(Index));
        }

        [Route("edit/{id}")]
        [HttpGet]
        public IActionResult EditHouse(int id)
        {

            EditHouseVM model = service.GetEditVM(id);
            return View(model);
        }

        [HttpPost("/edit/{id}")]
        public IActionResult EditHouse(EditHouseVM model, int id)
        {
            if (!ModelState.IsValid)
            {
                return View(service.GetEditVM(id));
            }
            if (!service.IsAddressAvailable(model.Address, id))
            {
                ModelState.AddModelError(nameof(model.Address), "The Address is already taken");
                return View(service.GetEditVM(id));
            }
            service.EditHouse(model, id);
            return RedirectToAction(nameof(Index));
        }

        [HttpDelete("delete/{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var houseToDelete = service.GetHouseById(id);
                if (houseToDelete == null)
                {
                    return NotFound($"House with Id = {id} not found");
                }

                service.DeleteHouse(houseToDelete);
                return Ok();
            }

            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error deleting house");
            }
        }

        [HttpGet("DeleteAll")]
        public IActionResult DeleteAll()
        {
            service.DeleteAllHouses();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost("/saveMovings/")]
        public IActionResult SaveMovings(int[] idArray)
        {
            service.ReorderHouses(idArray);
            return Ok();
        }
    }


}

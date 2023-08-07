using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YousifsProject.Services.Interfaces;
using YousifsProject.Views.Houses.ViewModels;

namespace YousifsProject.Controllers
{
    [Authorize]
    public class HousesController : Controller
    {

        readonly IHouseService _houseService;
        public HousesController(IHouseService service)
        {
            this._houseService = service;
        }

        [HttpGet("")]
        [HttpGet("index")]
        public IActionResult Index()
        {
            var model = _houseService.GetIndexVM();
            return View(model);
        }


        [HttpGet("indexpartial/")]
        public async Task<IActionResult> IndexPartialAsync(string sort, bool isAscending, string roofs, int minFloor, int MaxFloor)
        {
            var houseCount = _houseService.GetHouseCount();

            if (houseCount == 0)
                return PartialView("~/Views/Houses/PartialViews/_NoHouseIndex.cshtml");

            var model = await _houseService.GetIndexPartialVMAsync(sort, isAscending, roofs, minFloor, MaxFloor);
            return PartialView("~/Views/Houses/PartialViews/_IndexPartial.cshtml", model);
        }

        [HttpGet("Build")]
        public IActionResult BuildHouse()
        {
            var model = _houseService.getBuildVM();
            return View(model);
        }

        [HttpPost("Build")]
        public IActionResult BuildHouse(BuildHouseVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(_houseService.getBuildVM());
            }

            if (!_houseService.IsAddressAvailable(model.Address, -1))
            {
                ModelState.AddModelError(nameof(model.Address), "The Address is already taken");
                return View(_houseService.getBuildVM());
            }

            _houseService.AddHouse(model);
            return RedirectToAction(nameof(Index));
        }

        [Route("edit/{id}")]
        [HttpGet]
        public IActionResult EditHouse(int id)
        {

            var model = _houseService.GetEditVM(id);
            return View(model);
        }

        [HttpPost("/edit/{id}")]
        public IActionResult EditHouse(EditHouseVM model, int id)
        {
            if (!ModelState.IsValid)
            {
                return View(_houseService.GetEditVM(id));
            }

            if (!_houseService.IsAddressAvailable(model.Address, id))
            {
                ModelState.AddModelError(nameof(model.Address), "The Address is already taken");
                return View(_houseService.GetEditVM(id));
            }

            _houseService.EditHouse(model, id);
            return RedirectToAction(nameof(Index));
        }

        [HttpDelete("delete/{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var houseToDelete = _houseService.GetHouseById(id);
                if (houseToDelete == null)
                {
                    return NotFound($"House with Id = {id} not found");
                }

                _houseService.DeleteHouse(houseToDelete);
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
            _houseService.DeleteAllHouses();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost("/saveMovings/")]
        public IActionResult SaveMovings(int[] idArray)
        {
            _houseService.ReorderHouses(idArray);
            return Ok();
        }
    }


}

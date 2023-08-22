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
            var model = _houseService.GetBuildHouseVM();
            return View(model);
        }

        [HttpPost("Build")]
        public IActionResult BuildHouse(BuildHouseVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(_houseService.GetBuildHouseVM());
            }

            if (!_houseService.IsAddressAvailable(model.Address, -1))
            {
                ModelState.AddModelError(nameof(model.Address), "The Address is already taken");
                return View(_houseService.GetBuildHouseVM());
            }

            _houseService.AddHouse(model);
            return RedirectToAction(nameof(Index));
        }

        [Route("edit/{id}")]
        [HttpGet]
        public IActionResult EditHouse(int id)
        {

            var model = _houseService.GetEditHouseVM(id);
            return View(model);
        }

        [HttpPost("/edit/{id}")]
        public IActionResult EditHouse(EditHouseVM model, int id)
        {
            if (!ModelState.IsValid)
            {
                return View(_houseService.GetEditHouseVM(id));
            }

            if (!_houseService.IsAddressAvailable(model.Address, id))
            {
                ModelState.AddModelError(nameof(model.Address), "The Address is already taken");
                return View(_houseService.GetEditHouseVM(id));
            }

            _houseService.EditHouse(model, id);
            return RedirectToAction(nameof(Index));
        }
    }


}

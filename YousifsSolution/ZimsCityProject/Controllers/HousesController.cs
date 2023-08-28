using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZimsCityProject.Services.Interfaces;
using ZimsCityProject.Views.Houses.ViewModels;

namespace ZimsCityProject.Controllers
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

        [HttpGet("configure-house/{id?}")]
        public IActionResult ConfigureHouse(int? id)
        {
            try
            {
                var model = _houseService.GetConfigureHouseVM(id);
                return View(model);
            }
            catch (Exception ex)
            {
                return NotFound($"No house with id {id}");
            }

        }

        [HttpPost("configure-house/{id?}")]
        public IActionResult ConfigureHouse(int? id, ConfigureHouseVM model)
        {
            if (id.HasValue)
            {
                bool isValidId = _houseService.CheckIfValidHouseId(id.Value);
                if (!isValidId)
                    return NotFound($"No house with id {id}");
            }

            if (!ModelState.IsValid)
            {
                return View(_houseService.GetConfigureHouseVM(id));
            }

            if (!_houseService.IsAddressAvailable(model.Address, id))
            {
                ModelState.AddModelError(nameof(model.Address), "The Address is already taken");
                return View(_houseService.GetConfigureHouseVM(id));
            }

            _houseService.ConfigureHouse(id, model);
            return RedirectToAction(nameof(Index));
        }
    }
}

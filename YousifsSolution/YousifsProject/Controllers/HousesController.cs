using Microsoft.AspNetCore.Mvc;
using YousifsProject.Models;
using YousifsProject.Models.Entities;
using YousifsProject.Views.Houses;

namespace YousifsProject.Controllers
{
    public class HousesController : Controller
    {

        HouseService service;
        public HousesController(HouseService service)
        {
            this.service = service;
        }

        [Route("")]
        [Route("index")]
        public IActionResult Index()
        {

            return View();
        }

        [HttpGet("indexpartial/")]
        public async Task<IActionResult> IndexPartialAsync(string sort, bool isAscending, string roofs, int minFloor, int MaxFloor)
        {
            IndexVM[] model = await service.GetIndexVMAsync(sort, isAscending, roofs, minFloor, MaxFloor);
            
            return PartialView("_IndexPartial", model);
        }

        [Route("Build")]
        [HttpGet]
        public IActionResult Build()
        {
            BuildVM model = service.getBuildVM();
            return View(model);
        }

        [Route("Build")]
        [HttpPost]
        public IActionResult Build(BuildVM model)
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
            service.Add(model);
            return RedirectToAction(nameof(Index));
    }

        [Route("edit/{id}")]
        [HttpGet]
        public IActionResult Edit(int id)
        {

            EditVM model = service.GetEditVM(id);
            return View(model);
        }

        [Route("/edit/{id}")]
        [HttpPost]
        public IActionResult Edit(EditVM model, int id)
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
            service.PostEditEmployee(model, id);
            return RedirectToAction(nameof(Index));
        }

        [Route("delete/{id}")]
        [HttpGet]
        public IActionResult Delete(House house)
        {
            service.Delete(house);
            return RedirectToAction(nameof(Index));
        }
        [Route("DeleteAll")]
        [HttpGet]
        public IActionResult DeleteAll()
        {
            service.DeleteAll();
            return RedirectToAction(nameof(Index));
        }

        [Route("/saveMovings/")]
        [HttpPost]
        public IActionResult SaveMovings(int[] idArray)
        {
            service.Reorder(idArray);
            return Ok();
        }
    }


}

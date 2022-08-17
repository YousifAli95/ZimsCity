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
            //IndexVM[] model = service.GetIndexVM();
            //var x = model.ToList().OrderBy(o => o.GetType().GetProperty("Address").GetValue(o, null)).ToArray();
            //return PartialView("_IndexPartial", model);
            return View();
        }

        [HttpGet("indexpartial/")]
        public IActionResult IndexPartial(string sort, bool isAscending, string roofs, int minFloor = 0, int MaxFloor = 6)
        {
            IndexVM[] model = service.GetIndexVM();
            if (isAscending)
            {
            model = model.ToList().OrderBy(o => o.GetType().GetProperty(sort).GetValue(o, null)).
                    Where(o=> o.NumberOfFloors >= minFloor && o.NumberOfFloors <= MaxFloor && roofs.Contains(o.TypeOfRoof)).
                    ToArray();
            }
            else if (!isAscending)
            {
                model = model.ToList().OrderByDescending(o => o.GetType().GetProperty(sort).GetValue(o, null)).
                    Where(o => o.NumberOfFloors >= minFloor && o.NumberOfFloors <= MaxFloor && roofs.Contains(o.TypeOfRoof)).
                    ToArray();
            }
            
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

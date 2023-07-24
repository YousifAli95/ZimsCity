using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using YousifsProject.Models.Entities;
using YousifsProject.Services.Interfaces;
using YousifsProject.Views.Houses.ViewModels;

namespace YousifsProject.Services.Implementations
{

    public class HouseServiceDB : IHouseService
    {
        CityContext cityContext;
        IHttpContextAccessor accessor;

        public HouseServiceDB(CityContext cityContext, IHttpContextAccessor accessor)
        {
            this.cityContext = cityContext;
            this.accessor = accessor;
        }

        public void AddHouse(BuildHouseVM model)
        {
            int ThisSortingOrder = 1;
            if (cityContext.Houses.Count() > 0)
            {
                ThisSortingOrder = cityContext.Houses.Max(o => o.SortingOrder) + 1;
            }

            var roofId = GetRoofId(model.TypeOfRoof);
            string userId = GetUserId();

            cityContext.Houses.Add(new House
            {
                NumberOfFloors = model.NumberOfFloors,
                Color = model.Color,
                HaveBalcony = model.HaveBalcony,
                HaveDoor = model.HaveDoor,
                HaveWindow = model.HaveWindow,
                Address = model.Address,
                RoofId = roofId,
                SortingOrder = ThisSortingOrder,
                UserId = userId
            });
            cityContext.SaveChanges();
        }

        private string GetUserId()
        {
            return accessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        public bool IsAddressAvailable(string address, int id)
        {
            var userId = GetUserId();

            var house = cityContext.Houses.SingleOrDefault(o => o.Address == address && o.UserId == userId);
            if (house == null)
            {
                return true;
            }
            else
            {
                return house.Id == id ? true : false;
            }
        }

        public async Task<IndexPartialVM[]> GetIndexPartialVMAsync(string sort, bool isAscending, string roofs, int minFloor, int MaxFloor)
        {
            var userId = GetUserId();

            var model = await cityContext.Houses.OrderBy(o => o.SortingOrder).Where(o =>
            o.NumberOfFloors >= minFloor &&
            o.NumberOfFloors <= MaxFloor &&
            o.UserId == userId &&
            roofs.Contains(o.Roof.TypeOfRoof)).
            Select(o => new IndexPartialVM
            {
                Color = o.Color,
                Address = o.Address,
                HaveBalcony = o.HaveBalcony,
                HaveDoor = o.HaveDoor,
                HaveWindow = o.HaveWindow,
                NumberOfFloors = o.NumberOfFloors,
                TypeOfRoof = o.Roof.TypeOfRoof,
                id = o.Id,
            }).ToArrayAsync();

            if (isAscending)
            {
                return model.OrderBy(o => o.GetType().GetProperty(sort).GetValue(o, null)).ToArray();
            }
            else
            {
                return model.OrderByDescending(o => o.GetType().GetProperty(sort).GetValue(o, null)).ToArray();
            }
        }

        public EditHouseVM GetEditVM(int id)
        {
            House house = cityContext.Houses.SingleOrDefault(o => o.Id == id);
            return new EditHouseVM
            {
                Address = house.Address,
                Id = house.Id,
                Color = house.Color,
                HaveBalcony = house.HaveBalcony,
                HaveDoor = house.HaveDoor,
                HaveWindow = house.HaveWindow,
                NumberOfFloors = house.NumberOfFloors,
                FloorArray = CreateFloorArray(),
                TypeOfRoofsArray = cityContext.Roofs.Select(o => o.TypeOfRoof).ToArray(),
                TypeOfRoof = cityContext.Roofs.Find(house.RoofId).TypeOfRoof
            };
        }

        public void DeleteAllHouses()
        {
            var userId = GetUserId();
            var housesToDelete = cityContext.Houses.Where(h => h.UserId == userId);

            cityContext.Houses.RemoveRange(housesToDelete);
            cityContext.SaveChanges();
        }

        public void ReorderHouses(int[] idArray)
        {
            for (int i = 0; i < idArray.Length; i++)
            {
                cityContext.Houses.Find(idArray[i]).SortingOrder = i;
            }
            cityContext.SaveChanges();
        }

        public void DeleteHouse(House house)
        {
            // Check if the current user has the necessary permission to delete the house
            if (house.UserId != GetUserId())
            {
                throw new UnauthorizedAccessException("You don't have permission to delete this house.");
            }

            cityContext.Remove(house);
            cityContext.SaveChanges();
        }

        public void EditHouse(EditHouseVM model, int id)
        {
            House house = cityContext.Houses.Find(id);
            house.Color = model.Color;
            house.RoofId = cityContext.Roofs.SingleOrDefault(o => o.TypeOfRoof == model.TypeOfRoof).Id;
            house.Address = model.Address;
            house.HaveBalcony = model.HaveBalcony;
            house.HaveDoor = model.HaveDoor;
            house.HaveWindow = model.HaveWindow;
            house.NumberOfFloors = model.NumberOfFloors;
            cityContext.SaveChanges();
        }

        public BuildHouseVM getBuildVM()
        {
            BuildHouseVM buildVM = new BuildHouseVM();
            SelectListItem[] selectListItems = CreateFloorArray();
            buildVM.FloorArray = selectListItems;
            return buildVM;
        }

        public SelectListItem[] CreateFloorArray()
        {
            SelectListItem[] selectListItems = new SelectListItem[7];
            for (int i = 0; i <= 6; i++)
            {
                selectListItems[i] = new SelectListItem { Value = "" + i, Text = "" + (i + 3) };
            }
            return selectListItems;
        }

        public string[] GetRoofsArray()
        {
            return cityContext.Roofs.Select(o => o.TypeOfRoof).ToArray();
        }

        public House? GetHouseById(int id)
        {
            return cityContext.Houses.SingleOrDefault(o => o.Id == id);
        }

        private int GetRoofId(string typeOfRoof)
        {
            var RoofId = cityContext.Roofs.SingleOrDefault(o => o.TypeOfRoof.Contains(typeOfRoof))?.Id;

            if (RoofId == null)
            {
                cityContext.Roofs.AddRange(
                     new Roof { TypeOfRoof = "Flat Roof" },
                     new Roof { TypeOfRoof = "Dome Roof" },
                     new Roof { TypeOfRoof = "Triangle Roof" }
                    );
                cityContext.SaveChanges();
                return cityContext.Roofs.SingleOrDefault(o => o.TypeOfRoof.Contains(typeOfRoof)).Id;
            }
            else
                return (int)RoofId;
        }

        public int GetHouseCount()
        {
            var userId = GetUserId();
            return cityContext.Houses.Where(h => h.UserId == userId).Count();
        }

        public IndexVM GetIndexVM()
        {
            return new IndexVM { HouseCount = GetHouseCount() };
        }
    }
}

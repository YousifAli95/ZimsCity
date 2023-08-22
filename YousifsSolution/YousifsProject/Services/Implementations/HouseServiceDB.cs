using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using YousifsProject.Loggers;
using YousifsProject.Models.Entities;
using YousifsProject.Services.Interfaces;
using YousifsProject.Utils;
using YousifsProject.Views.Houses.ViewModels;


namespace YousifsProject.Services.Implementations
{

    public class HouseServiceDB : IHouseService
    {
        readonly CityContext _cityContext;
        readonly CustomLogger _logger;
        readonly ServiceUtilsDB _serviceUtilsDB;


        public HouseServiceDB(CityContext cityContext, ServiceUtilsDB serviceUtilsDB, CustomLogger logger)
        {
            _cityContext = cityContext;
            _logger = logger;
            _serviceUtilsDB = serviceUtilsDB;
        }

        public void AddHouse(BuildHouseVM model)
        {
            int ThisSortingOrder = 1;
            if (_cityContext.Houses.Count() > 0)
            {
                ThisSortingOrder = _cityContext.Houses.Max(o => o.SortingOrder) + 1;
            }

            var roofId = GetRoofId(model.TypeOfRoof);
            string userId = _serviceUtilsDB.GetUserId();

            _cityContext.Houses.Add(new House
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
            _cityContext.SaveChanges();
        }

        public bool IsAddressAvailable(string address, int id)
        {
            var userId = _serviceUtilsDB.GetUserId();

            var house = _cityContext.Houses.SingleOrDefault(o => o.Address == address && o.UserId == userId);
            if (house == null)
            {
                return true;
            }
            else
            {
                return house.Id == id ? true : false;
            }
        }

        public async Task<IndexPartialVM[]> GetIndexPartialVMAsync(string sort, bool isAscending, string roofs, int minFloor, int maxFloor)
        {
            _logger.LogArguments(LogLevel.Debug, nameof(GetIndexPartialVMAsync), new
            {
                Sort = sort,
                IsAscending = isAscending,
                Roofs = roofs,
                MinFloor = minFloor,
                MaxFloor = maxFloor
            });

            var userId = _serviceUtilsDB.GetUserId();
            var model = await _cityContext.Houses.Where(o =>
            o.NumberOfFloors >= minFloor &&
            o.NumberOfFloors <= maxFloor &&
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
                SortingOrder = o.SortingOrder,
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

        public EditHouseVM GetEditHouseVM(int id)
        {
            House house = _cityContext.Houses.SingleOrDefault(o => o.Id == id);
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
                TypeOfRoofsArray = _cityContext.Roofs.Select(o => o.TypeOfRoof).ToArray(),
                TypeOfRoof = _cityContext.Roofs.Find(house.RoofId).TypeOfRoof
            };
        }

        public void EditHouse(EditHouseVM model, int id)
        {
            House house = _cityContext.Houses.Find(id);
            house.Color = model.Color;
            house.RoofId = _cityContext.Roofs.SingleOrDefault(o => o.TypeOfRoof == model.TypeOfRoof).Id;
            house.Address = model.Address;
            house.HaveBalcony = model.HaveBalcony;
            house.HaveDoor = model.HaveDoor;
            house.HaveWindow = model.HaveWindow;
            house.NumberOfFloors = model.NumberOfFloors;
            _cityContext.SaveChanges();
        }

        public BuildHouseVM GetBuildHouseVM()
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
            return _cityContext.Roofs.Select(o => o.TypeOfRoof).ToArray();
        }

        private int GetRoofId(string typeOfRoof)
        {
            var RoofId = _cityContext.Roofs.SingleOrDefault(o => o.TypeOfRoof.Contains(typeOfRoof))?.Id;

            if (RoofId == null)
            {
                _cityContext.Roofs.AddRange(
                     new Roof { TypeOfRoof = "Flat Roof" },
                     new Roof { TypeOfRoof = "Dome Roof" },
                     new Roof { TypeOfRoof = "Triangle Roof" }
                    );
                _cityContext.SaveChanges();
                return _cityContext.Roofs.SingleOrDefault(o => o.TypeOfRoof.Contains(typeOfRoof)).Id;
            }
            else
                return (int)RoofId;
        }

        public int GetHouseCount()
        {
            var userId = _serviceUtilsDB.GetUserId();
            return _cityContext.Houses.Where(h => h.UserId == userId).Count();
        }

        public IndexVM GetIndexVM()
        {
            return new IndexVM { HouseCount = GetHouseCount() };
        }
    }
}

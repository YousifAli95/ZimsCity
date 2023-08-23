using Microsoft.EntityFrameworkCore;
using YousifsProject.Exceptions;
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

        public bool IsAddressAvailable(string address, int? id)
        {
            var userId = _serviceUtilsDB.GetUserId();

            var house = _cityContext.Houses.SingleOrDefault(o => o.Address == address && o.UserId == userId);

            if (house == null)
                return true;

            else
            {
                if (id.HasValue)
                    return house.Id == id.Value;

                return false;
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

        public ConfigureHouseVM GetConfigureHouseVM(int? id)
        {
            var model = new ConfigureHouseVM
            {
                FloorArray = _serviceUtilsDB.CreateFloorArray(),
                TypeOfRoofsArray = _cityContext.Roofs.Select(o => o.TypeOfRoof).ToArray()
            };

            if (id.HasValue)
            {
                House house = _serviceUtilsDB.GetHouseById(id.Value);
                if (house == null)
                {
                    throw new NotFoundException($"No house found with id {id}");
                }

                _serviceUtilsDB.CheckAuthorization(house, failMessage: "You don't have permission to configure this house");

                model.Address = house.Address;
                model.Id = house.Id;
                model.Color = house.Color;
                model.HaveBalcony = house.HaveBalcony;
                model.HaveDoor = house.HaveDoor;
                model.HaveWindow = house.HaveWindow;
                model.NumberOfFloors = house.NumberOfFloors;
                model.TypeOfRoof = _cityContext.Roofs.Find(house.RoofId).TypeOfRoof;
            };

            return model;
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

        public bool CheckIfValidId(int id)
        {
            House house = _serviceUtilsDB.GetHouseById(id);
            if (house == null)
                return false;
            try
            {
                _serviceUtilsDB.CheckAuthorization(house, "You don't have permission to configure this house");
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        public void ConfigureHouse(int? id, ConfigureHouseVM model)
        {
            House house = id.HasValue ? _cityContext.Houses.Find(id) : new House();

            house.Color = model.Color;
            house.RoofId = _serviceUtilsDB.GetRoofId(model.TypeOfRoof);
            house.Address = model.Address;
            house.HaveBalcony = model.HaveBalcony;
            house.HaveDoor = model.HaveDoor;
            house.HaveWindow = model.HaveWindow;
            house.NumberOfFloors = model.NumberOfFloors;

            if (!id.HasValue)
            {
                var userId = _serviceUtilsDB.GetUserId();

                int ThisSortingOrder = 1;
                if (_cityContext.Houses.Count() > 0)
                {
                    ThisSortingOrder = _cityContext.Houses.Max(o => o.SortingOrder) + 1;
                }
                house.SortingOrder = ThisSortingOrder;
                house.UserId = userId;

                _cityContext.Houses.Add(house);
            }

            _cityContext.SaveChanges();
        }
    }
}

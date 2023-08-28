using ZimsCityProject.Exceptions;
using ZimsCityProject.Loggers;
using ZimsCityProject.Models.Entities;
using ZimsCityProject.Services.Interfaces;
using ZimsCityProject.Utils;

namespace ZimsCityProject.Services.Implementations
{

    public class WebApiServiceDB : IWebApiService
    {
        readonly CityContext _cityContext;
        readonly CustomLogger _logger;
        readonly ServiceUtilsDB _serviceUtilsDB;

        public WebApiServiceDB(CityContext cityContext, CustomLogger logger, ServiceUtilsDB serviceUtilsDB)
        {
            _cityContext = cityContext;
            _logger = logger;
            _serviceUtilsDB = serviceUtilsDB;
        }

        public void DeleteAllHouses()
        {
            var userId = _serviceUtilsDB.GetUserId();
            var housesToDelete = _cityContext.Houses.Where(h => h.UserId == userId);

            _cityContext.Houses.RemoveRange(housesToDelete);
            _cityContext.SaveChanges();
        }

        public void ReorderHouses(int[] idArray)
        {
            for (int i = 0; i < idArray.Length; i++)
            {
                House house = _cityContext.Houses.Find(idArray[i]);
                _serviceUtilsDB.CheckAuthorization(house, failMessage: "You don't have permission to reorder this house!");

                house.SortingOrder = i;
            }
            _cityContext.SaveChanges();
        }

        public void DeleteHouse(int id)
        {
            var houseToDelete = _serviceUtilsDB.GetHouseById(id);
            if (houseToDelete == null)
            {
                throw new NotFoundException($"House with Id = {id} not found");
            }

            _serviceUtilsDB.CheckAuthorization(houseToDelete, failMessage: "You don't have permission to delete this house!");

            _cityContext.Remove(houseToDelete);
            _cityContext.SaveChanges();
        }
    }
}

using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using YousifsProject.Models.Entities;

namespace YousifsProject.Utils
{
    public class ServiceUtilsDB
    {
        readonly IHttpContextAccessor _accessor;
        readonly CityContext _cityContext;


        public ServiceUtilsDB(IHttpContextAccessor accessor, CityContext cityContext)
        {
            _accessor = accessor;
            _cityContext = cityContext;
        }

        public string GetUserId()
        {
            return _accessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        public House? GetHouseById(int id)
        {
            return _cityContext.Houses.SingleOrDefault(o => o.Id == id);
        }

        public void CheckAuthorization(House house, string failMessage)
        {
            if (house.UserId != GetUserId())
            {
                throw new UnauthorizedAccessException(failMessage);
            }
        }

        public SelectListItem[] CreateFloorArray()
        {
            SelectListItem[] selectListItems = new SelectListItem[7];
            for (int i = 0; i <= 6; i++)
            {
                selectListItems[i] = new SelectListItem { Value = i.ToString(), Text = (i + 3).ToString() };
            }
            return selectListItems;
        }

        public string[] GetRoofsArray()
        {
            return _cityContext.Roofs.Select(o => o.TypeOfRoof).ToArray();
        }

        public int GetRoofId(string typeOfRoof)
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
    }
}

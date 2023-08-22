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
    }
}

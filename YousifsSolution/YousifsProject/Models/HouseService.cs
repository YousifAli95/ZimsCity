using YousifsProject.Models.Entities;
using YousifsProject.Views.Houses;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace YousifsProject.Models
{
    
    public class HouseService
    {
        CityContext cityContext;

        public HouseService(CityContext cityContext)
        {
            this.cityContext = cityContext;
        }

        public void Add(BuildVM model)
        {
            int ThisSortingOrder = 1;
            if (cityContext.Houses.Count() > 0)
            {
             ThisSortingOrder = cityContext.Houses.Max(o => o.SortingOrder) + 1;
            }
            cityContext.Houses.Add(new House
            {
                NumberOfFloors = model.NumberOfFloors,
                Color = model.Color,
                HaveBalcony = model.HaveBalcony,
                HaveDoor = model.HaveDoor,
                HaveWindow = model.HaveWindow,
                Address = model.Address,
                RoofId = cityContext.Roofs.SingleOrDefault(o=> o.TypeOfRoof == model.TypeOfRoof).Id,
                SortingOrder = ThisSortingOrder,
            });
            cityContext.SaveChanges();
        }

        internal bool IsAddressAvailable(string address, int id)
        {
            House? house = cityContext.Houses.SingleOrDefault(o=> o.Address == address);
            if (house == null) {
                return true;
            }
            else
            {
                 return (house.Id == id ? true: false);
            }
        }

        public async Task<IndexVM[]> GetIndexVMAsync(string sort, bool isAscending, string roofs, int minFloor, int MaxFloor)
        {
            var model = await cityContext.Houses.OrderBy(o => o.SortingOrder).Where(o =>
            o.NumberOfFloors >= minFloor &&
            o.NumberOfFloors <= MaxFloor &&
            roofs.Contains(o.Roof.TypeOfRoof)).
            Select(o => new IndexVM
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

        internal EditVM GetEditVM(int id)
        {
            House house = cityContext.Houses.SingleOrDefault(o => o.Id == id);
            return new EditVM
            {
                Address = house.Address,
                Id = house.Id,
                Color = house.Color,
                HaveBalcony = house.HaveBalcony,
                HaveDoor = house.HaveDoor,
                HaveWindow = house.HaveWindow,
                NumberOfFloors = house.NumberOfFloors,
                FloorArray = CreateFloorArray(),
                TypeOfRoofsArray = cityContext.Roofs.Select(o=> o.TypeOfRoof).ToArray(),
                TypeOfRoof = cityContext.Roofs.Find(house.RoofId).TypeOfRoof
            };
        }

        internal void DeleteAll()
        {
            foreach (var house in cityContext.Houses)
            {
                cityContext.Houses.Remove(house);
            }
            cityContext.SaveChanges();
        }

        internal void Reorder(int[] idArray)
        {
            for (int i = 0; i < idArray.Length; i++)
            {
                cityContext.Houses.Find(idArray[i]).SortingOrder = i;
            }
            cityContext.SaveChanges();
        }

        internal void Delete(House house)
        {
            cityContext.Remove(house);
            cityContext.SaveChanges();
        }

        internal void PostEditEmployee(EditVM model, int id)
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

        public BuildVM getBuildVM()
        {
            BuildVM buildVM = new BuildVM();
            SelectListItem[] selectListItems = CreateFloorArray();
            buildVM.FloorArray = selectListItems;
            return buildVM;
        }

        public SelectListItem[] CreateFloorArray()
        {
            SelectListItem[] selectListItems = new SelectListItem[7];
            for (int i = 0; i <= 6; i++)
            {
                selectListItems[i] = new SelectListItem { Value = "" + (i), Text = "" + (i + 3) };
            }
            return selectListItems;
        }

        public string[] GetRoofsArray()
        {
            return cityContext.Roofs.Select(o => o.TypeOfRoof).ToArray();
        }
    }
}

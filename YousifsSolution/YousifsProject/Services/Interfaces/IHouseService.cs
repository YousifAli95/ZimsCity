using Microsoft.AspNetCore.Mvc.Rendering;
using YousifsProject.Models.Entities;
using YousifsProject.Views.Houses.ViewModels;

namespace YousifsProject.Services.Interfaces
{
    public interface IHouseService
    {
        public void AddHouse(BuildHouseVM model);
        public bool IsAddressAvailable(string address, int id);

        public Task<IndexPartialVM[]> GetIndexPartialVMAsync(string sort, bool isAscending, string roofs, int minFloor, int MaxFloor);
        public EditHouseVM GetEditVM(int id);

        public void DeleteAllHouses();

        public void ReorderHouses(int[] idArray);
        public void DeleteHouse(House house);

        public void EditHouse(EditHouseVM model, int id);
        public BuildHouseVM getBuildVM();

        public SelectListItem[] CreateFloorArray();

        public string[] GetRoofsArray();

        public House? GetHouseById(int id);

        public int GetHouseCount();

        public IndexVM GetIndexVM();
    }
}

using Microsoft.AspNetCore.Mvc.Rendering;
using YousifsProject.Views.Houses.ViewModels;

namespace YousifsProject.Services.Interfaces
{
    public interface IHouseService
    {
        public void AddHouse(BuildHouseVM model);
        public bool IsAddressAvailable(string address, int id);
        public Task<IndexPartialVM[]> GetIndexPartialVMAsync(string sort, bool isAscending, string roofs, int minFloor, int MaxFloor);
        public EditHouseVM GetEditHouseVM(int id);
        public void EditHouse(EditHouseVM model, int id);
        public BuildHouseVM GetBuildHouseVM();
        public SelectListItem[] CreateFloorArray();
        public string[] GetRoofsArray();
        public int GetHouseCount();
        public IndexVM GetIndexVM();
    }
}

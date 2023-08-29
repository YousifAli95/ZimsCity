using ZimsCityProject.Views.Houses.ViewModels;

namespace ZimsCityProject.Services.Interfaces
{
    public interface IHouseService
    {
        public Task<IndexPartialVM[]> GetIndexPartialVMAsync(string sort, bool isAscending, string roofs, int minFloor, int MaxFloor);
        public ConfigureHouseVM GetConfigureHouseVM(int? id);
        public IndexVM GetIndexVM();
        void ConfigureHouse(int? id, ConfigureHouseVM model);
        public bool IsAddressAvailable(string address, int? id);
        public int GetHouseCount();
        bool CheckIfValidHouseId(int id);
    }
}

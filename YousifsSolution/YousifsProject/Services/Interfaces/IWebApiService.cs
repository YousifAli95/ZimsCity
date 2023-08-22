namespace YousifsProject.Services.Interfaces
{
    public interface IWebApiService
    {
        public void DeleteAllHouses();
        public void ReorderHouses(int[] idArray);
        public void DeleteHouse(int id);

    }
}

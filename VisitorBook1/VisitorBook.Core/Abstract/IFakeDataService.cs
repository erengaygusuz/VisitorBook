namespace VisitorBook.Core.Abstract
{
    public interface IFakeDataService
    {
        Task InsertUserDatas(int amount);
        Task InsertVisitedCountyDatas(int amount);
    }
}

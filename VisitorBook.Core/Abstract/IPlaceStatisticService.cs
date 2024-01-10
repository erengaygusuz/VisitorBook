namespace VisitorBook.Core.Abstract
{
    public interface IPlaceStatisticService
    {
        int GetTotalRegionCount();
        int GetTotalSubRegionCount();
        int GetTotalCountryCount();
        int GetTotalCityCount();
        int GetTotalCountyCount();
        int GetTotalVisitedCountyCount();
    }
}

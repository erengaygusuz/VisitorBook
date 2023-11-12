using VisitorBook.Core.Abstract;

namespace VisitorBook.Core.Dtos.CountyDtos
{
    public class CountyToCountyGetResponseDtoMappingCollection<TSource, TDestination> : PropertyMappingCollectionBase<TSource, TDestination>
    {
        protected override IList<IPropertyMapping> _mappings => new List<IPropertyMapping>
        {
            new PropertyMapping("Name", "Name"),
            new PropertyMapping("City.Name", "City.Name"),
            new PropertyMapping("Latitude", "Latitude"),
            new PropertyMapping("Longitude", "Longitude")
        };

        public override IList<IPropertyMapping> GetAssociatedMappings()
        {
            return _mappings;
        }
    }
}

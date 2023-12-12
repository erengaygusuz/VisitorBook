using VisitorBook.Backend.Core.Abstract;

namespace VisitorBook.Backend.Core.Dtos.CountryDtos
{
    public class CountryToCountryGetResponseDtoMappingCollection<TSource, TDestination> : PropertyMappingCollectionBase<TSource, TDestination>
    {
        protected override IList<IPropertyMapping> _mappings => new List<IPropertyMapping>
        {
            new PropertyMapping("Name", "Name"),
            new PropertyMapping("SubRegion.Name", "SubRegion.Name")
        };

        public override IList<IPropertyMapping> GetAssociatedMappings()
        {
            return _mappings;
        }
    }
}

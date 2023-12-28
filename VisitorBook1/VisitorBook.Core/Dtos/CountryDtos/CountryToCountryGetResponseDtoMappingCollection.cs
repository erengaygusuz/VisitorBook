using VisitorBook.Core.Abstract;

namespace VisitorBook.Core.Dtos.CountryDtos
{
    public class CountryToCountryGetResponseDtoMappingCollection<TSource, TDestination> : PropertyMappingCollectionBase<TSource, TDestination>
    {
        protected override IList<IPropertyMapping> _mappings => new List<IPropertyMapping>
        {
            new PropertyMapping("Name", "Name"),
            new PropertyMapping("Code", "Code"),
            new PropertyMapping("SubRegion.Name", "SubRegion.Name")
        };

        public override IList<IPropertyMapping> GetAssociatedMappings()
        {
            return _mappings;
        }
    }
}

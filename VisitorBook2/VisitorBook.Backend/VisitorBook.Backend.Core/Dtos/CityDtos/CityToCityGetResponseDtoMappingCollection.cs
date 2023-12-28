using VisitorBook.Backend.Core.Abstract;

namespace VisitorBook.Backend.Core.Dtos.CityDtos
{
    public class CityToCityGetResponseDtoMappingCollection<TSource, TDestination> : PropertyMappingCollectionBase<TSource, TDestination>
    {
        protected override IList<IPropertyMapping> _mappings => new List<IPropertyMapping>
        {
            new PropertyMapping("Name", "Name"),
            new PropertyMapping("Code", "Code"),
            new PropertyMapping("Country.Name", "Country.Name")
        };

        public override IList<IPropertyMapping> GetAssociatedMappings()
        {
            return _mappings;
        }
    }
}

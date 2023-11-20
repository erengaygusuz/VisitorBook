using VisitorBook.Backend.Core.Abstract;

namespace VisitorBook.Backend.Core.Dtos.CityDtos
{
    public class CityToCityGetResponseDtoMappingCollection<TSource, TDestination> : PropertyMappingCollectionBase<TSource, TDestination>
    {
        protected override IList<IPropertyMapping> _mappings => new List<IPropertyMapping>
        {
            new PropertyMapping("Id", "Id"),
            new PropertyMapping("Name", "Name"),
            new PropertyMapping("Code", "Code")
        };

        public override IList<IPropertyMapping> GetAssociatedMappings()
        {
            return _mappings;
        }
    }
}

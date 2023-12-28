using VisitorBook.Core.Abstract;

namespace VisitorBook.Core.Dtos.VisitedCountyDtos
{
    public class VisitedCountyToVisitedCountyGetResponseDtoMappingCollection<TSource, TDestination> : PropertyMappingCollectionBase<TSource, TDestination>
    {
        protected override IList<IPropertyMapping> _mappings => new List<IPropertyMapping>
        {
            new PropertyMapping("User.Name", "User.Name"),
            new PropertyMapping("County.Name", "County.Name"),
            new PropertyMapping("County.City.Name", "County.City.Name"),
            new PropertyMapping("VisitDate", "VisitDate")
        };

        public override IList<IPropertyMapping> GetAssociatedMappings()
        {
            return _mappings;
        }
    }
}

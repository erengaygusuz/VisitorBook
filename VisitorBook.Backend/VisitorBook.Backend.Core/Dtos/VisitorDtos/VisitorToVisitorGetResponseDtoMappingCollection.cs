using VisitorBook.Backend.Core.Abstract;

namespace VisitorBook.Backend.Core.Dtos.VisitorDtos
{
    public class VisitorToVisitorGetResponseDtoMappingCollection<TSource, TDestination> : PropertyMappingCollectionBase<TSource, TDestination>
    {
        protected override IList<IPropertyMapping> _mappings => new List<IPropertyMapping>
        {
            new PropertyMapping("User.Name", "User.Name"),
            new PropertyMapping("User.Surname", "User.Surname"),
            new PropertyMapping("User.BirthDate", "User.BirthDate"),
            new PropertyMapping("User.Gender", "User.Gender")
        };

        public override IList<IPropertyMapping> GetAssociatedMappings()
        {
            return _mappings;
        }
    }
}

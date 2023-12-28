using VisitorBook.Backend.Core.Abstract;

namespace VisitorBook.Backend.Core.Dtos.RoleDtos
{
    public class RoleToRoleGetResponseDtoMappingCollection<TSource, TDestination> : PropertyMappingCollectionBase<TSource, TDestination>
    {
        protected override IList<IPropertyMapping> _mappings => new List<IPropertyMapping>
        {
            new PropertyMapping("Name", "Name"),
            new PropertyMapping("Region.Name", "Region.Name")
        };

        public override IList<IPropertyMapping> GetAssociatedMappings()
        {
            return _mappings;
        }
    }
}

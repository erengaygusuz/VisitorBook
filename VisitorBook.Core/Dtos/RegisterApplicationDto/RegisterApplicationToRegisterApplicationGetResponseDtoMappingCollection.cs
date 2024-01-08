using VisitorBook.Core.Abstract;

namespace VisitorBook.Core.Dtos.UserDtos
{
    public class RegisterApplicationToRegisterApplicationGetResponseDtoMappingCollection<TSource, TDestination> : PropertyMappingCollectionBase<TSource, TDestination>
    {
        protected override IList<IPropertyMapping> _mappings => new List<IPropertyMapping>
        {
            new PropertyMapping("User.Name", "User.Name"),
            new PropertyMapping("User.Surname", "User.Surname"),
            new PropertyMapping("User.Username", "User.Username"),
            new PropertyMapping("User.Email", "User.Email"),
            new PropertyMapping("CreatedDate", "CreatedDate"),
            new PropertyMapping("Status", "Status")
        };

        public override IList<IPropertyMapping> GetAssociatedMappings()
        {
            return _mappings;
        }
    }
}

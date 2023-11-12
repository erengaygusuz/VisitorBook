
namespace VisitorBook.Core.Abstract
{
    public interface IPropertyMappingService
    {
        IList<IPropertyMapping> GetMappings<TSource, TDestination>();
    }
}

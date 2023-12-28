namespace VisitorBook.Backend.Core.Abstract
{
    public interface IPropertyMappingService
    {
        IList<IPropertyMapping> GetMappings<TSource, TDestination>();
    }
}

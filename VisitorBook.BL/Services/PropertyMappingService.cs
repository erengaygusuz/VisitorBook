using VisitorBook.Core.Abstract;

namespace VisitorBook.BL.Services
{
    public class PropertyMappingService : IPropertyMappingService
    {
        private readonly IEnumerable<IPropertyMappingCollection> _mappingCollections;

        public PropertyMappingService(IEnumerable<IPropertyMappingCollection> mappingCollections)
        {
            _mappingCollections = mappingCollections;
        }

        public IList<IPropertyMapping> GetMappings<TSource, TDestination>()
        {
            return _mappingCollections.FirstOrDefault(c => c.IsApplicable<TSource, TDestination>())?.GetAssociatedMappings()
                   ?? new List<IPropertyMapping>();
        }
    }
}

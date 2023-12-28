using VisitorBook.Backend.Core.Abstract;

namespace VisitorBook.Backend.BL.Concrete
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

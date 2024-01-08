namespace VisitorBook.Core.Abstract
{
    public abstract class PropertyMappingCollectionBase<TSource, TDestination> : IPropertyMappingCollection
    {
        protected virtual IList<IPropertyMapping> _mappings { get; } = new List<IPropertyMapping>();

        public abstract IList<IPropertyMapping> GetAssociatedMappings();

        public virtual bool IsApplicable<TMappingSource, TMappingDestination>()
        {
            return typeof(TMappingSource) == typeof(TSource) && typeof(TMappingDestination) == typeof(TDestination);
        }
    }
}

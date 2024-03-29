﻿namespace VisitorBook.Core.Abstract
{
    public interface IPropertyMappingCollection
    {
        IList<IPropertyMapping> GetAssociatedMappings();
        bool IsApplicable<TSource, TDestination>();
    }
}

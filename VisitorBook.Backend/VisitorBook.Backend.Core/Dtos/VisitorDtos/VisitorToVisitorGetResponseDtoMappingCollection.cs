﻿using VisitorBook.Backend.Core.Abstract;

namespace VisitorBook.Backend.Core.Dtos.VisitorDtos
{
    public class VisitorToVisitorGetResponseDtoMappingCollection<TSource, TDestination> : PropertyMappingCollectionBase<TSource, TDestination>
    {
        protected override IList<IPropertyMapping> _mappings => new List<IPropertyMapping>
        {
            new PropertyMapping("Name", "Name"),
            new PropertyMapping("Surname", "Surname"),
            new PropertyMapping("BirthDate", "BirthDate"),
            new PropertyMapping("Gender", "Gender")
        };

        public override IList<IPropertyMapping> GetAssociatedMappings()
        {
            return _mappings;
        }
    }
}
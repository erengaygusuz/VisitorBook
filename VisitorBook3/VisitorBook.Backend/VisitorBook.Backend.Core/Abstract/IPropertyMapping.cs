namespace VisitorBook.Backend.Core.Abstract
{
    public interface IPropertyMapping
    {
        string SourceProperty { get; }
        string DestinationProperty { get; }
    }
}

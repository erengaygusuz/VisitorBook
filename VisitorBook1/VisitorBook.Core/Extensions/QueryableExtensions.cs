using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using VisitorBook.Core.Abstract;
using VisitorBook.Core.Utilities.DataTablesServerSideHelpers;

namespace VisitorBook.Core.Extensions
{
    public static class QueryableExtensions
    {
        public static PagedList<T> ToPagedList<T>(this IQueryable<T> query, DataTablesOptions options)
        {
            return PagedList<T>.Create(query, options);
        }

        public static IQueryable<TEntity> ApplySearch<TEntity>(this IQueryable<TEntity> query, DataTablesOptions options, IList<IPropertyMapping> mappings)
        {
            if (!mappings.Any())
            {
                return query;
            }

            var searchableColumns = options.GetSearchableColums();

            if (!searchableColumns.Any())
            {
                return query;
            }

            var expression = GetSearchableProperties(mappings, searchableColumns)
                .Select(property => BuildLambaExpression<TEntity>(options.Search.Value, property))
                .Where(lambda => lambda != null)
                .Aggregate<Expression<Func<TEntity, bool>>, Expression<Func<TEntity, bool>>>(null, (current, lambda) => current == null ? lambda : current.Or(lambda));

            return expression == null ? query : query.Where(expression);
        }

        public static IQueryable<T> ApplySort<T>(this IQueryable<T> query, DataTablesOptions options, IList<IPropertyMapping> mappings)
        {
            if (!options.Order.Any())
            {
                return query;
            }

            if (!mappings.Any())
            {
                return query;
            }

            var sortableColumns = options.GetSortableColumns().ToArray();

            if (!sortableColumns.Any())
            {
                return query;
            }

            var sortOptions = options.Order.Select(order => new
            {
                sortBy = mappings.First(m => m.DestinationProperty.Equals(sortableColumns[order.Column].Data, StringComparison.OrdinalIgnoreCase)).SourceProperty,
                sortDirection = order.GetSortDirection().ToString()
            });

            return sortOptions.Aggregate(query, (current, option) => current.OrderBy($"{option.sortBy} {option.sortDirection}"));
        }

        private static IEnumerable<string> GetSearchableProperties(IList<IPropertyMapping> mappings, IEnumerable<Column> searchColumns)
        {
            return searchColumns
                .Select(c => mappings.First(m => m.DestinationProperty.Equals(c.Data, StringComparison.OrdinalIgnoreCase)).SourceProperty)
                .Where(match => !string.IsNullOrWhiteSpace(match));
        }

        private static Expression<Func<TEntity, bool>> BuildLambaExpression<TEntity>(string valueToSearch, string property)
        {
            var typeOfString = typeof(string);

            var parameterExpression = Expression.Parameter(typeof(TEntity), "TEntity");

            var member = property.Split('.').Aggregate<string, Expression>(parameterExpression, Expression.PropertyOrField);

            var canConvert = CanConvertToType(valueToSearch, member.Type.FullName);

            if (canConvert)
            {
                var value = ConvertToType(valueToSearch, member.Type.FullName);

                var constant = Expression.Constant(value);

                if (member.Type == typeOfString)
                {
                    var methodInfo = typeOfString.GetMethod("StartsWith", new[] { typeOfString });
                    var call = Expression.Call(member, methodInfo, constant);
                    return Expression.Lambda<Func<TEntity, bool>>(call, parameterExpression);
                }

                return Expression.Lambda<Func<TEntity, bool>>(Expression.Equal(member, constant), parameterExpression);
            }

            return null;
        }

        private static bool CanConvertToType(object value, string type)
        {
            try
            {
                ConvertToType(value, type);

                return true;
            }
            catch
            {
                return false;
            }
        }

        private static dynamic ConvertToType(object value, string type)
        {
            return Convert.ChangeType(value, Type.GetType(type));
        }
    }
}

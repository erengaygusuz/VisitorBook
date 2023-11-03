using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;
using VisitorBook.Core.Abstract;
using VisitorBook.Core.Models;

namespace VisitorBook.BL.Concrete
{
    public class Service<T> : IService<T> where T : BaseModel
    {
        private readonly IUnitOfWork<T> _unitOfWork;
        private readonly IRepository<T> _repository;

        public Service(IUnitOfWork<T> unitOfWork, IRepository<T> repository)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
        }

        public async Task<IEnumerable<T>> GetAllAsync(
            Expression<Func<T, bool>>? expression = null,
            bool trackChanges = false,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null)
        {
            var entities = await _repository.GetAll(expression, trackChanges, include, orderBy).ToListAsync();

            return entities;
        }

        public async Task<Tuple<int, int, IEnumerable<T>>> GetAllAsync(
            int page, int pageSize,
            Expression<Func<T, bool>>? expression = null,
            bool trackChanges = false,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null)
        {
            var repoTuple = _repository.GetAll(page, pageSize, expression, trackChanges, include, orderBy);

            var serviceTuple = new Tuple<int, int, IEnumerable<T>>(
                repoTuple.Item1,
                repoTuple.Item2,
                await repoTuple.Item3.ToListAsync()
            );

            return serviceTuple;
        }

        public async Task<T> GetAsync(
           Expression<Func<T, bool>>? expression = null,
           bool trackChanges = false,
           Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null)
        {
            var entity = await _repository.GetAsync(expression, trackChanges, include);

            return entity;
        }

        public async Task UpdateAsync(T entity)
        {
            _repository.Update(entity);
            await _unitOfWork.SaveAsync();
        }

        public async Task RemoveAsync(T entity)
        {
            _repository.Remove(entity);
            await _unitOfWork.SaveAsync();
        }

        public async Task AddAsync(T entity)
        {
            await _repository.AddAsync(entity);
            await _unitOfWork.SaveAsync();
        }

        public async Task AddRangeAsync(IEnumerable<T> entities)
        {
            await _repository.AddRangeAsync(entities);
            await _unitOfWork.SaveAsync();
        }
    }
}

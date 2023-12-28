using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;
using VisitorBook.Backend.Core.Abstract;
using VisitorBook.Backend.Core.Extensions;
using VisitorBook.Backend.Core.Entities;
using VisitorBook.Backend.Core.Utilities.DataTablesServerSideHelpers;

namespace VisitorBook.Backend.BL.Concrete
{
    public class Service<TEntity> : IService<TEntity> where TEntity : BaseEntity
    {
        private readonly IUnitOfWork<TEntity> _unitOfWork;
        private readonly IRepository<TEntity> _repository;
        private readonly IPropertyMappingService _propertyMappingService;
        private readonly IMapper _mapper;

        public Service(IUnitOfWork<TEntity> unitOfWork, IRepository<TEntity> repository, IPropertyMappingService propertyMappingService, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
            _mapper = mapper;
            _propertyMappingService = propertyMappingService;
        }

        public PagedList<TResult> GetAll<TResult>(DataTablesOptions model, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null)
        {
            var propertyMappings = _propertyMappingService.GetMappings<TEntity, TResult>();

            var result = _repository.GetAll(include: include)
                .ApplySearch(model, propertyMappings)
                .ApplySort(model, propertyMappings)
                .ProjectTo<TResult>(_mapper.ConfigurationProvider)
                .ToPagedList(model);

            return result;
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(
            Expression<Func<TEntity, bool>>? expression = null,
            bool trackChanges = false,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null)
        {
            var entities = await _repository.GetAll(expression, trackChanges, include, orderBy).ToListAsync();

            return entities;
        }

        public async Task<Tuple<int, int, IEnumerable<TEntity>>> GetAllAsync(
            int page, int pageSize,
            Expression<Func<TEntity, bool>>? expression = null,
            bool trackChanges = false,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null)
        {
            var repoTuple = _repository.GetAll(page, pageSize, expression, trackChanges, include, orderBy);

            var serviceTuple = new Tuple<int, int, IEnumerable<TEntity>>(
                repoTuple.Item1,
                repoTuple.Item2,
                await repoTuple.Item3.ToListAsync()
            );

            return serviceTuple;
        }

        public async Task<TEntity> GetAsync(
           Expression<Func<TEntity, bool>>? expression = null,
           bool trackChanges = false,
           Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null)
        {
            var entity = await _repository.GetAsync(expression, trackChanges, include);

            return entity;
        }

        public async Task UpdateAsync(TEntity entity)
        {
            _repository.Update(entity);
            await _unitOfWork.SaveAsync();
        }

        public async Task RemoveAsync(TEntity entity)
        {
            _repository.Remove(entity);
            await _unitOfWork.SaveAsync();
        }

        public async Task AddAsync(TEntity entity)
        {
            await _repository.AddAsync(entity);
            await _unitOfWork.SaveAsync();
        }

        public async Task AddRangeAsync(IEnumerable<TEntity> entities)
        {
            await _repository.AddRangeAsync(entities);
            await _unitOfWork.SaveAsync();
        }
    }
}

using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;
using VisitorBook.Core.Abstract;
using VisitorBook.Core.Extensions;
using VisitorBook.Core.Entities;
using VisitorBook.Core.Utilities.DataTablesServerSideHelpers;

namespace VisitorBook.BL.Concrete
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

        public PagedList<TDto> GetAll<TDto>(DataTablesOptions model, Func<IQueryable<TEntity>, 
            IIncludableQueryable<TEntity, object>>? include = null, Expression<Func<TEntity, bool>>? expression = null)
        {
            var propertyMappings = _propertyMappingService.GetMappings<TEntity, TDto>();

            var result = _repository.GetAll(include: include, expression: expression)
                .ApplySearch(model, propertyMappings)
                .ApplySort(model, propertyMappings)
                .ProjectTo<TDto>(_mapper.ConfigurationProvider)
                .ToPagedList(model);

            return result;
        }

        public async Task<IEnumerable<TDto>> GetAllAsync<TDto>(
            Expression<Func<TEntity, bool>>? expression = null,
            bool trackChanges = false,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null)
        {
            var entities = await _repository.GetAll(expression, trackChanges, include, orderBy).ToListAsync();

            return _mapper.Map<List<TDto>>(entities);
        }

        public async Task<Tuple<int, int, IEnumerable<TDto>>> GetAllAsync<TDto>(
            int page, int pageSize,
            Expression<Func<TEntity, bool>>? expression = null,
            bool trackChanges = false,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null)
        {
            var repoTuple = _repository.GetAll(page, pageSize, expression, trackChanges, include, orderBy);

            var serviceTuple = new Tuple<int, int, IEnumerable<TDto>>(
                repoTuple.Item1,
                repoTuple.Item2,
                _mapper.Map<IEnumerable<TDto>>(await repoTuple.Item3.ToListAsync())
            );

            return serviceTuple;
        }

        public async Task<TDto> GetAsync<TDto>(
           Expression<Func<TEntity, bool>>? expression = null,
           bool trackChanges = false,
           Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null)
        {
            var entity = await _repository.GetAsync(expression, trackChanges, include);

            return _mapper.Map<TDto>(entity);
        }

        public async Task<bool> UpdateAsync<TDto>(TDto dto)
        {
            _repository.Update(_mapper.Map<TEntity>(dto));

            return await _unitOfWork.SaveAsync();
        }

        public async Task<bool> RemoveAsync<TDto>(TDto dto)
        {
            _repository.Remove(_mapper.Map<TEntity>(dto));

            return await _unitOfWork.SaveAsync();
        }

        public async Task<bool> AddAsync<TDto>(TDto dto)
        {
            await _repository.AddAsync(_mapper.Map<TEntity>(dto));

            return await _unitOfWork.SaveAsync();
        }

        public async Task<bool> AddRangeAsync<TDto>(IEnumerable<TDto> dtos)
        {
            await _repository.AddRangeAsync(_mapper.Map<IEnumerable<TEntity>>(dtos));

            return await _unitOfWork.SaveAsync();
        }
    }
}

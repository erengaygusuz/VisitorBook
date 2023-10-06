using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
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
           Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null)
        {
            var entities = await _repository.GetAll(expression, trackChanges, include).ToListAsync();

            return entities;
        }

        public async Task<T> GetAsync(int id)
        {
            var entity = await _repository.GetAsync(u => u.Id == id);

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
    }
}

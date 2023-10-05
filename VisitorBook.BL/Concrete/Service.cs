using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisitorBook.Core.Abstract;
using VisitorBook.Core.Models;

namespace VisitorBook.BL.Concrete
{
    public class Service<T> : IService<T> where T : BaseModel
    {
        private readonly IUnitOfWork<T> _unitOfWork;

        public Service(IUnitOfWork<T> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            var entities = await _unitOfWork.Repository.GetAll().ToListAsync();

            return entities;
        }

        public async Task<T> GetAsync(int id)
        {
            var entity = await _unitOfWork.Repository.GetAsync(u => u.Id == id);

            return entity;
        }

        public async Task UpdateAsync(T entity)
        {
            _unitOfWork.Repository.Update(entity);
            await _unitOfWork.SaveAsync();
        }

        public async Task RemoveAsync(T entity)
        {
            _unitOfWork.Repository.Remove(entity);
            await _unitOfWork.SaveAsync();
        }

        public async Task AddAsync(T entity)
        {
            await _unitOfWork.Repository.AddAsync(entity);
            await _unitOfWork.SaveAsync();
        }
    }
}

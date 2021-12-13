using Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infrastructure.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly DataContext _context;
        private DbSet<T> Table = null;

        public GenericRepository(DataContext context)
        {
            _context = context;
            Table = _context.Set<T>();
        }
        public void Delete(object Id)
        {
            T existing = GetById(Id);
            Table.Remove(existing);
        }

        public IEnumerable<T> GetAll()
        {
            return Table.ToList();
        }

        public T GetById(object Id)
        {
            return Table.Find(Id);
        }

        public void Insert(T entity)
        {
            Table.Add(entity);
        }

        public void Update(T entity)
        {
            Table.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }
    }
}

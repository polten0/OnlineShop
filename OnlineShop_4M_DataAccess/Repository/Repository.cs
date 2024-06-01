using System;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using OnlineShop_4M_DataAccess.Data;
using OnlineShop_4M_DataAccess.Repository.IRepository;

namespace OnlineShop_4M_DataAccess.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext context;
        private DbSet<T> dbSet;

        public Repository(ApplicationDbContext context)
        {
            this.context = context;
            this.dbSet = context.Set<T>();
        }

        public void Add(T entity)
        {
            dbSet.Add(entity);
        }

        public T Find(int id)
        {
            return dbSet.Find(id);
        }

        public void Remove(T entity)
        {
            dbSet.Remove(entity);
        }

        public void Save()
        {
            context.SaveChanges();
        }


        public IEnumerable<T> GetAll(Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeProperties = null, bool isTracking = true)
        {
            // 

            IQueryable<T> query = dbSet;

            // есть ли фильтр
            if (filter != null)
            {
                // добавить ссылку на фильтр - параметр метода
                // + сформировать запрос
                query = query.Where(filter);
            }

            // свойства
            if (includeProperties != null)
            {
                foreach (var includeProp in includeProperties.Split(','))
                {
                    query = query.Include(includeProp);
                }
            }

            // сортировка
            if (orderBy != null)
            {
                query = orderBy(query);
            }

            if (!isTracking)
            {
                query = query.AsNoTracking();
            }

            return query.ToList();
        }


        public T FirstOrDefault(Expression<Func<T, bool>> filter = null, string includeProperties = null, bool isTracking = true)
        {

            IQueryable<T> query = dbSet;

            // есть ли фильтр
            if (filter != null)
            {
                // добавить ссылку на фильтр - параметр метода
                // + сформировать запрос
                query = query.Where(filter);
            }

            // свойства
            if (includeProperties != null)
            {
                foreach (var includeProp in includeProperties.Split(','))
                {
                    query = query.Include(includeProp);
                }
            }

            if (!isTracking)
            {
                query = query.AsNoTracking();
            }

            return query.FirstOrDefault();
        }

        public void RemoveRange(IEnumerable<T> entity)
        {
            dbSet.RemoveRange(entity);
        }
    }
}


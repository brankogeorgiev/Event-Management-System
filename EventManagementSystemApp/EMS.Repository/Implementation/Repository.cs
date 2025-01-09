using EMS.Domain.Models;
using EMS.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Repository.Implementation
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly ApplicationDbContext _context;
        private DbSet<T> entities;

        public Repository(ApplicationDbContext context)
        {
            _context = context;
            entities = context.Set<T>();
        }

        public IEnumerable<T> GetAll()
        {
            if (typeof(T) == typeof(ScheduledEvent))
            {
                return entities.Include("Event").AsEnumerable();
            }
            if (typeof(T) == typeof(Ticket))
            {
                return entities.Include("ScheduledEvent")
                    .Include("ScheduledEvent.Event").AsEnumerable();
            }
            return entities.AsEnumerable();
        }

        public T Get(Guid? id)
        {
            if (typeof(T) == typeof(ScheduledEvent))
            {
                return entities.Include("Event").FirstOrDefault(z => z.Id == id);
            }
            if (typeof(T) == typeof(Ticket))
            {
                return entities.Include("ScheduledEvent")
                    .Include("ScheduledEvent.Event").FirstOrDefault(z => z.Id == id);
            }
            return entities.SingleOrDefault(z => z.Id == id);
        }

        public T Insert(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Add(entity);
            _context.SaveChanges();
            return entity;
        }

        public List<T> InsertMany(List<T> entities)
        {
            if (entities == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.AddRange(entities);
            _context.SaveChanges();
            return entities;
        }

        public T Update(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Update(entity);
            _context.SaveChanges();
            return entity;
        }

        public T Delete(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Remove(entity);
            _context.SaveChanges();
            return entity;
        }
    }
}

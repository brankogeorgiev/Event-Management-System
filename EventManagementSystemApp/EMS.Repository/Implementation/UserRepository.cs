using EMS.Domain.Identity;
using EMS.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Repository.Implementation
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        private DbSet<Attendee> entities;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
            entities = _context.Set<Attendee>();
        }

        public void Delete(Attendee entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Remove(entity);
            _context.SaveChanges();
        }

        public Attendee Get(string id)
        {
            var strGuid = id.ToString();
            return entities
                .Include(z => z.ShoppingCart)
                .Include(z => z.ShoppingCart.TicketsInShoppingCart)
                .Include("ShoppingCart.TicketsInShoppingCart.Ticket")
                .Include("ShoppingCart.TicketsInShoppingCart.Ticket.ScheduledEvent")
                .Include("ShoppingCart.TicketsInShoppingCart.Ticket.ScheduledEvent.Event")
                .First(z => z.Id == strGuid);
        }

        public IEnumerable<Attendee> GetAll()
        {
            return entities.AsEnumerable();
        }

        public void Insert(Attendee entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Add(entity);
            _context.SaveChanges();
        }

        public void Update(Attendee entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Update(entity);
            _context.SaveChanges();
        }
    }
}

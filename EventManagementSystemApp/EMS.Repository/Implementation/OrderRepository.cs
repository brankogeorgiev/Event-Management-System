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
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<Order> entities;

        public OrderRepository(ApplicationDbContext context)
        {
            _context = context;
            entities = _context.Set<Order>();
        }

        public List<Order> GetAllOrders()
        {
            return entities
                .Include(z => z.Owner)
                .Include(z => z.TicketsInOrder)
                .Include("TicketsInOrder.Ticket")
                .Include("TicketsInOrder.Ticket.ScheduledEvent")
                .Include("TicketsInOrder.Ticket.ScheduledEvent.Event")
                .ToListAsync().Result;
        }

        public Order GetOrderDetails(BaseEntity model)
        {
            return entities
                .Include(z => z.Owner)
                .Include(z => z.TicketsInOrder)
                .Include("TicketsInOrder.Ticket")
                .Include("TicketsInOrder.Ticket.ScheduledEvent")
                .Include("TicketsInOrder.Ticket.ScheduledEvent.Event")
                .SingleOrDefaultAsync(z => z.Id == model.Id).Result;
        }
    }
}

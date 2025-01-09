using EMS.Domain.Models;
using EMS.Repository.Interface;
using EMS.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Service.Implementation
{
    public class TicketService : ITicketService
    {
        private readonly IRepository<Ticket> _ticketRepository;

        public TicketService(IRepository<Ticket> ticketRepository)
        {
            _ticketRepository = ticketRepository;
        }

        public Ticket CreateNewTicket(Ticket ticket)
        {
            return _ticketRepository.Insert(ticket);
        }

        public Ticket DeleteTicket(Guid? id)
        {
            return _ticketRepository.Delete(GetTicketById(id));
        }

        public void DeleteTickets(ICollection<Ticket> tickets)
        {
            foreach(var ticket in tickets)
            {
                _ticketRepository.Delete(ticket);
            }
        }

        public ICollection<Ticket> GetAllTickets()
        {
            return _ticketRepository.GetAll().ToList();
        }

        public ICollection<Ticket> GetAllTicketsByScheduledEventId(Guid? id)
        {
            return _ticketRepository.GetAll().Where(z => z.ScheduledEventId == id).ToList();
        }

        public Ticket GetTicketById(Guid? id)
        {
            return _ticketRepository.Get(id);
        }

        public Ticket UpdateTicket(Ticket ticket)
        {
            return _ticketRepository.Update(ticket);
        }
    }
}

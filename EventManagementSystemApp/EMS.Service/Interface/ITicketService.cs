using EMS.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Service.Interface
{
    public interface ITicketService
    {
        ICollection<Ticket> GetAllTickets();
        ICollection<Ticket> GetAllTicketsByScheduledEventId(Guid? id);
        void DeleteTickets(ICollection<Ticket> tickets);
        Ticket GetTicketById(Guid? id);
        Ticket CreateNewTicket(Ticket ticket);
        Ticket UpdateTicket(Ticket ticket);
        Ticket DeleteTicket(Guid? id);
    }
}

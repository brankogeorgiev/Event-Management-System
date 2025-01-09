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
    public class ScheduledEventService : IScheduledEventService
    {
        private readonly IRepository<ScheduledEvent> _scheduledEventRepository;
        private readonly IRepository<Event> _eventRepository;
        private readonly ITicketService _ticketService;


        public ScheduledEventService(IRepository<ScheduledEvent> scheduledEventRepository, 
            IRepository<Event> eventRepository,
            ITicketService ticketService)
        {
            _scheduledEventRepository = scheduledEventRepository;
            _eventRepository = eventRepository;
            _ticketService = ticketService;
        }

        public ScheduledEvent CreateNewScheduledEvent(ScheduledEvent scheduledEvent)
        {
            return _scheduledEventRepository.Insert(scheduledEvent);
        }

        public ScheduledEvent DeleteNewScheduledEvent(ScheduledEvent scheduledEvent)
        {
            ICollection<Ticket> tickets = _ticketService.GetAllTicketsByScheduledEventId(scheduledEvent.Id);
            _ticketService.DeleteTickets(tickets);
            return _scheduledEventRepository.Delete(GetScheduledEventById(scheduledEvent.Id));
        }

        public List<ScheduledEvent> GetAllScheduledEvents()
        {
            return _scheduledEventRepository.GetAll().ToList();
        }

        public List<ScheduledEvent> GetAllScheduledEventsByEventId(Guid? id)
        {
            return _scheduledEventRepository.GetAll().Where(z => z.EventId == id).ToList();
        }

        public ScheduledEvent UpdateNewScheduledEvent(ScheduledEvent scheduledEvent)
        {
            return _scheduledEventRepository.Update(scheduledEvent);
        }

        public ScheduledEvent GetScheduledEventById(Guid? id)
        {
            return _scheduledEventRepository.Get(id);
        }
    }
}
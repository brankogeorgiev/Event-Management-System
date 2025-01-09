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
    public class EventService : IEventService
    {
        private readonly IRepository<Event> _eventRepository;
        private readonly IScheduledEventService _scheduledEventService;

        public EventService(IRepository<Event> eventRepository, IScheduledEventService scheduledEventService)
        {
            _eventRepository = eventRepository;
            _scheduledEventService = scheduledEventService;
        }

        public Event CreateNewEvent(Event newEvent)
        {
            return _eventRepository.Insert(newEvent);
        }

        public Event DeleteEvent(Guid id)
        {
            List<ScheduledEvent> scheduledEvents = _scheduledEventService.GetAllScheduledEventsByEventId(id);
            foreach (var scheduledEvent in scheduledEvents)
            {
                _scheduledEventService.DeleteNewScheduledEvent(scheduledEvent);
            }
            return _eventRepository.Delete(GetEventById(id));
        }

        public List<Event> GetAllEvents()
        {
            return _eventRepository.GetAll().ToList();
        }

        public Event GetEventById(Guid? id)
        {
            return _eventRepository.Get(id);
        }

        public Event UpdateEvent(Event newEvent)
        {
            return _eventRepository.Update(newEvent);
        }
    }
}

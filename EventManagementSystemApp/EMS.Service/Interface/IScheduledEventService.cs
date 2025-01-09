using EMS.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Service.Interface
{
    public interface IScheduledEventService
    {
        List<ScheduledEvent> GetAllScheduledEventsByEventId(Guid? id);
        ScheduledEvent GetScheduledEventById(Guid? id);
        List<ScheduledEvent> GetAllScheduledEvents();
        ScheduledEvent CreateNewScheduledEvent(ScheduledEvent scheduledEvent);
        ScheduledEvent UpdateNewScheduledEvent(ScheduledEvent scheduledEvent);
        ScheduledEvent DeleteNewScheduledEvent(ScheduledEvent scheduledEvent);

    }
}

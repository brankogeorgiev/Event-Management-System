using EMS.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Service.Interface
{
    public interface IEventService
    {
        List<Event> GetAllEvents();
        Event GetEventById(Guid? id);
        Event CreateNewEvent(Event newEvent);
        Event UpdateEvent(Event newEvent);
        Event DeleteEvent(Guid id);

    }
}

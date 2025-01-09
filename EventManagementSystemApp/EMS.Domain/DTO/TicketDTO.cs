using EMS.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Domain.DTO
{
    public class TicketDTO
    {
        public double? TicketPrice { get; set; }
        public Guid? ScheduledEventId { get; set; }
        public List<ScheduledEvent> ListModel { get; set; }

    }
}

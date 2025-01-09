using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Domain.Models
{
    public class Ticket : BaseEntity
    {
        [Display(Name = "Ticket Price")]
        public double? TicketPrice { get; set; }
        public Guid? ScheduledEventId{ get; set; }
        [Display(Name = "Scheduled Event")]
        public ScheduledEvent? ScheduledEvent{ get; set; }
        public string TicketDisplayString => $"{ScheduledEvent?.Event?.EventName}: {ScheduledEvent?.ScheduledEventLocation} - {ScheduledEvent?.ScheduledEventDateTime.Value.ToString("MM/dd/yy, HH:mm")}";

    }
}

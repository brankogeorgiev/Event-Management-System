using EMS.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Domain.DTO
{
    public class ScheduledEventDTO
    {
        public DateTime? ScheduledEventDateTime { get; set; }
        public string? ScheduledEventLocation { get; set; }
        public string? ScheduledEventImage { get; set; }
        public Guid EventId { get; set; }
        public List<Event>? ListModel { get; set; }
    }
}

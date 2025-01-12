using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManagementSystemAdminApp.Models
{
    public class ScheduledEvent
    {
        [Display(Name = "Date and Time")]
        public DateTime? ScheduledEventDateTime { get; set; }
        [Display(Name = "Location")]
        public string? ScheduledEventLocation { get; set; }
        [Display(Name = "Image Url")]
        public string? ScheduledEventImage { get; set; }
        public Guid? EventId { get; set; }
        public Event? Event { get; set; }

        public string EventNameDisplay => Event != null ? Event.EventName + ": " + ScheduledEventLocation + " - " + ScheduledEventDateTime.Value.ToString("MM/dd/yy, HH:mm"): string.Empty;
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManagementSystemAdminApp.Models
{
    public class Event
    {
        [Display(Name = "Event Name")]
        public string? EventName { get; set; }
        [Display(Name = "Event Description")]
        public string? EventDescription { get; set; }
        [Display(Name = "Event Image Url")]
        public string? EventImage { get; set; }
    }
}

using EMS.Domain.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Domain.Models
{
    public class Order : BaseEntity
    {
        public string? OwnerId { get; set; }
        public Attendee? Owner { get; set; }
        public ICollection<TicketInOrder>? TicketsInOrder { get; set; }
    }
}

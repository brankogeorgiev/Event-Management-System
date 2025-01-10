using EMS.Domain.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Domain.Models
{
    public class ShoppingCart : BaseEntity
    {
        public string? OwnerId { get; set; }
        public Attendee? Owner { get; set; }
        public virtual ICollection<TicketInShoppingCart>? TicketsInShoppingCart { get; set; }
    }
}

using EMS.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Domain.PartnerModels
{
    public class Author : BaseEntity
    {
        public string? name { get; set; }
        public string? surname { get; set; }
        public DateOnly? yearBorn{ get; set; }
        public List<Book>? books { get; set; }
    }
}

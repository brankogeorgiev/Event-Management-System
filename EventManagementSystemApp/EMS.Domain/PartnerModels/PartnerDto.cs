using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Domain.PartnerModels
{
    public class PartnerDto
    {
        public List<Book>? AllBooks { get; set; }
        public List<Author>? AllAuthors { get; set; }
    }
}

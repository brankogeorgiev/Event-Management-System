using EMS.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Domain.PartnerModels
{
    public class Book : BaseEntity
    {
        public string? title { get; set; }
        public string? description { get; set; }
        public double? price { get; set; }
        public double? rating { get; set; }
        public string? genre { get; set; }
        public string? image { get; set; }
        public Guid? authorId { get; set; }
        public Author? author { get; set; }
        public List<object>? bookPublishers { get; set; }
    }
}

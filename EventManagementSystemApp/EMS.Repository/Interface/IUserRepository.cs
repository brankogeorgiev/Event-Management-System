using EMS.Domain.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Repository.Interface
{
    public interface IUserRepository
    {
        IEnumerable<Attendee> GetAll();
        Attendee Get(string id);
        void Insert(Attendee entity);
        void Update(Attendee entity);
        void Delete(Attendee entity);
    }
}

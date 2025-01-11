using EMS.Domain.DTO;
using EMS.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Service.Interface
{
    public interface IShoppingCartService
    {
        ShoppingCartDTO GetShoppingCartDetails(string userId);
        AddToShoppingCartDTO GetTicketInfo(Guid id);
        ShoppingCart AddTicketToShoppingCart(string userId, AddToShoppingCartDTO model);
        bool DeleteFromShoppingCart(string userId, Guid? id);
        bool OrderTickets(string userId);
    }
}

using EMS.Domain.DTO;
using EMS.Domain.Models;
using EMS.Repository.Interface;
using EMS.Service.Interface;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Service.Implementation
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRepository<Ticket> _ticketRepository;
        private readonly IRepository<ShoppingCart> _shoppingCartRepository;

        public ShoppingCartService(IUserRepository userRepository, 
            IRepository<Ticket> ticketRepository, 
            IRepository<ShoppingCart> shoppingCartRepository)
        {
            _userRepository = userRepository;
            _ticketRepository = ticketRepository;
            _shoppingCartRepository = shoppingCartRepository;
        }

        public ShoppingCart AddTicketToShoppingCart(string userId, AddToShoppingCartDTO model)
        {
            if (userId != null)
            {
                var loggedInUser = _userRepository.Get(userId);
                var userCart = loggedInUser?.ShoppingCart;
                var selectedTicket = _ticketRepository.Get(model.SelectedTicketId);
                if (selectedTicket != null && userCart != null)
                {
                    var existingTicket = userCart.TicketsInShoppingCart?.
                        FirstOrDefault(z => z.TicketId == selectedTicket.Id);

                    if (existingTicket != null)
                    {
                        existingTicket.Quantity += model.Quantity;
                    }

                    else
                    {
                        userCart?.TicketsInShoppingCart?.Add(new TicketInShoppingCart
                        {
                            Ticket = selectedTicket,
                            TicketId = selectedTicket.Id,
                            ShoppingCart = userCart,
                            ShoppingCartId = userCart.Id,
                            Quantity = model.Quantity
                        });

                    }
                    return _shoppingCartRepository.Update(userCart);
                }
            }

            return null;
        }

        public bool DeleteFromShoppingCart(string userId, Guid? id)
        {
            if (userId != null)
            {
                var loggedInUser = _userRepository.Get(userId);
                var ticket_to_delete = loggedInUser?.ShoppingCart?.TicketsInShoppingCart.First(z => z.TicketId == id);
                loggedInUser?.ShoppingCart?.TicketsInShoppingCart?.Remove(ticket_to_delete);
                _shoppingCartRepository.Update(loggedInUser.ShoppingCart);

                return true;
            }
            return false;
        }

        public ShoppingCartDTO GetShoppingCartDetails(string userId)
        {
            if (userId != null && !userId.IsNullOrEmpty())
            {
                var loggedInUser = _userRepository.Get(userId);
                var allTickets = loggedInUser?.ShoppingCart?.TicketsInShoppingCart?.ToList();
                var totalPrice = allTickets.Select(z => (z.Ticket.TicketPrice * z.Quantity)).Sum();

                var model = new ShoppingCartDTO
                {
                    AllTickets = allTickets,
                    TotalPrice = (double)totalPrice
                };

                return model;
            }

            return new ShoppingCartDTO
            {
                AllTickets = new List<TicketInShoppingCart>(),
                TotalPrice = 0.0
            };
        }

        public AddToShoppingCartDTO GetTicketInfo(Guid id)
        {
            var selectedTicket = _ticketRepository.Get(id);
            if (selectedTicket != null)
            {
                var model = new AddToShoppingCartDTO
                {
                    SelectedTicketName = selectedTicket.ScheduledEvent.EventNameDisplay,
                    SelectedTicketId = selectedTicket.Id,
                    Quantity = 1
                };
                return model;
            }
            return null;
        }
    }
}

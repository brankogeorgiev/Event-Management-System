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
        private readonly IRepository<Order> _orderRepository;
        private readonly IRepository<TicketInOrder> _ticketInOrderRepository;
        private readonly IEmailService _emailService;

        public ShoppingCartService(IUserRepository userRepository, 
            IRepository<Ticket> ticketRepository,
            IRepository<ShoppingCart> shoppingCartRepository,
            IRepository<Order> orderRepository,
            IRepository<TicketInOrder> ticketInOrderRepository, 
            IEmailService emailService)
        {
            _userRepository = userRepository;
            _ticketRepository = ticketRepository;
            _shoppingCartRepository = shoppingCartRepository;
            _orderRepository = orderRepository;
            _ticketInOrderRepository = ticketInOrderRepository;
            _emailService = emailService;
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

        public bool OrderTickets(string userId)
        {
            if (userId != null  && !userId.IsNullOrEmpty())
            {
                var loggedInUser = _userRepository.Get(userId);
                var userCart = loggedInUser?.ShoppingCart;

                EmailMessage message = new EmailMessage();
                message.Subject = "Successfull order";
                message.MailTo = loggedInUser.Email;

                var userOrder = new Order
                {
                    Id = Guid.NewGuid(),
                    OwnerId = userId,
                    Owner = loggedInUser
                };
                _orderRepository.Insert(userOrder);
                
                var ticketsInOrder = userCart?.TicketsInShoppingCart.Select(z => new TicketInOrder
                {
                    Order = userOrder,
                    OrderId = userOrder.Id,
                    TicketId = z.TicketId,
                    Ticket = z.Ticket,
                    Quantity = z.Quantity
                });

                StringBuilder sb = new StringBuilder();
                var totalPrice = 0.0;
                sb.AppendLine("Your order is completed. The order contains: ");

                for (int i = 1; i <= ticketsInOrder.Count(); i++) 
                {
                    var currentItem = ticketsInOrder.ElementAt(i-1);
                    totalPrice += currentItem.Quantity * currentItem?.Ticket?.TicketPrice ?? 0;
                    sb.AppendLine(i.ToString() + ". " + currentItem.Ticket.TicketDisplayString + " with quantity of: " + currentItem.Quantity + ", and price of: " + currentItem.Ticket.TicketPrice + " MKD");
                }
                sb.AppendLine("Total price for your order: " + totalPrice.ToString());
                message.Content = sb.ToString();

                foreach (var ticket in ticketsInOrder)
                {
                    _ticketInOrderRepository.Insert(ticket);
                }

                userCart?.TicketsInShoppingCart.Clear();
                _shoppingCartRepository.Update(userCart);

                _emailService.SendEmailAsync(message);
                return true;
            }
            return false;
        }
    }
}

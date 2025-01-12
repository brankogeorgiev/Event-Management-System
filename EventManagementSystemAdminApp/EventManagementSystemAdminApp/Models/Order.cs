namespace EventManagementSystemAdminApp.Models
{
    public class Order
    {
        public Guid Id { get; set; }
        public string? OwnerId { get; set; }
        public Attendee? Owner { get; set; }
        public ICollection<TicketInOrder>? TicketsInOrder { get; set; }
    }
}

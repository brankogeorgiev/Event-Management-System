using EMS.Domain.Identity;
using EMS.Domain.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EMS.Repository
{
    public class ApplicationDbContext : IdentityDbContext<Attendee>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public virtual DbSet<Ticket> Tickets { get; set; }
        public virtual DbSet<Event> Events { get; set; }
        public virtual DbSet<ScheduledEvent> ScheduledEvents { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ScheduledEvent>()
                .HasOne(z => z.Event)
                .WithMany(z => z.ScheduledEvents)
                .HasForeignKey(z => z.EventId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

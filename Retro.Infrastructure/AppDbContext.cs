using Microsoft.EntityFrameworkCore;
using Retro.Domain;

namespace Retro.Infrastructure
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Conversation> Conversations { get; set; }
        public DbSet<ConversationParticipant> ConversationParticipants { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.Id);
                entity.Property(u => u.Username).IsRequired().HasMaxLength(50);
                entity.Property(u => u.Email).IsRequired().HasMaxLength(100);
                entity.Property(u => u.PasswordHash).IsRequired();
                entity.Property(u => u.CreatedAt).IsRequired();
            });
            
            modelBuilder.Entity<ConversationParticipant>()
                .HasKey(cp => new { cp.UserId, cp.ConversationId });

            modelBuilder.Entity<ConversationParticipant>()
                .HasOne(cp => cp.User)
                .WithMany(u => u.ConversationParticipants)
                .HasForeignKey(cp => cp.UserId);

            modelBuilder.Entity<ConversationParticipant>()
                .HasOne(cp => cp.Conversation)
                .WithMany(c => c.Participants)
                .HasForeignKey(cp => cp.ConversationId);
            
            // modelBuilder.Entity<ConversationParticipant>()
            //     .HasOne(cp => cp.User)
            //     .WithMany(u => u.ConversationParticipants)
            //     .HasForeignKey(cp => cp.UserId);
            //
            // modelBuilder.Entity<ConversationParticipant>()
            //     .HasKey(cp => new { cp.ConversationId, cp.UserId });
            //
            // modelBuilder.Entity<ConversationParticipant>()
            //     .HasOne(cp => cp.User)
            //     .WithMany()
            //     .HasForeignKey(cp => cp.UserId);
            //
            // modelBuilder.Entity<ConversationParticipant>()
            //     .HasOne(cp => cp.Conversation)
            //     .WithMany(c => c.Participants)
            //     .HasForeignKey(cp => cp.ConversationId);
        }
    }
}

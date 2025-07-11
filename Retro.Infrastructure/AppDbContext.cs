using Microsoft.EntityFrameworkCore;
using Retro.Domain.Entities;

namespace Retro.Infrastructure;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Conversation> Conversations { get; set; }
    public DbSet<ConversationParticipant> ConversationParticipants { get; set; }

    public DbSet<Message> Messages { get; set; }

    public DbSet<MessageReaction> MessageReactions { get; set; }
    public DbSet<EmojiReaction> EmojiReactions { get; set; }
    public DbSet<MessageStatus> MessageStatuses { get; set; }

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


        modelBuilder.Entity<MessageReaction>(entity =>
        {
            entity.HasKey(r => r.Id);
            entity.HasIndex(r => new { r.MessageId, r.UserId, r.EmojiReactionId })
                .IsUnique();

            entity.HasOne(r => r.Message)
                .WithMany(m => m.Reactions)
                .HasForeignKey(r => r.MessageId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(r => r.User)
                .WithMany()
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(r => r.EmojiReaction)
                .WithMany(rt => rt.MessageReactions)
                .HasForeignKey(r => r.EmojiReactionId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent accidental deletion
        });

        modelBuilder.Entity<EmojiReaction>(entity =>
        {
            entity.HasKey(rt => rt.Id);
            entity.HasIndex(rt => rt.Name).IsUnique(); // enforce unique names
        });


        modelBuilder.Entity<EmojiReaction>().HasData(
            new EmojiReaction { Id = 1, Name = "Like", Emoji = "👍" },
            new EmojiReaction { Id = 2, Name = "Love", Emoji = "❤️" },
            new EmojiReaction { Id = 3, Name = "Laugh", Emoji = "😂" },
            new EmojiReaction { Id = 4, Name = "Surprised", Emoji = "😮" },
            new EmojiReaction { Id = 5, Name = "Angry", Emoji = "😡" }
        );


        modelBuilder.Entity<MessageStatus>()
            .HasKey(ms => new { ms.MessageId, ms.UserId });

        modelBuilder.Entity<MessageStatus>()
            .HasOne(ms => ms.Message)
            .WithMany(m => m.MessageStatuses)
            .HasForeignKey(ms => ms.MessageId);

        modelBuilder.Entity<MessageStatus>()
            .HasOne(ms => ms.User)
            .WithMany()
            .HasForeignKey(ms => ms.UserId);
    }
}
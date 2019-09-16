using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OmniaDelivery.NotificationService.Models
{
    public class NotificationContext : DbContext
    {


        public NotificationContext(DbContextOptions<NotificationContext> options)
            : base(options)
        { }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Fluent API

            //modelBuilder.Entity<SettingsDataModel>().HasIndex(a => a.Name);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=LTP00056;Database=KLA0000;Trusted_Connection=True;");
        }


        public DbSet<Notification> Notifications { get; set; }
        public DbSet<NotificationType> NotificationTypes { get; set; }
    }

    [Table("NotificationType", Schema = "notification")]
    public class NotificationType
    {
        [Key]
        public int NotificationTypeId { get; set; }
        public string Description { get; set; }

        public ICollection<Notification> Notifications { get; set; }
    }

    [Table("Notification", Schema = "notification")]
    public class Notification
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int NotificationId { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }

        public DateTime Created {get;set;}
        public int NotificationTypeId { get; set; }
        public NotificationType Blog { get; set; }
    }
}
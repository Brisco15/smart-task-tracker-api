using Microsoft.EntityFrameworkCore;
using SmartTaskTracker.API.Models;

namespace SmartTaskTracker.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<TaskItem> Tasks { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Priority> Priorities { get; set; }
        public DbSet<Status> Statuses { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<TimeTracking> TimeTrackings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // TaskItem Configuration
            modelBuilder.Entity<TaskItem>(entity =>
            {
                entity.HasKey(e => e.TaskID);

                entity.HasOne(e => e.AssignedUser)
                    .WithMany(u => u.Tasks)
                    .HasForeignKey(e => e.AssignedTo)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Project)
                    .WithMany(p => p.Tasks)
                    .HasForeignKey(e => e.ProjectID)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Status)
                    .WithMany(s => s.Tasks)
                    .HasForeignKey(e => e.StatusID)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Priority)
                    .WithMany(p => p.Tasks)
                    .HasForeignKey(e => e.PriorityID)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.CreatedByUser)
                    .WithMany()
                    .HasForeignKey(e => e.CreatedBy)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.ModifiedByUser)
                    .WithMany()
                    .HasForeignKey(e => e.ModifiedBy)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.DeletedByUser)
                    .WithMany()
                    .HasForeignKey(e => e.DeletedBy)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.ArchivedByUser)
                    .WithMany()
                    .HasForeignKey(e => e.ArchivedBy)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // User Configuration
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.UserID);

                entity.HasOne(e => e.Role)
                    .WithMany(r => r.Users)
                    .HasForeignKey(e => e.RoleID)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.ModifiedByUser)
                    .WithMany()
                    .HasForeignKey(e => e.ModifiedBy)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.DeletedByUser)
                    .WithMany()
                    .HasForeignKey(e => e.DeletedBy)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.ArchivedByUser)
                    .WithMany()
                    .HasForeignKey(e => e.ArchivedBy)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Project Configuration
            modelBuilder.Entity<Project>(entity =>
            {
                entity.HasKey(e => e.ProjectID);

                entity.HasOne(e => e.CreatedByUser)
                    .WithMany()
                    .HasForeignKey(e => e.CreatedBy)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.ModifiedByUser)
                    .WithMany()
                    .HasForeignKey(e => e.ModifiedBy)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.DeletedByUser)
                    .WithMany()
                    .HasForeignKey(e => e.DeletedBy)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.ArchivedByUser)
                    .WithMany()
                    .HasForeignKey(e => e.ArchivedBy)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // TimeTracking Configuration
            modelBuilder.Entity<TimeTracking>(entity =>
            {
                entity.HasKey(e => e.TimeTrackingID); // ?? Name aktualisiert
    
                entity.HasOne(e => e.Task)
                    .WithMany()
                    .HasForeignKey(e => e.TaskID)
                    .OnDelete(DeleteBehavior.Cascade);
    
                entity.HasOne(e => e.User)
                    .WithMany()
                    .HasForeignKey(e => e.UserID)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Priority Configuration
            modelBuilder.Entity<Priority>(entity =>
            {
                entity.HasKey(e => e.PriorityID);
            });

            // Status Configuration
            modelBuilder.Entity<Status>(entity =>
            {
                entity.HasKey(e => e.StatusID);
            });

            // Role Configuration
            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasKey(e => e.RoleID);
            });

            // SEED DATA
            modelBuilder.Entity<Role>().HasData(
                new Role { RoleID = 1, RoleName = "Admin" },
                new Role { RoleID = 2, RoleName = "Developer" }
            );

            modelBuilder.Entity<Status>().HasData(
                new Status { StatusID = 1, StatusName = "Not Started" },
                new Status { StatusID = 2, StatusName = "In Progress" },
                new Status { StatusID = 3, StatusName = "Completed" }
            );

            modelBuilder.Entity<Priority>().HasData(
                new Priority { PriorityID = 1, PriorityName = "Low" },
                new Priority { PriorityID = 2, PriorityName = "Medium" },
                new Priority { PriorityID = 3, PriorityName = "High" }
            );
        }
    }
}
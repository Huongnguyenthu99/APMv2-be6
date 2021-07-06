using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace APMv2.Model.Entities
{
    public partial class APMv2Context : DbContext
    {
        public APMv2Context()
        {
        }

        public APMv2Context(DbContextOptions<APMv2Context> options)
            : base(options)
        {
        }

        public virtual DbSet<Backlog> Backlog { get; set; }
        public virtual DbSet<Project> Project { get; set; }
        public virtual DbSet<ProjectUser> ProjectUser { get; set; }
        public virtual DbSet<Sprint> Sprint { get; set; }
        public virtual DbSet<SprintBacklog> SprintBacklog { get; set; }
        public virtual DbSet<SprintTarget> SprintTarget { get; set; }
        public virtual DbSet<TaskDetail> TaskDetail { get; set; }
        public virtual DbSet<Tasks> Tasks { get; set; }
        public virtual DbSet<TimeWorkingOff> TimeWorkingOff { get; set; }
        public virtual DbSet<User> User { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=DESKTOP-V39QQ74\\SQLEXPRESS01;Initial Catalog=APMv2;Integrated Security=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Backlog>(entity =>
            {
                entity.Property(e => e.Description).HasMaxLength(50);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Priority).HasMaxLength(5);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Backlog)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_Backlog_User");
            });

            modelBuilder.Entity<Project>(entity =>
            {
                entity.Property(e => e.Description).HasMaxLength(50);

                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<ProjectUser>(entity =>
            {
                entity.HasOne(d => d.Project)
                    .WithMany(p => p.ProjectUser)
                    .HasForeignKey(d => d.ProjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProjectUser_Project");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.ProjectUser)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProjectUser_User");
            });

            modelBuilder.Entity<Sprint>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.TimeEnd).HasMaxLength(10);

                entity.Property(e => e.TimeStart).HasMaxLength(10);

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.Sprint)
                    .HasForeignKey(d => d.ProjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Sprint_Project");
            });

            modelBuilder.Entity<SprintBacklog>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Priority).HasMaxLength(5);

                entity.HasOne(d => d.Backlog)
                    .WithMany(p => p.SprintBacklog)
                    .HasForeignKey(d => d.BacklogId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SprintBacklog_Backlog");

                entity.HasOne(d => d.Sprint)
                    .WithMany(p => p.SprintBacklog)
                    .HasForeignKey(d => d.SprintId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SprintBacklog_Sprint");
            });

            modelBuilder.Entity<SprintTarget>(entity =>
            {
                entity.Property(e => e.Name).HasMaxLength(50);

                entity.HasOne(d => d.Sprint)
                    .WithMany(p => p.SprintTarget)
                    .HasForeignKey(d => d.SprintId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SprintTarget_Sprint");
            });

            modelBuilder.Entity<TaskDetail>(entity =>
            {
                entity.Property(e => e.Date).HasColumnType("date");

                entity.HasOne(d => d.Task)
                    .WithMany(p => p.TaskDetail)
                    .HasForeignKey(d => d.TaskId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TaskDetail_Tasks");
            });

            modelBuilder.Entity<Tasks>(entity =>
            {
                entity.Property(e => e.Description).HasMaxLength(50);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Note).HasMaxLength(50);

                entity.Property(e => e.Priority)
                    .IsRequired()
                    .HasMaxLength(5);

                entity.HasOne(d => d.SprintBacklog)
                    .WithMany(p => p.Tasks)
                    .HasForeignKey(d => d.SprintBacklogId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Tasks_SprintBacklog");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Tasks)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_Tasks_User");
            });

            modelBuilder.Entity<TimeWorkingOff>(entity =>
            {
                entity.Property(e => e.DayOff).HasMaxLength(50);

                entity.HasOne(d => d.Sprint)
                    .WithMany(p => p.TimeWorkingOff)
                    .HasForeignKey(d => d.SprintId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TimeWorkingOff_Sprint");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.TimeWorkingOff)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TimeWorkingOff_User");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Dob).HasMaxLength(20);

                entity.Property(e => e.Email).HasMaxLength(50);

                entity.Property(e => e.FullName).HasMaxLength(50);

                entity.Property(e => e.Password).HasMaxLength(50);

                entity.Property(e => e.Username).HasMaxLength(50);
            });
        }
    }
}

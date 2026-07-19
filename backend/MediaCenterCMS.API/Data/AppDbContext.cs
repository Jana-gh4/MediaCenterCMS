using MediaCenterCMS.API.Models;
using Microsoft.EntityFrameworkCore;

namespace MediaCenterCMS.API.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Role> Roles => Set<Role>();

    public DbSet<User> Users => Set<User>();

    public DbSet<News> News => Set<News>();

    public DbSet<NewsVersion> NewsVersions => Set<NewsVersion>();

    public DbSet<ApprovalRequest> ApprovalRequests => Set<ApprovalRequest>();

    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();

    public DbSet<Media> Media => Set<Media>();

    public DbSet<GalleryImage> GalleryImages => Set<GalleryImage>();
    public DbSet<GalleryVideo> GalleryVideos => Set<GalleryVideo>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        //Relationships
        modelBuilder.Entity<Role>()
            .HasMany(r => r.Users)
            .WithOne(u => u.Role)
            .HasForeignKey(u => u.RoleId)
            .OnDelete(DeleteBehavior.Restrict);

        //Constraints
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Username)
            .IsUnique();

        // News Creator    
        modelBuilder.Entity<News>()
            .HasOne(n => n.Creator)
            .WithMany(u => u.CreatedNews)
            .HasForeignKey(n => n.CreatedBy)
            .OnDelete(DeleteBehavior.Restrict);

        // News Updater
        modelBuilder.Entity<News>()
            .HasOne(n => n.Updater)
            .WithMany(u => u.UpdatedNews)
            .HasForeignKey(n => n.UpdatedBy)
            .OnDelete(DeleteBehavior.Restrict);

        // News CurrentVersion
        modelBuilder.Entity<News>()
            .HasOne(n => n.CurrentVersion)
            .WithOne()
            .HasForeignKey<News>(n => n.CurrentVersionId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Restrict);

       // NewsVersion News
       modelBuilder.Entity<NewsVersion>()
            .HasOne(v => v.News)
            .WithMany(n => n.Versions)
            .HasForeignKey(v => v.NewsId)
            .OnDelete(DeleteBehavior.Cascade);

        // NewsVersion Creator
        modelBuilder.Entity<NewsVersion>()
            .HasOne(v => v.Creator)
            .WithMany(u => u.CreatedVersions)
            .HasForeignKey(v => v.CreatedBy)
            .OnDelete(DeleteBehavior.Restrict);

        // NewsVersion ApprovalRequest
        modelBuilder.Entity<NewsVersion>()
            .HasOne(v => v.ApprovalRequest)
            .WithMany()
            .HasForeignKey(v => v.ApprovalRequestId)
            .OnDelete(DeleteBehavior.Restrict);

        // ApprovalRequest Requester
        modelBuilder.Entity<ApprovalRequest>()
            .HasOne(a => a.Requester)
            .WithMany(u => u.RequestedApprovals)
            .HasForeignKey(a => a.RequestedBy)
            .OnDelete(DeleteBehavior.Restrict);

        // ApprovalRequest Reviewer
        modelBuilder.Entity<ApprovalRequest>()
            .HasOne(a => a.Reviewer)
            .WithMany(u => u.ReviewedApprovals)
            .HasForeignKey(a => a.ReviewedBy)
            .OnDelete(DeleteBehavior.Restrict);

        // AuditLog User
        modelBuilder.Entity<AuditLog>()
            .HasOne(a => a.User)
            .WithMany(u => u.AuditLogs)
            .HasForeignKey(a => a.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        //Seed Data
        modelBuilder.Entity<Role>().HasData(
            new Role
            {
                RoleId = 1,
                Name = "Admin"
            },
            new Role
            {
                RoleId = 2,
                Name = "Editor"
            });

        // Media Uploader
        modelBuilder.Entity<Media>()
            .HasOne(m => m.Uploader)
            .WithMany(u => u.UploadedMedia)
            .HasForeignKey(m => m.UploadedBy)
            .OnDelete(DeleteBehavior.Restrict);

        // NewsVersion Cover Media
        modelBuilder.Entity<NewsVersion>()
            .HasOne(v => v.CoverMedia)
            .WithMany()
            .HasForeignKey(v => v.CoverMediaId)
            .OnDelete(DeleteBehavior.SetNull);
            
            // GalleryImage Media
            modelBuilder.Entity<GalleryImage>()
                .HasOne(g => g.Media)
                .WithMany()
                .HasForeignKey(g => g.MediaId)
                .OnDelete(DeleteBehavior.Restrict);

            // GalleryImage ApprovalRequest
            modelBuilder.Entity<GalleryImage>()
                .HasOne(g => g.ApprovalRequest)
                .WithMany()
                .HasForeignKey(g => g.ApprovalRequestId)
                .OnDelete(DeleteBehavior.Restrict);

            // GalleryImage Creator
            modelBuilder.Entity<GalleryImage>()
                .HasOne(g => g.Creator)
                .WithMany(u => u.GalleryImages)
                .HasForeignKey(g => g.CreatedBy)
                .OnDelete(DeleteBehavior.Restrict);

            // GalleryVideo Media
        modelBuilder.Entity<GalleryVideo>()
            .HasOne(g => g.Media)
            .WithMany()
            .HasForeignKey(g => g.MediaId)
            .OnDelete(DeleteBehavior.Restrict);

        // GalleryVideo ApprovalRequest
        modelBuilder.Entity<GalleryVideo>()
            .HasOne(g => g.ApprovalRequest)
            .WithMany()
            .HasForeignKey(g => g.ApprovalRequestId)
            .OnDelete(DeleteBehavior.Restrict);

        // GalleryVideo Creator
        modelBuilder.Entity<GalleryVideo>()
            .HasOne(g => g.Creator)
            .WithMany(u => u.GalleryVideos)
            .HasForeignKey(g => g.CreatedBy)
            .OnDelete(DeleteBehavior.Restrict);
            }

}
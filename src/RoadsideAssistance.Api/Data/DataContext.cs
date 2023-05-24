using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RoadsideAssistance.Api.Data.DomianEntities;

namespace RoadsideAssistance.Api.Data
{
    public class DataContext :DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            
        }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Assistant> Assistants { get; set; }
        public virtual DbSet<CustomerAssistant> CustomerAssistants { get; set; }
        public virtual DbSet<GeoLocation> GeoLocations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>(entity => 
            {
                entity.HasKey(e=> e.Id).HasName("PK_Customer_Id");
                entity.Property(c=> c.Id).ValueGeneratedOnAdd();
                entity.Property(e=> e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e=> e.VehicleVINNumber).IsRequired().HasMaxLength(17);
                entity.Property(e=> e.VehicleMakeModel).IsRequired().HasMaxLength(50);
            });
            modelBuilder.Entity<GeoLocation>(entity =>
            {
                entity.HasKey(e=> e.Id).HasName("PK_GeoLocation_Id");
                entity.Property(e=> e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.Latitude).IsRequired();
                entity.Property(e => e.Longitude).IsRequired();
                entity.Property(e=> e.Address).IsRequired().HasMaxLength(250);

                entity.HasIndex(e => new { e.Longitude, e.Latitude })
                .IsUnique()
                .HasDatabaseName("UC_GeoLocation_Longitude_Latitude");
              
            });

            modelBuilder.Entity<Assistant>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK_Assistant_Id");
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.Description).IsRequired().HasMaxLength(100);
                entity.Property(e => e.IsReserved).IsRequired();
                entity.Property(e => e.CurrentGeoLocationId).IsRequired();
             
                entity.Property(p => p.IsReserved).IsConcurrencyToken();

                entity.HasOne(e => e.GeoLocation)
                .WithMany(a => a.Assistants)
                .HasForeignKey(e => e.CurrentGeoLocationId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Assistant_GeoLocation_CurrentGeoLocationId");
            });
            modelBuilder.Entity<CustomerAssistant>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK_CustomerAssistant_Id");
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.CustomerId).IsRequired();
                entity.Property(e => e.AssistantId).IsRequired();
                entity.Property(e => e.GeoLocationId).IsRequired();
                entity.Property(e => e.ServiceStartDate).IsRequired();
                entity.Property(e => e.ServiceCompleteDate);
                entity.Property(e => e.Status).IsRequired().HasMaxLength(50);

                entity.HasOne(e => e.Customer)
               .WithMany(a => a.CustomerAssistants)
               .HasForeignKey(e => e.CustomerId)
               .OnDelete(DeleteBehavior.Restrict)
               .HasConstraintName("FK_CustomerAssistant_Customer_CustomerId");

                entity.HasOne(e => e.Assistant)
               .WithMany(a => a.CustomerAssistants)
               .HasForeignKey(e => e.AssistantId)
               .OnDelete(DeleteBehavior.Restrict)
               .HasConstraintName("FK_CustomerAssistant_Assistant_AssistantId");

                entity.HasOne(e => e.GeoLocation)
                .WithMany(a => a.CustomerAssistants)
                .HasForeignKey(e => e.GeoLocationId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_CustomerAssistant_GeoLocation_GeoLocationId");
            });
           
        }

    }
}

using Microsoft.EntityFrameworkCore;
using VoguMap.Domain.Entities;

namespace VoguMap.Infrastructure.Data.Context;

public partial class VoguMapContext : DbContext
{
    public VoguMapContext(DbContextOptions<VoguMapContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Building> Buildings { get; set; }

    public virtual DbSet<Room> Rooms { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Building>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("buildings_pkey");

            entity.ToTable("buildings", tb => tb.HasComment("Учебные корпуса"));

            entity.Property(e => e.Id).HasComment("Идентификатор");
            entity.Property(e => e.Address).HasComment("Полный адрес");
            entity.Property(e => e.Latitude).HasComment("Широта");
            entity.Property(e => e.Longitude).HasComment("Долгота");
            entity.Property(e => e.Name).HasComment("Название");
        });

        modelBuilder.Entity<Room>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("rooms_pkey");

            entity.ToTable("rooms", tb => tb.HasComment("Помещения"));

            entity.Property(e => e.Id).HasComment("Идентификатор");
            entity.Property(e => e.BuildingId).HasComment("Корпус");
            entity.Property(e => e.Description).HasComment("Описание");
            entity.Property(e => e.Floor).HasComment("Этаж");
            entity.Property(e => e.Name).HasComment("Название");

            entity.HasOne(d => d.Building).WithMany(p => p.Rooms)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("rooms_building_id_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

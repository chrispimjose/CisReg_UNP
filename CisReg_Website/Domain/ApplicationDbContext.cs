using CisReg_Website.Models;
using Microsoft.EntityFrameworkCore;
using MongoDB.EntityFrameworkCore.Extensions;

namespace CisReg_Website.Domain;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<UserModel> Users { get; set; }
    public DbSet<CombinedInfoModel> Professional { get; set; }
    public DbSet<FormationModel> Formations { get; set; }
    public DbSet<SpecialtyModel> Specialties { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseMongoDB("mongodb+srv://root:admin@cisreg.kzr70.mongodb.net/?retryWrites=true&w=majority&appName=CisReg", "cisreg");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      base.OnModelCreating(modelBuilder);

      modelBuilder.Entity<UserModel>().ToCollection("users");
      modelBuilder.Entity<CombinedInfoModel>().ToCollection("profissional");
      modelBuilder.Entity<FormationModel>().ToCollection("formação");
      modelBuilder.Entity<SpecialtyModel>().ToCollection("especialidade");
    }
}
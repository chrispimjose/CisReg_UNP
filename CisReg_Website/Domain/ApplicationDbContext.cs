using CisReg_Website.Models;
using CisReg_Website.Models.Vacancy;
using Microsoft.EntityFrameworkCore;
using MongoDB.EntityFrameworkCore.Extensions;

namespace CisReg_Website.Domain;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    required public DbSet<Professional> Professionals { get; set; }
    required public DbSet<Patient> Patients { get; set; }
    required public DbSet<HallModel> Halls { get; set; }
    required public DbSet<VacancyModel> Vacancies { get; set; }
    required public DbSet<FormationModel> Formations { get; set; }
    required public DbSet<SpecialtyModel> Specialties { get; set; }
    required public DbSet<Admin> Admins { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseMongoDB("mongodb+srv://Kaion:kaionmurilo123@cisregdb.lagqc.mongodb.net/?retryWrites=true&w=majority&appName=Cisregdb", "CisregDB");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Professional>().ToCollection("users");
        modelBuilder.Entity<Patient>().ToCollection("users");
        modelBuilder.Entity<Admin>().ToCollection("users");
        modelBuilder.Entity<HallModel>().ToCollection("hall");
        modelBuilder.Entity<VacancyModel>().ToCollection("vacancy");
    }

    public DbSet<CisReg_Website.Models.UserHall> UserHall { get; set; } = default!;

}
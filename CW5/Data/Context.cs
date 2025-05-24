using CW5.Models;
using Microsoft.EntityFrameworkCore;

namespace CW5.Data;

public class Context : DbContext
{
    public DbSet<Patient> Patients { get; set; }
    public DbSet<Doctor> Doctors { get; set; }
    public DbSet<Prescription> Prescriptions { get; set; }
    public DbSet<Medicament> Medicaments { get; set; }
    public DbSet<PrescriptionMedicament> PrescriptionMedicaments { get; set; }

    protected Context()
    {
        
    }

    public Context(DbContextOptions options) : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Seed Doctors
        modelBuilder.Entity<Doctor>().HasData(
            new Doctor { IdDoctor = 1, FirstName = "John", LasttName = "Smith", Email = "john.smith@hospital.com" },
            new Doctor { IdDoctor = 2, FirstName = "Sarah", LasttName = "Johnson", Email = "sarah.johnson@hospital.com" }
        );

        // Seed Patients
        modelBuilder.Entity<Patient>().HasData(
            new Patient { IdPatient = 1, FirstName = "Michael", LasttName = "Brown", Birthdate = new DateTime(1980, 5, 15) },
            new Patient { IdPatient = 2, FirstName = "Emily", LasttName = "Davis", Birthdate = new DateTime(1992, 8, 23) }
        );

        // Seed Medicaments
        modelBuilder.Entity<Medicament>().HasData(
            new Medicament { IdMedicament = 1, Name = "Aspirin", Description = "Pain reliever and anti-inflammatory", Type = "Analgesic" },
            new Medicament { IdMedicament = 2, Name = "Amoxicillin", Description = "Antibiotic for bacterial infections", Type = "Antibiotic" },
            new Medicament { IdMedicament = 3, Name = "Ibuprofen", Description = "Non-steroidal anti-inflammatory drug", Type = "NSAID" }
        );

        // Seed Prescriptions
        modelBuilder.Entity<Prescription>().HasData(
            new Prescription 
            { 
                IdPrescription = 1, 
                Date = new DateTime(2025, 5, 1), 
                DueDate = new DateTime(2025, 5, 15),
                IdPatient = 1,
                IdDoctor = 1
            },
            new Prescription 
            { 
                IdPrescription = 2, 
                Date = new DateTime(2025, 5, 2), 
                DueDate = new DateTime(2025, 5, 16),
                IdPatient = 2,
                IdDoctor = 2
            }
        );

        // Seed PrescriptionMedicaments
        modelBuilder.Entity<PrescriptionMedicament>().HasData(
            new PrescriptionMedicament 
            { 
                IdPrescription = 1, 
                IdMedicament = 1, 
                Dose = 2, 
                Details = "Take twice daily with meals" 
            },
            new PrescriptionMedicament 
            { 
                IdPrescription = 1, 
                IdMedicament = 2, 
                Dose = 1, 
                Details = "Take once daily" 
            },
            new PrescriptionMedicament 
            { 
                IdPrescription = 2, 
                IdMedicament = 3, 
                Dose = 1, 
                Details = "Take as needed for pain" 
            }
        );
    }
}
using CW5.Data;
using CW5.DTOs;
using CW5.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CW5.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PrescriptionController : ControllerBase
{
    private readonly Context _context;

    public PrescriptionController(Context context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> CreatePrescription(CreatePrescriptionRequestDTO request)
    {
        if (request.DueDate < request.Date)
        {
            return BadRequest("DueDate must be greater than or equal to Date");
        }

        if (request.Medicaments.Count > 10)
        {
            return BadRequest("Prescription cannot contain more than 10 medicaments");
        }
        
        var doctor = await _context.Doctors.FindAsync(request.IdDoctor);
        if (doctor == null)
        {
            return NotFound($"Doctor with ID {request.IdDoctor} not found");
        }
        
        // Find or create patient
        var patient = await _context.Patients
            .FirstOrDefaultAsync(p => 
                p.FirstName == request.Patient.FirstName && 
                p.LasttName == request.Patient.LastName && 
                p.Birthdate == request.Patient.Birthdate);

        if (patient == null)
        {
            patient = new Patient
            {
                FirstName = request.Patient.FirstName,
                LasttName = request.Patient.LastName,
                Birthdate = request.Patient.Birthdate
            };
            _context.Patients.Add(patient);
            await _context.SaveChangesAsync();
        }

        // Validate all medicaments exist
        var medicamentIds = request.Medicaments.Select(m => m.IdMedicament).ToList();
        var existingMedicaments = await _context.Medicaments
            .Where(m => medicamentIds.Contains(m.IdMedicament))
            .Select(m => m.IdMedicament)
            .ToListAsync();

        var nonExistingMedicaments = medicamentIds.Except(existingMedicaments).ToList();
        if (nonExistingMedicaments.Any())
        {
            return BadRequest($"Medicaments with IDs {string.Join(", ", nonExistingMedicaments)} do not exist");
        }

        // Create prescription
        var prescription = new Prescription
        {
            Date = request.Date,
            DueDate = request.DueDate,
            IdDoctor = request.IdDoctor,
            IdPatient = patient.IdPatient
        };

        _context.Prescriptions.Add(prescription);
        await _context.SaveChangesAsync();

        // Add prescription medicaments
        var prescriptionMedicaments = request.Medicaments.Select(m => new PrescriptionMedicament
        {
            IdPrescription = prescription.IdPrescription,
            IdMedicament = m.IdMedicament,
            Dose = m.Dose,
            Details = m.Description
        });

        _context.PrescriptionMedicaments.AddRange(prescriptionMedicaments);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetPrescription), new { id = prescription.IdPrescription }, prescription);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PrescriptionResponseDTO>> GetPrescription(int id)
    {
        var prescription = await _context.Prescriptions
            .Include(p => p.Doctor)
            .Include(p => p.Patient)
            .Include(p => p.Medicaments)
            .FirstOrDefaultAsync(p => p.IdPrescription == id);

        if (prescription == null)
        {
            return NotFound();
        }

        var prescriptionMedicaments = await _context.PrescriptionMedicaments
            .Include(pm => pm.Medicament)
            .Where(pm => pm.IdPrescription == id)
            .ToListAsync();

        var response = new PrescriptionResponseDTO
        {
            IdPrescription = prescription.IdPrescription,
            Date = prescription.Date,
            DueDate = prescription.DueDate,
            Doctor = new DoctorResponseDTO
            {
                IdDoctor = prescription.Doctor.IdDoctor,
                FirstName = prescription.Doctor.FirstName,
                LastName = prescription.Doctor.LasttName,
                Email = prescription.Doctor.Email
            },
            Patient = new PatientResponseDTO
            {
                IdPatient = prescription.Patient.IdPatient,
                FirstName = prescription.Patient.FirstName,
                LastName = prescription.Patient.LasttName,
                Birthdate = prescription.Patient.Birthdate
            },
            Medicaments = prescriptionMedicaments.Select(pm => new MedicamentResponseDTO
            {
                IdMedicament = pm.Medicament.IdMedicament,
                Name = pm.Medicament.Name,
                Description = pm.Medicament.Description,
                Type = pm.Medicament.Type,
                Dose = pm.Dose
            }).ToList()
        };

        return response;
    }
}
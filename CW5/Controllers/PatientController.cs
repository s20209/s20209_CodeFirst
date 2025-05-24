using CW5.Data;
using CW5.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CW5.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PatientController : ControllerBase
{
    private readonly Context _context;

    public PatientController(Context context)
    {
        _context = context;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PatientResponseDTO>> GetPatient(int id)
    {
        var patient = await _context.Patients
            .Include(p => p.Prescriptions)
                .ThenInclude(p => p.Doctor)
            .Include(p => p.Prescriptions)
                .ThenInclude(p => p.Medicaments)
            .FirstOrDefaultAsync(p => p.IdPatient == id);

        if (patient == null)
        {
            return NotFound();
        }

        var response = new PatientResponseDTO
        {
            IdPatient = patient.IdPatient,
            FirstName = patient.FirstName,
            LastName = patient.LasttName,
            Birthdate = patient.Birthdate,
            Prescriptions = (await Task.WhenAll(patient.Prescriptions.Select(async p => 
            {
                var prescriptionMedicaments = await _context.PrescriptionMedicaments
                    .Include(pm => pm.Medicament)
                    .Where(pm => pm.IdPrescription == p.IdPrescription)
                    .ToListAsync();

                return new PrescriptionResponseDTO
                {
                    IdPrescription = p.IdPrescription,
                    Date = p.Date,
                    DueDate = p.DueDate,
                    Doctor = new DoctorResponseDTO
                    {
                        IdDoctor = p.Doctor.IdDoctor,
                        FirstName = p.Doctor.FirstName,
                        LastName = p.Doctor.LasttName,
                        Email = p.Doctor.Email
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
            }))).ToList()
        };

        return Ok(response);
    }
} 
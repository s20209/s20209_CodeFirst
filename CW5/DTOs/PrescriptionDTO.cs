using System.ComponentModel.DataAnnotations;

namespace CW5.DTOs;

public class CreatePrescriptionRequestDTO
{
    public PatientCreateDTO Patient { get; set; }
    public int IdDoctor { get; set; }
    public DateTime Date { get; set; }
    public DateTime DueDate { get; set; }
    public List<PrescriptionMedicamentCreateDTO> Medicaments { get; set; }
}

public class PatientCreateDTO
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime Birthdate { get; set; }
}

public class PrescriptionMedicamentCreateDTO
{
    public int IdMedicament { get; set; }
    public int Dose { get; set; }
    public string Description { get; set; }
}

public class PrescriptionResponseDTO
{
    public int IdPrescription { get; set; }
    public DateTime Date { get; set; }
    public DateTime DueDate { get; set; }
    public DoctorResponseDTO Doctor { get; set; }
    public PatientResponseDTO Patient { get; set; } // Only for PrescriptionController
    public List<MedicamentResponseDTO> Medicaments { get; set; }
}

public class DoctorResponseDTO
{
    public int IdDoctor { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
}

public class MedicamentResponseDTO
{
    public int IdMedicament { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Type { get; set; }
    public int Dose { get; set; }
} 
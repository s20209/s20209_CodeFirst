using System.ComponentModel.DataAnnotations;

namespace CW5.DTOs;

public class PatientResponseDTO
{
    public int IdPatient { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime Birthdate { get; set; }
    public List<PrescriptionResponseDTO> Prescriptions { get; set; }
} 
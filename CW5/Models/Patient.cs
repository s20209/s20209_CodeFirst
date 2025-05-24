using System.ComponentModel.DataAnnotations;

namespace CW5.Models;

public class Patient
{
    [Key] 
    public int IdPatient { get; set; }
    [MaxLength(100)] 
    public string FirstName { get; set; }
    [MaxLength(100)]
    public string LasttName { get; set; }

    public DateTime Birthdate { get; set; }

    public ICollection<Prescription> Prescriptions { get; set; }
}
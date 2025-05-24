using System.ComponentModel.DataAnnotations;

namespace CW5.Models;

public class Doctor
{
    [Key] 
    public int IdDoctor { get; set; }
    [MaxLength(100)] 
    public string FirstName { get; set; }
    [MaxLength(100)]
    public string LasttName { get; set; }
    [MaxLength(100)]
    public string Email { get; set; }
    public ICollection<Prescription> Prescriptions { get; set; }
}
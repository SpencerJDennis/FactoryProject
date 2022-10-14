using System.Collections.Generic;

namespace DocOffice.Models
{
  public class Patient
  {
    public Patient()
    {
      this.JoinEntities = new HashSet<DoctorPatient>();
    }

    public int PatientId { get; set; }
    public string Name { get; set; }
    public string Birthdate { get; set; }
    public virtual ICollection<DoctorPatient> JoinEntities {get; set;}
  }
}
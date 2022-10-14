using System.Collections.Generic;

namespace DocOffice.Models
{
  public class Doctor
  {
    public Doctor()
    {
      this.JoinEntities = new HashSet<DoctorPatient>();
      this.JoinEntSpec = new HashSet<DoctorSpecialty>();
    }

    public int DoctorId { get; set; }
    public string Name { get; set; }
    public virtual ICollection<DoctorPatient> JoinEntities { get; set; }
    public virtual ICollection<DoctorSpecialty> JoinEntSpec { get; set; }
  }
}
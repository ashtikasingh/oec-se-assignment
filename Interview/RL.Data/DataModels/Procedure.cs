using System.ComponentModel.DataAnnotations;
using RL.Data.DataModels.Common;

namespace RL.Data.DataModels;

public class Procedure : IChangeTrackable
{
    [Key]
    public int ProcedureId { get; set; }
    public string ProcedureTitle { get; set; }
    public DateTime CreateDate { get; set; }
    public DateTime UpdateDate { get; set; }

    public Procedure()
    {
        PlanProcedures = new List<PlanProcedure>();
        AssignedUsers = new List<AssignedUser>();
    }

    public virtual ICollection<PlanProcedure> PlanProcedures { get; set; }
    public virtual ICollection<AssignedUser> AssignedUsers { get; set; }
}

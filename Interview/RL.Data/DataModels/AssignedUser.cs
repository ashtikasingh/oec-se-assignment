using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using RL.Data.DataModels.Common;

    namespace RL.Data.DataModels
    {
        public class AssignedUser : IChangeTrackable
        {
        [ForeignKey("User")]
        public int UserId { get; set; }

        [ForeignKey("Procedure")]
        public int ProcedureId { get; set; }

        [ForeignKey("Plan")]
        public int PlanId { get; set; }
        public virtual User User { get; set; }
        public virtual Plan Plan { get; set; }
        public virtual Procedure Procedure { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
    }
    }


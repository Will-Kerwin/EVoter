using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_VoterApi.Models
{
    [Table("Nominee")]
    public class Nominee
    {
        [Key]
        public Guid electionID { get; set; }
        [ForeignKey("VoterUsers")]
        public Guid userID { get; set; }
        public string association { get; set; }
        public string description { get; set; }
    }
}

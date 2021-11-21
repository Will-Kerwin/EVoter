using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_VoterApi.Models
{
    [Table("Results")]
    public class ElectionResult
    {
        [ForeignKey("Nominee")]
        public Guid nomineeID {  get; set; }
        [ForeignKey("Election")]
        public Guid electionID {  get; set; }
        public int totalVotes {  get; set; }
        public int ranking { get; set; }
    }
}

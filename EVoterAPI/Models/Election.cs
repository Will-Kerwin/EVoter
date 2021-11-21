using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_VoterApi.Models
{
    [Table("Election")]
    public class Election
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid electionID { get; set; }
        public DateTime electionStartDate { get; set; }
        public DateTime electionEndDate { get; set; }
        public string electionName { get; set; }
        public int? totalVotes {  get; set; }
        [ForeignKey("VoterUsers")]
        public Guid winnerID {  get; set; }
        public long modifiedTicks { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_VoterApi.Models
{
    [Table("Vote")]
    public class Vote
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid voteID {  get; set; }
        [ForeignKey("Election")]
        public Guid electionID {  get; set; }
        [ForeignKey("Nominee")]
        public Guid nomineeID {  get; set; }
        [ForeignKey("VoterUsers")]
        public Guid voterID {  get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long modifiedTicks { get; set; }
    }
}

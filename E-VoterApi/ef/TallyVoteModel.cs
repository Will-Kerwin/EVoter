namespace E_VoterApi.Models
{
    public class TallyVoteModel : IEquatable<TallyVoteModel>, IComparable<TallyVoteModel>
    {
        public Guid nominee {  get; set; }
        public int votes {  get; set; }

        public int CompareTo(TallyVoteModel compareVote)
        {
            // A null value means that this object is greater.
            if (compareVote == null)
                return 1;

            else
                return this.votes.CompareTo(compareVote.votes);
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            TallyVoteModel objAsPart = obj as TallyVoteModel;
            if (objAsPart == null) return false;
            else return Equals(objAsPart);
        }

        public bool Equals(TallyVoteModel other)
        {
            if (other == null) return false;
            return (this.votes.Equals(other.votes));
        }
    }
}

using E_VoterApi.Models;
using E_VoterApi.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_VoterApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class VoteController : ControllerBase
    {
        private readonly VoteRepository Vote;

        public VoteController()
        {
            Vote = new VoteRepository();
        }

        [HttpPost("vote")]
        public async Task<IActionResult> SubmitVote([FromBody] VoteModel voteData)
        {
            var result = await Vote.SubmitVote(voteData);

            if (result.status == 1)
                return Ok();

            else if (result.status == 0)
                return NotFound($"{result.reason} was not found");

            else
                return StatusCode(500, "Could not submit vote");
        }

        [HttpGet("total/{nomineeID}/{electionID}")]
        public async Task<IActionResult> GetTotalVotesForNominee(Guid nomineeID, Guid electionID)
        {
            var result = await Vote.GetVotesForNominee(nomineeID, electionID);

            if (result.status == 1)
                return Ok(result.total);

            else if (result.status == 0)
                return NotFound($"{result.reason} was not found");

            else
                return StatusCode(500, "Could not tally votes for nominee in given election");
        }
    }
}

using E_VoterApi.Models;
using E_VoterApi.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_VoterApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ElectionController : ControllerBase
    {

        private readonly ElectionRepository Election;

        public ElectionController()
        {
            Election = new ElectionRepository();
        }

        [HttpGet]
        public async Task<IActionResult> GetElections()
        {
            var result = await Election.GetAllElections();
            if (result.success == 1)
                return Ok(result.elections);
            else if (result.success == 0)
                return NotFound();
            else
                return StatusCode(500, "Error finding elections");
        }

        [HttpGet("{electionID}")]
        public async Task<IActionResult> GetElection(Guid electionID)
        {
            var result = await Election.GetElection(electionID);
            if (result.success == 1)
                return Ok(result.election);
            else if (result.success == 0)
                return NotFound();
            else
                return StatusCode(500, "Error finding elections");
        }

        [HttpPost("new")]
        public async Task<IActionResult> NewElection([FromBody] ElectionModel election)
        {
            var result = await Election.NewElection(election);
            if (result.success)
                return Ok(election);
            else
                return StatusCode(500, "Error creating election");
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddNominee(NomineeModel nominee)
        {
            var result = await Election.AddNominee(nominee);

            if (result.status == 1)
                return Ok(nominee);

            else if (result.status == 0)
                if(result.reason == "Nominee")
                    return BadRequest($"{result.reason} is already a nominee");
                else
                    return NotFound($"{result.reason} was not found");

            else
                return StatusCode(500, "Could not add to election");
        }

        [HttpDelete("{electionId}/{userId}")]
        public async Task<IActionResult> RemoveNominee(Guid electionId, Guid userId)
        {
            var result = await Election.RemoveNominee(electionId, userId);

            if (result.status == 1)
                return Ok();

            else if (result.status == 0)
                return NotFound($"{result.reason} was not found");

            else
                return StatusCode(500, "Could not remove from election");
        }

        [HttpGet("tally/{electionID}")]
        public async Task<IActionResult> GetTotalVotesForElection(Guid electionID)
        {
            var tally = await Election.TallyElection(electionID);

            if (tally.status == 0)
                return NotFound($"{tally.reason} was not found");

            else
                return StatusCode(500, "Could not remove from election");

            var result = await Election.SubmitResults(electionID, tally.results);

            if (result.status == 1)
                return Ok();

            else if (result.status == 0)
                return NotFound($"{result.reason} was not found");

            else
                return StatusCode(500, "Could not remove from election");

        }
    }
}

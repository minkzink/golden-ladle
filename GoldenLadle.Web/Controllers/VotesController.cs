using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GoldenLadle.Data;
using GoldenLadle.Data.Interfaces;
using GoldenLadle.Models;

namespace GoldenLadle.Controllers
{
    [Produces("application/json")]
    [Route("api/Votes")]
    public class VotesController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public VotesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: api/Votes
        [HttpGet]
        public IEnumerable<Vote> GetAllVotes()
        {
            var votes = _unitOfWork.Votes.GetAll();
            return votes;
        }

        // GET: api/Votes/get?id={id}&userId={userId}&entryId={entryId}
        [HttpGet("get")]
        public async Task<IActionResult> GetVote([FromQuery] int? id = null, string userId = "", int? entryId = null)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Vote vote = new Vote();
            vote = id.HasValue ? await _unitOfWork.Votes.GetAsync((int)id) : await _unitOfWork.Votes.GetAsync(userId, entryId);
            return vote == null ? NotFound() : (IActionResult)Ok(vote);
        }

        // POST: api/Votes
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> PostVote([FromForm] [Bind("Id,Value,EntryId,EventId,UserId")] Vote vote)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _unitOfWork.Votes.Add(vote);
            _unitOfWork.Complete();
            await UpdateValueCount(vote.EntryId);

            return CreatedAtAction("GetVote", new { id = vote.Id }, vote);
        }

        // DELETE: api/Votes/5
        [HttpDelete("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> DeleteVote([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var vote = await _unitOfWork.Votes.GetAsync(id);
            if (vote == null)
            {
                return NotFound();
            }

            Entry originalEntry = _unitOfWork.Entries.Get(vote.EntryId);
            _unitOfWork.Votes.Remove(vote);
            await _unitOfWork.CompleteAsync();
            await UpdateValueCount(originalEntry.Id);

            return Ok(vote);
        }

        private bool VoteExists(int id)
        {
            return _unitOfWork.Votes.CheckIfAnyExist(id);
        }

        private async Task UpdateValueCount(int id)
        {
            Entry entry = _unitOfWork.Entries.Get(id);
            entry.Value = await _unitOfWork.Votes.GetEntryVoteCount(entry.Id);
            _unitOfWork.Entries.Update(entry);
            await _unitOfWork.CompleteAsync();
        }
    }
}
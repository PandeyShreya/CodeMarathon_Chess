using ChessMarathon.DAO;
using ChessMarathon.Models;
using Microsoft.AspNetCore.Mvc;
using System.Numerics;

namespace ChessMarathon.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChessController : ControllerBase
    {
        private readonly IChess _chessDAO;

        public ChessController(IChess chessDAO)
        {
            _chessDAO = chessDAO;
        }

        [HttpGet("{id:int}", Name = "GetMatchesById")]
        public async Task<ActionResult<Matches?>> GetMatchesById(int id)
        {
            Matches? match = await _chessDAO.GetMatchesById(id);
            if (match == null)
            {
                return NotFound();
            }
            return Ok(match);
        }

        //Problem 1: Insert Player
        [HttpPost]
        public async Task<ActionResult<int>> InsertMatches(Matches m)
        {
            if (m != null)
            {
                if (ModelState.IsValid)
                {
                    int res = await _chessDAO.InsertMatches(m);
                    if (res > 0)
                    {
                        return CreatedAtRoute(nameof(GetMatchesById), new { id = m.MatchId }, m);

                    }
                }
                return BadRequest("Failed to add product");

            }
            else
            {
                return BadRequest();
            }
        }

        [HttpGet("PlayerByCountry")]
        public async Task<ActionResult<List<Players>>> GetPlayersByCountry([FromQuery] string country)
        {
            List<Players>? playerList = await _chessDAO.GetPlayersByCountry(country);
            if (playerList == null)
            {
                return NotFound();
            }
            return Ok(playerList);
        }

        [HttpGet("PlayerPerformance")]
        public async Task<ActionResult<List<PlayerPerformance>>> GetPlayerPerformance()
        {
            List<PlayerPerformance>? playerPerformanceList = await _chessDAO.GetPlayerPerformance();
            if (playerPerformanceList == null)
            {
                return NotFound();
            }
            return Ok(playerPerformanceList);
        }

        [HttpGet("PlayerWithHigeshtWin")]
        public async Task<ActionResult<List<PlayerPerformance>>> GetPlayerWithHighestWon()
        {
            List<PlayerPerformance>? playerPerformanceList = await _chessDAO.GetPlayerWithHighestWon();
            if (playerPerformanceList == null)
            {
                return NotFound();
            }
            return Ok(playerPerformanceList);
        }
    }
}

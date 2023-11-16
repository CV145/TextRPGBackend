using Firebase.Database;
using Firebase.Database.Query;
using Microsoft.AspNetCore.Mvc;
using TextRPGBackend.Models;

namespace TextRPGBackend.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class StatsController : Controller
    {
        private readonly FirebaseClient _firebaseClient;

        public StatsController()
        {
            _firebaseClient = new FirebaseClient("https://textrpg-6e9c2-default-rtdb.firebaseio.com/");
        }

        // GET api/playerstats/{playerId}
        [HttpGet("{playerId}")]
        public async Task<ActionResult<Player>> GetPlayerStats(string playerId)
        {
            var stats = await _firebaseClient
                .Child("PlayerStats")
                .Child(playerId)
                .OnceSingleAsync<Player>();

            if (stats == null) return NotFound();
            return stats;
        }

        // POST api/playerstats
        [HttpPost]
        public async Task<ActionResult> PostPlayerStats([FromBody] Player playerStat)
        {
            await _firebaseClient
                .Child("PlayerStats")
                .Child(playerStat.PlayerId)
                .PutAsync(playerStat);

            return Ok();
        }

        [HttpGet("test")]
        public async Task<IActionResult> TestFirebase()
        {
            var playerStats = await _firebaseClient
                .Child("PlayerStats")
                .OnceAsync<Player>();

            return Ok(playerStats.Select(stat => new { stat.Key, stat.Object.Health, stat.Object.Mana }));
        }
    }
}

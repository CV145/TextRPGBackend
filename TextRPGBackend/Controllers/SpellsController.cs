using Firebase.Database;
using Firebase.Database.Query;
using Microsoft.AspNetCore.Mvc;
using TextRPGBackend.Models;

namespace TextRPGBackend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SpellsController : Controller
    {
        private readonly FirebaseClient _firebaseClient;
        public SpellsController()
        {
            _firebaseClient = new FirebaseClient("https://textrpg-6e9c2-default-rtdb.firebaseio.com/");
        }


        [HttpGet("{playerId}")]
        public async Task<ActionResult<IEnumerable<Spell>>> GetSpells(string playerId)
        {
            var spells = await _firebaseClient
                .Child("Spells")
                .Child(playerId)
                .OnceAsync<Spell>();

            return Ok(spells.Select(spell => spell.Object));
        }


        [HttpPost("{playerId}")]
        public async Task<ActionResult> AddSpell(string playerId, [FromBody] Spell newSpell)
        {
            await _firebaseClient
                .Child("Spells")
                .Child(playerId)
                .PostAsync(newSpell);

            return Ok();
        }


        [HttpDelete("{playerId}/{spellName}")]
        public async Task<ActionResult> DeleteSpell(string playerId, string spellName)
        {
            var spells = await _firebaseClient
                .Child("Spells")
                .Child(playerId)
                .OnceAsync<Spell>();

            var spellToDelete = spells.FirstOrDefault(spell => spell.Object.Name == spellName);
            if (spellToDelete != null)
            {
                await _firebaseClient
                    .Child("Spells")
                    .Child(playerId)
                    .Child(spellToDelete.Key)
                    .DeleteAsync();
            }

            return Ok();
        }


    }
}

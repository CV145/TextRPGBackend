using Firebase.Database;
using Firebase.Database.Query;
using Microsoft.AspNetCore.Mvc;
using TextRPGBackend.Models;

namespace TextRPGBackend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InventoryController : Controller
    {
        private readonly FirebaseClient _firebaseClient;

        public InventoryController()
        {
            _firebaseClient = new FirebaseClient("https://textrpg-6e9c2-default-rtdb.firebaseio.com/");
        }

        [HttpGet("{playerId}")]
        public async Task<ActionResult<IEnumerable<Item>>> GetInventory(string playerId)
        {
            var inventory = await _firebaseClient
                .Child("Inventories")
                .Child(playerId)
                .OnceAsync<Item>();

            return Ok(inventory.Select(item => item.Object));
        }


        [HttpPost("{playerId}")]
        public async Task<ActionResult> AddItem(string playerId, [FromBody] Item newItem)
        {
            await _firebaseClient
                .Child("Inventories")
                .Child(playerId)
                .PostAsync(newItem);

            return Ok();
        }


        [HttpDelete("{playerId}/{itemName}")]
        public async Task<ActionResult> DeleteItem(string playerId, string itemName)
        {
            var items = await _firebaseClient
                .Child("Inventories")
                .Child(playerId)
                .OnceAsync<Item>();

            var itemToDelete = items.FirstOrDefault(item => item.Object.Name == itemName);
            if (itemToDelete != null)
            {
                await _firebaseClient
                    .Child("Inventories")
                    .Child(playerId)
                    .Child(itemToDelete.Key)
                    .DeleteAsync();
            }

            return Ok();
        }

    }
}

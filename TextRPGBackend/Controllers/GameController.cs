using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace TextRPGBackend.Controllers
{
    /*TODO
     * - Parse responses (CONFLICT RESOLVED, ITEM OBTAINED, SPELL OBTAINED, X HP LOST, ERROR)
     * - Pass in player items/stats to prompts
     * - Integrate and setup Google Firebase backend
     */

    //Used for handling game interactions
    [ApiController]
    [Route("[controller]")]
    public class GameController : Controller
    {
        private readonly GameInstructionService _instructionService;
        private readonly IHttpClientFactory _clientFactory;
        private readonly string _chatGptUrl;
        private readonly string _apiKey;

        //Instance created in startup, passed using dep. injection
        public GameController(IHttpClientFactory clientFactory, GameInstructionService instructionService, IConfiguration configuration)
        {
            _clientFactory = clientFactory;
            _chatGptUrl = "https://api.openai.com/v1/chat/completions";
            _instructionService = instructionService;
            _apiKey = configuration["OPENAI_API_KEY"];
        }

        [HttpPost("sendprompt")]
        public async Task<IActionResult> SendPrompt([FromBody] string playerPrompt)
        {
            Console.WriteLine("API Key: " + _apiKey);
            var instructions = _instructionService.GetGameInstructions();
            //var playerStats = string with player items and stats
            var fullPrompt = $"{instructions}\n\n{playerPrompt}";


            //process instruction and interact with AI service
            var client = _clientFactory.CreateClient();

            var requestContent = new
            {
                model = "gpt-3.5-turbo",
                messages = new[] { new { role = "user", content = fullPrompt } }
            };

            var requestMessage = new HttpRequestMessage(HttpMethod.Post, _chatGptUrl)
            {
                Content = new StringContent(JsonConvert.SerializeObject(requestContent), Encoding.UTF8, "application/json")
            };
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);

            var response = await client.SendAsync(requestMessage);
            var responseContent = await response.Content.ReadAsStringAsync();

            return Ok(responseContent);
        }

        [HttpGet("instructions")]
        public IActionResult GetInstructions()
        {
            var instructions = _instructionService.GetGameInstructions();
            return Ok(instructions);
        }
    }
}

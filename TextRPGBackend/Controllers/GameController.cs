using Microsoft.AspNetCore.Mvc;

namespace TextRPGBackend.Controllers
{

    //Used for handling game interactions
    [ApiController]
    [Route("[controller]")]
    public class GameController : Controller
    {
        private readonly GameInstructionService _instructionService;

        //Instance created in startup, passed using dep. injection
        public GameController(GameInstructionService instructionService)
        {
            _instructionService = instructionService;
        }

        [HttpPost("sendprompt")]
        public async Task<IActionResult> SendPrompt([FromBody] string playerPrompt)
        {
            //process instruction and interact with AI service
            
        }

        [HttpGet("instructions")]
        public IActionResult GetInstructions()
        {
            var instructions = _instructionService.GetGameInstructions();
            return Ok(instructions);
        }
    }
}

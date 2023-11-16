using System.IO;

public class GameInstructionService
{
    private readonly string _filePath;

    public GameInstructionService()
    {
        // Adjust the path as per your project structure
        _filePath = "./Config/GameInstructions.txt";
    }

    public string GetGameInstructions()
    {
        // Read and return the content of the file
        return File.ReadAllText(_filePath);
    }
}

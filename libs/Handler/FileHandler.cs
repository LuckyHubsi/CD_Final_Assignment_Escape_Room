using System.Reflection.Metadata.Ecma335;
using Newtonsoft.Json;

namespace libs;

public static class FileHandler
{
    // Path to the current level JSON file
    private static string filePath;
    // private static string savePath = "../savedGame.json"; // Uncomment if save functionality is added
    private static int levelNumber = 1; // Current level number
    private static string levelPath = "../Level" + levelNumber + ".json"; // Path to the level JSON file
    private static string dialogPath = "../Dialog.json"; // Path to the Dialog JSON file

    // Static constructor to initialize the file handler
    static FileHandler()
    {
        Initialize();
    }

    // Method to initialize the file path
    private static void Initialize()
    {
        filePath = levelPath;
    }

    // Method to change the level and update the file path
    public static void ChangeLevel()
    {
        levelNumber++; // Increment the level number

        if (levelNumber > 2)
        {
            // If level number exceeds 2, reset to level 1
            levelNumber = 1;
            levelPath = "../Level" + levelNumber + ".json";
            filePath = levelPath;
            ReadJson(); // Read the JSON file for the new level
        }
        else
        {
            // Update the file path for the new level
            levelPath = "../Level" + levelNumber + ".json";
            filePath = levelPath;
            ReadJson(); // Read the JSON file for the new level
        }
    }

    // Method to read JSON data from the file
    public static dynamic ReadJson()
    {
        if (string.IsNullOrEmpty(filePath))
        {
            throw new InvalidOperationException("JSON file path not provided in environment variable");
        }

        try
        {
            // Read the JSON content from the file
            string jsonContent = File.ReadAllText(filePath);
            // Deserialize the JSON content to a dynamic object
            dynamic jsonData = JsonConvert.DeserializeObject(jsonContent);
            return jsonData;
        }
        catch (FileNotFoundException)
        {
            throw new FileNotFoundException($"JSON file not found at path: {filePath}");
        }
        catch (Exception ex)
        {
            throw new Exception($"Error reading JSON file: {ex.Message}");
        }
    }

    // Method to get a specific dialog from the Dialog JSON file
    public static string GetDialog(string key)
    {
        try
        {
            // Read the dialog JSON content from the file
            string jsonContent = File.ReadAllText(dialogPath);
            // Deserialize the JSON content to a dynamic object
            dynamic dialogs = JsonConvert.DeserializeObject(jsonContent);
            // Return the dialog text corresponding to the given key
            return dialogs[key];
        }
        catch (FileNotFoundException)
        {
            throw new FileNotFoundException($"Messages JSON file not found at path: {dialogPath}");
        }
        catch (Exception ex)
        {
            throw new Exception($"Error reading messages JSON file: {ex.Message}");
        }
    }

    // Method to get the current level number
    public static int GetLevelNumber()
    {
        return levelNumber;
    }
}

using System.Reflection.Metadata.Ecma335;

namespace libs;

using Newtonsoft.Json;

public static class FileHandler
{
    private static string filePath;
    // private static string savePath = "../savedGame.json";
    private static int levelNumber = 1;
    private static string levelPath = "../Level" + levelNumber + ".json";
    private static string dialogPath = "../Dialog.json"; // Path to the Dialog JSON file

    static FileHandler()
    {
        Initialize();
    }

    private static void Initialize()
    {
        filePath = levelPath;
    }

    public static void ChangeLevel()
    {
        levelNumber++;

        if (levelNumber > 2)
        {
            levelNumber = 1;
            levelPath = "../Level" + levelNumber + ".json";
            filePath = levelPath;
            ReadJson();
        }
        else
        {
            levelPath = "../Level" + levelNumber + ".json";
            filePath = levelPath;
            ReadJson();
        }
    }

    public static dynamic ReadJson()
    {
        if (string.IsNullOrEmpty(filePath))
        {
            throw new InvalidOperationException("JSON file path not provided in environment variable");
        }

        try
        {
            string jsonContent = File.ReadAllText(filePath);
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

    public static string GetDialog(string key)
    {
        try
        {
            string jsonContent = File.ReadAllText(dialogPath);
            dynamic dialogs = JsonConvert.DeserializeObject(jsonContent);
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
}

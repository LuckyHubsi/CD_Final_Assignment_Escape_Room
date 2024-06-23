using System.Reflection.Metadata.Ecma335;
using System.Text.Json.Nodes;

namespace libs;

using System.Security.Cryptography;
using Newtonsoft.Json;

public sealed class GameEngine
{
    private static GameEngine? _instance; // Singleton instance of GameEngine
    private IGameObjectFactory gameObjectFactory; // Factory for creating GameObjects

    private bool lastLevelCheck = false; // Flag to check if it's the last level

    // Singleton property to get the instance of GameEngine
    public static GameEngine Instance {
        get{
            if(_instance == null)
            {
                _instance = new GameEngine();
            }
            return _instance;
        }
    }

    // Private constructor to prevent external instantiation
    private GameEngine() {
        // Initialize properties if needed
        gameObjectFactory = new GameObjectFactory();
    }

    private GameObject? _focusedObject; // The currently focused GameObject

    private Map map = new Map(); // The game map

    public List<GameObject> gameObjects = new List<GameObject>(); // List of all GameObjects in the game

    // Linked list of snapshots of game objects for undo functionality
    private LinkedList<List<GameObject>> gameObjectSnapshots = new LinkedList<List<GameObject>>();

    // Method to store the current state of the map
    public void StoreMap() {
        List<GameObject> gameObjectClones = new List<GameObject>();

        // Create clones of all GameObjects to store the current state
        foreach (GameObject gameObject in gameObjects) {
            gameObjectClones.Add((GameObject) gameObject.Clone());
        }

        gameObjectSnapshots.AddLast(gameObjectClones);
    }

    // Method to restore the map to the previous state
    public void RestoreMap() {
        if (gameObjectSnapshots.Count <= 0) return;

        gameObjects = gameObjectSnapshots.Last!.Value;
        gameObjectSnapshots.RemoveLast();

        // Promote the cloned player to the focused object
        _focusedObject = gameObjects.OfType<Player>().First();
    }

    // Method to get the map
    public Map GetMap() {
        return map;
    }

    // Method to get the currently focused GameObject
    public GameObject GetFocusedObject(){
        return _focusedObject;
    }

    private bool characterCreated = false; // Flag to check if the character is created

    private DialogWindow dialog; // Dialog window for displaying messages

    // Method to set up the game
    public void Setup(){
        gameObjects.Clear();
        gameObjectSnapshots.Clear();
        map = new Map();
        characterCreated = false;

        // Set console output encoding for proper display of characters
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        dynamic gameData = FileHandler.ReadJson();  
        
        map.MapWidth = gameData.map.width;
        map.MapHeight = gameData.map.height;

        // Add GameObjects to the map based on the JSON data
        foreach (var gameObject in gameData.gameObjects)
        {
            if(!characterCreated || gameObject.Type != 0){
                AddGameObject(CreateGameObject(gameObject));
                if(gameObject.Type == 0){characterCreated = true;}
            }
        }
        
        _focusedObject = gameObjects.OfType<Player>().First();
        StoreMap();

        Render();
        string tutorialMessage = FileHandler.GetDialog("TutorialMessage");
        string tutorialHeader = FileHandler.GetDialog("TutorialHeader");
        dialog.Draw(" "+ tutorialHeader +" ", tutorialMessage);
        Console.ReadKey(true);
    }
   
    // Method to render the game
    public void Render() {
        Console.Clear(); // Clear the console

        map.Initialize(); // Initialize the map

        PlaceGameObjects(); // Place GameObjects on the map

        // Render the map
        for (int i = 0; i < map.MapHeight; i++){
            for (int j = 0; j < map.MapWidth; j++){
                DrawObject(map.Get(i, j));
            }
            Console.WriteLine();
        }

        // Display hints
        string hintsHeader = FileHandler.GetDialog("HintsHeader");
        string hintsMessage = FileHandler.GetDialog("HintsMessage");
        dialog.Draw(" " + hintsHeader + " ",hintsMessage);
    }
    
    // Method to create GameObject using the factory from clients
    public GameObject CreateGameObject(dynamic obj){
        return gameObjectFactory.CreateGameObject(obj);
    }

    // Method to add a GameObject to the game
    public void AddGameObject(GameObject gameObject){
        gameObjects.Add(gameObject);
    }

    // Method to place GameObjects on the map
    private void PlaceGameObjects(){
        gameObjects.ForEach(delegate(GameObject obj)
        {
            map.Set(obj);
        });
    }

    // Method to draw a GameObject on the console
    private void DrawObject(GameObject gameObject){
        Console.ResetColor();

        if(gameObject != null)
        {
            Console.ForegroundColor = gameObject.Color;
            Console.Write(gameObject.CharRepresentation);
        }
        else{
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write(' ');
        }
    }

    // Method to show the main menu
    public void ShowMainMenu() {
        if (!lastLevelCheck){
            InitializeDialogWindow();
            Console.Clear();

            string mainMenuHeader = FileHandler.GetDialog("MainMenuHeader");
            string mainMenuMessage = FileHandler.GetDialog("MainMenuMessage");
            dialog.Draw(" " + mainMenuHeader + " ", mainMenuMessage);

            while (true) {
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                if (keyInfo.Key == ConsoleKey.Enter) {
                    break;
                }
            }
        }
    }

    // Method to check if the player has won the level
    public bool WinCheck() {
        if (!gameObjects.OfType<Door>().Any()) {
            return true;   
        }
        else {
            return false;
        }
    }

    // Method to handle winning the level
    public void WinLevel() {
        FileHandler.ChangeLevel();
        string winMessage = FileHandler.GetDialog("WinMessage");
        string winHeader = FileHandler.GetDialog("WinHeader");
        dialog.Draw(" " + winHeader + " ", winMessage);

        if (FileHandler.GetLevelNumber() == 1){
            lastLevelCheck = false;
        }
        else lastLevelCheck = true;
    }

    // Method to check if the player has lost the level
    public bool LoseCheck() {
        return false;
    }

    // Method to handle losing the level
    public void LoseLevel(){
        string loseHeader = FileHandler.GetDialog("LoseHeader");
        string loseMessage = FileHandler.GetDialog("LoseMessage");
        dialog.Draw(" "+ loseHeader + " ", loseMessage);
       
    }

    // Method to initialize the dialog window in the correct size
    public void InitializeDialogWindow()
    {
        int dialogWidth = 35;
        int dialogHeight = 10;
        int dialogX = 10;
        int dialogY = Math.Min(Console.BufferHeight - dialogHeight - 1, 1);

        dialog = new DialogWindow(dialogWidth, dialogHeight, dialogX, dialogY);
    }

    // Method to start the dialog with the witch
    public void DialogWitch(){
        Witch.dialog.Start();
    }
}
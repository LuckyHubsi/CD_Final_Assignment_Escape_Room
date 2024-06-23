﻿using System.Reflection.Metadata.Ecma335;
using System.Text.Json.Nodes;

namespace libs;

using System.Security.Cryptography;
using Newtonsoft.Json;

public sealed class GameEngine
{
    private static GameEngine? _instance;
    private IGameObjectFactory gameObjectFactory;

    private bool lastLevelCheck = false;

    public static GameEngine Instance {
        get{
            if(_instance == null)
            {
                _instance = new GameEngine();
            }
            return _instance;
        }
    }

    private GameEngine() {
        //INIT PROPS HERE IF NEEDED
        gameObjectFactory = new GameObjectFactory();
    }

    private GameObject? _focusedObject;

    private Map map = new Map();

    public List<GameObject> gameObjects = new List<GameObject>();

    // In a linked list: 1 object always shows to the next and the previous object
    // -> insertion & deletion is easier -> no need to shift elements after insertion/deletion & easier locatable, even after
    private LinkedList<List<GameObject>> gameObjectSnapshots = new LinkedList<List<GameObject>>();

    public void StoreMap() {

        // new list with gameobject clones
        // 1 list represents one snapshot of game
        List<GameObject> gameObjectClones = new List<GameObject>();

        // create clone for each original gameobject of original list -> dont want to effect original object
        foreach (GameObject gameObject in gameObjects) {
            gameObjectClones.Add((GameObject) gameObject.Clone());
        }

        gameObjectSnapshots.AddLast(gameObjectClones);
    }

    public void RestoreMap() {

        // at beginning of game wihtout any move made it obv cant restore
        if (gameObjectSnapshots.Count <= 0) return;

        gameObjects = gameObjectSnapshots.Last!.Value;
        gameObjectSnapshots.RemoveLast();

        // cloned player isnt the actual player, its just a clone -> "promote" clone to original player
        _focusedObject = gameObjects.OfType<Player>().First();
    }

    public Map GetMap() {
        return map;
    }

    public GameObject GetFocusedObject(){
        return _focusedObject;
    }

    private bool characterCreated = false;


    private DialogWindow dialog;

    public void Setup(){
        gameObjects.Clear();
        gameObjectSnapshots.Clear();
        map = new Map();
        characterCreated = false;


        //Added for proper display of game characters
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        dynamic gameData = FileHandler.ReadJson();  
        
        map.MapWidth = gameData.map.width;
        map.MapHeight = gameData.map.height;

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
   
    public void Render() {

        //Clean the map
        Console.Clear();

        map.Initialize();

        PlaceGameObjects();

        //Render the map
        for (int i = 0; i < map.MapHeight; i++){
            for (int j = 0; j < map.MapWidth; j++){
                DrawObject(map.Get(i, j));
            }
            Console.WriteLine();
        }

        string hintsHeader = FileHandler.GetDialog("HintsHeader");
        string hintsMessage = FileHandler.GetDialog("HintsMessage");
        dialog.Draw(" " + hintsHeader + " ",hintsMessage);
    }
    
    // Method to create GameObject using the factory from clients
    public GameObject CreateGameObject(dynamic obj){
        return gameObjectFactory.CreateGameObject(obj);
    }

    public void AddGameObject(GameObject gameObject){
        gameObjects.Add(gameObject);
    }

    private void PlaceGameObjects(){
        
        gameObjects.ForEach(delegate(GameObject obj)
        {
            map.Set(obj);
        });
    }

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

    public bool WinCheck() {
        if (!gameObjects.OfType<Door>().Any()) {
            return true;   
        }
        else {
            return false;
        }
    }

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

    public bool LoseCheck() {
        return false;
    }

    public void LoseLevel(){
        string loseHeader = FileHandler.GetDialog("LoseHeader");
        string loseMessage = FileHandler.GetDialog("LoseMessage");
        dialog.Draw(" "+ loseHeader + " ", loseMessage);
       
    }


    public void InitializeDialogWindow()
    {
        int dialogWidth = 35;
        int dialogHeight = 10;
        int dialogX = 10; // Adjust the X position as needed
        int dialogY = Math.Min(Console.BufferHeight - dialogHeight - 1, 1); // Adjust Y position

        dialog = new DialogWindow(dialogWidth, dialogHeight, dialogX, dialogY);
    }
}
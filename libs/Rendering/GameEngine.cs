using System.Reflection.Metadata.Ecma335;
using System.Text.Json.Nodes;

namespace libs;

using System.Security.Cryptography;
using Newtonsoft.Json;

public static class GameEngine
{
    private static GameObject? _focusedObject;

    private static Map map = new Map();

    private static List<GameObject> gameObjects = new List<GameObject>();

    public static Map GetMap() {
        return map;
    }

    public static GameObject GetFocusedObject(){
        return _focusedObject;
    }

    public static List<GameObject> GetGameObjects(){
        return gameObjects;
    }

    public static void Setup(){
        dynamic gameData = FileHandler.ReadJson();
        
        map.MapWidth = gameData.map.width;
        map.MapHeight = gameData.map.height;

        foreach (var gameObject in gameData.gameObjects)
        {
            GameObject newObj = new GameObject();
            int type = (int)gameObject.Type;

            switch (type)
            {
                case 0:
                    newObj = gameObject.ToObject<Queen>();
                    break;
                case 1:
                    newObj = gameObject.ToObject<King>();
                    break;
            }
            AddGameObject(newObj);
        }
    }

    public static void Render() {

        //Clean the map
        Console.Clear();

        _focusedObject = gameObjects[0];

        map.Initialize();

        PlaceGameObjects();

        //Render the map
        for (int i = 0; i < map.MapHeight; i++)
        {
            for (int j = 0; j < map.MapWidth; j++)
            {
                DrawObject(map.Get(i, j));
            }
            Console.WriteLine();
        }
    }
    
    public static void AddGameObject(GameObject gameObject){
        gameObjects.Add(gameObject);
    }

    private static void PlaceGameObjects(){
        
        gameObjects.ForEach(delegate(GameObject obj)
        {
            map.Set(obj);
        });
    }

    private static void DrawObject(GameObject gameObject){
        
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
}

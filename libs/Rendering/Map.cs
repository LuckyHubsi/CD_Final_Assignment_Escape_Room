namespace libs;
using Newtonsoft.Json;

public class Map {
    private char[,] RepresentationalLayer; // Character representation of the map
    private GameObject?[,] GameObjectLayer; // Layer containing GameObjects

    private int _mapWidth; // Width of the map
    private int _mapHeight; // Height of the map

    // Default constructor initializing a map with predefined dimensions
    public Map () {
        _mapWidth = 30;
        _mapHeight = 8;
        RepresentationalLayer = new char[_mapHeight, _mapWidth];
        GameObjectLayer = new GameObject[_mapHeight, _mapWidth];
    }

    // Constructor initializing a map with specified dimensions
    public Map (int width, int height) {
        _mapWidth = width;
        _mapHeight = height;
        RepresentationalLayer = new char[_mapHeight, _mapWidth];
        GameObjectLayer = new GameObject[_mapHeight, _mapWidth];
    }

    // Method to initialize the map with default values
    public void Initialize()
    {
        RepresentationalLayer = new char[_mapHeight, _mapWidth];
        GameObjectLayer = new GameObject[_mapHeight, _mapWidth];

        // Fill the map with Floor objects as default
        for (int i = 0; i < GameObjectLayer.GetLength(0); i++)
        {
            for (int j = 0; j < GameObjectLayer.GetLength(1); j++)
            {
                GameObjectLayer[i, j] = new Floor();
            }
        }
    }

    // Property to get and set the width of the map
    public int MapWidth
    {
        get { return _mapWidth; }
        set { _mapWidth = value; Initialize(); }
    }

    // Property to get and set the height of the map
    public int MapHeight
    {
        get { return _mapHeight; }
        set { _mapHeight = value; Initialize(); }
    }

    // Method to get the GameObject at a specific position
    public GameObject Get(int x, int y){
        return GameObjectLayer[x, y];
    }

    // Method to set a GameObject at its position on the map
    public void Set(GameObject gameObject){
        int posY = gameObject.PosY;
        int posX = gameObject.PosX;
        int prevPosY = gameObject.GetPrevPosY();
        int prevPosX = gameObject.GetPrevPosX();
        
        // Clear the previous position of the GameObject
        if (prevPosX >= 0 && prevPosX < _mapWidth &&
                prevPosY >= 0 && prevPosY < _mapHeight)
        {
            GameObjectLayer[prevPosY, prevPosX] = new Floor();
        }

        // Set the GameObject at the new position
        if (posX >= 0 && posX < _mapWidth &&
                posY >= 0 && posY < _mapHeight)
        {
            GameObjectLayer[posY, posX] = gameObject;
            RepresentationalLayer[gameObject.PosY, gameObject.PosX] = gameObject.CharRepresentation;
        }
    }
}

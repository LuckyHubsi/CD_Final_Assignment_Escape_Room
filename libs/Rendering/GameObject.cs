namespace libs;

public class GameObject : IMovement
{
    private char _charRepresentation = '#';
    private ConsoleColor _color;

    private int _posX;
    private int _posY;
    
    private int _prevPosX;
    private int _prevPosY;

    public GameObjectType Type;

    public GameObject() {
        this._posX = 5;
        this._posY = 0;
        this._color = ConsoleColor.Gray;
    }

    public GameObject(int posX, int posY){
        this._posX = posX;
        this._posY = posY;
    }

    public GameObject(int posX, int posY, ConsoleColor color){
        this._posX = posX;
        this._posY = posY;
        this._color = color;
    }

    public char CharRepresentation
    {
        get { return _charRepresentation ; }
        set { _charRepresentation = value; }
    }

    public ConsoleColor Color
    {
        get { return _color; }
        set { _color = value; }
    }

    public int PosX
    {
        get { return _posX; }
        set { _posX = value; }
    }

    public int PosY
    {
        get { return _posY; }
        set { _posY = value; }
    }

    public int GetPrevPosY() {
        return _prevPosY;
    }
    
    public int GetPrevPosX() {
        return _prevPosX;
    }

    //DIALOG STUFF
    public Dialog? dialog;

    protected List<DialogNode> dialogNodes = new List<DialogNode>();

    public void Move(int dx, int dy) {
        
        int targetX = _posX + dx;
        int targetY = _posY + dy;

        //Use LINQ to query objects in target Position.
        var collisionObjects = GameEngine.GetGameObjects()
        .Where(e => e.PosX == targetX && e.PosY == targetY);

        //If no Obstacles found --> MOVE
        if(collisionObjects.Count() == 0){
            _prevPosX = _posX;
            _prevPosY = _posY;
            _posX += dx;
            _posY += dy;
        }
        //Otherwise start dialog if exists
        else
        {
            if(collisionObjects.First().HasDialog()){
                collisionObjects.First().dialog.Start();
            }
        }
    }

    public bool HasDialog(){
        return (dialog == null) ? false : true;
    }
}

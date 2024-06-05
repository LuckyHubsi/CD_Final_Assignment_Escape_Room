namespace libs;

public sealed class InputHandler{

    private static InputHandler? _instance;
    private GameEngine engine;

    public static InputHandler Instance {
        get{
            if(_instance == null)
            {
                _instance = new InputHandler();
            }
            return _instance;
        }
    }

    private InputHandler() {
        //INIT PROPS HERE IF NEEDED
        engine = GameEngine.Instance;
    }

    public void Handle(ConsoleKeyInfo keyInfo)
    {
        GameObject focusedObject = engine.GetFocusedObject();

        if (focusedObject != null) {

            if(keyInfo.Key == ConsoleKey.Z && keyInfo.Modifiers.HasFlag(ConsoleModifiers.Control)) {
                engine.RestoreMap();
                return;
            }
            
            engine.StoreMap();

            // Handle keyboard input to move the player
            switch (keyInfo.Key)
            {
                case ConsoleKey.UpArrow:
                    focusedObject.CharRepresentation = '▲';
                    CollisionSituation(focusedObject, 0, -1);
                    break;
                case ConsoleKey.DownArrow:
                    focusedObject.CharRepresentation = '▼';
                    CollisionSituation(focusedObject, 0, 1);
                    break;
                case ConsoleKey.LeftArrow:
                    focusedObject.CharRepresentation = '◄';
                    CollisionSituation(focusedObject, -1, 0);
                    break;
                case ConsoleKey.RightArrow:
                    focusedObject.CharRepresentation = '►';
                    CollisionSituation(focusedObject, 1, 0);
                    break;
                default:
                    break;
            }

        }

        void CollisionSituation(GameObject player, int dx, int dy)
        {
            int nextPlayerPosX = player.PosX + dx;
            int nextPlayerPosY = player.PosY + dy;

            // get object at next position to which player moves
            GameObject nextObject = engine.GetMap().Get(nextPlayerPosY, nextPlayerPosX);


    // PLAYER hits BOX

            if (nextObject is Box box) {
                // calculate next position of box
                int nextBoxPosX = box.PosX + dx;
                int nextBoxPosY = box.PosY + dy;

                // position behind original box position = position box will be moved to -> is there an obstacle?
                GameObject posBehindBox = engine.GetMap().Get(nextBoxPosY, nextBoxPosX);


    // BOX hits OBSTACLE

                if (posBehindBox is ICollidable) {
                    engine.RestoreMap();
                }
                else {
                    box.Move(dx, dy);
                    player.Move(dx, dy);
                }
            }

    // PLAYER hits KEY

            else if (nextObject is Key key) {
                int nextKeyPosX = key.PosX + dx;
                int nextKeyPosY = key.PosY + dy;
                GameObject posBehindKey = engine.GetMap().Get(nextKeyPosY, nextKeyPosX);

    // KEY hits OBSTACLE

                if (posBehindKey is Wall or Box) {
                    engine.RestoreMap();
                }

    // KEY hits DOOR

                else if (posBehindKey is Door) {
                    engine.gameObjects.RemoveAll(obj => obj is Door or Key);
                }
                else {
                    key.Move(dx, dy);
                    player.Move(dx, dy);
                }
                
            }

    // PLAYER hits WALL or DOOR

            else if (nextObject is Wall or Door) {
                // player cant move forward
                engine.RestoreMap();
            }
            else {
                player.Move(dx, dy);
            }
        }
        
    }

}
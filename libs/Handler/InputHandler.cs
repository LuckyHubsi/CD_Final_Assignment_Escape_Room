namespace libs;

// Singleton class to handle input from the console
public sealed class InputHandler
{
    private static InputHandler? _instance; // Singleton instance of InputHandler
    private GameEngine engine; // Reference to the GameEngine instance

    // Singleton property to get the single instance of InputHandler
    public static InputHandler Instance {
        get {
            if(_instance == null)
            {
                _instance = new InputHandler();
            }
            return _instance;
        }
    }

    // Private constructor to prevent direct instantiation
    private InputHandler() {
        // Initialize properties if needed
        engine = GameEngine.Instance;
    }

    // Method to handle console key input
    public void Handle(ConsoleKeyInfo keyInfo)
    {
        // Get the currently focused object (player)
        GameObject focusedObject = engine.GetFocusedObject();

        if (focusedObject != null) {

            // Handle undo action (Ctrl + Z)
            if(keyInfo.Key == ConsoleKey.Z && keyInfo.Modifiers.HasFlag(ConsoleModifiers.Control)) {
                engine.RestoreMap();
                return;
            }
            
            engine.StoreMap();

            // Handle keyboard input to move the player
            switch (keyInfo.Key)
            {
                case ConsoleKey.UpArrow:
                    focusedObject.CharRepresentation = '▲'; // Change character representation
                    CollisionSituation(focusedObject, 0, -1); // Move up
                    break;
                case ConsoleKey.DownArrow:
                    focusedObject.CharRepresentation = '▼'; // Change character representation
                    CollisionSituation(focusedObject, 0, 1); // Move down
                    break;
                case ConsoleKey.LeftArrow:
                    focusedObject.CharRepresentation = '◄'; // Change character representation
                    CollisionSituation(focusedObject, -1, 0); // Move left
                    break;
                case ConsoleKey.RightArrow:
                    focusedObject.CharRepresentation = '►'; // Change character representation
                    CollisionSituation(focusedObject, 1, 0); // Move right
                    break;
                default:
                    break;
            }

        }

        // Method to handle collision situations
        void CollisionSituation(GameObject player, int dx, int dy)
        {
            int nextPlayerPosX = player.PosX + dx; // Calculate next X position
            int nextPlayerPosY = player.PosY + dy; // Calculate next Y position

            // Get the object at the next position
            GameObject nextObject = engine.GetMap().Get(nextPlayerPosY, nextPlayerPosX);

            // PLAYER hits BOX
            if (nextObject is Box box) {
                // Calculate next position of the box
                int nextBoxPosX = box.PosX + dx;
                int nextBoxPosY = box.PosY + dy;

                // Get the position behind the box
                GameObject posBehindBox = engine.GetMap().Get(nextBoxPosY, nextBoxPosX);

                // BOX hits OBSTACLE
                if (posBehindBox is ICollidable) {
                    engine.RestoreMap(); // Restore the map if there is an obstacle
                }
                else {
                    box.Move(dx, dy); // Move the box
                    player.Move(dx, dy); // Move the player
                }
            }

            // PLAYER hits KEY
            else if (nextObject is Key key) {
                int nextKeyPosX = key.PosX + dx;
                int nextKeyPosY = key.PosY + dy;
                GameObject posBehindKey = engine.GetMap().Get(nextKeyPosY, nextKeyPosX);

                // KEY hits OBSTACLE
                if (posBehindKey is Wall or Box) {
                    engine.RestoreMap(); // Restore the map if there is an obstacle
                }

                // KEY hits DOOR
                else if (posBehindKey is Door) {
                    engine.gameObjects.RemoveAll(obj => obj is Door or Key); // Remove all doors and keys
                }
                else {
                    key.Move(dx, dy); // Move the key
                    player.Move(dx, dy); // Move the player
                }
                
            }

            // PLAYER hits WALL or DOOR
            else if (nextObject is Wall or Door) {
                engine.RestoreMap(); // Restore the map if there is a wall or door
            }

            // PLAYER hits WITCH
            else if (nextObject is Witch) {
                engine.DialogWitch(); // Start dialog with the witch
            }
            else {
                player.Move(dx, dy); // Move the player if there is no obstacle
            }
        }
        
    }

}

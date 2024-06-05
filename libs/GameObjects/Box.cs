namespace libs;

public class Box : GameObject, ICollidable {

    public Box () : base(){
        Type = GameObjectType.Box;
        CharRepresentation = '○';
        Color = ConsoleColor.DarkGreen;
    }
}
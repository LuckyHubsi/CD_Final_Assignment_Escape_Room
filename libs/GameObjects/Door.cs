namespace libs;

public class Door : GameObject, ICollidable {
    public Door () : base() {
        this.Type = GameObjectType.Door;
        this.CharRepresentation = 'd';
        this.Color = ConsoleColor.Red;
    }
}
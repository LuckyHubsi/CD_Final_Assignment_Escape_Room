namespace libs;

public class Wall : GameObject, ICollidable {
    public Wall () : base() {
        this.Type = GameObjectType.Wall;
        this.CharRepresentation = '█';
        this.Color = ConsoleColor.Cyan;
    }
}
namespace libs;

public class Queen : GameObject {
    public Queen () : base() {
        this.Type = GameObjectType.Queen;
        this.CharRepresentation = '☻';
    }
}
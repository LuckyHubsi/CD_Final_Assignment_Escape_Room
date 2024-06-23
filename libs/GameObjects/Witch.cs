namespace libs;

public class Witch : GameObject, ICollidable {
    public static Dialog dialog; // Change to static to access it in GameEngine

    public Witch () : base() {
        this.Type = GameObjectType.Wall;
        this.CharRepresentation = 'W';
        this.Color = ConsoleColor.Cyan;

        DialogNode node1 = new DialogNode(FileHandler.GetDialog("WitchMessage1"));
        DialogNode node2 = new DialogNode(FileHandler.GetDialog("WitchMessage2-L"));
        DialogNode node3 = new DialogNode(FileHandler.GetDialog("WitchMessage2-R"));

        node1.AddResponse(FileHandler.GetDialog("Msg1-L"), node2);
        node1.AddResponse(FileHandler.GetDialog("Msg1-R"), node3);

        dialog = new Dialog(node1);
    }
}
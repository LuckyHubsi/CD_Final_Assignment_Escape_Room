namespace libs
{
    public class Key : GameObject, ICollidable
    {
        public Key () : base(){
            Type = GameObjectType.Key;
            CharRepresentation = 'k';
            Color = ConsoleColor.DarkGreen;
        }
    }
}
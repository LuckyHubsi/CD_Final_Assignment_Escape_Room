namespace libs;

public class GameObjectFactory : IGameObjectFactory
{

    public GameObject CreateGameObject(dynamic obj) {

        GameObject newObj = new GameObject();
        int type = obj.Type;

        switch (type)
        {
            case (int) GameObjectType.Player:
                newObj = obj.ToObject<Player>();
                break;
            case (int) GameObjectType.Wall:
                newObj = obj.ToObject<Wall>();
                break;
            case (int) GameObjectType.Box:
                newObj = obj.ToObject<Box>();
                break;
            case (int) GameObjectType.Door:
                newObj = obj.ToObject<Door>();
                break;
            case (int) GameObjectType.Key:
                newObj = obj.ToObject<Key>();
                break;
            case (int) GameObjectType.Witch:
                newObj = obj.ToObject<Witch>();
                break;
        }

        return newObj;
    }
}
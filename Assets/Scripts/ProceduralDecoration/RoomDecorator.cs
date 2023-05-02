public static class RoomDecorator
{
    public static void DecorateRoom(Room room, EnvironmentType environmentType)
    {
        DecorationVolume pdv = new DecorationVolume(room)
        {
            environmentType = EnvironmentType.Forest,
            contentType = ContentType.Tree
        };
    }
}
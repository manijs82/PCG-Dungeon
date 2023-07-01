public static class RoomDecorator
{
    public static DecorationVolume DecorateRoom(Room room, EnvironmentType environmentType)
    {
        DecorationVolume pdv = new DecorationVolume()
        {
            environmentType = EnvironmentType.Forest,
            contentType = ContentType.Tree
        };
        pdv.Init(room);

        return pdv;
    }
}
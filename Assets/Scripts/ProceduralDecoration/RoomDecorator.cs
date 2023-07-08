using UnityEngine;

public static class RoomDecorator
{
    public static DecorationVolume DecorateRoom(Room room, DecorationVolumeHierarchy volumeHierarchy)
    {
        var clone = Object.Instantiate(volumeHierarchy);
        var rootPdv = clone.volumeHierarchy.Root.Value;
        rootPdv.Init(room);

        return rootPdv;
    }
}
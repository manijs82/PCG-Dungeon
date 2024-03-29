﻿using System;
using UnityEngine;

[Serializable]
public class DungeonParameters : SampleParameters
{
    public Vector2Int roomCountRange;
    public Vector2Int roomWidthRange;
    public int width;
    public int height;
    public float desiredRoomDistance;
    [Space] 
    public RoomTypeLayout roomTypeLayout;
    public RiverProperties riverProperties;
}
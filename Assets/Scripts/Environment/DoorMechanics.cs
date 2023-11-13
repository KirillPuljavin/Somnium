using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InRoomPos
{
    Top,
    Bottom,
    Left,
    Right
}
public class DoorPlacement
{
    public InRoomPos position;
    public int roomIndex;
    public Vector2 transformPosition;
}

public class DoorMechanics : MonoBehaviour
{
    public Vector2 targetDoorPos;
    public int roomIndex;
}

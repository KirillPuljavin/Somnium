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
}

public class DoorMechanics : MonoBehaviour
{

    void Start()
    {

    }
}

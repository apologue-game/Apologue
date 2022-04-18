using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public string roomName { get; set; }
    public int topLeftCoordinate { get; set; }
    public int topRightCoordinate { get; set; }
    public int bottomLeftCoordinate { get; set; }
    public int bottomRightCoordinate { get; set; }
}

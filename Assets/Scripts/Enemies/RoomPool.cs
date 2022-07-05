using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Profiles/Room Pool")]
public class RoomPool : ScriptableObject
{
    public GameObject StartingRoom;
    public List<GameObject> Rooms = new List<GameObject>();
    public List<GameObject> BossRooms = new List<GameObject>();
}

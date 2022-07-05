using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Profiles/Enemy Pool")]
public class EnemyPool : ScriptableObject
{
    public List<GameObject> Spawners = new List<GameObject>();
}

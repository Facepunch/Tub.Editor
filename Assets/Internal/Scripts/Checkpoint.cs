using UnityEngine;

[SelectionBase]
public class Checkpoint : MonoBehaviour
{
    public bool Unlocked;
    public bool IsEntryPoint;
    public bool FreeSpawn;

    public Transform[] SpawnPoints;
}

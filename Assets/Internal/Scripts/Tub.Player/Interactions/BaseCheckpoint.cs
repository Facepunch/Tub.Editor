using Facepunch;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BaseCheckpoint : Networked
{
    public bool Unlocked;
    public bool IsEntryPoint;
    public bool FreeSpawn;

    public Transform[] SpawnPoints;
}
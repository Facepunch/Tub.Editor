using Facepunch;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent( typeof( Networker ) )]
public class TubLevel : Networked
{
    public MissionDef Mission;
    public Collectible.Information[] Collectibles;

    [Header( "ProgressMeasure" )]
    public bool CollectAllCollectables;

    public void RecountCollectibles()
    {
        Collectibles = gameObject.scene.GetRootGameObjects()
                                .SelectMany( x => x.GetComponentsInChildren<Collectible>() )
                                .Select( x => x.BuildInformation() )
                                .OrderBy( x => System.Guid.NewGuid() )
                                .ToArray();
    }

#if UNITY_EDITOR

[UnityEditor.Callbacks.PostProcessScene]
    public static void ProcessScene()
    {
        var tubLevel = Object.FindObjectOfType<TubLevel>();
        if ( tubLevel  )
        {
            tubLevel.RecountCollectibles();
        }
    }

#endif
}


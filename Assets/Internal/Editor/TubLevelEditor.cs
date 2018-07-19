using UnityEngine;
using System.Linq;
using UnityEditor;

[CustomEditor( typeof( Tub.TubLevel ) )]
public class TubLevelEditor : Editor
{
    Tub.TubLevel Target
    {
        get { return target as Tub.TubLevel; }
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
    }

#if UNITY_EDITOR

    [UnityEditor.Callbacks.PostProcessScene]
    public static void ProcessScene()
    {
        var tubLevel = Object.FindObjectOfType<Tub.TubLevel>();
        if ( tubLevel )
        {
            RecountCollectibles( tubLevel );
        }
    }

    public static void RecountCollectibles( Tub.TubLevel level )
    {
        level.Collectibles = level.gameObject.scene.GetRootGameObjects()
                                .SelectMany( x => x.GetComponentsInChildren<Tub.Collectable>() )
                                .Select( x => new Tub.CollectableInformation
                                {
                                    Coin = x,
                                    Sprite = x.HudIcon
                                } )
                                .OrderBy( x => System.Guid.NewGuid() )
                                .ToArray();
    }

#endif
}

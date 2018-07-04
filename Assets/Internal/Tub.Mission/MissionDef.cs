using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Video;

[CreateAssetMenu( menuName = "Tub/Mission") ]
public class MissionDef : ScriptableObject
{
    public string Identifier;
    public string Name;

    [TextArea]
    public string Description;

    [TextArea]
    public string Help;

    //public SceneField FirstScene;

    [Tooltip( "Should be 512x256" )]
    public Sprite TileImage;

    [Tooltip( "Should be 2048x2048" )]
    public Sprite HeadlineImage;

    [Tooltip( "Should be 1024x1024" )]
    public VideoClip HeadlineVideo;
}


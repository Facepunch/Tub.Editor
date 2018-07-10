using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectAnimation : MonoBehaviour
{
    public AnimationClip AnimationClip;

    private void Reset()
    {
        AnimationClip = null;
    }


    [ContextMenu( "Toggle Animator Visible" )]
    void ToggleAnimatorVisible()
    {
        var anim = GetComponent<Animation>();

        if ( anim.hideFlags == HideFlags.None )
            anim.hideFlags = HideFlags.HideInInspector;
        else
            anim.hideFlags = HideFlags.None;
    }
}

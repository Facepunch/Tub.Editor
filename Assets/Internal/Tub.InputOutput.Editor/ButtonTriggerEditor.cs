using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Tub.InputOutput
{
    [CustomEditor( typeof( ButtonTrigger ) ), CanEditMultipleObjects]
    public class ButtonTriggerEditor : ButtonEditor
    {
        ButtonTrigger Button { get { return target as ButtonTrigger; } }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
        }
    }

}

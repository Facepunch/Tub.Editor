using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tub.InputOutput
{
    [AddComponentMenu( "Tub.IO/Button" )]
    public class Button : Boolean
    {
        public enum PressType
        {
            ToggleOnPress,
            ToggleOnRelease,
            OnWhileHeld,
            OnWithTimedReset
        }

        public PressType Type;
        public float RepeatTime = 0.2f;
        public float DelayBeforeReset = 0.0f;
        TimeSince timeSincePressed;
    }
}
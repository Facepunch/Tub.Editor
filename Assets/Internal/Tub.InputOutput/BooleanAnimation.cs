using Facepunch;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Tub.InputOutput
{
    public class BooleanAnimation : Boolean
    {
        public Boolean Boolean;
        public Animation Animation;

        public AnimationClip OnClip;
        public AnimationClip OffClip;

        public bool AllowInterupt;
        public bool CurrentValue;
    }
}
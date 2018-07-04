using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Facepunch
{
    [AttributeUsage( AttributeTargets.Class, AllowMultiple =true, Inherited = true )]
    public class TriggersEventAttribute : Attribute
    {
        public string TriggerName;
        public string Description;

        public TriggersEventAttribute( string name, string description = "" )
        {
            TriggerName = name;
            Description = description;
        }
    }
}
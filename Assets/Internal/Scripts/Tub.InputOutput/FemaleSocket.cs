using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Facepunch;
using System;

namespace Tub.InputOutput
{
    [RequireComponent( typeof( Networker ) )]
    [AddComponentMenu( "Tub.IO/Female Socket" )]
    public class FemaleSocket : Boolean
    {
        public bool CanMountAnything;
        public string[] MountableIdentifiers;
        public string[] AcceptableIdentifiers;
        public Transform MountPoint;

        public string[] InfluenceTags;
        public float InfluenceSpeedMultiplier = 1.0f;

        public Boolean Lock;
        public bool LockDirection;

        public MaleSocket CurrentMale;

        public void UpdateMountedPosition()
        {
            if ( CurrentMale == null ) return;

            CurrentMale.transform.SetPositionAndRotation( MountPoint.position, MountPoint.rotation );
        }
    }



}
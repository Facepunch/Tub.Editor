using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Facepunch
{
    public class DestroyInSeconds : MonoBehaviour
    {
        public float TimeToDie = 5.0f;
        public float TimeToDieVariance = 0;

        void Start()
        {
            GameObject.Destroy( gameObject, TimeToDie + Random.Range( TimeToDieVariance * -0.5f, TimeToDieVariance * 0.5f ) );
        }
    }
}
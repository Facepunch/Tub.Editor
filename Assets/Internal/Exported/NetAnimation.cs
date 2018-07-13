using UnityEngine;
using Facepunch;
using System.Collections.Generic;


public class NetAnimation : Networked, INetworkStatic
{
   public AnimationClip AnimationClip;
   public float Speed;
   public float Delta;

   //
   // Methods here stubbed so you can hook them up with the Unity event stuff
   //

   public void GetComponents()
   {
   }

   public void UpdateToTime( float time,bool triggerevents )
   {
   }

   public virtual void TriggerEvents( float mintime,float maxtime )
   {
   }

   public virtual void SetClip( string name )
   {
   }

}

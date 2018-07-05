using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Facepunch
{
    [DisallowMultipleComponent]
    public sealed class Networker : MonoBehaviour
    {
        public enum ViewSynchronization { Off, ReliableDeltaCompressed, Unreliable, UnreliableOnChange }
        public enum OwnershipOption { Fixed, Takeover, Request }

        public int ownerId;
        public byte group = 0;
        public bool OwnerShipWasTransfered;
        public int prefixBackup = -1;
        public ViewSynchronization synchronization;
        public OwnershipOption ownershipTransfer = OwnershipOption.Fixed;
        public int viewIdField = 0;
        public int viewID { get { return viewIdField; } set { viewIdField = value; } }

        public bool SyncTransforms = true;
        public bool Frozen = false;
        public int instantiationId;
        public List<Component> ObservedComponents;

#if UNITY_EDITOR
           
        public void OnValidate()
        {
            if ( ObservedComponents == null )
                ObservedComponents = new List<Component>();

            var c = ObservedComponents.Sum( x => x?.GetHashCode() ?? 0 );

            var observables = GetComponents<IPunObservable>();
            ObservedComponents.Clear();
            ObservedComponents.AddRange( observables.Select( x => x as Component ) );

            if ( !SyncTransforms )
            {
                ObservedComponents.Remove( this );
            }

            if ( c != ObservedComponents.Sum( x => x.GetHashCode() ) )
            {
                UnityEditor.EditorUtility.SetDirty( gameObject );
            }

            if ( GetComponent<INetworkTakeover>() != null && ownershipTransfer != OwnershipOption.Takeover )
            {
                ownershipTransfer = OwnershipOption.Takeover;
                UnityEditor.EditorUtility.SetDirty( gameObject );
            }

            if ( GetComponent<INetworkTakeover>() == null && ownershipTransfer != OwnershipOption.Fixed )
            {
                ownershipTransfer = OwnershipOption.Fixed;
                UnityEditor.EditorUtility.SetDirty( gameObject );
            }

            if ( ObservedComponents.Count > 0 && synchronization != ViewSynchronization.UnreliableOnChange )
            {
                synchronization = ViewSynchronization.UnreliableOnChange;
                UnityEditor.EditorUtility.SetDirty( gameObject );
            }

            if ( ObservedComponents.Count == 0 && synchronization != ViewSynchronization.Off )
            {
                synchronization = ViewSynchronization.Off;
                UnityEditor.EditorUtility.SetDirty( gameObject );
            }

            if ( SyncTransforms && GetComponent<INetworkStatic>() != null )
            {
                SyncTransforms = false;
                UnityEditor.EditorUtility.SetDirty( gameObject );
            }
        }
            
#endif
        }
}



public interface IPunObservable
{

}
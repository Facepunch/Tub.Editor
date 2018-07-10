using UnityEngine;
using Facepunch;
using System.Collections.Generic;


namespace Facepunch
{
	public class Networker : MonoBehaviour
	{
	   public bool SyncTransforms;
	   public bool Frozen;

	   //
	   // PhotonView
	   //
	   public int ownerId;
	   public byte group;
	   public bool OwnerShipWasTransfered;
	   public int prefixBackup;
	   public ViewSynchronization synchronization;
	   public OwnershipOption ownershipTransfer;
	   public List<Component> ObservedComponents;
	   public bool ObservedComponentsFoldoutOpen;
	   public int viewIdField;
	   public int instantiationId;
	   public int currentMasterID;
	   public bool isRuntimeInstantiated;
	}
}

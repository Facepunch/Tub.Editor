using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Facepunch;

public class Collectible : Networked
{
    public GameObject PickupEffect;
    public Sprite HudIcon;

    [System.Serializable]
    public struct Information
    {
        public Collectible Coin;
        public Sprite Sprite;
    }

    public Information BuildInformation()
    {
        return new Information
        {
            Coin = this,
            Sprite = HudIcon
        };
    }
}

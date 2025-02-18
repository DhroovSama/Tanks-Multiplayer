using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

#region XML Documentation
/// <summary>
/// Manages the player's coin balance on the network.
/// Detects collisions with collectible coins and updates the total coin count accordingly.
/// Ensures that coin collection logic is handled only by the server for proper synchronization.
/// </summary>
#endregion
public class CoinWallet : NetworkBehaviour
{
    public NetworkVariable<int> TotalCoins = new NetworkVariable<int>();

    private void OnTriggerEnter2D(Collider2D col)
    {
        if(!col.TryGetComponent<Coin>(out Coin coin)) { return; }

        int coinValue = coin.Collect();


        if (!IsServer) {  return; }

        TotalCoins.Value += coinValue;
    }
}

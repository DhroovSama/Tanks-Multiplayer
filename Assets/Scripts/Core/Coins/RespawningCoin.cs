using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region XML Documentation
/// <summary>
/// Inherits from the Coin class and represents a coin that can respawn after being collected.
/// Handles collection logic, ensuring that only the server can process coin collection.
/// Prevents multiple collections if the coin has already been collected.
/// </summary>
#endregion

public class RespawningCoin : Coin
{
    public event Action<RespawningCoin> onCollected;

    private Vector3 previousPosition;

    private void Update()
    {
        if(previousPosition != transform.position)
        {
            Show(true);
        }

        previousPosition = transform.position;
    }

    public override int Collect()
    {
        if(!IsServer)
        {
            Show(false); 
            return 0;
        }

        if(alreadyCollected) { return 0; }

        alreadyCollected = true;

        onCollected?.Invoke(this);

        return coinValue;
    }

    public void Reset()
    {
        alreadyCollected = false;
    }
}

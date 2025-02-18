using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

#region XML Documentation
/// <summary>
/// Abstract base class representing a collectible coin in the game.
/// Manages the coin's value, visibility, and collection state.
/// Derived classes must implement the Collect() method to define specific collection behavior.
/// </summary>
#endregion

public abstract class Coin : NetworkBehaviour
{
    [SerializeField]
    private SpriteRenderer spriteRenderer;

    protected int coinValue = 10; 

    protected bool alreadyCollected;

    public abstract int Collect();

    public void SetValue(int value)
    {
        coinValue = value;
    }

    protected void Show(bool show)
    {
        spriteRenderer.enabled = show;  
    }
}

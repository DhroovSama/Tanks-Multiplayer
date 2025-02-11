using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Netcode;
using UnityEngine;

#region XML Documentation
/// <summary>
/// Manages the health of a networked game object, including taking damage,
/// restoring health, and handling death events.
/// </summary> 
#endregion
public class Health : NetworkBehaviour
{
    [field: SerializeField, Tooltip("The maximum health of the game object.")]
    public int MaxHealth { get; private set; } = 100;

    public NetworkVariable<int> currentHealth = new NetworkVariable<int>();

    private bool isDead;

    public Action<Health> onDie;

    public override void OnNetworkSpawn()
    {
        if (!IsServer) { return; }

        currentHealth.Value = MaxHealth;
    }

    public void TakeDamage(int damageValue)
    {
        ModifyHealth(-damageValue);
    }

    public void RestoreHealth(int healValue)
    {
        ModifyHealth(healValue);
    }

    private void ModifyHealth(int value)
    {
        if (isDead) { return; }

        int newHealth = currentHealth.Value + value;

        currentHealth.Value = Mathf.Clamp(newHealth, 0, MaxHealth);

        if (currentHealth.Value == 0)
        {
            onDie?.Invoke(this);
            isDead = true;
        }
    }
}

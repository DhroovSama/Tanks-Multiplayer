using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

#region XML Documentation
/// <summary>
/// Manages the Health Display from the Health component
/// on the player worldspace canvas
/// </summary> 
#endregion
public class HealthDisplay : NetworkBehaviour
{
    [Header("Refrences")]

    [SerializeField]
    private Health health;

    [SerializeField]
    private Image healthBarImage;

    public override void OnNetworkSpawn()
    {
        if(!IsClient) return;

        health.currentHealth.OnValueChanged += HandleHealthChanged;

        HandleHealthChanged(0, health.currentHealth.Value);
    }

    public override void OnNetworkDespawn()
    {
        if (!IsClient) return;

        health.currentHealth.OnValueChanged -= HandleHealthChanged;
    }

    private void HandleHealthChanged(int oldHealth, int newHealth)
    {
        healthBarImage.fillAmount = (float)newHealth/health.MaxHealth;
    }
}

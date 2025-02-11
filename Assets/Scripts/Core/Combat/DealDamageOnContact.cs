using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

#region XML Documentation
/// <summary>
/// Handles the logic for dealing damage to other objects upon collision.
/// This script checks for ownership to prevent self fire and applies damage
/// to objects with a Health component.
/// </summary>
#endregion

public class DealDamageOnContact : MonoBehaviour
{
    [SerializeField]
    private int damage = 5;

    private ulong ownerClientID;

    public void SetOwner(ulong ownerClientID)
    {
        this.ownerClientID = ownerClientID;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.attachedRigidbody == null) {  return; }    

        if(collision.attachedRigidbody.TryGetComponent<NetworkObject>(out NetworkObject netObj))
        {
            if(ownerClientID == netObj.OwnerClientId)
            {
                return;
            }
        }

        if(collision.attachedRigidbody.TryGetComponent<Health>(out Health health))
        {
            health.TakeDamage(damage);
        }
    }
}

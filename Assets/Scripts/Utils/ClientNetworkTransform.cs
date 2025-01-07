using System.Collections;
using System.Collections.Generic;
using Unity.Netcode.Components;
using UnityEngine;

#region XML documentation

/// <summary>
/// Custom implementation of the <see cref="NetworkTransform"/> class to provide client authority.
/// Allows the owner of the object to commit transform updates to the server.
/// </summary>
#endregion
public class ClientNetworkTransform : NetworkTransform
{
    #region XML documentation
    /// <summary>
    /// Called when the object is spawned on the network.
    /// Sets the transform commit authority based on ownership.
    /// </summary> 
    #endregion
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        // Only the owner of the object can commit transform updates.
        CanCommitToTransform = IsOwner;
    }

    // Updates the transform on each frame.
    // Checks if the client can commit the transform and sends updates to the server if allowed.
    
    protected override void Update()
    {
        // Ensure transform updates are committed only by the owner.
        CanCommitToTransform = IsOwner;

        base.Update();

        // Verify network state and commit transform updates if applicable.
        if (!IsHost && NetworkManager != null && NetworkManager.IsConnectedClient && CanCommitToTransform)
        {
            TryCommitTransformToServer(transform, NetworkManager.LocalTime.Time);
        }
    }

    #region XML documentation
    /// <summary>
    /// Specifies that the transform is not server-authoritative.
    /// This enables client-side authority for transform updates.
    /// </summary>
    /// <returns>Returns <c>false</c> indicating the server is not authoritative over this transform.</returns> 
    #endregion
    protected override bool OnIsServerAuthoritative()
    {
        return false;
    }
}
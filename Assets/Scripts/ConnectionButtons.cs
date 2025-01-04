using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;

#region XML documentation

/// <summary>
/// Provides methods to start a network host or client.
/// This script uses Unity Netcode's <see cref="NetworkManager"/> for network operations.
/// </summary>
#endregion
public class ConnectionButtons : MonoBehaviour
{
    #region XML Documentation
    /// <summary>
    /// Starts the application as a network host.
    /// A host is both a server and a client that can accept connections.
    /// </summary> 
    #endregion
    public void StartHost()
    {
        NetworkManager.Singleton.StartHost();
    }

    #region XML Documentation
    /// <summary>
    /// Starts the application as a network client.
    /// A client connects to an existing server/host.
    /// </summary> 
    #endregion
    public void StartClient()
    {
        NetworkManager.Singleton.StartClient();
    }
}
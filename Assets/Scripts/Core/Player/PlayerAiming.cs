using System.Collections;
using UnityEngine;
using Unity.Netcode;

#region XML Documentation
/// <summary>
/// Manages player aiming mechanics by updating the turret's orientation
/// based on the mouse position. This class is network-aware and works
/// only for the owning client.
/// </summary> 
#endregion
public class PlayerAiming : NetworkBehaviour
{
    [SerializeField, Tooltip("The InputReader ScriptableObject that provides input data.")]
    private InputReader inputReader;

    [SerializeField, Tooltip("The transform of the turret that needs to be rotated based on the mouse position.")]
    private Transform turretTransform;

    #region XML Documentaion
    /// <summary>
    /// Stores the last known position of the mouse in world coordinates.
    /// </summary> 
    #endregion
    private Vector2 mouseLastPosition;

    private void LateUpdate()
    {
        // Check if the current client is the owner of this object
        if (!IsOwner) return;

        ChangeTurretAimPosition(inputReader.AimPosition);
    }

    #region XML Documentation
    /// <summary>
    /// Updates the turret's rotation to face the specified mouse position.
    /// </summary>
    /// <param name="playerMouseLastPosition">The position of the mouse on the screen.</param> 
    #endregion
    private void ChangeTurretAimPosition(Vector2 playerMouseLastPosition)
    {
        // Convert screen coordinates to world coordinates
        mouseLastPosition = Camera.main.ScreenToWorldPoint(playerMouseLastPosition);

        // Calculate and set the turret's rotation
        turretTransform.up = new Vector2(
            mouseLastPosition.x - turretTransform.position.x,
            mouseLastPosition.y - turretTransform.position.y
        );
    }
}

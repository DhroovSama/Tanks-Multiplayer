using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

#region XML Documentation
/// <summary>
/// Handles player movement, including translation and rotation.
/// </summary> 
#endregion
public class PlayerMovement : NetworkBehaviour
{
    #region References

    [Header("References")]

    [SerializeField]
    private InputReader inputReader;

    [SerializeField]
    private Transform bodyTransform;

    [SerializeField]
    private Rigidbody2D tankRb;

    #endregion

    #region Settings

    [Header("Settings")]

    [SerializeField, Range(0f, 100f), Tooltip("Movement speed of the player, measured in units per second.")]
    private float movementSpeed;

    [SerializeField, Range(0f, 360f), Tooltip("Turning rate of the player, measured in degrees per second.")]
    private float turningRate;

    #endregion

    //Stores the movement input received from the player.
    private Vector2 previousMovementInput;

    public override void OnNetworkSpawn()
    {
        if (!IsOwner) return;

        // Subscribe to the MoveEvent from the input reader
        inputReader.MoveEvent += HandleMove;
    }

    public override void OnNetworkDespawn()
    {
        if (!IsOwner) return;

        // Unsubscribe from the MoveEvent to prevent memory leaks
        inputReader.MoveEvent -= HandleMove;
    }

    private void Update()
    {
        if (!IsOwner) return;

        // Calculate the rotation based on horizontal input
        float zRotation = previousMovementInput.x * -turningRate * Time.deltaTime;

        // Apply the rotation to the player's body
        bodyTransform.Rotate(0f, 0f, zRotation);
    }

    private void FixedUpdate()
    {
        if (!IsOwner) return;

        // Calculate the velocity based on vertical input and apply it to the Rigidbody2D
        tankRb.velocity = bodyTransform.up * previousMovementInput.y * movementSpeed;
    }

    #region XML Documentation
    /// <summary>
    /// Event handler for movement input.
    /// Updates the stored movement input values based on player input.
    /// </summary>
    /// <param name="movementInput">A Vector2 representing the movement input, where x is for rotation and y is for forward/backward movement.</param> 
    #endregion
    public void HandleMove(Vector2 movementInput)
    {
        previousMovementInput = movementInput;
    }
}

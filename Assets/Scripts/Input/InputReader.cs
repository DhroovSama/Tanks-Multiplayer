using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static Controls;

#region XML Documentation
/// <summary>
/// A ScriptableObject that listens for player input actions using Unity's Input System.
/// This class implements <see cref="IPlayerActions"/> and translates input events into
/// events that other parts of the game can subscribe to.
/// </summary> 
#endregion
[CreateAssetMenu(fileName = "New Input Reader", menuName = "Input/InputReader")]
public class InputReader : ScriptableObject, IPlayerActions
{
    #region XML Documentation
    /// <summary>
    /// Invoked when the player provides movement input.
    /// The passed <see cref="Vector2"/> represents the direction and magnitude
    /// of movement on the XY-plane.
    /// </summary> 
    #endregion
    public event Action<Vector2> MoveEvent;

    #region XML Documentation 
    /// <summary>
    /// Invoked when the player presses or releases the primary fire button.
    /// The boolean parameter is <c>true</c> when the fire button is pressed (performed),
    /// and <c>false</c> when it is released (canceled).
    /// </summary> 
    #endregion
    public event Action<bool> PrimaryFireEvent;

    #region XML Documentation
    /// <summary>
    /// Reference to the generated control asset from the Unity Input System.
    /// </summary> 
    #endregion
    private Controls controls;

    #region XML Documentation
    /// <summary>
    /// Called when the ScriptableObject is enabled. Initializes the <see cref="controls"/> 
    /// if it is null and sets up callbacks for player actions.
    /// </summary> 
    #endregion
    private void OnEnable()
    {
        if (controls == null)
        {
            controls = new Controls();

            // Assign this instance as the callback handler for player actions
            controls.Player.SetCallbacks(this);
        }
    }

    #region XML Documentation
    /// <summary>
    /// Callback method for the Move action. This method reads the input value as a Vector2
    /// and invokes the <see cref="MoveEvent"/> for any subscribers.
    /// </summary>
    /// <param name="context">The context of the current input action, which provides the raw input value.</param> 
    #endregion
    public void OnMove(InputAction.CallbackContext context)
    {
        // Read the 2D movement vector and notify subscribers
        MoveEvent?.Invoke(context.ReadValue<Vector2>());
    }

    #region XML Documentation
    /// <summary>
    /// Callback method for the Primary Fire action. This method checks if the action was performed or canceled 
    /// and invokes the <see cref="PrimaryFireEvent"/> with the appropriate state (<c>true</c> when performed, 
    /// <c>false</c> when canceled).
    /// </summary>
    /// <param name="context">The context of the current input action, which provides the action state.</param> 
    #endregion
    public void OnPrimaryfire(InputAction.CallbackContext context)
    {
        // Fire is pressed/held down
        if (context.performed)
        {
            PrimaryFireEvent?.Invoke(true);
        }
        // Fire is released
        else if (context.canceled)
        {
            PrimaryFireEvent?.Invoke(false);
        }
    }
}

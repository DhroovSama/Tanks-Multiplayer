using System.Collections;
using UnityEngine;

#region XML Documentation
/// <summary>
/// Manages the lifetime of a game object by destroying it after a specified time.
/// </summary> 
#endregion
public class Lifetime : MonoBehaviour
{
    [SerializeField, Tooltip("The lifetime of the game object in seconds.")]
    private float lifetime;

    private void OnEnable()
    {
        StartCoroutine(DestroyAfterSomeTime(lifetime));
    }

    #region XML Documentation
    /// <summary>
    /// Waits for the specified time before destroying the object.
    /// </summary>
    /// <param name="time">The time to wait before destruction.</param> 
    #endregion
    private IEnumerator DestroyAfterSomeTime(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(this.gameObject);
    }
}

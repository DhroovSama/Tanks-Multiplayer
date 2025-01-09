using UnityEngine;

#region XML Documentation
/// <summary>
/// Destroys the game object when it collides with another object.
/// </summary> 
#endregion
public class DestroySelfOnContact : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(this.gameObject);
    }
}

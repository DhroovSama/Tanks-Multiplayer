using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

#region XML Documentation
/// <summary>
/// Handles projectile launching functionality in a networked environment.
/// </summary> 
#endregion
public class ProjectileLauncher : NetworkBehaviour
{
    #region Refrences
    [Header("References")]
    [SerializeField, Tooltip("Reference to the input reader for handling user input.")]
    private InputReader inputReader;

    [SerializeField, Tooltip("Transform where projectiles will spawn.")]
    private Transform projectileSpawnPoint;

    [SerializeField, Tooltip("Prefab for projectiles instantiated on the server.")]
    private GameObject serverProjectilePrefab;

    [SerializeField, Tooltip("Prefab for projectiles instantiated on clients.")]
    private GameObject clientProjectilePrefab;

    [SerializeField, Tooltip("")]
    private GameObject muzzleFlash;

    [SerializeField, Tooltip("")]
    private Collider2D playerCollider;
    #endregion

    #region Settings
    [Header("Settings")]

    [SerializeField, Tooltip("Speed of the projectile ")]
    private float projectileSpeed;

    [SerializeField, Tooltip("Rate of fire of the projectile")]
    private float fireRate;

    [SerializeField, Tooltip("How long should the muzzle flash appear for")]
    private float muzzleFlashDuration;
    #endregion

    private bool shouldFire; // Indicates whether the player is holding the fire button.

    private float previousFireTime;
    private float muzzleFlashTimer;

    public override void OnNetworkSpawn()
    {
        if (!IsOwner) { return; }

        // Subscribe to the fire event from the input reader.
        inputReader.PrimaryFireEvent += HandlePrimaryFire;
    }

    public override void OnNetworkDespawn()
    {
        if (!IsOwner) { return; }

        // Unsubscribe from the fire event to prevent memory leaks.
        inputReader.PrimaryFireEvent -= HandlePrimaryFire;
    }

    private void Update()
    {
        if (muzzleFlashTimer > 0f)
        {
            muzzleFlashTimer -= Time.deltaTime;

            if(muzzleFlashTimer <= 0f )
            {
                muzzleFlash.SetActive(false);
            }
        }

        if (!IsOwner) { return; }

        if (!shouldFire) { return; }

        // Checking if enough time has elapsed since the last fire.
        if (Time.time < (1 / fireRate + previousFireTime))
        {
            return;
        }

        // Notify the server to spawn a projectile.
        PrimaryFireServerRpc(projectileSpawnPoint.position, projectileSpawnPoint.up);

        // Spawn a dummy projectile locally for visual feedback.
        SpawnDummyProjectile(projectileSpawnPoint.position, projectileSpawnPoint.up);

        previousFireTime = Time.time;
    }

    #region XML Documentation
    /// <summary>
    /// Handles the fire input event.
    /// Updates whether the launcher should fire.
    /// </summary>
    /// <param name="shouldFire">True if the fire button is pressed, false otherwise.</param> 
    #endregion
    private void HandlePrimaryFire(bool shouldFire)
    {
        this.shouldFire = shouldFire;
    }

    #region XML Documentation
    /// <summary>
    /// RPC called on the server to handle projectile spawning.
    /// </summary>
    /// <param name="spawnPos">The position to spawn the projectile.</param>
    /// <param name="direction">The direction the projectile should move towards.</param> 
    #endregion
    [ServerRpc]
    private void PrimaryFireServerRpc(Vector3 spawnPos, Vector3 direction)
    {
        // Instantiate the server projectile at the specified position and direction.
        GameObject projectileInstance = Instantiate(
            serverProjectilePrefab,
            spawnPos,
            Quaternion.identity);

        projectileInstance.transform.up = direction;

        // Ignore collisions between the player's collider and their projectile.
        Physics2D.IgnoreCollision(playerCollider, projectileInstance.GetComponent<Collider2D>());

        if(projectileInstance.TryGetComponent<DealDamageOnContact>(out DealDamageOnContact dealDamage))
        {
            dealDamage.SetOwner(OwnerClientId);
        }

        if (projectileInstance.TryGetComponent<Rigidbody2D>(out Rigidbody2D rb))
        {
            rb.velocity = rb.transform.up * projectileSpeed;
        }

        // Notify all clients to spawn dummy projectiles for visual consistency.
        SpawnDummyProjectileClientRpc(spawnPos, direction);
    }

    #region XML Documentation
    /// <summary>
    /// RPC called on clients to spawn dummy projectiles.
    /// </summary>
    /// <param name="spawnPos">The position to spawn the dummy projectile.</param>
    /// <param name="direction">The direction the dummy projectile should move towards.</param> 
    #endregion
    [ClientRpc]
    private void SpawnDummyProjectileClientRpc(Vector3 spawnPos, Vector3 direction)
    {
        if (IsOwner) { return; }

        // Spawn a dummy projectile for non-owner clients.
        SpawnDummyProjectile(spawnPos, direction);
    }

    #region XML Documentation
    /// <summary>
    /// Instantiates a projectile locally for visual feedback.
    /// </summary>
    /// <param name="spawnPos">The position to spawn the projectile.</param>
    /// <param name="direction">The direction the projectile should move towards.</param> 
    #endregion
    private void SpawnDummyProjectile(Vector3 spawnPos, Vector3 direction)
    {
        muzzleFlash.SetActive(true);

        muzzleFlashTimer = muzzleFlashDuration;

        // Instantiate the client projectile at the specified position and direction.
        GameObject projectileInstance = Instantiate(
            clientProjectilePrefab,
            spawnPos,
            Quaternion.identity);

        projectileInstance.transform.up = direction;

        // Ignore collisions between the player's collider and their projectile.
        Physics2D.IgnoreCollision(playerCollider, projectileInstance.GetComponent<Collider2D>());

        if(projectileInstance.TryGetComponent<Rigidbody2D>(out Rigidbody2D rb))
        {
            rb.velocity = rb.transform.up * projectileSpeed;
        }
    }
}

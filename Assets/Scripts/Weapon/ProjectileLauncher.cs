using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ProjectileLauncher 
{
    public GameObject projectilePrefab = null;
    public float      cooldown = 0f;
    public float      speedFactor = 0f;
    public float      damage = 0f;

    public void LaunchProjectile(Vector2 basePosition, Vector2 baseVelocity, Vector2 worldDirection, float lifeSpawn, GameObject player)
    {
        if(null != projectilePrefab)
        {
            GameObject  projGO = GameObject.Instantiate(projectilePrefab);
            Projectile  projectile = projGO.GetComponent<Projectile>();
                
            if(null != projectile)
            {
                projGO.transform.position = basePosition;
                projectile.SetAttributes(baseVelocity, worldDirection, speedFactor, lifeSpawn, damage, player);
            }
            else
            {
                Debug.LogError("A Projectile component is missing on this gameobject, gameobject is destroyed");
            }
        }
    }
}

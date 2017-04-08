using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityLifeManagement : MonoBehaviour 
{
    [Range(0, 100)]
    [SerializeField]
    private float life;

    [SerializeField]
    private EntityBrain brain = null;

    private void Awake()
    {
        if(brain)
        {
            brain.OnApplyDamage += ApplyDamage;
        }
    }

    private void ApplyDamage(float damage)
    {
        life -= damage;

        if(life <=0)
        {
            if (gameObject.name.Contains("SpawnedPlayerWall") || gameObject.name.Contains("DestructibleObstacle"))
            {
                GetComponent<SpawnedWallDestruction>().DoDestruction();
            }
            else
            {
                Destroy(transform.root.gameObject);
            }

        }
    }
}

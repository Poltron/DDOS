using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour 
{
    [SerializeField]
    private GameObject EclaboussureFX;

    private float       m_speedFactor = 0f;
    private float       m_damage = 0f;
    private float       m_lifeSpawn = 0f;
    private Vector2     m_baseVelocity;
    private Vector2     m_worldDirection;
    private Rigidbody2D m_rb;
    private GameObject m_playerToIgnore;

    [SerializeField]
    private float       m_sinusFrequency = 0f;

    [SerializeField]
    private float       m_sinusAmplitude = 0f;

    private Vector2     m_startPos = Vector2.zero;
    private float       m_time = 0f;

    public delegate void ProjectileHitEvent(Vector2 hitPoints);

    public event ProjectileHitEvent OnProjectileHit = (Vector2 p) => { };

    public void SetAttributes(Vector2 baseVelocity, Vector2 worldDirection, float speedFactor, float lifeSpawn, float damage, GameObject player)
    {
        m_worldDirection = worldDirection.normalized;
        m_speedFactor = speedFactor;
        m_baseVelocity = baseVelocity;
        m_lifeSpawn = lifeSpawn;
        m_damage = damage;
        m_playerToIgnore = player;

        Vector3 target = gameObject.transform.position + new Vector3(m_worldDirection.x, m_worldDirection.y, 0);
        Vector3 pos = transform.position;

        target.z = 0f;
        pos.z = 0f;


        transform.right = -(target - pos);
        m_startPos = gameObject.transform.position;
    }

    private void Awake()
    {
        m_rb = GetComponent<Rigidbody2D>();
        SetAttributes(new Vector2(0, 0), m_worldDirection, m_speedFactor, 5, 0, null);
    }

    private void FixedUpdate()
    {
        m_time += (Time.deltaTime * m_speedFactor);

        Vector2 oldPos = m_rb.position;

        //calculating the new pos in x local space, but in x and y in world space
        Vector2 newPos = m_startPos + (m_time * m_worldDirection) + (m_baseVelocity * Time.deltaTime);
        
        //calculating the y offset
        float sinusFactor = Mathf.Sin(Time.time * m_sinusFrequency) * m_sinusAmplitude;

        Vector2 yOffset = Vector2.zero;

        yOffset = sinusFactor * (Vector2)transform.up;

        newPos += yOffset;

        //transform.LookAt(newPos);

        m_rb.MovePosition(newPos);

        Debug.DrawRay(newPos, m_baseVelocity * Time.deltaTime, Color.red);
    }

    private void Update()
    {
        m_lifeSpawn -= Time.deltaTime;

        if(m_lifeSpawn < 0)
        {
            OnProjectileHit(transform.position);
            Destroy(gameObject);
        }

       // Debug.DrawRay(m_startPos, m_worldDirection * 100, Color.red);
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject == m_playerToIgnore || coll.gameObject.layer == LayerMask.NameToLayer("Projectile"))
            return;

        OnProjectileHit(transform.root.position);
        Destroy(gameObject);

        Instantiate(EclaboussureFX, transform.position, Quaternion.identity);

        GameObject  hitGO = coll.gameObject;
        EntityBrain brain = hitGO.GetComponent<EntityBrain>();

        if(null != brain)
        {
            brain.ApplyDamage(m_damage);
        }
    }


}

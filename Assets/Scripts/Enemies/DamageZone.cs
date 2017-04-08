using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageZone : MonoBehaviour 
{
    [SerializeField]
    private bool                m_hasCooldown;

    [SerializeField]
    [Range(0.5f, 100)]
    private float               m_activationTime;

    [SerializeField]
    [Range(0.5f, 100)]
    private float               m_deactivationTime;

    [SerializeField]
    [Range(1, 100)]
    private float               m_damage;
    
    [SerializeField]
    private TriggerDetection    m_triggerDetection = null;

    [SerializeField]
    private GameObject m_sawSparkleFX = null;

    [SerializeField]
    private float m_sawSparkleFXRandomMin = 0f;

    [SerializeField]
    private float m_sawSparkleFXRandomMAx = 0f;

    [SerializeField]
    private float m_sawSparkleFXTime = 0f;

    private Timer               m_activationTimer = new Timer();
    private Timer               m_sparkleTImer = new Timer();
    private Timer               m_deactivationTimer = new Timer();
    private bool                m_isActivated = true;

    public delegate void VoidDel();
    public event VoidDel OnActivate = () => { };
    public event VoidDel OnDeactivate = () => { };

    private void Awake()
    {
        float delta = Mathf.Abs(m_deactivationTime - 0.001f);


        if (true == m_hasCooldown && delta <= 0.001f)
        {
            m_hasCooldown = false;
        }
    }

    // Update is called once per frame
    void Update () 
    {
        m_activationTimer.ElapseTime(Time.deltaTime);
        m_deactivationTimer.ElapseTime(Time.deltaTime);

        //apply damage if the activation timer is elapsing time
        if (true == m_activationTimer.HasElapsedAllTime())
        {
            ApplyDamage();
        }

        if(true == m_hasCooldown)
        {
            //both timers are at -1 time value, their time is elapsed
            if (true == m_activationTimer.HasElapsedAllTime() &&
                true == m_deactivationTimer.HasElapsedAllTime())
            {
                //the zone is applying damage, thus must be deactivated
                if (true == m_isActivated)
                {
                    m_deactivationTimer.SetTimer(m_deactivationTime);
                    OnDeactivate();
                    m_isActivated = false;
                }
                else
                {
                    m_activationTimer.SetTimer(m_activationTime);
                    OnActivate();
                    m_isActivated = true;
                }
            }
        }

        //only for the saws
        if(false == m_hasCooldown && m_sawSparkleFX)
        {
            m_sparkleTImer.ElapseTime(Time.deltaTime);

            if(true == m_sparkleTImer.HasElapsedAllTime())
            {
                float resTime = Random.Range(m_sawSparkleFXRandomMin, m_sawSparkleFXRandomMAx);

                GameObject sparklesGO = GameObject.Instantiate(m_sawSparkleFX, gameObject.transform.position, Quaternion.identity);
                ParticleSystem sparkles = sparklesGO.GetComponent<ParticleSystem>();

                sparklesGO.transform.Rotate(new Vector3(1, 0, 0), -90f);

                sparkles.Play();
                Destroy(sparklesGO, m_sawSparkleFXTime);

                m_sparkleTImer.SetTimer(resTime);
            }
        }
    }



    private void ApplyDamage()
    {
        List<GameObject> targets = null;

        m_triggerDetection.GetGameObjects(out targets);

        foreach(GameObject go in targets)
        {
            EntityBrain brain = go.GetComponent<EntityBrain>();

            //only for the saws
            if (false == m_hasCooldown)
            {
                Platformer2DUserControl controller = brain.GetComponent<Platformer2DUserControl>();

                if (false == controller.IsHit())
                {
                    GameObject sparklesGO = GameObject.Instantiate(m_sawSparkleFX, gameObject.transform.position, Quaternion.identity);
                    ParticleSystem sparkles = sparklesGO.GetComponent<ParticleSystem>();

                    sparklesGO.transform.Rotate(new Vector3(1, 0, 0), -90f);

                    sparkles.Play();
                    Destroy(sparklesGO, m_sawSparkleFXTime);
                }
            }


            brain.ApplyDamage(m_damage);
        }
    }
}

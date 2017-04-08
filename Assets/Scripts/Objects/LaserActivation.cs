using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserActivation : MonoBehaviour 
{
    [SerializeField]
    private DamageZone      m_damageZone;

    [SerializeField]
    private LineRenderer    m_lineRenderer;

    private void Awake()
    {
        if(null != m_damageZone && null != m_lineRenderer)
        {
            m_damageZone.OnActivate += OnActivateLaser;
            m_damageZone.OnDeactivate += OnDeactivateLser;
        }
    }

    private void OnActivateLaser()
    {
        m_lineRenderer.enabled = false;
    }

    private void OnDeactivateLser()
    {
        m_lineRenderer.enabled = true;
    }
}

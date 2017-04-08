using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Timer
{
    private float m_elapsedTime = 0f;

    public delegate void VoidDel();
    public event VoidDel OnEndTimer = () => { };

    public Timer()
    {
        m_elapsedTime = -1f;
    }

    public void SetTimer(float time)
    {
        m_elapsedTime = Mathf.Clamp(time, 0f, float.MaxValue);
    }

    public void Reset()
    {
        m_elapsedTime = -1f;
    }

    public void ElapseTime(float deltaTime)
    {
        if(0 < m_elapsedTime)
        {
            m_elapsedTime -= deltaTime;

            if(0 > m_elapsedTime)
            {
                OnEndTimer();
                m_elapsedTime = -1f;
            }
        }
    }

    public bool HasElapsedAllTime()
    {
        return 0 >= m_elapsedTime;
    }
}

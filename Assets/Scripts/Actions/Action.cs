using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action : MonoBehaviour
{
    [SerializeField]
    public Color32 color;

    [SerializeField]
    private float memoryCost;
    public float MemoryCost { get { return memoryCost; } }

    protected Platformer2DUserControl m_controller;

    public bool isHacked;
    private Timer hackTimer = new Timer();

    private bool isEnabled;
    public bool IsEnabled
    {
        get
        {
            return isEnabled;
        }

        set
        {
            // if value has changed
            if (!(value && isEnabled))
            {
                if (value)
                {
                    OnEnabled();
                }
                else
                {
                    OnDisabled();
                }

                isEnabled = value;
            }
        }
    }

    protected virtual void Awake()
    {
        m_controller = gameObject.transform.parent.GetComponent<Platformer2DUserControl>();
    }

    private void Start()
    {
    }

    public virtual void Tick()
    {
    }

    public virtual void Do()
    {
    }

    public virtual void OnEnabled()
    {
    }

    public virtual void OnDisabled()
    {
    }

    protected bool TryGetValidGamepad(out Gamepad gp)
    {
        bool res = false;

        res = null != m_controller ? m_controller.HasValidGamepad() : false;

        gp = true == res ? m_controller.GetGamepad() : null;

        return res;

    }

    public void HackedFor(float time)
    {
        Debug.Log("hack " + gameObject.name + " for " + time);
        isHacked = true;
        hackTimer.SetTimer(time);
        hackTimer.OnEndTimer += () => { isHacked = false; m_controller.GetComponent<Stack>().OnStackMemoryUpdate(); };
    }

    private void Update()
    {
        hackTimer.ElapseTime(Time.deltaTime);
    }
}

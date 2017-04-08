using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityBrain : MonoBehaviour 
{
    public delegate void FloatDel(float f);

    public event FloatDel OnApplyDamage = (float d) => {};
    public event FloatDel OnApplyHeal = (float d) => {};

    public void ApplyDamage(float damage)
    {
        OnApplyDamage(damage);
    }

    public void ApplyHeal(float heal)
    {
        OnApplyHeal(heal);
    }
}

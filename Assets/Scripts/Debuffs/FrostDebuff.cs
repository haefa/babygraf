using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrostDebuff : Debuff {

    private float slowingFactor;

    private bool applied;

    public FrostDebuff(float slowingFactor, float duration, Virus target) : base(target, duration)
    {
        this.slowingFactor = slowingFactor;
    }

    public override void Update()
    {
        if (target != null)
        {
            if(!applied)
            {
                applied = true;
                target.Speed -= (target.MaxSpeed * slowingFactor) / 100;
            }
        }
        base.Update();
    }

    public override void Remove()
    {
        //Debug.Log(target);
        //if(target == null)
        //Debug.Log(target.Speed);
        //Debug.Log(target.MaxSpeed);
        //if(target.Speed == null) 
        //if (target.Speed == null) Virus.Instance.Release();
        //else if (target == null) Virus.Instance.Release();
        //else if (target.MaxSpeed == null) Virus.Instance.Release();
        target.Speed = target.MaxSpeed;
        base.Remove();
     }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class StormDebuff : Debuff
{
    public StormDebuff (Virus target, float duration) : base(target,duration)
    {
        if (target != null)
        {
            target.Speed = 0;
        }
    }

    public override void Remove()
    {
        if(target != null)
        {
            target.Speed = target.MaxSpeed;
            base.Remove();
        }
    }

    public override void Update()
    {
      //  target.TakeDamage(1, Element.STORM);
        base.Update();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokoliTower : Tower {
    //FireTower
    [SerializeField]
    private float slowingFactor;

    public float SlowingFactor
    {
        get
        {
            return slowingFactor;
        }
    }

    [SerializeField]
    private float tickTime;

    [SerializeField]
    private float tickDamage;

    public float TickTime
    {
        get
        {
            return tickTime;
        }
    }

    public float TickDamage
    {
        get
        {
            return tickDamage;
        }
    }

    private void Start()
    {
        ElementType = Element.FIRE;

        Upgrades = new TowerUpgrade[]
        {
            new TowerUpgrade(2,2,.5f,5,-0.1f,1),
            new TowerUpgrade(5, 3, .5f, 5, -0.1f, 1),
        };
    }

    public override string GetStats()
    {
        if (NextUpgrade != null) //If the next is avaliable
        {
            return string.Format("<color=#00ffffff>{0}</color>{1}", "<size=20><b>Hujan</b></size> ", base.GetStats(), NextUpgrade.TickTime, NextUpgrade.SpecialDamage);
        }

        //Returns the current upgrade
        return string.Format("<color=#00ffffff>{0}</color>{1}", "<size=20><b>Hujan</b></size> ", base.GetStats());
    }

    public override void Upgrade()
    {
        this.tickTime -= NextUpgrade.TickTime;
        this.tickDamage += NextUpgrade.SpecialDamage;
        base.Upgrade();
    }


    public override Debuff GetDebuff()
    {
        return new FireDebuff(tickDamage,tickTime,DebuffDuration,Target);
    }

}

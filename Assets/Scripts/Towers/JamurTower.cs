using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JamurTower : Tower {
    [SerializeField]
    private int splashDamage;
    [SerializeField]
    private float tickTime;
    [SerializeField]
    private PoisonSplash splashPrefab;

    public int SplashDamage
    {
        get
        {
            return splashDamage;
        }
    }

    public float TickTime
    {
        get
        {
            return tickTime;
        }
    }

    private void Start()
    {
        ElementType = Element.POISON;

        Upgrades = new TowerUpgrade[]
        {
            new TowerUpgrade(2,1,1,2,10),
            new TowerUpgrade(2,1,1,2,20),
        };
    }
    
    public override string GetStats()
    {
        if (NextUpgrade != null)  //If the next is avaliable
        {
            return string.Format("<color=#ffa500ff>{0}</color>{1}", "<size=20><b>Kemarau</b></size>", base.GetStats(), NextUpgrade.SlowingFactor);
        }

        //Returns the current upgrade
        return string.Format("<color=#ffa500ff>{0}</color>{1}", "<size=20><b>Kemarau</b></size>", base.GetStats());
    }

    public override void Upgrade()
    {
        base.Upgrade();
    }

    public override Debuff GetDebuff()
    {
        return new PoisonDebuff(splashDamage,tickTime,splashPrefab,DebuffDuration,Target);
    }
}

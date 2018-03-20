using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Element {FIRE, STORM, FROST, POISON, NONE}

public abstract class Tower : MonoBehaviour {

    [SerializeField]
    private string projectileType;
    [SerializeField]
    private float projectileSpeed;

    private Animator myAnimator;

    [SerializeField]
    private int damage;

    [SerializeField]
    private float debuffDuration;

    [SerializeField]
    private float proc;


    public Element ElementType { get; protected set; }

    public int Price { get; set; }

    public float ProjectileSpeed
    {
        get { return projectileSpeed; }
    }

    private SpriteRenderer mySpriteRenderer;
    private Virus target;

    public int Level { get; protected set; }

    public Virus Target
    {
        get { return target; }
    }

    public int Damage
    {
        get
        {
            return damage;
        }
    }

    public float DebuffDuration
    {
        get
        {
            return debuffDuration;
        }

        set
        {
            this.debuffDuration = value;
        }
    }

    public float Proc
    {
        get
        {
            return proc;
        }

        set
        {
            this.proc = value;
        }
    }

    public TowerUpgrade NextUpgrade
    {
        get
        {
            if (Upgrades.Length > Level-1)
            {
                return Upgrades[Level - 1];
            }

            return null;
        }
    }


    private Queue<Virus> viruss = new Queue<Virus>();
    private bool canAttack = true;
    private float attackTimer;
    [SerializeField]
    private float attackCooldown;

    public TowerUpgrade[] Upgrades { get; protected set; }

    // Use this for initialization
    void Awake() {
        myAnimator = transform.parent.GetComponent<Animator>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        Level = 1;
    }

    // Update is called once per frame
    void Update() {
        Attack();
        // Debug.Log(target);
    }

    public void Select()
    {
        mySpriteRenderer.enabled = !mySpriteRenderer.enabled;
        GameManagerScript.Instance.UpdateUpgradeTip();
    }

    private void Attack()
    {
        if (!canAttack)
        {
            attackTimer += Time.deltaTime;
            if (attackTimer >= attackCooldown)
            {
                canAttack = true;
                attackTimer = 0;
            }
        }
        if (target == null && viruss.Count > 0 && viruss.Peek().IsActive)
        {
            target = viruss.Dequeue();
        }
        if (target != null && target.IsActive)
        {
            if (canAttack)
            {
                Shoot();
                canAttack = false;
            }
        }

        if (target != null && !target.Alive || target != null && !target.IsActive)
        {
            target = null;
        }
    }

    public virtual string GetStats()
    {
        if (NextUpgrade != null)
        {
            return string.Format("\nLevel: {0} \nDamage: {1} <color=#fffffff> +{4}</color>\nProc: {2}% <color=#ffffffff>+{5}%</color>\nDebuff: {3}sec <color=#ffffffff>->{6}</color>", Level, damage, proc, DebuffDuration, NextUpgrade.Damage, NextUpgrade.ProcChance, NextUpgrade.DebuffDuration);
        }
        return string.Format("\nLevel: {0} \nDamage: {1} \nProc: {2}% \nDebuff: {3}sec", Level, damage, proc, DebuffDuration);
    }

    private void Shoot()
    {
        Projectile projectile = GameManagerScript.Instance.Pool.GetObject(projectileType).GetComponent<Projectile>();
        projectile.transform.position = transform.position;
        projectile.Initialize(this);
    }

    public virtual void Upgrade()
    {
        GameManagerScript.Instance.Currency -= NextUpgrade.Price;
        Price += NextUpgrade.Price;
        this.damage += NextUpgrade.Damage;
        this.proc += NextUpgrade.ProcChance;
        this.DebuffDuration += NextUpgrade.DebuffDuration;
        Level++;
        GameManagerScript.Instance.UpdateUpgradeTip();
    }

    public abstract Debuff GetDebuff();
    
    public void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Virus")
        {
            target = other.GetComponent<Virus>();
            //viruss.Enqueue(other.GetComponent<Virus>());
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Virus")
        {
            target = null;
        }
    }
}

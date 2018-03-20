using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    private Virus target;
    private Tower parent;

    private Animator myAnimator;

    private Element elementType;

    // Use this for initialization
    void Start () {
        myAnimator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        MoveToTarget();
	}

    public void Initialize(Tower parent)
    {
        this.target = parent.Target;
        this.parent = parent;
        this.elementType = parent.ElementType;
    }

    private void MoveToTarget()
    {
        if (target != null && target.IsActive)
        {
            transform.position = Vector3.MoveTowards(transform.position,target.transform.position,Time.deltaTime*parent.ProjectileSpeed);

            Vector2 dir = target.transform.position - transform.position;
            float angle = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle,Vector3.forward);
        }
        else if (!target.IsActive)
        {
            GameManagerScript.Instance.Pool.ReleaseObject(gameObject);
        }
    }

    private void ApplyDebuff()
    {
        if (target.ElementType != elementType)
        {
            float roll = Random.Range(0, 100);

            if (roll <= parent.Proc)
            {
                target.AddDebuff(parent.GetDebuff());
            }
        }
        target.AddDebuff(parent.GetDebuff());
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Virus")
        {
            if(target.gameObject == other.gameObject)
            {
                target.TakeDamage(parent.Damage, elementType);
                myAnimator.SetTrigger("Impact");
                ApplyDebuff();
            }
        }
    }
}

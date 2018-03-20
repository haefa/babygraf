using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Virus : Singleton<Virus> {
	[SerializeField]
	private float speed;

	private Stack<Node> path;

    private List<Debuff> debuffs = new List<Debuff>();

    private List<Debuff> debuffsToRemove = new List<Debuff>();

    private List<Debuff> newDebuffs = new List<Debuff>();


    [SerializeField]
    private Element elementType;

    private SpriteRenderer spriteRenderer;

    private int invulnerability = 2;

    private Animator myAnimator;

    [SerializeField]
    private GameObject upgradePanel;

    [SerializeField]
    private Stat health;
    
    public bool Alive
    {
        get { return health.CurrentVal > 0; }
    }


    public Point GridPosition { get; set;}

	private Vector3 destination;

	public bool IsActive { get; set; }

    public float MaxSpeed { get; set; }

    public Element ElementType
    {
        get
        {
            return elementType;
        }
    }

    public float Speed
    {
        get
        {
            return speed;
        }

        set
        {
            speed = value;
        }
    }

    private void Awake()
    {
        myAnimator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        MaxSpeed = speed;
        health.Initialize();
    }

    private void Update()
	{
        HandleDebuffs();
        Move();
	}

	public void Spawn(int health)
	{


		transform.position = LevelManager.Instance.portalStart.transform.position;

        myAnimator = GetComponent<Animator>();

        this.health.Bar.Reset();

        this.health.MaxVal = health;
        this.health.CurrentVal = this.health.MaxVal;

		StartCoroutine(Scale(new Vector3(0.1f,0.1f), new Vector3(1,1),false));

		SetPath(LevelManager.Instance.Path);

	}

	public IEnumerator Scale(Vector3 from, Vector3 to, bool remove)
	{
		
		float progress = 0;

		while (progress <= 1)
		{
			transform.localScale = Vector3.Lerp(from, to, progress);

			progress += Time.deltaTime;

			yield return null;
		}

		transform.localScale = to;

		IsActive = true;

        if(remove)
        {
            //Release();
            GameManagerScript.Instance.RemoveVirus(this);
            Destroy(gameObject);
        }
	}

	private void Move()
	{
		if (IsActive)
		{
		transform.position = Vector2.MoveTowards(transform.position,destination,Speed*Time.deltaTime);

		if (transform.position == destination)
		{
			if (path != null && path.Count > 0)
			{
                Animate(GridPosition, path.Peek().GridPosition);
                GridPosition = path.Peek().GridPosition;
				destination = path.Pop().WorldPosition;
			}
		}
		}

	}

	private void SetPath(Stack<Node> newPath)
	{
		if (newPath != null)
		{
			this.path = newPath;

            Animate(GridPosition, path.Peek().GridPosition);

            GridPosition = path.Peek().GridPosition;

			destination = path.Pop().WorldPosition;
		}
	}

    private void Animate(Point currentPos, Point newPos)
    {
        myAnimator.SetInteger("Horizontal", 0);
        if (currentPos.X > newPos.X)
        {
            //we are moving left 
            myAnimator.SetInteger("Horizontal", -1);
        }
        else if (currentPos.X < newPos.X)
        {
            //we are moving right
            myAnimator.SetInteger("Horizontal", 1);
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "PortalFinish")
        {
            StartCoroutine(Scale(new Vector3(1, 1), new Vector3(0.1f, 0.1f),true));
            other.GetComponent<Portal>().Crying();
            GameManagerScript.Instance.Lives--;
        }
        if (other.tag == "Tile")
        {
            spriteRenderer.sortingOrder = other.GetComponent<TileScript>().GridPosition.Y;
        }
    }

    public void Release()
    {
        debuffs.Clear();

        IsActive = false;
        GridPosition = LevelManager.Instance.PortalStart1;
        GameManagerScript.Instance.Pool.ReleaseObject(gameObject);
        GameManagerScript.Instance.RemoveVirus(this);
    }

    public void TakeDamage(float damage, Element dmgSource)
    {
        if(IsActive)
        {

            if (dmgSource == ElementType)
            {
                damage = damage / invulnerability;
                invulnerability++;
            }

            health.CurrentVal -= damage;

            if (health.CurrentVal <= 0)
            {
                SoundManager.Instance.PlaySFX("Splat");
                GameManagerScript.Instance.Currency += 2;
                myAnimator.SetTrigger("Die");
                IsActive = false;
                //
                //GameManagerScript.Instance.RemoveVirus(this);
                //Destroy(gameObject);
                GetComponent<SpriteRenderer>().sortingOrder--;
            }
        }
    }

    public void AddDebuff(Debuff debuff)
    {
        if (!debuffs.Exists(x => x.GetType() == debuff.GetType()))
        {
            newDebuffs.Add(debuff);
        }
    }

    public void RemoveDebuff(Debuff debuff)
    {
        debuffsToRemove.Add(debuff);
    }

    private void HandleDebuffs()
    {
        if(newDebuffs.Count > 0)
        {
            debuffs.AddRange(newDebuffs);

            newDebuffs.Clear();
        }

        foreach (Debuff debuff in debuffsToRemove)
        {
            debuffs.Remove(debuff);
        }

        debuffsToRemove.Clear();

        foreach (Debuff debuff in debuffs)
        {
            debuff.Update();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TileScript : Singleton<TileScript> {
	public Point GridPosition{ get; private set; }
	public bool IsEmpty { get; set; }
    private Tower myTower;
	
	private Color32 fullColor = new Color32(255,118,118,255);
	private Color32 emptyColor = new Color32(96,225,90,225);
	private SpriteRenderer spriteRenderer;
	// public SpriteRenderer SpriteRenderer { get; set; }
	[SerializeField]
	public bool WalkAble { get; set; }
	[SerializeField]
	
	private int gCost;
	public int GCost 
	{ 
		get
		{
			return gCost;
		} 
		set
		{
			this.gCost = value;
		}
	}
	// public int gCost {get; set;}
	public bool Debugging { get; set;}
	public Vector2 WorldPosition
	{
		get
		{
			return new Vector2(transform.position.x + (GetComponent<SpriteRenderer>().bounds.size.x/2), transform.position.y + (GetComponent<SpriteRenderer>().bounds.size.y/2));
		}
	}
	// Use this for initialization
	void Start () 
	{
		spriteRenderer = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

	public void Setup(Point gridPos, Vector3 worldPos, Transform parent, string tileType)
	{
        if (tileType == "0")
        {
            // WalkAble = true;
            // Debug.Log(WalkAble);
			gCost = 1;
        }
        else { 
			// WalkAble = false; 
			// Debug.Log(WalkAble); 
			gCost = 100000;
		}
		WalkAble=true;
		// tileIndex = tileType;
		// Debug.Log(tileIndex);
		IsEmpty = true;
		this.GridPosition = gridPos;
		transform.position = worldPos;
		transform.SetParent(parent);
		LevelManager.Instance.Tiles.Add(gridPos, this);
	}

	public void OnMouseOver()
	{
		if(!EventSystem.current.IsPointerOverGameObject() && GameManagerScript.Instance.ClickedBtn != null)
		{
			if(IsEmpty && !Debugging)
			{
				ColorTile(emptyColor);
			}
			if(!IsEmpty && !Debugging)
			{
				ColorTile(fullColor);
			}
			else if(Input.GetMouseButton(0))
			{
				PlaceTower();
			}
		}
        else if (!EventSystem.current.IsPointerOverGameObject() && GameManagerScript.Instance.ClickedBtn == null && Input.GetMouseButtonDown(0))
        {
            if (myTower != null)
            {
                GameManagerScript.Instance.SelectTower(myTower);
            }
            else
            {
                GameManagerScript.Instance.DeselectTower();
            }
        }
	}
	private void PlaceTower()
	{
        WalkAble = false;

        if (Astar.GetPath(LevelManager.Instance.PortalStart1, LevelManager.Instance.PortalFinish1) == null)
        {
            //we dont have path
            WalkAble = true;
            return;
        }

		GameObject tower = (GameObject)Instantiate(GameManagerScript.Instance.ClickedBtn.TowerPrefab, transform.position, Quaternion.identity);
		tower.GetComponent<SpriteRenderer>().sortingOrder = GridPosition.Y;
		tower.transform.SetParent(transform);
        this.myTower = tower.transform.GetChild(0).GetComponent<Tower>();
		IsEmpty = false;
		ColorTile(Color.white);
        myTower.Price = GameManagerScript.Instance.ClickedBtn.Price;
		GameManagerScript.Instance.BuyTower();
		WalkAble = false;
	}
	private void ColorTile(Color newColor)
	{
		spriteRenderer.color = newColor;
	}
	private void OnMouseExit()
	{
		if (!Debugging)
		{
			ColorTile(Color.white);
		}
	}
}

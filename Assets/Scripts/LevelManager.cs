using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : Singleton<LevelManager> 
{

    [SerializeField]
	private GameObject[] tilePrefabs;
	
	[SerializeField]
	private CameraMovementScript cameraMovement;
	
	[SerializeField]
	private Transform map;
	

	private Point PortalStart, PortalFinish;
	[SerializeField]
	private GameObject PortalStartPrefab;
	[SerializeField]
	private GameObject PortalFinishPrefab;

	public Portal portalStart { get; set;}
	private Point mapSize;

	private Stack<Node> path;

	public Stack<Node> Path
	{
		get
		{
			if (path == null)
			{
				GeneratePath();
			}
			return new Stack<Node>(new Stack<Node>(path));
		}
	}
	
	public Dictionary<Point, TileScript> Tiles { get; set; }
	
	public float TileSize{
		get {
			return tilePrefabs[0].GetComponent<SpriteRenderer>().sprite.bounds.size.x;
		}
	}

    public Point PortalStart1
    {
        get
        {
            return PortalStart;
        }
    }

    public Point PortalFinish1
    {
        get
        {
            return PortalFinish;
        }
    }

    // Use this for initialization
    void Start () {
		CreateLevel();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void CreateLevel(){
		Tiles = new Dictionary<Point, TileScript>();
		string[] mapData = readLevelText();
		mapSize = new Point(mapData[0].ToCharArray().Length, mapData.Length);
		int mapX = mapData[0].ToCharArray().Length;
		int mapY = mapData.Length;
		Vector3 maxTile = Vector3.zero;
		Vector3 worldStart = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height));
		for (int y = 0; y < mapY; y++)
		{
			char[] newTiles = mapData[y].ToCharArray();
			for (int x = 0; x < mapX; x++)
			{
				PlaceTile(newTiles[x].ToString(), x, y, worldStart);
			}
		}
		maxTile = Tiles[new Point(mapX-1,mapY-1)].transform.position;
		cameraMovement.SetLimits(new Vector3(maxTile.x + TileSize, maxTile.y-TileSize));
		SpawnPortals();
	}

	private void PlaceTile(string tileType, int x, int y, Vector3 worldStart){
		int tileIndex = int.Parse(tileType);
		TileScript newTile = Instantiate(tilePrefabs[tileIndex]).GetComponent<TileScript>(); 
		newTile.Setup(new Point(x, y), new Vector3(worldStart.x + TileSize*x, worldStart.y - TileSize*y, 0), map, tileType);
		// return newTile.transform.position;
	}

	private string[] readLevelText(){
        TextAsset bindData = Resources.Load("Level") as TextAsset;
        if (LevelController.Level == 2)
        {
            bindData = Resources.Load("Level 1") as TextAsset;
        }
		string data =  bindData.text.Replace(Environment.NewLine, string.Empty);
		return data.Split('-');
	}

	private void SpawnPortals()
	{
        if (LevelController.Level == 1)
        {
            PortalStart = new Point(1, 3);
            PortalFinish = new Point(12, 2);
        }
        else
        {
            PortalStart = new Point(0, 2);
            PortalFinish = new Point(0, 10);
        }
        GameObject tmp = (GameObject)Instantiate(PortalStartPrefab, Tiles[PortalStart1].GetComponent<TileScript>().WorldPosition, Quaternion.identity);
		portalStart = tmp.GetComponent<Portal>();

		portalStart.name = "PortalStart";

		Instantiate(PortalFinishPrefab, Tiles[PortalFinish1].GetComponent<TileScript>().WorldPosition, Quaternion.identity);
	}
	public bool InBounds(Point position)
	{
		return position.X >= 0 && position.Y >= 0 && position.X < mapSize.X && position.Y < mapSize.Y;
	}
	
	public void GeneratePath()
	{
		path = Astar.GetPath(PortalStart1, PortalFinish1);
	}
}

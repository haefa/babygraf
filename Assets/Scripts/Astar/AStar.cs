using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class Astar
{
	private static Dictionary<Point, Node> nodes;
	private static void CreateNodes()
	{
		nodes = new Dictionary<Point, Node>();
		foreach (TileScript tile in LevelManager.Instance.Tiles.Values)
		{
			nodes.Add(tile.GridPosition, new Node(tile));
		}
	}
	public static Stack<Node> GetPath(Point start, Point goal)
	{
		if (nodes == null)
		{
			CreateNodes();
		}
		HashSet<Node> openList = new HashSet<Node>();
		HashSet<Node> closedList = new HashSet<Node>();
		
		// 1,2,3

		Stack<Node> finalPath = new Stack<Node>();


		Node currentNode = nodes[start];

		openList.Add(currentNode);

		while (openList.Count > 0) //Step10
		{
			for (int x = -1; x <= 1; x++)
			{
				for (int y = -1; y <= 1; y++)
				{
					Point neighbourPos = new Point(currentNode.GridPosition.X - x, currentNode.GridPosition.Y - y);
					// Debug.Log(neighbourPos.X + " " + neighbourPos.Y);
					if (LevelManager.Instance.InBounds(neighbourPos) && LevelManager.Instance.Tiles[neighbourPos].WalkAble && neighbourPos != currentNode.GridPosition)
					{
						int gCost = 0;
						// Debug.Log(TileScript.Instance.GCost);
						// if (TileScript.Instance.GetComponent<TileScript>().tileIndex == "1")
						// {
						// 	Debug.Log("Hehehe");
						// 	gCost = 10;
						// }
						// Debug.Log(LevelManager.Instance.Tiles[neighbourPos].GCost);
						if (Math.Abs(x-y) == 1)
						{
							gCost = LevelManager.Instance.Tiles[neighbourPos].GCost;//TileScript.Instance.GCost;
						}
						else
						{
							if (!ConnectedDiagonally(currentNode,nodes[neighbourPos]))
							{
								continue;
							}
							gCost = 100 + LevelManager.Instance.Tiles[neighbourPos].GCost;
						}
						Node neighbour = nodes[neighbourPos];
						
						//neighbour.TileRef.SpriteRenderer.color = Color.black;	
						
						if (openList.Contains(neighbour))
						{
							// if (currentNode.G + gCost < neighbour.G)
							if (currentNode.G + gCost < neighbour.G)
							{
								neighbour.CalcValues(currentNode, nodes[goal], gCost);
							}
						}
						else if (!closedList.Contains(neighbour))
						{
							openList.Add(neighbour);
							neighbour.CalcValues(currentNode,nodes[goal], gCost);
						}
					}
					// 
				}
			}

			// Moves the current node from the open list to the closed list
			openList.Remove(currentNode);
			closedList.Add(currentNode);

			if(openList.Count > 0)
			{
				//Sorts the list by F value, and selects the first on the lists
				currentNode = openList.OrderBy(n => n.F).First();
			}
			
			if (currentNode == nodes[goal])
			{
				while (currentNode.GridPosition != start)
				{
				finalPath.Push(currentNode);
				currentNode = currentNode.Parent;
				}
                return finalPath;
            }
		}


        return null;

		//*****FOR DEBUGGING ONLY, REMOVE LATER */
		//GameObject.Find("AStarDebugger").GetComponent<AStarDebugger>().DebugPath(openList, closedList, finalPath);
	}
	
	private static bool ConnectedDiagonally(Node currentNode, Node neighbor)
	{
		Point direction = neighbor.GridPosition - currentNode.GridPosition;

		Point first = new Point(currentNode.GridPosition.X + direction.X, currentNode.GridPosition.Y);

		Point second = new Point(currentNode.GridPosition.X, currentNode.GridPosition.Y + direction.Y);

		if (LevelManager.Instance.InBounds(first) && !LevelManager.Instance.Tiles[first].WalkAble)
		{
			return false;
		}
		if (LevelManager.Instance.InBounds(second) && !LevelManager.Instance.Tiles[second].WalkAble)
		{
			return false;
		}

		return true;
	}
	
}


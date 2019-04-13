using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using UnityEngine;
using UnityEngine.Tilemaps;


public class Node
{
    public List<Node> neighbors;
    public Vector2 pos;

    public bool isFree;

    public bool hasBeenVisited = false;
    public bool isPath = false;
    public Node cameFrom = null;
}

public class Disjskra : MonoBehaviour
{

    private int sizeX;
    private int sizeY;

   Node[,] graph;

    void Start()
    {
      sizeX = GameManager.Instance.BspScript.SizeX*2;
      sizeY = GameManager.Instance.BspScript.SizeY*2;

    }

    public void GenerateGraph(Bsp.Cell[,] cells) {
      
        graph=new Node[sizeX, sizeY];

        for(int x = 0;x < sizeX;x++) {
            for(int y = 0;y < sizeY;y++) {

                Node newNode = new Node {
                    pos = new Vector2(x +0.5f, y +0.5f),
                    neighbors = new List<Node>()
                };

                newNode.isFree = !cells[x, y].isNotFree;

                graph[x, y] = newNode;
            }
        }

        BoundsInt bounds = new BoundsInt(-1, -1, 0, 3, 3, 1);

        for(int i = 0;i < graph.GetLength(0);i++) {
            for(int j = 0;j < graph.GetLength(1);j++) {
                Node node = graph[i, j];

                if(node == null) continue;
                if(!node.isFree) continue;

                foreach(Vector2Int b in bounds.allPositionsWithin) {
                    if(i + b.x < 0 || i + b.x >= sizeX || j + b.y < 0 || j + b.y >= sizeY) continue;
                    if(b.x == 0 && b.y == 0) continue;

                    if(graph[i + b.x, j + b.y] == null) continue;
                    if(!graph[i + b.x, j + b.y].isFree) continue;


                    if (b.x != 0 || b.y != 0)
                    {
                        if (graph[i, j + b.y] == null) continue;
                        if (!graph[i, j + b.y].isFree) continue;
                        if (graph[i + b.x, j] == null) continue;
                        if (!graph[i + b.x, j].isFree) continue;
                    }
                    node.neighbors.Add(graph[i + b.x, j + b.y]);
                }
            }
        }
        
    }


    public List<Vector2> BFS(Vector2 startPos, Vector2 goalPos)
    {
        List<Vector2> Path=new List<Vector2>();
        Node startingNode = GetNodeNearly(startPos);
        Node goalNode = GetNodeNearly(goalPos);

        List<Node> openList = new List<Node> { startingNode };
        List<Node> closedList = new List<Node>();

        int crashValue = 1000;

        while (openList.Count > 0 && --crashValue > 0)
        {
            openList = openList.OrderBy(i =>Vector2.Distance(i.pos, goalNode.pos)).ToList();

            Node currentNode = openList[0];
            openList.RemoveAt(0);

            currentNode.hasBeenVisited = true;

            closedList.Add(currentNode);

            if (currentNode == goalNode)
            {
                break;
            }
            else
            {
                foreach (Node currentNodeNeighbor in currentNode.neighbors)
                {

                    if (currentNodeNeighbor.hasBeenVisited) continue;
                    if (openList.Contains(currentNodeNeighbor)) continue;
                    if (currentNodeNeighbor.cameFrom != null)
                    {
                        if (Vector2.Distance(currentNodeNeighbor.cameFrom.pos, goalPos) <
                            Vector2.Distance(currentNode.pos, goalPos)) continue;
                    }
                    currentNodeNeighbor.cameFrom = currentNode;
                    openList.Add(currentNodeNeighbor);
                }
            }

        }

        if (crashValue <= 0)
        {
            Debug.Log("crasher");
        }


        {
            Node currentNode = goalNode;

            while (currentNode.cameFrom != startingNode)
            {
                currentNode.isPath = true;
                currentNode = currentNode.cameFrom;
                Path.Add(currentNode.pos);
            }

            currentNode.isPath = true;
        }
        ResetNode();
        return Path;
    }

    Node GetNodeNearly(Vector2 pos)
    {
        float minDistance = float.MaxValue;
        Node returnNode =null;

        foreach (Node node in graph)
        {
            if (node == null) continue;
            if (!node.isFree) continue;
            if (Vector2.Distance(node.pos,pos)<minDistance)
            {
                minDistance = Vector2.Distance(node.pos, pos);
                returnNode = node;
            }
        }

        return returnNode;

    }

    void ResetNode()
    {
        foreach (Node node in graph)
        {
            if (node == null) continue;
            node.cameFrom=new Node();
            node.hasBeenVisited = false;
            node.isPath = false;
        
        }
    }

    void OnDrawGizmos() {

        foreach(Node node in graph) {
            if(node == null) continue;

            Gizmos.color = node.isFree ? Color.blue : Color.red;

            if(node.hasBeenVisited) {
                Gizmos.color = Color.yellow;
            }

            if(node.isPath) {
                Gizmos.color = Color.green;
            }

            Gizmos.DrawCube(node.pos, Vector3.one * 0.75f);

            foreach(Node nodeNeighbor in node.neighbors) {
                Gizmos.DrawLine(node.pos, nodeNeighbor.pos);
            }
        }
    }
}

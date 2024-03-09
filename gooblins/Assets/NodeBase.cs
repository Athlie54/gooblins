using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class NodeBase
{
    public NodeBase Connection { get; private set; }
    public float G { get; private set; }
    public float H { get; private set; }
    public float F => G + H;
    public Tile tile { get; set; }

    public NodeBase(Tile tile) { this.tile = tile; }

    public void SetConnection(NodeBase nodeBase)
    {
        this.Connection = nodeBase;
    }

    public void SetG(float g) => G = g;
    public void SetH(float h) => H = h;

    public List<NodeBase> Neighbors()
    {
        Dictionary<Vector2, Tile> map = GridManager._tiles;
        List<NodeBase> l = new List<NodeBase>();
        Vector2 pos = tile._position;


        if (pos.x > 0)
            l.Add(new NodeBase(map[pos + new Vector2(-1, 0)]));
        if (pos.x < GridManager._width - 1)
            l.Add(new NodeBase(map[pos + new Vector2(1, 0)]));
        if (pos.y > 0)
            l.Add(new NodeBase(map[pos + new Vector2(0, -1)]));
        if (pos.y < GridManager._height - 1)
            l.Add(new NodeBase(map[pos + new Vector2(0, 1)]));

        return l;
    }

    public static List<NodeBase> FindPath(NodeBase startNode, NodeBase targetNode)
    {
        var toSearch = new List<NodeBase>() { startNode };
        var processed = new List<NodeBase>();

        while (toSearch.Any())
        {
            
            var current = toSearch.First();
            foreach (var t in toSearch)
            {
                if (t.F < current.F || t.F == current.F && t.H < current.H)
                {
                    current = t;
                }
            }

            Debug.Log($"HEY THERE BUDDY!!! Current: {current.tile._position.x} {current.tile._position.y}; Target: {targetNode.tile._position.x} {targetNode.tile._position.y}");

            processed.Add(current);
            toSearch.Remove(current);

            if (current.tile._position == targetNode.tile._position)
            {
                var currentPathTile = targetNode;
                var path = new List<NodeBase>();

                if (currentPathTile.tile._position == null)
                {
                    Debug.Log("currentPathTile null");
                }
                if (startNode.tile._position == null)
                {
                    Debug.Log("startNode null");
                }

                Debug.Log($"currentPathTile: {currentPathTile.tile._position}");
                Debug.Log($"startNode: {currentPathTile.tile._position}");


                //while (currentPathTile != null && currentPathTile.tile._position != startNode.tile._position)
                while (currentPathTile != null && (startNode.tile._position.x != currentPathTile.tile._position.x || startNode.tile._position.y != currentPathTile.tile._position.y))
                {
                    path.Add(currentPathTile);
                    currentPathTile = currentPathTile.Connection;
                    if (currentPathTile == null)
                    {
                        Debug.Log("it's null baybeeeee");
                    }
                }

                return path;
            }

            foreach (var neighbor in current.Neighbors())
            {
                if (neighbor.tile._isWalkable && processed.Where(n => n.tile._position == neighbor.tile._position).Count() == 0)
                {
                    var inSearch = toSearch.Contains(neighbor);

                    var costToNeighbor = current.G + 1; // No orthogonal movement, distance to neighbor always 1

                    if (!inSearch || costToNeighbor < neighbor.G)
                    {
                        neighbor.SetG(costToNeighbor);
                        neighbor.SetConnection(current);

                        if (!inSearch)
                        {
                            neighbor.SetH(Mathf.Abs(targetNode.tile._position.x - neighbor.tile._position.x) +
                                Mathf.Abs(targetNode.tile._position.y - neighbor.tile._position.y));
                            toSearch.Add(neighbor);
                        }
                    }
                }
            }
        }

        return null;
    }
}

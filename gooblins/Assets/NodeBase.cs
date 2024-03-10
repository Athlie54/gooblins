using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class NodeBase
{
    private static Dictionary<Vector2, NodeBase> _nodeMap = new Dictionary<Vector2, NodeBase>();
    public NodeBase Connection { get; private set; }
    public float G { get; private set; }
    public float H { get; private set; }
    public float F => G + H;
    //public Tile tile { get; set; }

    public Vector2 Position { get; private set; }

    //public NodeBase(Tile tile) { this.tile = tile; }

    public NodeBase(Vector2 position) {  Position = position; G = 0; H = 0; Connection = null; }

    public void SetConnection(NodeBase nodeBase)
    {
        this.Connection = nodeBase;
    }

    public void SetG(float g) => G = g;
    public void SetH(float h) => H = h;

    private static void updateMap()
    {
        Dictionary<Vector2, Tile> map = GridManager._tiles;
        foreach (var (pos, tile) in map)
        {
            if (pos == null) Debug.Log("POSITOIN ASDHKH");
            NodeBase node = new NodeBase(pos);

            _nodeMap[pos] = node;
        }
    }

    public List<NodeBase> Neighbors()
    {
        Dictionary<Vector2, Tile> map = GridManager._tiles;
        List<NodeBase> l = new List<NodeBase>();


        if (this.Position.x > 0)
            //l.Add(new NodeBase(map[pos + new Vector2(-1, 0)]));
            l.Add(_nodeMap[this.Position + new Vector2(-1, 0)]);
        if (this.Position.x < GridManager._width - 1)
            //l.Add(new NodeBase(map[pos + new Vector2(1, 0)]));
            l.Add(_nodeMap[this.Position + new Vector2(1, 0)]);

        if (this.Position.y > 0)
            //l.Add(new NodeBase(map[pos + new Vector2(0, -1)]));
            l.Add(_nodeMap[this.Position + new Vector2(0, -1)]);

        if (this.Position.y < GridManager._height - 1)
            //l.Add(new NodeBase(map[pos + new Vector2(0, 1)]));
            l.Add(_nodeMap[this.Position + new Vector2(0, 1)]);


        return l;
    }

    public static Vector2 ClosestAccessibleTo(Vector2 start_pos, Vector2 end_pos, int max_distance)
    {
        var p = FindPath(start_pos, end_pos);//list of node bases
        if(p.Count() <= max_distance)
        {
            if(GridManager._tiles[end_pos].OccupiedUnit != null)
            {
                return p[1].Position;
            }
            
            return end_pos;
        }
        else
        {
            return p[p.Count - max_distance].Position;
        }
    }

    /*
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
    */

    public static List<NodeBase> FindPath(Vector2 startPos, Vector2 targetPos)
    {
        updateMap();
        NodeBase startNode = _nodeMap[startPos];
        NodeBase targetNode = _nodeMap[targetPos];
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

            Debug.Log($"HEY THERE BUDDY!!! Current: {current.Position.x} {current.Position.y}; Target: {targetNode.Position.x} {targetNode.Position.y}");

            processed.Add(current);
            toSearch.Remove(current);

            if (current.Position == targetNode.Position)
            {
                var currentPathTile = targetNode;
                var path = new List<NodeBase>();

                if (currentPathTile.Position == null)
                {
                    Debug.Log("currentPathTile null");
                }
                if (startNode.Position == null)
                {
                    Debug.Log("startNode null");
                }

                Debug.Log($"currentPathTile: {currentPathTile.Position}");
                Debug.Log($"startNode: {currentPathTile.Position}");


                //while (currentPathTile != null && currentPathTile.tile._position != startNode.tile._position)
                while (currentPathTile != null && (startNode.Position.x != currentPathTile.Position.x || startNode.Position.y != currentPathTile.Position.y))
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
                if (GridManager._tiles[neighbor.Position]._isWalkable && (GridManager._tiles[neighbor.Position].OccupiedUnit == null || neighbor.Position == targetPos )&& processed.Where(n => n.Position == neighbor.Position).Count() == 0)
                {
                    var inSearch = toSearch.Contains(neighbor);

                    var costToNeighbor = current.G + 1; // No orthogonal movement, distance to neighbor always 1

                    if (!inSearch || costToNeighbor < neighbor.G)
                    {
                        neighbor.SetG(costToNeighbor);
                        neighbor.SetConnection(current);

                        if (!inSearch)
                        {
                            neighbor.SetH(Mathf.Abs(targetNode.Position.x - neighbor.Position.x) +
                                Mathf.Abs(targetNode.Position.y - neighbor.Position.y));
                            toSearch.Add(neighbor);
                        }
                    }
                }
            }
        }
        return null;
    }
}

using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Unity.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class GridManager : MonoBehaviour {
    public static GridManager Instance;
    public static int _width, _height;

    [SerializeField] private Tile _grass, _mountain, _floor, _wall, _crack, _doorUp, _doorDown, _doorLeft, _doorRight;

    [SerializeField] private Transform _cam;

    public static Dictionary<Vector2, Tile> _tiles;

    public TextAsset level;
    public TextAsset levelObjects;

    public Tile tileToSpawn;


    void Awake() {
        _width = 28;
        _height = 28;
        Instance = this;
    }

    public void GenerateGrid()
    {
        _tiles = new Dictionary<Vector2, Tile>();

        List<int> tiles = new List<int>();


        // Place grid
        string[] levelContent = (level.ToString().Split(','));
        foreach (string item in levelContent)
        {
            int tile = int.Parse(item);
            Debug.Log(tile);
            tiles.Add(tile);
        }
       for (int y = 0; y < _width; y++)
       {
            for (int x = 0; x < _height; x++)
            {
                
                switch (tiles[((_height - 1 - y) * _width) + x])
                {
                    case (int)TileTypes.Grass:
                        tileToSpawn = _grass;
                        break;
                    case (int)TileTypes.Floor:
                        tileToSpawn = _floor;
                        break;
                    case (int)TileTypes.Wall:
                        tileToSpawn = _wall;
                        break;
                    case (int)TileTypes.Crack:
                        tileToSpawn = _crack;
                        break;
                    case (int)TileTypes.DoorUp:
                        tileToSpawn = _doorUp;
                        break;
                    case (int)TileTypes.DoorDown:
                        tileToSpawn = _doorDown;
                        break;
                    case (int)TileTypes.DoorLeft:
                        tileToSpawn = _doorLeft;
                        break;
                    case (int)TileTypes.DoorRight:
                        tileToSpawn = _doorRight;
                        break;
                    default:
                        Debug.Log("switch case failed");
                        tileToSpawn = _mountain;
                        break;
                }  
                
                var spawnedTile = Instantiate(tileToSpawn, new Vector3(x, y), Quaternion.identity);
                spawnedTile.name = $"Tile {x} {y}";

                spawnedTile.Init(x, y);
                spawnedTile._position = new Vector2Int(x, y);

                Debug.Log($"Generated {spawnedTile._position}");

                _tiles[new Vector2(x, y)] = spawnedTile;
            }
       }
        _cam.transform.position = new Vector3((float)_width / 2 - 0.5f, (float)_height / 2 - 0.5f, -10);

        // Place objects
        string[] levelObjectContent = Regex.Split(levelObjects.text, "\n");

        Debug.Log($"Found {levelObjectContent.Count()} objects");

        foreach (var obj in levelObjectContent)
        {
            string[] data = Regex.Split(obj, ",");
            Tile t;
            switch (int.Parse(data[2]))
            {
                case 1:
                    var rug = Instantiate(Resources.LoadAll<Rug>("Interactible").First());
                    t = _tiles[new Vector2(int.Parse(data[0]), int.Parse(data[1]))];
                    rug.transform.position = t.transform.position;
                    t._interactibles.Add(rug);
                    break;
                case 2:
                    var gold = Instantiate(Resources.LoadAll<Gold>("Interactible").First());
                    t = _tiles[new Vector2(int.Parse(data[0]), int.Parse(data[1]))];
                    gold.transform.position = t.transform.position;
                    t._interactibles.Add(gold);
                    break;
                case 3:
                    UnitManager.Instance.SpawnEnemy("Knight", new Vector2(int.Parse(data[0]), int.Parse(data[1])));
                    break;
                case 4:
                    UnitManager.Instance.SpawnEnemy("Dog", new Vector2(int.Parse(data[0]), int.Parse(data[1])));
                    break;
                case 5:
                    UnitManager.Instance.SpawnEnemy("Gargoyle", new Vector2(int.Parse(data[0]), int.Parse(data[1])));
                    break;
            }
        }

        GameManager.Instance.ChangeState(GameState.SpawnHeroes);
    }

    public Tile GetHeroSpawnTile(int i) {
        Vector2 set_spawn = new Vector2(1 + i, 1);
        return _tiles[set_spawn];

    } //_tiles.Where(t => t.Key.x < _width / 2 && t.Value.Walkable).OrderBy(t => Random.value).First().Value;
    

    public Tile GetEnemySpawnTile()
    {
        Vector2 set_spawn = new Vector2(1, 3);
        return _tiles[set_spawn];

        //_tiles.Where(t => t.Key.x > _width / 2 && t.Value.Walkable).OrderBy(t => Random.value).First().Value;
    }

    public Tile GetTileAtPosition(Vector2 pos)
    {
        if (_tiles.TryGetValue(pos, out var tile)) return tile;
        return null;
    }

}

public enum TileTypes
{
    Air = 0,
    Grass = 1,
    Floor = 2,
    Wall = 3,
    Crack = 4,
    DoorUp = 5,
    DoorDown = 6,
    DoorLeft = 7,
    DoorRight = 8
}
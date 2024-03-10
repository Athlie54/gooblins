using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class UnitManager : MonoBehaviour {
    public static UnitManager Instance;

    private List<ScriptableUnit> _units;
    public List<BaseEnemy> enemies;
    public List<BaseHero> heros; 
    public BaseHero SelectedHero;

    void Awake() {
        Instance = this;

        _units = Resources.LoadAll<ScriptableUnit>("Units").ToList();
        //foreach (ScriptableUnit enemy in _units.Where(u => u.Faction == Faction.Enemy))
        //{
        //    enemies.Add((BaseEnemy)enemy.UnitPrefab);
        //    Debug.Log(enemy)
        //}
    }

    public void SpawnHeroes() {
        var heroCount = 3;

        for (int i = 0; i < heroCount; i++) {
            var heroPrefab = _units.Where(u => u.name == CharacterSelectManager.spriteNames[ CharacterSelectManager.team[i]]).First().UnitPrefab;
            var spawnedHero = Instantiate(heroPrefab);
            var randomSpawnTile = GridManager.Instance.GetHeroSpawnTile(i);

            SetUnit(spawnedHero, randomSpawnTile);
            spawnedHero.OccupiedTile = randomSpawnTile;
            heros.Add((BaseHero)spawnedHero);

        }

        GameManager.Instance.ChangeState(GameState.SpawnEnemies);
    }

    public void ResetHeroMovement()
    {
        foreach (BaseHero hero in heros)
        {
            hero.Movement = hero.MaxMovement;
        }
    }

    public void SpawnEnemies()
    {
        var enemyCount = 1;

        for (int i = 0; i < enemyCount; i++)
        {
            //var randomPrefab = GetRandomUnit<BaseEnemy>(Faction.Enemy);
            var knight = _units.Where(u => u.name == "Gargoyle").First().UnitPrefab;
            var spawnedEnemy = Instantiate(knight);
            var randomSpawnTile = GridManager.Instance.GetEnemySpawnTile();

            SetUnit(spawnedEnemy, randomSpawnTile);
            spawnedEnemy.OccupiedTile = randomSpawnTile;
            enemies.Add((BaseEnemy)spawnedEnemy);

        }

        GameManager.Instance.ChangeState(GameState.HeroesTurn);
    }

    public void SpawnEnemy(string name, Vector2 position)
    {
        var enemy = _units.Where(u => u.name == name).First().UnitPrefab;
        var spawnedEnemy = Instantiate(enemy);

        SetUnit(spawnedEnemy, GridManager._tiles[position]);
        spawnedEnemy.OccupiedTile = GridManager._tiles[position];
        enemies.Add((BaseEnemy)spawnedEnemy);
    }

    private T GetRandomUnit<T>(Faction faction) where T : BaseUnit {
        return (T)_units.Where(u => u.Faction == faction).OrderBy(o => Random.value).First().UnitPrefab;
    }

    public void SetSelectedHero(BaseHero hero) {
        SelectedHero = hero;
        MenuManager.Instance.ShowSelectedHero(hero);
    }


    public int MoveUnit(BaseUnit unit, Tile destination)
    {
        if (destination== null || !destination._isWalkable)
        {
            Debug.Log("bad destination");
            return -1;
        }
        Debug.Log("MoveUnit() started");
        //List<NodeBase> path = NodeBase.FindPath(new NodeBase(unit.OccupiedTile), new NodeBase(this));
        List<NodeBase> path = NodeBase.FindPath(unit.OccupiedTile._position, destination._position);

        if (path != null && path.Count <= unit.Movement )
        {
            if(path.Count==0)
            {
                return -1;
            }
            Debug.Log($"Path found: {path.Count}");
            Debug.Log("Movement should be happening");
            SetUnit(unit, destination);
            unit.Movement -= path.Count;
        }
        else
        {
            if (path == null)
            {
                Debug.Log("No path :(");
                return -1;
            }
            else
            {
                Debug.Log($"Path too long: {path.Count} ({unit.Movement})");
                return -1;
            }
        }
        //SetUnit(unit);
        return 0;
    }

    public void MoveToClosestReachableTile(BaseUnit unit, Tile destination)
    {
        Vector2 closest_dest_tile = NodeBase.ClosestAccessibleTo(unit.OccupiedTile._position, destination._position, unit.Movement);
        MoveUnit(unit, GridManager._tiles[closest_dest_tile]);
    }

    public void SetUnit(BaseUnit unit, Tile destination)
    {
        if (unit.OccupiedTile != null) unit.OccupiedTile.OccupiedUnit = null;
        unit.transform.position = destination.transform.position;
        destination.OccupiedUnit  = unit;
        unit.OccupiedTile = destination;
    }

    public void EnemyTurn()
    {
        foreach (BaseEnemy e in enemies)
        {
            //Debug.Log($"{e}'s turn!");
            e.Movement = e.MaxMovement;
            switch (e.alertness)
            {
                case (int)BaseEnemy.Alertness.Normal:
                    e.Patrol();
                    break;
                case (int)BaseEnemy.Alertness.On_Edge:
                    //do nothing
                    break;

                case (int)BaseEnemy.Alertness.In_Combat:

                    var target = heros[0].OccupiedTile;
                    if(e.MaxMovement != 0)
                    {
                        MoveToClosestReachableTile(e, target);//change form [0] later
                    }
                    //aka enemy next to hero
                    if(Mathf.Abs(e.OccupiedTile._position.x - target._position.x) + Mathf.Abs(e.OccupiedTile._position.y - target._position.y) == e.range)
                    {
                        e.UnitAction(e, heros[0]);
                    }
                    break;
                default:
                    Debug.Log("unexpeceted alertness");
                    break;

            }
        }

        MenuManager.Instance.ToggleTurn();
    }
}

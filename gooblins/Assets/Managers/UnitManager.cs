using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class UnitManager : MonoBehaviour {
    public static UnitManager Instance;

    private List<ScriptableUnit> _units;
    public List<BaseEnemy> enemies;
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
        var heroCount = 1;

        for (int i = 0; i < heroCount; i++) {
            var randomPrefab = GetRandomUnit<BaseHero>(Faction.Hero);
            var spawnedHero = Instantiate(randomPrefab);
            var randomSpawnTile = GridManager.Instance.GetHeroSpawnTile();

            SetUnit(spawnedHero, randomSpawnTile);
            spawnedHero.OccupiedTile = randomSpawnTile;
        }

        GameManager.Instance.ChangeState(GameState.SpawnEnemies);
    }

    public void ResetHeroMovement()
    {
        foreach (ScriptableUnit h in _units.Where(u => u.Faction == Faction.Hero))
        {
            var hero = h.UnitPrefab;
            hero.Movement = hero.MaxMovement;
        }
    }

    public void SpawnEnemies()
    {
        var enemyCount = 1;

        for (int i = 0; i < enemyCount; i++)
        {
            var randomPrefab = GetRandomUnit<BaseEnemy>(Faction.Enemy);
            var spawnedEnemy = Instantiate(randomPrefab);
            var randomSpawnTile = GridManager.Instance.GetEnemySpawnTile();

            SetUnit(spawnedEnemy, randomSpawnTile);
            spawnedEnemy.OccupiedTile = randomSpawnTile;
            enemies.Add(spawnedEnemy);

        }

        GameManager.Instance.ChangeState(GameState.HeroesTurn);
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

        if (path != null && path.Count <= unit.Movement)
        {
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
            }
            else
            {
                Debug.Log($"Path too long: {path.Count} ({unit.Movement})");
            }
        }
        //SetUnit(unit);
        return 0;
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
            Debug.Log($"{e}'s turn!");
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
                    //move attack
                    break;
                default:
                    Debug.Log("unexpeceted alertness");
                    break;

            }
        }

        MenuManager.Instance.ToggleTurn();
    }
}

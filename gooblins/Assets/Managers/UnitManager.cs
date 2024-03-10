using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class UnitManager : MonoBehaviour {
    public static UnitManager Instance;

    private List<ScriptableUnit> _units;
    private List<BaseEnemy> enemies;
    public BaseHero SelectedHero;

    void Awake() {
        Instance = this;

        _units = Resources.LoadAll<ScriptableUnit>("Units").ToList();
        foreach (ScriptableUnit enemy in _units.Where(u => u.Faction == Faction.Enemy))
        {
            enemies.Add((BaseEnemy)enemy.UnitPrefab);
        }
    }

    public void SpawnHeroes() {
        var heroCount = 1;

        for (int i = 0; i < heroCount; i++) {
            var randomPrefab = GetRandomUnit<BaseHero>(Faction.Hero);
            var spawnedHero = Instantiate(randomPrefab);
            var randomSpawnTile = GridManager.Instance.GetHeroSpawnTile();

            randomSpawnTile.SetUnit(spawnedHero);
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

            randomSpawnTile.SetUnit(spawnedEnemy);
            spawnedEnemy.OccupiedTile = randomSpawnTile;
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
        if (!destination._isWalkable)
        {
            Debug.Log("NOT WALKABLE NERD");
            return 0;
        }
        Debug.Log("MoveUnit() started");
        //List<NodeBase> path = NodeBase.FindPath(new NodeBase(unit.OccupiedTile), new NodeBase(this));
        List<NodeBase> path = NodeBase.FindPath(unit.OccupiedTile._position, destination._position);

        if (path != null && path.Count <= unit.Movement)
        {
            Debug.Log($"Path found: {path.Count}");
            Debug.Log("Movement should be happening");
            SetUnit(unit);
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

    //public void SetUnit(BaseUnit unit)
    //{
    //    if (unit.OccupiedTile != null) unit.OccupiedTile.OccupiedUnit = null;
    //    unit.transform.position = transform.position;
    //    OccupiedUnit = unit;
    //    unit.OccupiedTile = this;
    //}

    //public void EnemyTurn()
    //{
    //    foreach(BaseEnemy e in enemies)
    //    {
    //        e
    //    }
    //}
}

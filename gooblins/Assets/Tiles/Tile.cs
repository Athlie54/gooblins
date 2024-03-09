using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public abstract class Tile : MonoBehaviour {
    public string TileName;
    [SerializeField] protected SpriteRenderer _renderer;
    [SerializeField] private GameObject _highlight;
    [SerializeField] public bool _isWalkable;
    public Vector2Int _position;

    public BaseUnit OccupiedUnit;
    public bool Walkable => _isWalkable && OccupiedUnit == null;


    public virtual void Init(int x, int y)
    {
      _position = new Vector2Int(x, y);
    }

    void OnMouseEnter()
    {
        _highlight.SetActive(true);
        MenuManager.Instance.ShowTileInfo(this);
    }

    void OnMouseExit()
    {
        _highlight.SetActive(false);
        MenuManager.Instance.ShowTileInfo(null);
    }

    void OnMouseDown() {
        //if(GameManager.Instance.GameState != GameState.HeroesTurn) return;

        if (OccupiedUnit != null) {
            if(OccupiedUnit.Faction == Faction.Hero) UnitManager.Instance.SetSelectedHero((BaseHero)OccupiedUnit);
            else {
                if (UnitManager.Instance.SelectedHero != null) {
                    var enemy = (BaseEnemy) OccupiedUnit;
                    Destroy(enemy.gameObject);
                    UnitManager.Instance.SetSelectedHero(null);
                }
            }
        }
        else {
            if (UnitManager.Instance.SelectedHero != null) {
                Debug.Log("About to move");
                MoveUnit(UnitManager.Instance.SelectedHero);
                Debug.Log("Successfully moved");
                UnitManager.Instance.SetSelectedHero(null);
            }
        }

    }

    public int MoveUnit(BaseUnit unit)
    {
        if (!_isWalkable)
        {
            Debug.Log("NOT WALKABLE NERD");
            return 0;
        }
        Debug.Log("MoveUnit() started");
        List<NodeBase> path = NodeBase.FindPath(new NodeBase(unit.OccupiedTile), new NodeBase(this));
        if (path != null && path.Count <= 5)
        {
            Debug.Log($"Path found: {path.Count}");
            Debug.Log("Movement should be happening");
            SetUnit(unit);
        } else
        {
            if (path == null)
            {
                Debug.Log("No path :(");
            } else
            {
                Debug.Log($"Path too long: {path.Count}");
            }
        }
        //SetUnit(unit);
        return 0;
    }

    public void SetUnit(BaseUnit unit) {
        if (unit.OccupiedTile != null) unit.OccupiedTile.OccupiedUnit = null;
        unit.transform.position = transform.position;
        OccupiedUnit = unit;
        unit.OccupiedTile = this;
    }
}
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Unity.VisualScripting;
using UnityEditorInternal;
using UnityEngine;

public abstract class Tile : MonoBehaviour {
    public string TileName;
    [SerializeField] protected SpriteRenderer _renderer;
    [SerializeField] private GameObject _highlight;
    [SerializeField] public bool _isWalkable;
    [SerializeField] private Tile _floorTile;
    public List<Interactible> _interactibles;
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
        if (GameManager.Instance.GameState == GameState.HeroesTurn && UnitManager.Instance.SelectedHero != null)
        {
            
            List<NodeBase> path = NodeBase.FindPath(UnitManager.Instance.SelectedHero.OccupiedTile._position, this._position);
            MenuManager.Instance.movableHighlighted = (this._isWalkable && path.Count <= UnitManager.Instance.SelectedHero.Movement);
            MenuManager.Instance.ShowTileInfo(this);

            // bombs
            if (UnitManager.Instance.SelectedHero.name.StartsWith("Bammo") && this is CrackTile)
            {
                if (path.Count == 2)
                {
                    // bomb possibie
                }
            }
        }
    }

    void OnMouseExit()
    {
        _highlight.SetActive(false);
        if (GameManager.Instance.GameState == GameState.HeroesTurn)
        {
            MenuManager.Instance.ShowTileInfo(null);
        }
    }

    void OnMouseDown() {
        //if(GameManager.Instance.GameState != GameState.HeroesTurn) return;
        if (GameManager.Instance.GameState == GameState.HeroesTurn)
        {
            if (OccupiedUnit != null)
            {
                if (OccupiedUnit.Faction == Faction.Hero) UnitManager.Instance.SetSelectedHero((BaseHero)OccupiedUnit);
                else
                {
                    if (UnitManager.Instance.SelectedHero != null)
                    {
                        var enemy = (BaseEnemy)OccupiedUnit;
                        var hero = UnitManager.Instance.SelectedHero;
                        hero.UnitAction(enemy, hero);
                        UnitManager.Instance.SetSelectedHero(null);
                    }
                }
            }
            else
            {
                Debug.Log(UnitManager.Instance.SelectedHero.name);
                Debug.Log(this is CrackTile);
                if (UnitManager.Instance.SelectedHero.name.StartsWith("Bammo") && this is CrackTile)
                {
                    this._isWalkable = true;
                    List<NodeBase> path = NodeBase.FindPath(UnitManager.Instance.SelectedHero.OccupiedTile._position, this._position);

                    if ( path.Count <= 2)
                    {
                        Debug.Log("BOMBING TIME BABY");
                        this.GetComponentInChildren<SpriteRenderer>().sprite = Resources.LoadAll<Sprite>("Sprites").ToList().Where(s => s.name == "FloorTile").First();
                        this._isWalkable = true;
                    } else
                    {
                        this._isWalkable = false;
                    }
                                   
                }
                if (UnitManager.Instance.SelectedHero != null)
                {
                    Debug.Log("about to move");
                    UnitManager.Instance.MoveUnit(UnitManager.Instance.SelectedHero, this);
                    Debug.Log("successfully moved");
                    UnitManager.Instance.SelectedHero = null;

                }
            }
        }

    }

    // Moves given unit to this tile if possible
    //public int MoveUnit(BaseUnit unit)
    //{
    //    if (!_isWalkable)
    //    {
    //        Debug.Log("NOT WALKABLE NERD");
    //        return 0;
    //    }
    //    Debug.Log("MoveUnit() started");
    //    //List<NodeBase> path = NodeBase.FindPath(new NodeBase(unit.OccupiedTile), new NodeBase(this));
    //    List<NodeBase> path = NodeBase.FindPath(unit.OccupiedTile._position, this._position);

    //    if (path != null && path.Count <= unit.Movement)
    //    {
    //        Debug.Log($"Path found: {path.Count}");
    //        Debug.Log("Movement should be happening");
    //        SetUnit(unit);
    //        unit.Movement -= path.Count;
    //    } else
    //    {
    //        if (path == null)
    //        {
    //            Debug.Log("No path :(");
    //        } else
    //        {
    //            Debug.Log($"Path too long: {path.Count} ({unit.Movement})");
    //        }
    //    }
    //    //SetUnit(unit);
    //    return 0;
    //}

    //public void SetUnit(BaseUnit unit) {
    //    if (unit.OccupiedTile != null) unit.OccupiedTile.OccupiedUnit = null;
    //    unit.transform.position = transform.position;
        //OccupiedUnit = unit;
    //    unit.OccupiedTile = this;
    //}

}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class MenuManager : MonoBehaviour {
    public static MenuManager Instance;

    [SerializeField] private GameObject _selectedHeroObject,_tileObject,_tileUnitObject, _endTurnButtonObject;
    public bool movableHighlighted = false;

    void Awake() {
        Instance = this;
    }

    public void ShowTileInfo(Tile tile) {

        //Debug.Log("did it");

        //Debug.Log(_tileObject);
        //_tileObject.GetComponentInChildren<TextMeshProUGUI>().text = "bob";
        

         if (tile == null)
         {
             _tileObject.SetActive(false);
             _tileUnitObject.SetActive(false);
             return;
         }

         _tileObject.GetComponentInChildren<TextMeshProUGUI>().text = movableHighlighted ? $"Move to {tile.TileName}" : $"Cannot move to {tile.TileName}";
         _tileObject.SetActive(true);

         if (tile.OccupiedUnit) {
             _tileUnitObject.GetComponentInChildren<TextMeshProUGUI>().text = tile.OccupiedUnit.UnitName;
             _tileUnitObject.SetActive(true);
         }
         
    }

    public void ShowSelectedHero(BaseHero hero) {
        
        if (hero == null) {
            _selectedHeroObject.SetActive(false);
            return;
        }

        _selectedHeroObject.GetComponentInChildren<TextMeshProUGUI>().text = hero.UnitName;
        _selectedHeroObject.SetActive(true);
        
        
    }

    public void ToggleTurn()
    {

        Debug.Log("turn toggled");

        if(GameManager.Instance.GameState == GameState.HeroesTurn) //switch to enemy turn
        {
            //toggle all ui
            _selectedHeroObject.SetActive(false);
            _tileObject.SetActive(false);
            _tileUnitObject.SetActive(false);
            _endTurnButtonObject.SetActive(false);
            GameManager.Instance.ChangeState(GameState.EnemiesTurn);
            UnitManager.Instance.EnemyTurn();
        }
        else //switch to hero turn
        {
            _endTurnButtonObject.SetActive(true);
            GameManager.Instance.ChangeState(GameState.HeroesTurn);
        }
        return;
    }

    //public void EndTurn()
    //{
    //    _selectedHeroObject.SetActive(false);
    //    _tileObject.SetActive(false);
    //    _tileUnitObject.SetActive(false);
    //    _endTurnButtonObject.SetActive(false);

    //    GameManager.Instance.ChangeState(GameState.EnemiesTurn);
    //    UnitManager.Instance.EnemyTurn();
    //    return;
    //}
}

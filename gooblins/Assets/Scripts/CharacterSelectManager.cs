using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class CharacterSelectManager : MonoBehaviour
{
    public static CharacterSelectManager Instance;

    [SerializeField]
    private GameObject preview1;
    [SerializeField]
    private GameObject preview2;
    [SerializeField]
    private GameObject preview3;

    [SerializeField]
    private CharacterSelect defaultHighlight;

    public GameObject mainPreview;
    public GameObject mainName;
    public GameObject mainDesc;

    public static Gooblins[] team = { Gooblins.Bammo, Gooblins.Gab, Gooblins.Skittles, };

    private static List<Sprite> _sprites;

    public static Dictionary<Gooblins, string> spriteNames = new Dictionary<Gooblins, string>()
    {
        {Gooblins.Bammo, "Bammo" },
        {Gooblins.Gab, "Gab"  },
        {Gooblins.Kaklik, "Kaklik" },
        {Gooblins.Pleep, "Pleep" },
        {Gooblins.Skittles, "Skittles" },
        {Gooblins.Thud, "Thud" },
        {Gooblins.Zyinks, "Zyinks" }
    };

    void Awake()
    {
        Instance = this;
        _sprites = Resources.LoadAll<Sprite>("Sprites").ToList();

    }

    // Start is called before the first frame update
    void Start()
    {
        defaultHighlight.Highlight();

        preview1.SetActive(false);
        preview2.SetActive(false);
        preview3.SetActive(false);


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdatePreview(Gooblins gooblin)
    {
        Debug.Log(this.mainPreview.GetComponentInChildren<UnityEngine.UI.Image>());
        Debug.Log(_sprites.Count);
        Debug.Log(_sprites.Where(s => s.name == spriteNames[gooblin]).First());
        this.mainPreview.GetComponentInChildren<UnityEngine.UI.Image>().sprite = _sprites.Where(s => s.name == spriteNames[gooblin]).First();
    }

    public void AddTeamMember(Gooblins gooblin)
    {
        int slot = 0;
        while (team[slot] != Gooblins.None && slot < 3)
        {
            slot++;
        }
        if (slot < 3)
        {
            team[slot] = gooblin;
            switch (slot)
            {
                case 0:
                    this.preview1.GetComponentInChildren<UnityEngine.UI.Image>().sprite = _sprites.Where(s => s.name == spriteNames[gooblin]).First();
                    this.preview1.SetActive(true);
                    break;
                case 1:
                    this.preview2.GetComponentInChildren<UnityEngine.UI.Image>().sprite = _sprites.Where(s => s.name == spriteNames[gooblin]).First();
                    this.preview2.SetActive(true);
                    break;
                case 2:
                    this.preview3.GetComponentInChildren<UnityEngine.UI.Image>().sprite = _sprites.Where(s => s.name == spriteNames[gooblin]).First();
                    this.preview3.SetActive(true);
                    break;
            }
        }
    }

    public void RemoveTeamMember(int slot)
    {
        while (team[slot] != Gooblins.None && slot < 2)
        {
            team[slot] = team[slot + 1];
            switch (slot)
            {
                case 0:
                    if (team[slot] == Gooblins.None)
                        this.preview1.SetActive(false);
                    else
                        this.preview1.GetComponentInChildren<UnityEngine.UI.Image>().sprite = _sprites.Where(s => s.name == spriteNames[team[slot]]).First();
                    break;
                case 1:
                    if (team[slot] == Gooblins.None)
                        this.preview2.SetActive(false);
                    else
                        this.preview2.GetComponentInChildren<UnityEngine.UI.Image>().sprite = _sprites.Where(s => s.name == spriteNames[team[slot]]).First();
                    break;
                case 2:
                    if (team[slot] == Gooblins.None)
                        this.preview3.SetActive(false);
                    else
                        this.preview3.GetComponentInChildren<UnityEngine.UI.Image>().sprite = _sprites.Where(s => s.name == spriteNames[team[slot]]).First();
                    break;
            }
            slot++;
        }
        team[2] = Gooblins.None;

        switch (slot)
        {
            case 0:
                this.preview1.SetActive(false);
                break;
            case 1:
                this.preview2.SetActive(false);
                break;
            case 2:
                this.preview3.SetActive(false);
                break;
        }

    }
}

public enum Gooblins
{
    Bammo,
    Gab,
    Kaklik,
    Pleep,
    Skittles,
    Thud,
    Zyinks,
    None
}

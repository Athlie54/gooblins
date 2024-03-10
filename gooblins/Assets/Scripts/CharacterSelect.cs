using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelect : MonoBehaviour
{
    public Gooblins gooblin;
    public string name;
    [TextAreaAttribute]
    public string description;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Highlight()
    {
        Debug.Log($"hey ya {CharacterSelectManager.Instance.mainDesc}");
        CharacterSelectManager.Instance.mainDesc.GetComponentInChildren<TextMeshProUGUI>().text = description;
        CharacterSelectManager.Instance.mainName.GetComponentInChildren<TextMeshProUGUI>().text = name;
        CharacterSelectManager.Instance.UpdatePreview(gooblin);

        // Image
    }

    public void AddToTeam()
    {
        Debug.Log($"hey ya {CharacterSelectManager.Instance.mainDesc}");
        CharacterSelectManager.Instance.AddTeamMember(gooblin);
    }
}

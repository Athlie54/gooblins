using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    // Start is called before the first frame update
    public static void loadMenu()
    {
        Debug.Log("button clicked");
        SceneManager.LoadScene("MenuScene");
    }
    public static void loadSelect()
    {
        Debug.Log("button clicked");
        SceneManager.LoadScene("CharacterSelectScene");
    }
    public static void loadLevel()
    {
        Debug.Log("button clicked");
        SceneManager.LoadScene("SampleScene");
    }
}

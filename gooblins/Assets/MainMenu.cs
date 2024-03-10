using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    //This function goes to the next scene in the Build Settings Queue; this is 
    //hand-ordered, and in this case goes to SceneOne
    //Window -> Build Settings -> drop scenes in order to order the build settings
    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    //This function quits the game after logging the quit message
    public void QuitGame()
    {
        Debug.Log("QUITTING!");
        Application.Quit();
    }
}

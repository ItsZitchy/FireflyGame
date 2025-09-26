using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
   public void StartGame()
   {
        Debug.Log("IN-GAME");
        SceneManager.LoadScene("Gort Scene");
   }
    public void QuitGame()
    {
        Debug.Log("QUIT");
        Application.Quit();

        Input.GetButtonDown("Escape");
    }
}

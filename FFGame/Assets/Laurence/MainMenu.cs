using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            Debug.Log("QUIT");
            Application.Quit();
        }
    }

    public void StartGame()
   {
        Debug.Log("IN-GAME");
        SceneManager.LoadScene("Jol Scene");
   }
    public void QuitGame()
    {
        Debug.Log("QUIT");
        Application.Quit();
    }
}

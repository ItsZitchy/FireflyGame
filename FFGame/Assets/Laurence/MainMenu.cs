using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject _mainMenuObject;
    [SerializeField] GameObject _quitObject;
    [SerializeField] GameObject _optionsMenuObject; 

    public void InGame()
    {
        Debug.Log("In-game");
        SceneManager.LoadScene("Jol Scene");
    }
    public void QuitGame()
    {
        Debug.Log("Quit"); 
    }
    public void OptionsMenu()
    {
        _mainMenuObject.SetActive(false);
        _optionsMenuObject.SetActive(true); 
    }
}

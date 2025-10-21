using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject _mainMenuObject;
    [SerializeField] GameObject _quitConfirmObject;
    public Animator Anim;
    public string sceneSwitch;

    public void Start()
    {
        Anim = GetComponent<Animator>();
    }

    public void InGame()
    {
        Debug.Log("In-game");
        //trigger animation

        //SceneManager.LoadScene("Jol Movement Scene");
        

        Anim.SetTrigger("SceneSwitch");
    }

    public void OutGame()
    {
        Anim.SetTrigger("QuitSwitch");

    }

    public void QuitGame()
    {
        _mainMenuObject.SetActive(false);
        _quitConfirmObject.SetActive(true);
    }

    public void ChangeScene()
    {
        SceneManager.LoadScene(sceneSwitch);
    }
}

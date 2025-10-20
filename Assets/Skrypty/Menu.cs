using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [SerializeField]
    private Canvas canvasMenu;
    [SerializeField]
    private Canvas canvasWyniki;
    void Start()
    {
        canvasMenu.gameObject.SetActive(true);
        canvasWyniki.gameObject.SetActive(false);
    }
    public void koniecGry()
    {
        Application.Quit();
    }
    public void setWyniki()
    {
        canvasMenu.gameObject.SetActive(false);
        canvasWyniki.gameObject.SetActive(true);
    }
    public void powrot()
    {
        canvasMenu.gameObject.SetActive(true);
        canvasWyniki.gameObject.SetActive(false);
    }
    public void startGry()
    {
        SceneManager.LoadSceneAsync(1);
    }
}

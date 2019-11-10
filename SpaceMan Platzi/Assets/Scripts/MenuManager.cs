using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public static MenuManager sharedInstance;
    public GameObject menuCanvas;
    public GameObject hudCanvas;

    private void Awake()
    {
        if (sharedInstance == null)
        {
            sharedInstance = this;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ShowMainMenu()
    {
        menuCanvas.SetActive(true);
        hudCanvas.SetActive(false);
    }
    public void HideMainMenu()
    {
        menuCanvas.SetActive(false);
        hudCanvas.SetActive(true);
    }
    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}

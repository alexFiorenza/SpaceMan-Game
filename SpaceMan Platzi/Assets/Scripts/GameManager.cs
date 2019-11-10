using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Este es un numerador que sirve para ponerlo en diferentes estados
public enum GameState
{
    menu,
    inGame,
    gameOver
}
public class GameManager : MonoBehaviour
{
    public GameState currentGameState = GameState.menu;
    public static GameManager sharedInstance; //Un singletone
    PlayerController controller;
    public int collectedObject = 0;
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
        controller = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SetGameState(GameState.menu);
        }
    }


    //Empezar el juego en el menu
    public void StartGame()
    {
        if (currentGameState == GameState.menu || currentGameState==GameState.gameOver)
        {
            SetGameState(GameState.inGame);
        }
        
    }

    //Volver al menu
    public void BackToMenu()
    {
        SetGameState(GameState.menu);
    }

    //Perder
    public void GameOver()
    {
        SetGameState(GameState.gameOver);
    }

    private void SetGameState(GameState newGameState)
    {
        if (newGameState == GameState.menu)
        {
            MenuManager.sharedInstance.ShowMainMenu();
            Cursor.visible = true;
        }else if (newGameState == GameState.inGame)
        {
            LevelManager.sharedInstance.RemoveAllLevelBlocks();
            LevelManager.sharedInstance.GenerateInitialBlocks();
            controller.StartGame();
            MenuManager.sharedInstance.HideMainMenu();
            Cursor.visible = false;
        }

        else
        {
            MenuManager.sharedInstance.ShowMainMenu();  
        }

        this.currentGameState = newGameState;
    }

    public void CollectObject(Collectable collectable)
    {
        collectedObject += collectable.value;
    }
   
}
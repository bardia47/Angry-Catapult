using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameStates { 

    MAINMENU,
    INGAME,
    PAUSE,
    END
}
public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public const string SCENE_MENU = "Menu";
    public const string SCENE_LEVEL = "Level";

    public GameStates currentState;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(this.gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode arg1)
    {
        if (scene.name == SCENE_MENU)
        {
            ChangeState(GameStates.MAINMENU);
        }
        else
            ChangeState(GameStates.INGAME);
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded-= OnSceneLoaded;
    }

    public static void ChangeState(GameStates newState)
    {
        if (newState == instance.currentState)
            return;
        instance.currentState = newState;
        switch (newState)
        {
            case GameStates.MAINMENU:
            case GameStates.INGAME:
            case GameStates.END:
                Time.timeScale = 1f;
                break;
            case GameStates.PAUSE:
                Time.timeScale = 0f;
                break;
              
        }
    }
}

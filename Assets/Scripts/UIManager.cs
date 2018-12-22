using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject mainCamera;

    private MainMenuState state;

    private GameController gameController;

    private void Start()
    {
        gameController = GetComponent<GameController>();

        state = MainMenuState.START_SCREEN;
    }

    private void OnGUI()
    {
        switch (state)
        {
            case MainMenuState.START_SCREEN:
                if (GUILayout.Button("Enter", GUILayout.Height(40)))
                {
                    LoadMainMenu();
                }
                break;
            case MainMenuState.MAIN_MENU:
                if (GUILayout.Button("Start", GUILayout.Height(40)))
                {
                    LoadGame();
                }
                if (GUILayout.Button("Quit", GUILayout.Height(40)))
                {
                    ExitGame();
                }
                break;
            case MainMenuState.IN_GAME:
                GUILayout.TextField("Level: " + gameController.MapLevel + " - Time left: " + gameController.TimeLeftForMap);
                GUILayout.TextField("Points: " + gameController.Points);
                foreach(KeyValuePair<RockType, int> pair in gameController.RocksBroken)
                {
                    GUILayout.TextField(pair.Key + ": " + pair.Value);
                }
                break;
            case MainMenuState.LEVEL_FINISHED:
                GUILayout.TextField("Level: " + gameController.MapLevel + " - Time left: " + gameController.TimeLeftForMap);
                GUILayout.TextField("Points: " + gameController.Points);
                foreach (KeyValuePair<RockType, int> pair in gameController.RocksBroken)
                {
                    GUILayout.TextField(pair.Key + ": " + pair.Value);
                }
                if (GUILayout.Button("Next map!", GUILayout.Height(40)))
                {
                    LoadGame();
                }
                break;
            case MainMenuState.GAME_FINISHED:
                GUILayout.TextField("Level: " + gameController.MapLevel + " - Time left: " + gameController.TimeLeftForMap);
                GUILayout.TextField("Points: " + gameController.Points);
                foreach (KeyValuePair<RockType, int> pair in gameController.RocksBroken)
                {
                    GUILayout.TextField(pair.Key + ": " + pair.Value);
                }
                if (GUILayout.Button("Exit to Main Menu", GUILayout.Height(40)))
                {
                    ExitToMainMenu();
                }
                break;
        }
    }

    private void LoadMainMenu()
    {
        mainCamera.transform.position += Vector3.left * 600;
        state = MainMenuState.MAIN_MENU;
    }

    private void LoadGame()
    {
        state = MainMenuState.IN_GAME;
        mainCamera.SetActive(false);
        gameController.StartGame();
    }

    public void LevelFinished()
    {
        state = MainMenuState.LEVEL_FINISHED;
    }

    public void GameFinished()
    {
        state = MainMenuState.GAME_FINISHED;
    }

    private void ExitToMainMenu()
    {
        state = MainMenuState.MAIN_MENU;
        gameController.MainMenuMusic();
        mainCamera.SetActive(true);
        Destroy(StaticObjects.Player);
    }

    private void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
                    Application.Quit();
#endif
    }
}

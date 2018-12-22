using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject startScreenCamera;
    [SerializeField]
    private GameObject mainMenuCamera;

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
        }
    }

    private void LoadMainMenu()
    {
        startScreenCamera.SetActive(false);
        mainMenuCamera.SetActive(true);
        state = MainMenuState.MAIN_MENU;
    }

    private void LoadGame()
    {
        state = MainMenuState.IN_GAME;
        gameController.StartGame();
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

using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private MainMenuState state;

    [SerializeField]
    private GameController gameController;
    [SerializeField]
    private MenuManager menuManager;

    private void OnGUI()
    {
        switch (state)
        {
            case MainMenuState.STAND_BY:
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
                    menuManager.StartGame();
                    SetInGame();
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
                    menuManager.OnClickExitToMain();
                    SetStandBy();
                }
                break;
        }
    }
    
    public void SetStandBy()
    {
        state = MainMenuState.STAND_BY;
    }

    public void SetInGame()
    {
        state = MainMenuState.IN_GAME;
    }

    public void SetLevelFinished()
    {
        state = MainMenuState.LEVEL_FINISHED;
    }

    public void SetGameFinished()
    {
        state = MainMenuState.GAME_FINISHED;
    }
}

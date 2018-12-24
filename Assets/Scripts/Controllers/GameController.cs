using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private GameObject playerPrefab;

    private MapGenerator mapGenerator;

    private UIManager uiManager;

    public int Points { get; private set; }
    public int MapLevel { get; private set; }
    public int TimeLeftForMap { get; private set; }
    public Dictionary<RockType, int> RocksBroken { get; private set; }

    private int numberOfMapsPerGame;

    private int startTimeForMap;
    private WaitForSeconds delaySecond;

    private GameController()
    {
        startTimeForMap = 60;
        numberOfMapsPerGame = 3;
    }

    private void Start()
    {
        StaticObjects.GameController = this;

        uiManager = GetComponent<UIManager>();

        mapGenerator = GetComponent<MapGenerator>();
        playerPrefab = Resources.Load<GameObject>("Player");

        delaySecond = new WaitForSeconds(1);
    }

    public void StartGame()
    {
        mapGenerator.GenerateMap();
        uiManager.SetInGame();
        StaticObjects.BGMManager.PlayBGM(1);
        if (StaticObjects.Player != null)
        {
            StaticObjects.Player.transform.position = mapGenerator.MapTerrain.PlayerSpawn;
        }
        else
        {
            SpawnPlayer();
        }
        StaticObjects.PlayerMovement.CanMove = true;
        StaticObjects.PlayerPickaxeManager.CanMine = true;
        StartCoroutine(TimeForMap());
    }

    private void SpawnPlayer()
    {
        RocksBroken = new Dictionary<RockType, int>();
        RocksBroken.Add(RockType.COAL, 0);
        RocksBroken.Add(RockType.IRON, 0);
        RocksBroken.Add(RockType.GOLD, 0);
        RocksBroken.Add(RockType.DIAMOND, 0);

        MapLevel = 0;

        GameObject player = Instantiate(playerPrefab, mapGenerator.MapTerrain.PlayerSpawn, Quaternion.identity);
        StaticObjects.Player = player;
        StaticObjects.PlayerMouseManager = player.GetComponent<MouseManager>();
        StaticObjects.PlayerMovement = player.GetComponent<PlayerMovement>();
        StaticObjects.PlayerPickaxeManager = player.GetComponent<PickaxeManager>();
    }

    public void RockDestroyed(int points)
    {
        RocksBroken[(RockType)points]++;
        Points += points + 1;
    }

    private IEnumerator TimeForMap()
    {
        MapLevel++;
        TimeLeftForMap = startTimeForMap;

        while (TimeLeftForMap > 0)
        {
            yield return delaySecond;

            TimeLeftForMap--;
            if (TimeLeftForMap == 11)
            {
                StaticObjects.BGMManager.PlayBGM(2);
            }
            if (TimeLeftForMap <= 5)
            {
                if (TimeLeftForMap > 0)
                {
                    StaticObjects.SoundEffectManager.PlaySound(0);
                }
            }
        }

        StaticObjects.PlayerPickaxeManager.CanMine = false;
        StaticObjects.PlayerMovement.CanMove = false;
        StaticObjects.PlayerMovement.StopAllMovement();

        StaticObjects.SoundEffectManager.PlaySound(1);
        if (numberOfMapsPerGame == MapLevel)
        {
            StaticObjects.BGMManager.PlayBGM(4);
            uiManager.SetGameFinished();
        }
        else
        {
            StaticObjects.BGMManager.PlayBGM(3);
            uiManager.SetLevelFinished();
        }
    }
}

using System.Collections;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private GameObject playerPrefab;

    private MapGenerator mapGenerator;

    [SerializeField]
    private BGMManager bgmManager;

    public int Points { get; private set; }
    public int MapLevel { get; private set; }
    public int TimeLeftForMap { get; private set; }

    private int startTimeForMap;
    private WaitForSeconds delaySecond;

    private GameController()
    {
        startTimeForMap = 20;
    }

    private void Start()
    {
        StaticObjects.GameController = this;

        mapGenerator = GetComponent<MapGenerator>();
        playerPrefab = Resources.Load<GameObject>("Player");

        delaySecond = new WaitForSeconds(1);
    }

    public void StartGame()
    {
        MapLevel++;
        mapGenerator.GenerateMap();
        bgmManager.PlayBGM(1);
        SpawnPlayer();
        StartCoroutine(TimeForMap());
    }

    private void SpawnPlayer()
    {
        GameObject player = Instantiate(playerPrefab, mapGenerator.MapTerrain.PlayerSpawn, Quaternion.identity);
        StaticObjects.Player = player;
        StaticObjects.PlayerMouseManager = player.GetComponent<MouseManager>();
    }

    public void RockDestroyed(int points)
    {
        Points += points + 1;
    }

    private IEnumerator TimeForMap()
    {
        TimeLeftForMap = startTimeForMap;

        while (TimeLeftForMap > 0)
        {
            yield return delaySecond;

            TimeLeftForMap--;
            if (TimeLeftForMap == 11)
            {
                bgmManager.PlayBGM(2);
            }
            if (TimeLeftForMap <= 10)
            {
                if (TimeLeftForMap > 0)
                {
                    //play tick sound
                }
                else
                {
                    //play end sound
                }
            }
        }
    }
}

using UnityEngine;

public class GameController : MonoBehaviour
{
    private GameObject playerPrefab;

    private MapGenerator mapGenerator;

    public int Points { get; protected set; }

    private void Start()
    {
        StaticObjects.GameController = this;

        mapGenerator = GetComponent<MapGenerator>();
        playerPrefab = Resources.Load<GameObject>("Player");
    }

    public void StartGame()
    {
        mapGenerator.GenerateMap();
        SpawnPlayer();
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
}

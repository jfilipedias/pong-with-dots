using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // Singleton design
    public static GameManager main;

    public GameObject ballPrefab;

    public float xBound = 3f;
    public float yBound = 3f;
    public float ballSpeed = 3f;
    public float respawnDelay = 2f;
    public int[] playerScores;

    public Text mainText;
    public Text[] playerTexts;

    private Entity ballEntityPrefab;
    private EntityManager manager;

    private WaitForSeconds oneSecond;
    private WaitForSeconds delay;

    private void Awake()
    {
        if(main != null && main != this)
        {
            Destroy(gameObject);
            return;
        }

        main = this;
        playerScores = new int[2];

        oneSecond = new WaitForSeconds(1f);
        delay = new WaitForSeconds(respawnDelay);

        StartCoroutine(CountdownAndSpawnBall());
    }
    

    public void PlayerScored(int playerID)
    {
        playerScores[playerID]++;

        for (int count = 0; count < playerScores.Length; count++)
            playerTexts[count].text = playerScores[count].ToString();
    }


    private IEnumerator CountdownAndSpawnBall()
    {
        mainText.text = "Get Ready";
        yield return delay;

        mainText.text = "3";
        yield return oneSecond;

        mainText.text = "3";
        yield return oneSecond;

        mainText.text = "3";
        yield return oneSecond;

        SpawnBall();
    }


    private void SpawnBall()
    {

    }
}

using System.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager main;

    public GameObject ballPrefab;

    public float xBound = 3f;
    public float yBound = 3f;
    public float ballSpeed = 3f;
    public float respawnDelay = 2f;
    private int[] playerScores;

    public Text mainText;
    public Text[] playerTexts;

    private Entity ballEntityPrefab;
    private EntityManager entityManager;

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


        entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

        GameObjectConversionSettings conversionSettings =  GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, new BlobAssetStore());
        ballEntityPrefab = GameObjectConversionUtility.ConvertGameObjectHierarchy(ballPrefab, conversionSettings);

        oneSecond = new WaitForSeconds(1f);
        delay = new WaitForSeconds(respawnDelay);

        StartCoroutine(CountdownAndSpawnBall());
    }
    

    public void PlayerScored(int playerID)
    {
        playerScores[playerID]++;

        for (int count = 0; count < playerScores.Length; count++)
            playerTexts[count].text = playerScores[count].ToString();

        StartCoroutine(CountdownAndSpawnBall());
    }


    private IEnumerator CountdownAndSpawnBall()
    {
        mainText.text = "Get Ready";
        yield return delay;

        mainText.text = "3";
        yield return oneSecond;

        mainText.text = "2";
        yield return oneSecond;

        mainText.text = "1";
        yield return oneSecond;

        mainText.text = "";

        SpawnBall();
    }


    private void SpawnBall()
    {
        Entity ball = entityManager.Instantiate(ballEntityPrefab);

        Vector3 direction = new Vector3(UnityEngine.Random.Range(0, 2) == 0 ? -1 : 1, UnityEngine.Random.Range(-.5f, .5f), 0f).normalized;
        Vector3 speed = direction * ballSpeed;

        PhysicsVelocity velocity = new PhysicsVelocity()
        {
            Linear = speed,
            Angular = float3.zero
        };

        entityManager.AddComponentData(ball, velocity);
    }
}

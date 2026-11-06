using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private float spawnInterval = 3f;
    [SerializeField] private int maxEnemies = 5;
    [SerializeField] private Vector2 areaSize = new Vector2(10f, 6f);
    [SerializeField] private float minDistanceFromPlayer = 3f;

    private Transform player;
    private float timer;

    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null) player = playerObj.transform;
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer < spawnInterval) return;
        timer = 0f;

        int alive = FindObjectsByType<Enemy>(FindObjectsSortMode.None).Length;
        if (alive >= maxEnemies) return;

        Instantiate(enemyPrefab, RandomSpawnPoint(), Quaternion.identity);
    }

    Vector3 RandomSpawnPoint()
    {
        Vector2 point = transform.position;

        for (int i = 0; i < 10; i++)
        {
            point = (Vector2)transform.position + new Vector2(
                Random.Range(-areaSize.x / 2f, areaSize.x / 2f),
                Random.Range(-areaSize.y / 2f, areaSize.y / 2f));

            if (player == null || Vector2.Distance(point, player.position) >= minDistanceFromPlayer)
                break;
        }
        return point;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, areaSize);
    }
}
using UnityEngine;

public class FlyingObjectSpawnScript : MonoBehaviour
{
    ScreenBoundariesScript screenBoundariesScript;
    [Header("Cloud Prefabs")]
    public GameObject[] cloudPrefabs;
    [Header("Plain Prefabs")]
    public GameObject[] plainPrefabs;

    [Header("Spawn Point")]
    public Transform spawnPoint;

    [Header("Spawn Interval")]
    public float cloudSpawnInterval = 2f;
    public float plainSpawnInterval = 3f;

    private float minY, maxY;

    [Header("Cloud Speed")]
    public float cloudMinSpeed = 1.5f;
    public float cloudMaxSpeed = 150f;

    [Header("Plain Speed")]
    public float plainMinSpeed = 2f;
    public float plainMaxSpeed = 100f;

    
    void Start()
    {
        // Calculate vertical bounds dynamically based on Camera size and aspect ratio
        if (Camera.main != null && Camera.main.orthographic)
        {
            float camHeight = Camera.main.orthographicSize;
            // Leave a little padding (e.g., 10% from top and bottom so they don't spawn half-cut off)
            minY = -camHeight * 0.8f;
            maxY = camHeight * 0.8f;
        }
        else
        {
            // Fallback if camera isn't orthographic
            minY = -4f;
            maxY = 4f;
        }

        InvokeRepeating(nameof(SpawnCloud), 0f, cloudSpawnInterval);
        InvokeRepeating(nameof(SpawnPlain), 0f, plainSpawnInterval);
    }

   void SpawnCloud()
    {
        if (cloudPrefabs.Length == 0) return;

        GameObject prefab = cloudPrefabs[Random.Range(0, cloudPrefabs.Length)];

        // Fallback safeguard for mobile resolution initialization timing
        float currentMinY = (screenBoundariesScript != null) ? screenBoundariesScript.worldBounds.yMin : -5f;
        float currentMaxY = (screenBoundariesScript != null) ? screenBoundariesScript.worldBounds.yMax : 5f;

        // If bounds are somehow still returning 0, fallback to a safe range based on screen size
        if (Mathf.Approximately(currentMinY, currentMaxY))
        {
            currentMinY = -4f;
            currentMaxY = 4f;
        }

        float y = Random.Range(currentMinY, currentMaxY);
        Vector3 pos = new Vector3(spawnPoint.position.x, y, spawnPoint.position.z);
        GameObject cloud = Instantiate(prefab, pos, Quaternion.identity, spawnPoint);

        float speed = Random.Range(cloudMinSpeed, cloudMaxSpeed);
        FlyingObjectControllerScript controller = cloud.GetComponent<FlyingObjectControllerScript>();
        if (controller != null)
            controller.speed = speed;
    }

    void SpawnPlain()
    {
        if (plainPrefabs.Length == 0) return;
        GameObject prefab = plainPrefabs[Random.Range(0, plainPrefabs.Length)];

        float currentMinY = (screenBoundariesScript != null) ? screenBoundariesScript.worldBounds.yMin : -5f;
        float currentMaxY = (screenBoundariesScript != null) ? screenBoundariesScript.worldBounds.yMax : 5f;

        if (Mathf.Approximately(currentMinY, currentMaxY))
        {
            currentMinY = -4f;
            currentMaxY = 4f;
        }

        float y = Random.Range(currentMinY, currentMaxY);
        Vector3 pos = new Vector3(-spawnPoint.position.x, y, spawnPoint.position.z);
        GameObject plain = Instantiate(prefab, pos, Quaternion.identity, spawnPoint);

        float speed = Random.Range(plainMinSpeed, plainMaxSpeed);
        FlyingObjectControllerScript controller = plain.GetComponent<FlyingObjectControllerScript>();
        if (controller != null)
            controller.speed = -speed;
    }
}

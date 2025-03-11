using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IconController : MonoBehaviour
{
    public GameObject iconSpawnerPrefab;  // Reference to the IconSpawner prefab (must be a UI element)
    public float timeBetweenSpawns = 3f;  // Time between each spawn in seconds

    // Reference to the Transform where you want to spawn the icon (could be any UI element)
    public Transform spawnTransform;

    private float timeSinceLastSpawn = 0f;  // Time passed since the last spawn
    public float moveSpeed = 3f;  // Speed of the object moving from -500 to 500 and back
    public float leftX = -500f;  // Left position
    public float rightX = 500f;  // Right position

    public LayerMask layerMask;  // Reference to the LayerMask that will define the correct layer for the spawn

    void Update()
    {
        MoveObjectBackAndForth();

        // Increment time since last spawn
        timeSinceLastSpawn += Time.deltaTime;

        // Check if enough time has passed to spawn a new icon
        if (timeSinceLastSpawn >= timeBetweenSpawns)
        {
            // Spawn the icon and reset the timer
            ShootIconDown();
            timeSinceLastSpawn = 0f;  // Reset the spawn timer
        }
    }

    private void MoveObjectBackAndForth()
    {
        // Use Mathf.PingPong to move the object between leftX and rightX
        float pingPongX = Mathf.PingPong(Time.time * moveSpeed, rightX - leftX) + leftX;

        // Set the new position of the IconController (this object)
        transform.position = new Vector3(pingPongX, transform.position.y, transform.position.z);
    }

    private void ShootIconDown()
    {
        // Check if the prefab and spawnTransform are set
        if (iconSpawnerPrefab != null && spawnTransform != null)
        {
            // Find all GameObjects in the specified layer
            GameObject[] objectsInLayer = FindObjectsInLayer(layerMask);
            if (objectsInLayer.Length == 0)
            {
                Debug.LogWarning("No objects found in the specified layer.");
                return;
            }

            // Use the first found object as the reference
            GameObject referenceObject = objectsInLayer[0];

            // Instantiate the prefab as a UI element at the position of spawnTransform
            GameObject iconClone = Instantiate(iconSpawnerPrefab, spawnTransform.position, Quaternion.identity);

            // Set the parent of the spawned object to the reference object's parent (use the layer's object parent)
            iconClone.transform.SetParent(referenceObject.transform, false);

            // Optionally, if you want the icon to move downward, use RectTransform for UI movement
            RectTransform rectTransform = iconClone.GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                // Set the icon's X and Y position to match the spawnTransform's X and Y values
                rectTransform.anchoredPosition = new Vector2(
                    spawnTransform.position.x,
                    spawnTransform.position.y  // Maintain the same Y position for the spawn
                );
            }
        }
    }

    // Helper method to find all GameObjects in a specific layer
    private GameObject[] FindObjectsInLayer(LayerMask layer)
    {
        List<GameObject> objectsInLayer = new List<GameObject>();
        Transform[] allTransforms = GameObject.FindObjectsOfType<Transform>(true); // Include inactive objects
        foreach (Transform t in allTransforms)
        {
            if (((1 << t.gameObject.layer) & layer.value) != 0)
            {
                objectsInLayer.Add(t.gameObject);
            }
        }
        return objectsInLayer.ToArray();
    }
}

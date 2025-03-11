using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;  // For accessing the Image component

public class IconSpawner : MonoBehaviour
{
    public Sprite[] sprites;  // Array of sprites to choose from
    public float speed = 200f;  // Speed of the downward movement
    public float timeObjAlive = 6f;  // Time before the icon is destroyed

    private Image imageComponent;  // Reference to the Image component of the child

    private void Start()
    {
        // Destroy the icon after a certain amount of time
        Destroy(gameObject, timeObjAlive);

        // Find the Image component in the child object
        imageComponent = GetComponentInChildren<Image>();

        // Check if the Image component is found and there are sprites in the array
        if (sprites.Length > 0 && imageComponent != null)
        {
            // Assign a random sprite from the array to the Image component
            imageComponent.sprite = sprites[Random.Range(0, sprites.Length)];
        }
        else
        {
            Debug.LogWarning("No sprites assigned to the array or no Image component found.");
        }
    }

    private void Update()
    {
        // Move the icon downward each frame, only affecting the Y position
        Vector3 newPosition = transform.position;
        newPosition.y += -speed * Time.deltaTime;  // Only move downwards (negative Y)
        transform.position = newPosition;
    }
}

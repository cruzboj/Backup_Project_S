using UnityEngine;
using UnityEngine.InputSystem;

public class CursorSpawnHandler : MonoBehaviour
{
    
    private void Awake()
    {
        // Get the Player Input Manager component
        PlayerInputManager playerInputManager = GetComponent<PlayerInputManager>();

        // Subscribe to the onPlayerJoined event
        if (playerInputManager != null)
        {
            playerInputManager.onPlayerJoined += OnPlayerJoined;
        }
        else
        {
            Debug.LogError("PlayerInputManager component is missing on this GameObject!");
        }
    }

    private void OnPlayerJoined(PlayerInput playerInput)
    {
        // Set the new player's parent to the Player Input Manager GameObject
        playerInput.transform.SetParent(transform);

        //playerInput.gameObject.name = $"Player_{playerInput.playerIndex}"; //

        Debug.Log($"Player {playerInput.playerIndex} joined and set as a child of Player Input Manager.");

        //Transform playerChild = playerInput.transform.GetChild(0);
        //NewVirtualMouse childScript = playerChild.GetComponent<NewVirtualMouse>();

        //if (childScript != null)
        //{
        //    // Assign the player's index to the NewVirtualMouse script
        //    childScript.index = playerInput.playerIndex;
        //}
        //else
        //{
        //    Debug.LogError("NewVirtualMouse script is missing on the player child!");
        //}
    }
}
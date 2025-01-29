using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterSelectManager : MonoBehaviour
{
    private PlayerInputManager playerInputManager;

    private void Start()
    {
        playerInputManager = FindObjectOfType<PlayerInputManager>();

        if (playerInputManager != null)
        {
            // Retrieve all PlayerInput components managed by PlayerInputManager
            var playerInputs = playerInputManager.GetComponentsInChildren<PlayerInput>();

            // Retrieve the devices from PlayerInput
            foreach (var playerInput in playerInputs)
            {
                var devices = playerInput.devices; // This gives you the devices for the player
                foreach (var device in devices)
                {
                    Debug.Log($"Device connected: {device.displayName}");
                }

                // Save devices here for use in Scene 2
                DeviceManager.Instance.SaveDevices(devices);
            }
        }
    }
}
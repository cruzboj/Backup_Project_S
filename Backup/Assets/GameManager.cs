//using UnityEngine;
//using UnityEngine.InputSystem;
//using System.Collections.Generic;
//using UnityEngine.InputSystem.Utilities;

//public class GameManager : MonoBehaviour
//{
//    private PlayerInputManager playerInputManager;

//    private void Start()
//    {
//        playerInputManager = FindObjectOfType<PlayerInputManager>();

//        if (playerInputManager != null)
//        {
//            // Retrieve the saved devices from DeviceManager
//            List<InputDevice> devices = DeviceManager.Instance.GetDevices();

//            // Loop through all PlayerInput components managed by PlayerInputManager
//            var playerInputs = playerInputManager.GetComponentsInChildren<PlayerInput>();

//            foreach (var playerInput in playerInputs)
//            {
//                playerInput.SwitchCurrentActionMap("Gameplay"); // Switch to the correct action map

//                // Clone the existing devices (if any) into a list
//                List<InputDevice> allDevices = new List<InputDevice>(playerInput.devices);

//                // Add the previously saved devices from DeviceManager to the list
//                allDevices.AddRange(devices);

//                // Clear the current devices
//                //playerInput.devices.Clear();

//                // Assign the new list of devices to PlayerInput
//                playerInput.devices = new ReadOnlyArray<InputDevice>(allDevices.ToArray());
//            }
//        }
//        else
//        {
//            Debug.LogError("PlayerInputManager not found in the scene.");
//        }
//    }
//}

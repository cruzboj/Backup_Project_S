using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DeviceManager : MonoBehaviour
{
    public static DeviceManager Instance;

    // List to store connected devices
    private List<InputDevice> connectedDevices = new List<InputDevice>();

    private void Awake()
    {
        // Singleton pattern to make sure only one instance exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keep this object across scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Method to save devices
    public void SaveDevices(IEnumerable<InputDevice> devices)
    {
        connectedDevices.Clear();
        connectedDevices.AddRange(devices);
    }

    // Method to get saved devices
    public List<InputDevice> GetDevices()
    {
        return connectedDevices;
    }
}

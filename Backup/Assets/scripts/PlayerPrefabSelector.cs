using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class PlayerPrefabSelector : MonoBehaviour
{
    [SerializeField]
    private PlayerInputManager playerInputManager;

    [SerializeField]
    private List<GameObject> availablePrefabs = new List<GameObject>();

    [SerializeField]
    private int currentPrefabIndex = 0;


    private void Start()
    {
        if (playerInputManager == null)
        {
            playerInputManager = GetComponent<PlayerInputManager>();
        }

        // Set initial prefab
        if (availablePrefabs.Count > 0)
        {
            playerInputManager.playerPrefab = availablePrefabs[currentPrefabIndex];
        }
    }

    //private void Awake()
    //{
    //    DontDestroyOnLoad(gameObject);
    //}

    public void SelectNextPrefab()
    {
        if (availablePrefabs.Count == 0) return;

        currentPrefabIndex = (currentPrefabIndex + 1) % availablePrefabs.Count;
        playerInputManager.playerPrefab = availablePrefabs[currentPrefabIndex];
    }

    public void SelectPreviousPrefab()
    {
        if (availablePrefabs.Count == 0) return;

        currentPrefabIndex--;
        if (currentPrefabIndex < 0) currentPrefabIndex = availablePrefabs.Count - 1;
        playerInputManager.playerPrefab = availablePrefabs[currentPrefabIndex];
    }

    public void SelectPrefabByIndex(int index)
    {
        if (index >= 0 && index < availablePrefabs.Count)
        {
            currentPrefabIndex = index;
            playerInputManager.playerPrefab = availablePrefabs[currentPrefabIndex];
        }
    }

    // Optional: Get current prefab info
    public GameObject GetCurrentPrefab()
    {
        return availablePrefabs[currentPrefabIndex];
    }

    public int GetCurrentPrefabIndex()
    {
        return currentPrefabIndex;
    }

    public int GetPrefabCount()
    {
        return availablePrefabs.Count;
    }
}
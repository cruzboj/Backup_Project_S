using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using UnityEngine.InputSystem.UI;
using UnityEngine.UIElements;

using UnityEngine.SceneManagement;

public class PlayerPrefabSelector : MonoBehaviour
{
    //[SerializeField]private PlayerInputManager playerInputManager;

    [SerializeField]private List<GameObject> availablePrefabs = new List<GameObject>();

    [SerializeField]private int currentPrefabIndex = 0;

    private NewVirtualMouse PlayerChooseIndex;


    //new 
    //private GameObject Player_Manager;
    public GameObject currentPrefab; 
    public GameObject getCurrentPrefab() { return currentPrefab; }

    private void Start()
    {
        // Ensure playerInputManager is assigned from any loaded scene
        //foreach (var scene in SceneManager.GetAllScenes())
        //{
        //    if (scene.isLoaded)
        //    {
        //        GameObject[] rootObjects = scene.GetRootGameObjects();
        //        foreach (GameObject obj in rootObjects)
        //        {
        //            PlayerInputManager manager = obj.GetComponent<PlayerInputManager>();
        //            if (manager != null)
        //            {
        //                Player_Manager = manager.gameObject; // FIXED: Assign the GameObject, not the component
        //                playerInputManager = manager; // Assigning the component for use
        //                break;
        //            }
        //        }
        //    }
        //}

        PlayerChooseIndex = GetComponent<NewVirtualMouse>();

        //if (playerInputManager == null)
        //{
        //    playerInputManager = GetComponent<PlayerInputManager>();
        //}

        // Set initial prefab
        //if (availablePrefabs.Count > 0)
        //{
        //    playerInputManager.playerPrefab = availablePrefabs[currentPrefabIndex];
        //}
    }

    private void Update()
    {
        if (PlayerChooseIndex != null)
        {
            int chosenIndex = PlayerChooseIndex.getbuttonIndex();
            //Debug.Log($"Chosen index: {chosenIndex}");
            currentPrefabIndex = chosenIndex;

            //playerInputManager.playerPrefab = availablePrefabs[chosenIndex];

            //new
            currentPrefab = availablePrefabs[currentPrefabIndex];
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
        //playerInputManager.playerPrefab = availablePrefabs[currentPrefabIndex];
    }

    public void SelectPreviousPrefab()
    {
        if (availablePrefabs.Count == 0) return;

        currentPrefabIndex--;
        if (currentPrefabIndex < 0) currentPrefabIndex = availablePrefabs.Count - 1;
        //playerInputManager.playerPrefab = availablePrefabs[currentPrefabIndex];
    }

    public void SelectPrefabByIndex(int index)
    {
        if (index >= 0 && index < availablePrefabs.Count)
        {
            currentPrefabIndex = index;
            //playerInputManager.playerPrefab = availablePrefabs[currentPrefabIndex];
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


    //public GameObject SpawnSelectedPrefab()
    //{
    //    if (currentPrefab != null)
    //    {
    //        Debug.Log("Spawning prefab at index: " + currentPrefab);
    //        return currentPrefab;
            
    //    }
    //    else
    //    {
    //        Debug.LogError("Selected prefab is null.");
    //    }
    //}
}
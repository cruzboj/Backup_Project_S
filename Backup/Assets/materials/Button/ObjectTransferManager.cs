using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class ObjectTransferManager : MonoBehaviour
{
    private static ObjectTransferManager instance;
    private List<GameObject> spawnedObjects = new List<GameObject>();
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static ObjectTransferManager Instance
    {
        get { return instance; }
    }

    public void AddSpawnedObject(GameObject obj)
    {
        spawnedObjects.Add(obj);
        DontDestroyOnLoad(obj);
    }

    public void TransferObjectsToScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        foreach (GameObject obj in spawnedObjects)
        {
            if (obj != null)
            {
                SceneManager.MoveGameObjectToScene(obj, scene);
            }
        }
        
        spawnedObjects.Clear();
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
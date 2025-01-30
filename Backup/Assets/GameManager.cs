using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using UnityEngine.InputSystem.Utilities;

public class GameManager : MonoBehaviour
{
    [SerializeField] public LayerMask layerMask;
    private PlayerInputManager playerInputManager;
    private HashSet<GameObject> processedObjects = new HashSet<GameObject>(); // לעקוב אחר אובייקטים שכבר טופלו

    void Start()
    {
        playerInputManager = FindObjectOfType<PlayerInputManager>();
        if (playerInputManager == null)
        {
            GameObject inputManagerObject = new GameObject("PlayerInputManager");
            playerInputManager = inputManagerObject.AddComponent<PlayerInputManager>();
        }

        // מכבה את כל ה-PlayerInput components בהתחלה
        DisableAllPlayerInputs();
        AssignPlayersToInputManager();
    }

    void Update()
    {
        AssignPlayersToInputManager();
    }

    private void DisableAllPlayerInputs()
    {
        PlayerInput[] allPlayerInputs = FindObjectsOfType<PlayerInput>();
        foreach (PlayerInput playerInput in allPlayerInputs)
        {
            playerInput.enabled = false;
        }
    }

    private void AssignPlayersToInputManager()
    {
        if (playerInputManager == null) return;

        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            // בודק אם האובייקט נמצא בשכבה המתאימה וטרם טופל
            if ((layerMask.value & (1 << obj.layer)) != 0 && !processedObjects.Contains(obj))
            {
                HandlePlayerObject(obj);
            }
        }
    }

    private void HandlePlayerObject(GameObject obj)
    {
        // שומר מיקום מקורי
        Vector3 originalPosition = obj.transform.position;
        Quaternion originalRotation = obj.transform.rotation;
        Vector3 originalScale = obj.transform.localScale;

        // מעביר להיות ילד של ה-PlayerInputManager
        obj.transform.SetParent(playerInputManager.transform);

        // משחזר מיקום
        obj.transform.position = originalPosition;
        obj.transform.rotation = originalRotation;
        obj.transform.localScale = originalScale;

        // מטפל ב-PlayerInput component
        PlayerInput playerInput = obj.GetComponent<PlayerInput>();
        if (playerInput == null)
        {
            playerInput = obj.AddComponent<PlayerInput>();
        }

        // מפעיל את ה-PlayerInput פעם אחת
        if (!processedObjects.Contains(obj))
        {
            playerInput.enabled = true;
            processedObjects.Add(obj);
            Debug.Log($"Activated PlayerInput for {obj.name}");
        }
    }

    public void AddPlayerToInputManager(GameObject player)
    {
        if (playerInputManager != null && (layerMask.value & (1 << player.layer)) != 0)
        {
            HandlePlayerObject(player);
        }
    }

    // פונקציה לריסט של האקטיבציה (אם תצטרך)
    public void ResetPlayerActivation()
    {
        processedObjects.Clear();
        DisableAllPlayerInputs();
        AssignPlayersToInputManager();
    }
}
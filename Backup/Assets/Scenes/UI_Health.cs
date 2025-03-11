using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro; // Import TMP namespace

public class UI_Health : MonoBehaviour
{
    public GameObject playerUIPrefab; // Assign in Inspector
    public Transform uiContainer; // Parent for UI elements
    //private DynamicLayout boardManager;

    private Color32[] playerColors = new Color32[] {
        new Color32(255, 90, 76, 255),     // Red
        new Color32(52, 150, 255, 255),     // blue
        new Color32(169, 255, 76, 255),     // green
        new Color32(255, 255, 76, 255),   // yellow
        new Color32(255, 150, 63, 255),   // orange
        new Color32(255, 63, 176, 255)    // purple
    };

    private string UIContainer = "UI Container";
    void Start()
    {

        if (uiContainer == null)
        {
            uiContainer = new GameObject(UIContainer).transform;
        }
    }

    public void OnPlayerJoined(PlayerInput playerInput)
    {
        // Instantiate the UI prefab
        GameObject playerUI = Instantiate(playerUIPrefab, uiContainer);

        // קביעת צבע לפי ה-playerIndex
        int playerIndex = playerInput.playerIndex;
        if (playerIndex < playerColors.Length)
        {
            Color32 playerColor = playerColors[playerIndex];

            // מציאת רכיב ה-Image ושינוי צבעו
            Image uiImage = playerUI.GetComponentInChildren<Image>();
            if (uiImage != null)
            {
                uiImage.color = playerColor;
            }
            else
            {
                Debug.LogWarning($"No Image component found on UI for Player {playerIndex}!");
            }
        }
        else
        {
            Debug.LogWarning($"Player index {playerIndex} is out of color range!");
        }

        // Find the TextMeshPro component and rename it
        TextMeshProUGUI healthText = playerUI.GetComponentInChildren<TextMeshProUGUI>();
        if (healthText != null)
        {
            healthText.name = $"healthText{playerIndex}"; // Rename TMP object
            healthText.text = $"0";    // Update text (optional)
        }
        else
        {
            Debug.LogWarning($"No TextMeshPro component found in player UI {playerIndex}!");
        }

        Debug.Log($"Player {playerIndex} joined, UI Created with color {playerColors[playerIndex]}.");
    }
}
using UnityEngine;
using UnityEngine.InputSystem;

public class UI_Char_Select : MonoBehaviour
{
    //https://www.youtube.com/watch?v=EMrE1a82BiQ

    public GameObject playerUIPrefab; // Assign in Inspector
    public Transform board; // Parent for UI elements
    private DynamicLayout boardManager;

    private string Board = "Board";
    void Start()
    {
        // חיפוש הסקריפט שמכיל את הפונקציה GenerateBoard()
        boardManager = FindObjectOfType<DynamicLayout>();

        if (board == null)
        {
            board = new GameObject(Board).transform;
        }
    }

    public void OnPlayerJoined(PlayerInput playerInput)
    {
        // Instantiate the UI prefab
        GameObject playerUI = Instantiate(playerUIPrefab, board);

        // קריאה לפונקציה GenerateBoard() כאשר שחקן מצטרף
        if (boardManager != null)
        {
            boardManager.GenerateBoard();
        }
        else
        {
            Debug.LogError("BoardManager not found in the scene!");
        }

        Debug.Log($"Player {playerInput.playerIndex} joined, UI Created.");
    }
}
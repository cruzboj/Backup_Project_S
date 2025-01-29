using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelectionMenu : MonoBehaviour
{
    [SerializeField]
    private PlayerPrefabSelector prefabSelector;

    private void Start()
    {
        prefabSelector = FindObjectOfType<PlayerPrefabSelector>();

        //int selectedIndex = PlayerPrefs.GetInt("SelectedCharacterIndex", -1); // if not found index is -1
        //if (selectedIndex != -1)
        //{
        //    Debug.Log($"Selected character index: {selectedIndex}");
        //    // load character from index
        //}
        //else
        //{
        //    Debug.LogError("No character index selected!");
        //}
    }


    public void SelectPrefab(int index)
    {
        if (prefabSelector != null)
        {
            prefabSelector.SelectPrefabByIndex(index);
        }
        else
        {
            Debug.LogError("PlayerPrefabSelector is not assigned or found in the scene!");
        }

        //PlayerPrefs.SetInt("SelectedCharacterIndex", index);
        //PlayerPrefs.Save();
        //SceneManager.LoadScene("Game"); // טוען את סצנת המשחק
    }
}

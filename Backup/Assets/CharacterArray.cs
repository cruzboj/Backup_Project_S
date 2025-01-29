using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterArray : MonoBehaviour
{
    //MAX_Player = 10;
    //public int[] Character_Arr = new int[10];
    
    //tester
    //public int[] Character_Arr = {1,2,3,4};
    public GameObject[] SpawnPoints;

    public static CharacterArray instance = null;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        SpawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");
    }

    private void Start()
    {
        PlayerInputManager.instance.JoinPlayer(0, -1, null);
    }

    void OnPlayerJoined(PlayerInput playerInput)
    {
        Debug.Log("player join the game");
    }
    void OnPlayerleft(PlayerInput playerInput)
    {

    }

}

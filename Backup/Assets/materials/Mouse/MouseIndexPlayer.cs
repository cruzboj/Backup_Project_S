using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class MouseIndexPlayer : MonoBehaviour
{
    public int number_index;


    void Start()
    {
        PlayerInput playerInput = GetComponentInParent<PlayerInput>();
        number_index = playerInput.playerIndex + 1;



        TextMeshProUGUI textMeshPro = GetComponent<TextMeshProUGUI>();

        if (textMeshPro != null)
        {
            textMeshPro.text = "P" + number_index.ToString();
        }
        else
        {
            Debug.LogError("TextMeshProUGUI component not found!");
        }
    }

}
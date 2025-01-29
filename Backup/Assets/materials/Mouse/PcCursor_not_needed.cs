using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    [SerializeField] private Texture2D cursorTexture;
    [SerializeField] private Texture2D cursorTextureClick;
    private Vector2 cursorHotspot;
    

    void Start()
    {
        cursorHotspot = new Vector2(cursorTexture.width/2, cursorTexture.height/2);
        Cursor.SetCursor(cursorTexture, cursorHotspot, CursorMode.Auto);
    }

    
    void Update()
    {
        // user press = true
        if (Input.GetMouseButtonDown(0))
        {
            Cursor.SetCursor(cursorTextureClick, cursorHotspot, CursorMode.Auto);
        }
        // user press = false
        else if (Input.GetMouseButtonUp(0))
        {
            Cursor.SetCursor(cursorTexture, cursorHotspot, CursorMode.Auto);
        }
    }
}

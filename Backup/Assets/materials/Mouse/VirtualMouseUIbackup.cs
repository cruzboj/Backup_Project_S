using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.UI;

public class VirtualMouseUIBackUp : MonoBehaviour
{
    //cursor tutorial https://www.youtube.com/watch?v=j2XyzSAD4VU
    
    [SerializeField] private RectTransform canvasRectTransform;

    private VirtualMouseInput virtualMouseInput;

    private void Awake()
    {
        virtualMouseInput = GetComponent<VirtualMouseInput>();
    }

    private void Update()
    {
        transform.localScale = Vector3.one * (1f / canvasRectTransform.localScale.x);
        //transform.SetAsFirstSibling(); //allwats place it on top of the Hierarchy 
    }
    private void LateUpdate()
    {
        //cursor rander area if(its out of sceen dont go further)
        Vector2 virtualmousePosition = virtualMouseInput.virtualMouse.position.value;
        virtualmousePosition.x = Mathf.Clamp(virtualmousePosition.x,0f,Screen.width);
        virtualmousePosition.y = Mathf.Clamp(virtualmousePosition.y, 0f, Screen.height);
        InputState.Change(virtualMouseInput.virtualMouse, virtualmousePosition);
    }

}

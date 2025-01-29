using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ButtonScript : MonoBehaviour
{
    public int buttonIndex;
    //public int indexRecived;
    private Image buttonImage;
    //private NewVirtualMouse mouseStatus;
    
    public int getButtonIndex() { return buttonIndex; }
    
    void Start()
    {
        buttonImage = transform.GetChild(0).GetComponent<Image>();

    }

    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //mouseStatus = GameObject.FindObjectOfType<NewVirtualMouse>();
            //Debug.Log("Mouse inside me nnya!");
            buttonImage.color = new Color(0f, 0f, 0f); // #000000
            //if (NewVirtualMouse.getClicked())
            //{
                
            //    indexRecived = mouseStatus.getindex();
            //}
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Mouse out");
            buttonImage.color = new Color(0f, 255f, 0f); // #00FF00
        }
        //mouseStatus = null;
    }
}

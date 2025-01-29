using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class IndexPlayer : MonoBehaviour
{
    public int number_index;


    void Start()
    {
        number_index = indexManagerHeader.getindex();
        GetComponent<TextMeshPro>().text = number_index.ToString();

    }
    // Update is called once per frame
    void Update()
    {
        transform.rotation = new Quaternion(0, 1, 0, 0);
    }

}
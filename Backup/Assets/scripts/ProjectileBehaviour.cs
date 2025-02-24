using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehaviour : MonoBehaviour
{
    public float speed = 15.5f;
    public float TimeObjAlive = 1f;

    private void Start()
    {
        Destroy(gameObject, TimeObjAlive); //destroy object after 4sec
    }

    private void Update()
    {
        transform.position += transform.forward * Time.deltaTime * speed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }
}

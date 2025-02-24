using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class leadge_script : MonoBehaviour
{
    //note: maybe i shoud do 1 leadge then mirror dup it to the other side so it will have a diffrent ob
    [Header("Leadge Hitbox")]
    public Vector3 boxSize = new Vector3(1f, 1f, 1f);
    public float location_ray_height_1;
    public float location_ray_position_1;


    private void OnDrawGizmos()
    {
        //left leadge
        Gizmos.color = Color.blue;
        Vector3 positionInFront = transform.position + transform.forward * location_ray_position_1; // "2f" הוא המרחק מהדמות
        positionInFront.x += location_ray_height_1; // הזז את הגיזמו למעלה ב-0.5 יחידות
        Gizmos.DrawCube(positionInFront, boxSize);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    [Header("Attack Cycle")]
    private PlayerControler playerControler;
    public int attackNumber;

    public float attackCooldown = 0.5f; // Time between attacks
    private float nextAttackTime = 0f; // Time when the next attack is allowed

    public LayerMask layermask;
    public Vector3 v3Knockback = new Vector3(50, 5, 0);

    //[SerializeField] 
    private float damage;
    [Header("refrence for health controller")]
    [SerializeField] private HealthControler healthControler;

    private CharacterController controller;
    private void Awake()
    {
        playerControler = GetComponent<PlayerControler>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (layermask == (layermask | (1 << other.transform.gameObject.layer)))
        {
            //Apply knockback
            controller = gameObject.GetComponent<CharacterController>();
            if (controller != null)
            {
                //controller.addForce(v3Knockback, ForceMode.Impulse);
                
            }
        }
    }
}
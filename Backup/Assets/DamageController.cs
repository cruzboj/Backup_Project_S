using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageControl : MonoBehaviour
{
    public LayerMask layermask;
    //ComboCharacter _comboCharacter;
    public float damage = 10f;
    public float getDamage() { return  damage; }

    //public PlayerControler playerMovment;
    [SerializeField] private HealthControler healthControler;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.IsChildOf(transform))
        {
            Debug.Log("damage script hit self");
            return;
        }
        else if (layermask == (layermask | (1 << other.transform.gameObject.layer)))
        {
            
            ////https://www.youtube.com/watch?v=0u2R9MDi-_w
            //send knockback to statemachine where is movment controls placed
            PlayerControler player = other.GetComponent<PlayerControler>();
            if (player != null)
            {
                Debug.Log("Damage controll" + player);
                player.KBCounter = player.KBTotalTime;

                if (other.transform.position.x <= transform.position.x)
                {
                    player.KnockFromRight = true;
                }
                if (other.transform.position.x > transform.position.x)
                {
                    player.KnockFromRight = false;
                }

                //send dmg to health ui
                healthControler.takeDamage(damage);
            }
        }
    }
    
}

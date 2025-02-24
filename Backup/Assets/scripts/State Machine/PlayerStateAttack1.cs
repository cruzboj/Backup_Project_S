using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateAttack1 : PlayerBaseState
{
    public PlayerStateAttack1(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base(currentContext, playerStateFactory) { }

    //[Header("Bullet obj")]
    //public GameObject bullet;

    //[Header("Bullet Force")]
    //public float ShootForce;
    //public float upwardForce;

    //[Header("Gun stats")]
    //public float timeBetweenShooting;
    //public float Spred;
    //public float timeBetweenShots;
    //public bool allowbuttonHold;

    ////bools
    //bool shoting, readyToShoot;

    public override void EnterState() {
        Debug.Log("Attack State");
        HandleAttack();
    }
    public override void UpdateState() {
        if (!_ctx.IsAttackPressed && _ctx.Grounded)
        {
            SwitchState(_factory.Grounded());
        }
    }
    public override void ExitState() { }
    public override void CheckSwitchStates() { }
    public override void InitalizeSubState() { }

    private void OnDrawGizmos()
    {
        // Ensure _ctx has a reference to the player's transform
        if (_ctx.PlayerTransform == null) return;
        //attack1_hitbox
        Gizmos.color = new Color(1f, 0f, 0f, 0.5f);
        Vector3 positionInFront = _ctx.transform.position + _ctx.transform.forward * _ctx.Location_Ray_Position_1; // "2f" הוא המרחק מהדמות
        positionInFront.y += _ctx.Location_Ray_height_1; // הזז את הגיזמו למעלה ב-0.5 יחידות
        Gizmos.DrawCube(positionInFront, _ctx.BoxSize_hitbox_1);
    }

    void HandleAttack()
    {

        _ctx.ANIMATOR.Play(_ctx.PLAYER_ATTACK);
        Object.Instantiate(_ctx.projectilePrefab, _ctx.LauchOffSet.position, _ctx.transform.rotation);

        Collider[] hitPlayers = Physics.OverlapBox(_ctx.transform.position, _ctx.BoxSize_hitbox_1, _ctx.transform.rotation, _ctx.Global_KB_PlayerMask);
        if(!_ctx._hasProjectil)
            if (hitPlayers.Length > 0)
            {

                //Debug.Log("attack 1 active");
                _ctx.HitBox1 = true;

                foreach (Collider player in hitPlayers)
                {
                    PlayerControler playerScript = player.GetComponent<PlayerControler>();
                    HealthControler playerHealthScript = player.GetComponent<HealthControler>();

                    if (playerScript != null && playerScript.getPlayerIndex() != this._ctx.getPlayerIndex())
                    {
                        Debug.Log("Damage control: " + player.name);

                        // קביעת זמן Knockback
                        playerScript.KBCounter = playerScript.KBTotalTime;

                        // בדיקת כיוון הפגיעה
                        playerScript.KnockFromRight = (player.transform.position.x <= _ctx.transform.position.x); //if hit from the left go left and if right go right

                        // שליחת נזק ל-HealthController
                        playerHealthScript.takeDamage(_ctx.damage1);
                    }
                }
            }
            else
            {
                Debug.Log("NOT HIT");
                _ctx.HitBox1 = false;
            }
        else
        {
            //projectil logic
            Object.Instantiate(_ctx.projectilePrefab, _ctx.LauchOffSet.position, Quaternion.Euler(90, 0, 0));
        }

        //tartCoroutine(SpawnDelay(TimeAttack1));
    }

}
//Quaternion.Euler(0, _ctx.transform.eulerAngles.y, 0) make it go flat
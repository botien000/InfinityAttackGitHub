using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeapon : MonoBehaviour
{
    public int attackDamage;

    public Vector3 attackOffset;
    public float attackRangeDamage;
    public LayerMask attackMask;
    Enemy enemy;
    public Vector3 attackRangeRight;
    public Vector3 attackRangeLeft;
    public float fireballSpeed;

    public static EnemyWeapon instance;
    private void Start()
    {
        instance = this;
        enemy = GetComponent<Enemy>();
    }

    public void MeleeAttack()
    {
        Vector3 pos = transform.position;
        pos += transform.right * attackOffset.x;
        pos += transform.up * attackOffset.y;

        //Physics2D.OverlapCircle se tao ra 1 vong tron, pos la vi tri tam^ hinh tron
        //attackRangeDamage la ban kinh, hay la khoang cach gay damage
        //attackMask la muc tieu gay damage neu trong vong tron
        Collider2D colInfo = Physics2D.OverlapCircle(pos, attackRangeDamage, attackMask);
        if (colInfo != null)
        {
            //colInfo.GetComponent<PlayerHealth>().TakeDamage(attackDamage);
        }
    }

    public void RangeAttack()
    {
    
        GameObject ball = (GameObject)Instantiate(Resources.Load("Prefabs/Enemy/Fire Ball"));
        if (!enemy.isFlipped)
        {
            Vector3 pos = transform.position;
            pos += transform.right * attackRangeRight.x;
            pos += transform.up * attackRangeRight.y;
            ball.transform.localPosition = pos;
            ball.GetComponent<FireBall>().setAttackDamage(attackDamage);
            ball.GetComponent<FireBall>().setVelocity(new Vector2(fireballSpeed, 0));          
        }
        else
        {
            Vector3 pos = transform.position;
            pos += -transform.right * attackRangeLeft.x;
            pos += transform.up * attackRangeLeft.y;
            ball.transform.localPosition = pos;
            ball.transform.Rotate(0,0,180);
            ball.GetComponent<FireBall>().setAttackDamage(attackDamage);
            ball.GetComponent<FireBall>().setVelocity(new Vector2(-fireballSpeed, 0));
        }
    }
}

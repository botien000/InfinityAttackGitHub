using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackRange_Boss2 : MonoBehaviour
{
    public BoxCollider2D box;

    // Start is called before the first frame update
    void Start()
    {
        box = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && !Boss2.instance.Skill1_ing && Boss2.instance.canBeAttacked && !Boss2.instance.dead)
        {
            if(Boss2.instance.canAttack == true)
            {
                Boss2.instance.attacking = true;
                Boss2.instance.canAttack = false;
                Boss2.instance.canRun = false;
                Boss2.instance.anim.SetTrigger("Attack");
            }
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && !Boss2.instance.Skill1_ing && Boss2.instance.canBeAttacked && !Boss2.instance.dead)
        {
            if (Boss2.instance.canAttack == true)
            {
                Boss2.instance.attacking = true;
                Boss2.instance.canAttack = false;
                Boss2.instance.canRun = false;
                Boss2.instance.anim.SetTrigger("Attack");
            }
        }
    }

}

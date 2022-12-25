using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackRange_Boss3 : MonoBehaviour
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
        if (collision.gameObject.tag == "Player" && !Boss3.instance.Skill1_ing && Boss3.instance.canBeAttacked && !Boss3.instance.dead)
        {
            if(Boss3.instance.canAttack == true)
            {
                Boss3.instance.attacking = true;
                Boss3.instance.canAttack = false;
                Boss3.instance.canRun = false;
                Boss3.instance.anim.SetTrigger("Attack");
            }
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && !Boss3.instance.Skill1_ing && Boss3.instance.canBeAttacked && !Boss3.instance.dead)
        {
            if (Boss3.instance.canAttack == true)
            {
                Boss3.instance.attacking = true;
                Boss3.instance.canAttack = false;
                Boss3.instance.canRun = false;
                Boss3.instance.anim.SetTrigger("Attack");
            }
        }
    }

}

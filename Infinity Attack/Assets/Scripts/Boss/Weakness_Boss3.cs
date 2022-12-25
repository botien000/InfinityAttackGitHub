using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Weakness_Boss3 : MonoBehaviour
{
    BoxCollider2D box;
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
        if(collision.gameObject.tag == "PlayerAttack")
        {
            Debug.Log("Attack");
            Boss3.instance.TakeDameHeart(InGameCharLoading.instance.damage);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "PlayerAttack")
        {
            Debug.Log("Attack");
            Boss3.instance.TakeDameHeart(InGameCharLoading.instance.damage);
        }
    }
}

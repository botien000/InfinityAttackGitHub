using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerAttack : MonoBehaviour
{
    private BoxCollider2D box;

    // Start is called before the first frame update
    void Start()
    {
        box = GetComponentInChildren<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            if (box != null)
            {
                EnemyHealth.instance.TakeDamage(InGameCharLoading.instance.damage);
            }
        }
    }
}

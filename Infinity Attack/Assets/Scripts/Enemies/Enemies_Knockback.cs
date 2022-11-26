using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemies_Knockback : MonoBehaviour
{
    [SerializeField] private float thrust;
    [SerializeField] private float knockbackTime;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D player)
    {
        if (player.gameObject.CompareTag("Player"))
        {
            Debug.Log("Knockback");
            Rigidbody2D playerRigid = player.GetComponent<Rigidbody2D>();
            if(playerRigid != null)
            {
                /*if(playerRigid.position.x < transform.position.x)
                {
                    Debug.Log("Knockback left");
                    playerRigid.AddForce(Vector2.left * thrust, ForceMode2D.Force);
                } else
                {
                    Debug.Log("Knockback right");
                    playerRigid.AddForce(Vector2.right * thrust, ForceMode2D.Force);
                }*/
                Vector2 difference = playerRigid.transform.position - transform.position;
                Debug.Log("diff: " + difference.normalized * thrust);
                playerRigid.AddForce(difference.normalized * thrust, ForceMode2D.Impulse);
                StartCoroutine(KnockCo(playerRigid));
                Debug.Log("Knockback");
            }
        }   
    }

    private IEnumerator KnockCo(Rigidbody2D player)
    {
        if(player != null)
        {
            yield return new WaitForSeconds(knockbackTime);
            player.velocity = Vector2.zero;
        } 
    }
}

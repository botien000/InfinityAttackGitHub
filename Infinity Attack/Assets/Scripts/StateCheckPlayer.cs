using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateCheckPlayer : MonoBehaviour
{
    [SerializeField] private Transform transTile_1, transTile_2;
    [SerializeField] private Transform transTileMove_1, transTileMove_2;
    [SerializeField] private float speedRotate, speedMove;
    [SerializeField] private Dark_Samurai dark_Samurai;

    private bool firstCollide = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (firstCollide)
            return;
        if (collision.CompareTag("Player"))
        {
            firstCollide = true;
            StartCoroutine(IEMoveTiles());
            if(dark_Samurai != null)
            dark_Samurai.Active(collision.transform);
        }
    }

    private IEnumerator IEMoveTiles()
    {
        while (true)
        {
            if (transTile_1.eulerAngles.z >= 90f && transTile_2.eulerAngles.z >= 90f) 
                break;
            if (transTile_1.position == transTileMove_1.position)
            {
                transTile_1.eulerAngles += Vector3.MoveTowards(Vector3.forward, Vector3.forward * 90, speedRotate * Time.deltaTime);
            }
            if (transTile_2.position == transTileMove_2.position)
            {
                transTile_2.eulerAngles += Vector3.MoveTowards(Vector3.forward, Vector3.forward * 90, speedRotate * Time.deltaTime);
            }
            transTile_1.position = Vector2.MoveTowards(transTile_1.position, transTileMove_1.position, speedMove * Time.deltaTime);
            transTile_2.position = Vector2.MoveTowards(transTile_2.position, transTileMove_2.position, speedMove * Time.deltaTime);
            yield return null;
        }

    }


}

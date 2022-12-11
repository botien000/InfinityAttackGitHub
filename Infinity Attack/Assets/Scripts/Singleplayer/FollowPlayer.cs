using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] private Transform TransfSpawningPlayer;
    [SerializeField] private float speedRotate;

    void Update()
    {
        transform.Rotate(Vector3.forward * speedRotate * Time.deltaTime);
    }

    void Start()
    {
        CharacterObject player = FindObjectOfType<CharacterObject>();
        if(player != null)
        {
            player.transform.position = TransfSpawningPlayer.transform.position;
        }
    }
}

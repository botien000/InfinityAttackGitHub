using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_Knockback : MonoBehaviour
{
    private BoxCollider2D box;

    // Start is called before the first frame update
    void Start()
    {
        box = GetComponentInChildren<BoxCollider2D>();
    }
}

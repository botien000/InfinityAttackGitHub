using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minimap : MonoBehaviour
{

    private Transform player;
    private bool haveTransform = false;

    private void LateUpdate()
    {
        if (haveTransform && player != null)
        {
            Vector3 newPos = player.position;
            newPos.z = transform.position.z;
            transform.position = newPos;
        }
        else
        {
            Debug.Log("Chua co transform");
        }

    }

    public void SetTransform (Transform transform)
    {
        this.player = transform;
        haveTransform = true;
    }
}

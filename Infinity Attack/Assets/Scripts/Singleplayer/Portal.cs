using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    [SerializeField] private float speedRotate;

    void Update()
    {
        transform.Rotate(Vector3.forward * speedRotate * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            SceneManager.LoadScene($"Map{GameManager.instance.typeMap}_2");
        }
    }
}

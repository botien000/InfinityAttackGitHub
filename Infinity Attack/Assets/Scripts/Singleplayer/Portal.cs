using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Portal : MonoBehaviour
{
    [SerializeField] private float speedRotate;
    [SerializeField] private Image imgAniSwScene;
    [SerializeField] private float timeToEffect;

    private void Start()
    {
        timeToEffect = 1.5f;
        GameManager.instance.CheckBossMap(false);
    }

    private void Update()
    {
        transform.Rotate(Vector3.forward * speedRotate * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            imgAniSwScene.raycastTarget = true;
            GameManager.instance.SetImageToPlayer(imgAniSwScene);
            GameManager.instance.SetPlayerFreeze(true);
            GameManager.instance.SetPlayerActive(false);
            StartCoroutine(IEClosingMapEff());
        }
    }

    private IEnumerator IEClosingMapEff() // switch scene effect
    {
        float curTimeToEffect = 0;
        while (curTimeToEffect <= timeToEffect)
        {
            curTimeToEffect += Time.deltaTime;
            Color colorAniSwScene = Color.black;
            colorAniSwScene.a = curTimeToEffect / timeToEffect;
            imgAniSwScene.color = colorAniSwScene;
            yield return null;
        }
        imgAniSwScene.color = Color.black;
        LoadScene();
    }

    public void LoadScene()
    {
        SceneManager.LoadScene($"Map{GameManager.instance.typeMap}_2");
        SoundManager.instance.SetMapBossMusic();
    }
}

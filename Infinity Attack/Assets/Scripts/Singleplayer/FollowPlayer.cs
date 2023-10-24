using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] private Transform TransfSpawningPlayer;
    [SerializeField] private float speedRotate,timeToEffect;
    private Image imgAniSwScene;

    void Update()
    {
        transform.Rotate(Vector3.forward * speedRotate * Time.deltaTime);
    }

    void Start()
    {
        timeToEffect = 1.5f;
        CharacterObject player  = GameManager.instance.GetPlayer();
        if(player != null)
        {
            player.gameObject.SetActive(true);
            player.SetFreeze(true);
            imgAniSwScene = player.GetImage();
            player.transform.position = TransfSpawningPlayer.transform.position;
            StartCoroutine(IEOpeningMapEff());
        }
    }

    private IEnumerator IEOpeningMapEff() // switch scene effect
    {
        float curTimeToEffect = timeToEffect;
        while (curTimeToEffect >= 0)
        {
            curTimeToEffect -= Time.deltaTime;
            Color colorAniSwScene = Color.black;
            colorAniSwScene.a = curTimeToEffect / timeToEffect;
            imgAniSwScene.color = colorAniSwScene;
            yield return null;
        }
        imgAniSwScene.raycastTarget = false;
        GameManager.instance.CheckBossMap(true);
        GameManager.instance.SentInfoToConver();
    }
}

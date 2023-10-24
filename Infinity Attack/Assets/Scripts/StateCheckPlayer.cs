using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateCheckPlayer : MonoBehaviour
{
    [SerializeField] private Transform transTile_1, transTile_2;
    [SerializeField] private Transform transTileMove_1, transTileMove_2;
    [SerializeField] private float speedRotate, speedMove;

    private bool firstCollide = false;
    private Vector3 originTile_1Pos = Vector3.zero;
    private Vector3 originTile_2Pos = Vector3.zero;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (firstCollide)
            return;
        if (collision.CompareTag("Player"))
        {
            firstCollide = true;
            originTile_1Pos = transTile_1.position;
            if (transTile_2 == null)
            {
                StartCoroutine(IEMoveTilesForward(transTile_1, transTileMove_1.position, null, Vector3.zero));
                return;
            }
            originTile_2Pos = transTile_2.position;
            StartCoroutine(IEMoveTilesForward(transTile_1, transTileMove_1.position, transTile_2, transTileMove_2.position));
        }
    }

    private IEnumerator IEMoveTilesForward(Transform fromPos_1, Vector3 toPos_1, Transform fromPos_2, Vector3 toPos_2)
    {
        while (true)
        {
            if (fromPos_2 == null)
            {
                if (fromPos_1.eulerAngles.z >= 90f)
                    break;
            }
            else if (fromPos_1.eulerAngles.z >= 90f && fromPos_2.eulerAngles.z >= 90f)
                break;

            if (fromPos_1.position == toPos_1)
            {
                fromPos_1.eulerAngles += Vector3.MoveTowards(Vector3.forward, Vector3.forward * 90, speedRotate * Time.deltaTime);
            }
            fromPos_1.position = Vector2.MoveTowards(fromPos_1.position, toPos_1, speedMove * Time.deltaTime);

            if (fromPos_2 != null)
            {
                if (fromPos_2.position == toPos_2)
                {
                    fromPos_2.eulerAngles += Vector3.MoveTowards(Vector3.forward, Vector3.forward * 90, speedRotate * Time.deltaTime);
                }
                fromPos_2.position = Vector2.MoveTowards(fromPos_2.position, toPos_2, speedMove * Time.deltaTime);
            }
            yield return null;
        }
    }

    private IEnumerator IEMoveTilesBack(Transform fromPos_1, Vector3 toPos_1, Transform fromPos_2, Vector3 toPos_2)
    {
        while (true) // rotate tiles
        {
            if (fromPos_2 == null)
            {
                if (fromPos_1.rotation.z <= 0F)
                    break;
            }
            else if (fromPos_1.rotation.z <= 0F && fromPos_2.rotation.z <= 0F)
                break;

            fromPos_1.eulerAngles -= Vector3.MoveTowards(Vector3.forward, Vector3.forward * 90, speedRotate * Time.deltaTime);

            if (fromPos_2 != null)
                fromPos_2.eulerAngles -= Vector3.MoveTowards(Vector3.forward, Vector3.forward * 90, speedRotate * Time.deltaTime);

            yield return null;
        }
        while (true) // move tiles back to origial position
        {
            if (fromPos_2 == null)
            {
                if (fromPos_1.position.x == toPos_1.x)
                    break;
            }
            else if (fromPos_1.position.x == toPos_1.x && fromPos_2.position.x == toPos_2.x)
                break;

            fromPos_1.position = Vector2.MoveTowards(fromPos_1.position, toPos_1, speedMove * Time.deltaTime);

            if (fromPos_2 != null)
                fromPos_2.position = Vector2.MoveTowards(fromPos_2.position, toPos_2, speedMove * Time.deltaTime);

            yield return null;
        }
        gameObject.SetActive(false);
    }

    public void MoveToOrigin()
    {
        if (transTile_2 == null)
        {
            StartCoroutine(IEMoveTilesBack(transTile_1, originTile_1Pos, null, Vector3.zero));
        }
        else
            StartCoroutine(IEMoveTilesBack(transTile_1, originTile_1Pos, transTile_2, originTile_2Pos));
    }


}

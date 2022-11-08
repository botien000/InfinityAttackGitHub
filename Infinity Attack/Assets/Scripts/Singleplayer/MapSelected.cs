using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapSelected : MonoBehaviour
{
    [SerializeField] private GameObject loadingPanel;

    private void OnEnable()
    {

    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void BtnPlay()
    {
        StartCoroutine(IEMapLoading());
    }
    IEnumerator IEMapLoading()
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync("Map1_1");
        while (!asyncOperation.isDone)
        {
            loadingPanel.SetActive(true);
            yield return null;
        }
    }
}

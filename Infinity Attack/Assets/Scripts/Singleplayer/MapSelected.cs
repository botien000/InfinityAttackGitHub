using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapSelected : MonoBehaviour
{
    [SerializeField] private GameObject loadingPanel;
    [SerializeField] private string map1Description, map2Description, map3Description;

    int mapSeleted;
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
        StartCoroutine(IEMapLoading(mapSeleted));
    }
    IEnumerator IEMapLoading(int map)
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync($"Map{map}_1");
        while (!asyncOperation.isDone)
        {
            loadingPanel.SetActive(true);
            yield return null;
        }
    }
    public void BtnMapSelected(int type)
    {
        mapSeleted = type;
    }
}

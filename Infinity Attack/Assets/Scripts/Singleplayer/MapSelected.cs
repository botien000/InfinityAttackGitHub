using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Device;
using UnityEngine.SceneManagement;

public class MapSelected : MonoBehaviour
{
    [SerializeField] private GameObject loadingPanel;
    [SerializeField] private string[] mapsDescription = new string[3];
    [SerializeField] private TextMeshProUGUI txtDescription;

    int mapSeleted = 1;
    private void OnEnable()
    {
        txtDescription.text= mapsDescription[mapSeleted - 1];
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
        Debug.Log(map);
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
        txtDescription.text = mapsDescription[mapSeleted - 1];
    }
}

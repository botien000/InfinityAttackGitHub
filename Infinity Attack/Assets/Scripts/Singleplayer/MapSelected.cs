using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Device;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class MapSelected : MonoBehaviour
{
    [SerializeField] private GameObject loadingPanel;
    [SerializeField] private string[] mapsDescription = new string[3];
    [SerializeField] private TextMeshProUGUI txtDescription;
    [SerializeField] private SpellShow spellShow;

    int mapSeleted = 1;
    private void OnEnable()
    {
        txtDescription.text = mapsDescription[mapSeleted - 1];
    }

    public void BtnPlay()
    {
        StartCoroutine(IEMapLoading(mapSeleted));
    }
    public void ExcuteUpdateAmountSpell()
    {
        int count = SpellSingleton.Instance.GetCountSpell();
        for (int i = 0; i < count; i++)
        {
            SpellSingleton.SpellValuable spellValuable = SpellSingleton.Instance.GetSpell(i);
            if (spellValuable.id == null)
                continue;

            if (spellValuable.amount == 1) // remove spell own
            {
                StartCoroutine(IERemoveSpellOwn(i));
            }
            else
            {
                StartCoroutine(IEUpdateAmountSpell(i));
            }
        }
    }

    IEnumerator IEMapLoading(int map)
    {
        ExcuteUpdateAmountSpell();
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync($"Map{map}_1");
        while (!asyncOperation.isDone)
        {
            loadingPanel.SetActive(true);
            yield return null;
        }
    }

    IEnumerator IEUpdateAmountSpell(int index)
    {
        WWWForm form = new WWWForm();
        UnityWebRequest unityWebRequest = null;
        form.AddField("_id", SpellSingleton.Instance.GetSpell(index).id);
        form.AddField("amount", SpellSingleton.Instance.GetSpell(index).amount - 1);
        unityWebRequest = UnityWebRequest.Post(Api.Instance.api + Api.Instance.routerUpdateAmount, form);
        var handler = unityWebRequest.SendWebRequest();
        while (!handler.isDone)
        {
            yield return null;
        }
        if (unityWebRequest.result == UnityWebRequest.Result.Success)
        {
            string json = unityWebRequest.downloadHandler.text;
            Debug.Log(json);
            if (json != "[]")
            {
                Debug.Log("Update Amount Sucessfully");
                SpellOwnUtility spellOwnUtility = JsonConvert.DeserializeObject<SpellOwnUtility>(json);
            }
            else
            {
                Debug.Log("Failed Update");
            }
        }
        else
        {
            Debug.Log("Failed to connecting server");
        }
        unityWebRequest.Dispose();
    }

    IEnumerator IERemoveSpellOwn(int index)
    {
        WWWForm form = new WWWForm();
        UnityWebRequest unityWebRequest = null;
        form.AddField("_id", SpellSingleton.Instance.GetSpell(index).id);
        unityWebRequest = UnityWebRequest.Post(Api.Instance.api + Api.Instance.routerRemoveSpellOwn, form);
        var handler = unityWebRequest.SendWebRequest();
        while (!handler.isDone)
        {
            yield return null;
        }
        if (unityWebRequest.result == UnityWebRequest.Result.Success)
        {
            Debug.Log(unityWebRequest.downloadHandler.text);
        }
        else
        {
            Debug.Log("Failed to connecting server");
        }
        unityWebRequest.Dispose();
    }

    public void BtnMapSelected(int type)
    {
        mapSeleted = type;
        txtDescription.text = mapsDescription[mapSeleted - 1];
    }
}

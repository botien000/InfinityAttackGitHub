using TMPro;
using UnityEngine;

public class LeaderboardDetail : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI txtTop, txtNameUser, txtID, txtGem;

    internal void Init(int top, string name, string id, int gem, bool isPlayer)
    {
        if (isPlayer)
            txtNameUser.color = Color.yellow;
        txtTop.text = top.ToString();
        txtNameUser.text = name;
        txtID.text = "ID: " + id;
        txtGem.text = gem.ToString();
    }
}

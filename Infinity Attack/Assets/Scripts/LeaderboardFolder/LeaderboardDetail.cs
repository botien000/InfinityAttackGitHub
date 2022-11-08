using TMPro;
using UnityEngine;

public class LeaderboardDetail : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI txtTop, txtNameUser, txtGem; 

    internal void Init(int top, string name, int gem,bool isPlayer)
    {
        if (isPlayer)
            txtNameUser.color = Color.yellow;
        txtTop.text = top.ToString();
        txtNameUser.text = name;
        txtGem.text = gem.ToString();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ConversationData", menuName = "ScriptableObjects/CreateConversionData", order = 1)]
public class SctblObjConversation : ScriptableObject
{
    public List<ConversionText> conversions;
    public bool isCharacterFirstTalking;
}

[System.Serializable]
public class ConversionText
{
    public string characterText;
    public string bossText;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackScreen : MonoBehaviour
{
    private void OnEnable()
    {
        SoundManager.instance.SetSoundClick();
    }
}

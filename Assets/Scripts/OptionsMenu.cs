using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    public Toggle soundToggle;
    private bool firstRender;

    private void OnEnable()
    {
        if (soundToggle.isOn == !AudioManager.instance.isMuted)
        {
            firstRender = false;
        }
        else
        {
            firstRender = true;
            soundToggle.isOn = !AudioManager.instance.isMuted;
        }
    }
    public void ToggleMute()
    {
        if (!firstRender)
            AudioManager.instance.ToggleMute();
        else
            firstRender = false;
    }
}

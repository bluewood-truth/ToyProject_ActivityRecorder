using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Toggle_Text_Color : MonoBehaviour
{
    Text text;
    Color color_default;
    Color color_toggled;

    private void Awake()
    {
        var toggle = GetComponent<Toggle>();
        text = GetComponentInChildren<Text>();

        color_default = text.color;
        color_toggled = GetComponent<Image>().color;

        toggle.onValueChanged.AddListener(Toggle_Color);
        toggle.onValueChanged.Invoke(toggle.isOn);
    }   

    void Toggle_Color(bool is_on)
    {
        text.color = is_on ? color_toggled : color_default;
    }
}

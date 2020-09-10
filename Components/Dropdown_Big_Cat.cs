using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dropdown_Big_Cat : MonoBehaviour
{
    const string NONE = "<color=#939393><i>대분류 선택</i></color>";
    [HideInInspector] public Dropdown dropdown;

    public void Update_Options()
    {
        if (dropdown == null)
            dropdown = GetComponent<Dropdown>();

        var options = new List<string>();
        options.Add(NONE);
        options.AddRange(DataController.instance.Get_Big_Categories());

        dropdown.ClearOptions();
        dropdown.AddOptions(options);
    }

    public string Get_Value()
    {
        if (dropdown == null)
            dropdown = GetComponent<Dropdown>();

        string result;

        if (dropdown.value == 0)
            result = string.Empty;
        else
            result = dropdown.options[dropdown.value].text;

        return result;
    }
}

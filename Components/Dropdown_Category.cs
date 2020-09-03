using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dropdown_Category : MonoBehaviour
{
    const string NONE = "<color=#939393><i>분류 선택</i></color>";
    [HideInInspector] public Dropdown dropdown;

    public void Update_Options()
    {
        if (dropdown == null)
            dropdown = GetComponent<Dropdown>();

        var options = new List<string>();
        options.Add(NONE);
        options.AddRange(DataController.instance.categories);

        dropdown.ClearOptions();
        dropdown.AddOptions(options);
    }

    public string Get_Value()
    {
        string result;

        if (dropdown.value == 0)
            result = string.Empty;
        else
            result = DataController.instance.categories[dropdown.value - 1];

        return result;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dropdown_Big_Cat : MonoBehaviour
{
    const string NONE = "<color=#939393><i>대분류 선택</i></color>";
    [HideInInspector] public Dropdown dropdown;

    static List<Dropdown_Big_Cat> static_dropdown_big_cat_list;

    public static void Update_All()
    {
        for (int i = 0; i < static_dropdown_big_cat_list.Count; i++)
        {
            static_dropdown_big_cat_list[i].Update_Options();
        }
    }

    private void Awake()
    {
        Update_Options();

        if (static_dropdown_big_cat_list == null)
            static_dropdown_big_cat_list = new List<Dropdown_Big_Cat>();
        static_dropdown_big_cat_list.Add(this);
    }

    void Update_Options()
    {
        if (dropdown == null)
            dropdown = GetComponent<Dropdown>();

        var options = new List<string>();
        options.Add(NONE);
        options.AddRange(DataController.instance.Get_Big_Categories());

        dropdown.ClearOptions();
        dropdown.AddOptions(options);

        dropdown.value = 0;
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

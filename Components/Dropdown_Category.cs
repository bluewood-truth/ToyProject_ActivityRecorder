﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dropdown_Category : MonoBehaviour
{
    const string NONE = "<color=#939393><i>분류 선택</i></color>";
    [HideInInspector] public Dropdown dropdown;
    [SerializeField] Dropdown_Big_Cat dropdown_big_cat;

    void Init()
    {
        dropdown = GetComponent<Dropdown>();
        if (dropdown_big_cat != null)
        {
            dropdown_big_cat.dropdown.onValueChanged.AddListener((int value) =>
            {
                string prev_cat = dropdown.options[dropdown.value].text;
                Update_Options(dropdown_big_cat.Get_Value());
                for(int i = 0; i < dropdown.options.Count; i++)
                {
                    if (dropdown.options[i].text == prev_cat)
                    {
                        dropdown.value = i;
                        break;
                    }
                }
            });
        }
    }

    public void Update_Options()
    {
        if (dropdown == null)
            Init();

        var options = new List<string>();
        options.Add(NONE);
        options.AddRange(DataController.instance.categories);

        dropdown.ClearOptions();
        dropdown.AddOptions(options);
    }

    public void Update_Options(string _big_cat)
    {
        if (_big_cat == string.Empty)
        {
            Update_Options();
            return;
        }

        if (dropdown == null)
            Init();

        var options = new List<string>();
        options.Add(NONE);
        options.AddRange(DataController.instance.Get_Categories_By_Big_Cat(_big_cat));

        dropdown.ClearOptions();
        dropdown.AddOptions(options);
    }

    public string Get_Value()
    {
        if (dropdown == null)
            Init();

        string result;

        if (dropdown.value == 0)
            result = string.Empty;
        else
            result = dropdown.options[dropdown.value].text;

        return result;
    }
}

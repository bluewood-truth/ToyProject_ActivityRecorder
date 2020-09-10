using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dropdown_Activity : MonoBehaviour
{
    const string NONE = "<color=#939393><i>활동 선택</i></color>";
    [HideInInspector] public Dropdown dropdown;
    List<int> activity_index;
    [SerializeField] Dropdown_Category dropdown_category;

    void Init()
    {
        dropdown = GetComponent<Dropdown>();
        if (dropdown_category != null)
        {
            dropdown_category.dropdown.onValueChanged.AddListener((int value) =>
            {
                Update_Options(dropdown_category.Get_Value());
            });
        }
    }

    public void Update_Options()
    {
        if (dropdown == null)
            Init();

        var options = new List<string>();
        options.Add(NONE);
        dropdown.ClearOptions();
        dropdown.AddOptions(options);

        dropdown.interactable = false;
    }

    public void Update_Options(string _category)
    {
        if(_category == string.Empty)
        {
            Update_Options();
            return;
        }

        if (dropdown == null)
            Init();

        dropdown.interactable = true;
        var options = new List<string>();
        activity_index = new List<int>();
        options.Add(NONE);
        activity_index.Add(-1);
        for(int i = 0; i < DataController.instance.activities.Count; i++)
        {
            var activity = DataController.instance.activities[i];
            if (activity.category == _category)
            {
                options.Add(activity.name);
                activity_index.Add(i);
            }
        }

        dropdown.ClearOptions();
        dropdown.AddOptions(options);
    }

    public Activity Get_Value()
    {
        Activity result;

        if (dropdown.value == 0)
            result = default;
        else
            result = DataController.instance.activities[activity_index[dropdown.value]];

        return result;
    }

    public int Get_Activity_Index()
    {
        if (dropdown == null)
            Init();

        if (activity_index != null)
            return activity_index[dropdown.value];
        else
            return -1;
    }
}

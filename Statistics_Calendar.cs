using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class Statistics_Calendar : MonoBehaviour
{
    [SerializeField] InputField input_year;
    [SerializeField] InputField input_month;
    [SerializeField] Transform container_days;

    [Space(10)]

    Color[] color_filled;
    [SerializeField] Color color_none;
    [SerializeField] Color color_disabled;

    const string EMPTY = "empty";

    string big_cat;
    string category;
    int activity_index;

    private void Start() // 나중에 OnEnable로 바꿔야됨
    {
        Set_Filled_Color(DataController.instance.color);
    }


    public void Update_Calendar(DateTime _month, string _big_cat = "", string _category = "", int _activity_index = -1)
    {
        big_cat = _big_cat;
        category = _category;
        activity_index = _activity_index;

        var date = _Functions.Get_Firstday_of_Month(_month);
        int first_weekday = (int)date.DayOfWeek;
        var today = DateTime.Today;

        for (int i = 0; i < container_days.childCount; i++)
        {
            var child = container_days.GetChild(i);
            if(i < first_weekday || date.Month > _month.Month)
            {
                Set_Empty_Date(child);
            }
            else if (date > today)
            {
                Set_Disabled_Date(child);
                date = date.AddDays(1);
            }
            else
            {
                Set_Date(child, date);
                date = date.AddDays(1);
            }
        }
    }

    void Set_Filled_Color(Color _origin)
    {
        color_filled = new Color[4] {
            Get_Alpha_Color(_origin, .4f),
            Get_Alpha_Color(_origin, .6f),
            Get_Alpha_Color(_origin, .8f),
            Get_Alpha_Color(_origin, 1.0f)
        };
    }

    Color Get_Alpha_Color(Color _origin, float _alpha)
    {
        return new Color(_origin.r, _origin.g, _origin.b, _alpha);
    }

    void Set_Empty_Date(Transform _child)
    {
        _child.name = EMPTY;
        _child.GetComponent<Button>().enabled = false;
        _child.GetComponent<Image>().enabled = false;
    }

    void Set_Disabled_Date(Transform _child)
    {
        _child.name = EMPTY;
        _child.GetComponent<Button>().enabled = false;
        _child.GetComponent<Image>().color = color_disabled;
    }

    void Set_Date(Transform _child, DateTime _date)
    {
        _child.name = _date.Day.ToString();
        var btn = _child.GetComponent<Button>();
        var img = _child.GetComponent<Image>();
        btn.enabled = true;
        img.enabled = true;
        btn.onClick.RemoveAllListeners();

        if (DataController.instance.records.ContainsKey(_date))
        {
            var record = DataController.instance.records[_date];
            if (record.Count < 1)
            {
                img.color = color_none;
            }
            else if (record.Count < 4)
            {
                img.color = color_filled[record.Count - 1];
            }
            else
            {
                img.color = color_filled[3];
            }
        }
        else
        {
            img.color = color_none;
        }
    }
}

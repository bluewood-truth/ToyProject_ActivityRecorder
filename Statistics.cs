using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Statistics : MonoBehaviour
{
    [SerializeField] Dropdown_Category dropdown_category;
    [SerializeField] Dropdown_Activity dropdown_activity;
    [SerializeField] Statistics_Calendar calendar;

    [Space(10)]

    [SerializeField] GameObject[] colored_objects;


    private void Start()
    {
        Init();

        calendar.Update_Calendar(System.DateTime.Today);
    }

    void Init()
    {
        dropdown_category.Update_Options();
        dropdown_activity.Update_Options();



        _Functions.Colored_Object_Caching(colored_objects);
    }
}

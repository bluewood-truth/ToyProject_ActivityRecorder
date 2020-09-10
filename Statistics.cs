using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Statistics : MonoBehaviour
{
    [SerializeField] Dropdown_Big_Cat select_big_cat;
    [SerializeField] Dropdown_Category select_category;
    [SerializeField] Dropdown_Activity select_activity;

    [Space(10)]

    [SerializeField] Statistics_Calendar calendar;

    [Space(10)]

    [SerializeField] GameObject[] colored_objects;


    private void Start()
    {
        Init();
        calendar.Update_Calendar(System.DateTime.Today, new Filtering_Data());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (calendar.Check_Daily_Window_Close() && (select_category.dropdown.Close_Check() && select_activity.dropdown.Close_Check()))
                gameObject.SetActive(false);
        }
    }

    void Init()
    {
        select_big_cat.Update_Options();
        select_category.Update_Options();
        select_activity.Update_Options();

        _Functions.Colored_Object_Caching(colored_objects);
    }

    public void Btn_Show_Calendar()
    {
        string big_cat = select_big_cat.Get_Value();
        string category = select_category.Get_Value();
        int activity_index = select_activity.Get_Activity_Index();

        var data = new Filtering_Data(big_cat, category, activity_index);

        calendar.Update_Calendar(default, data);
    }
}

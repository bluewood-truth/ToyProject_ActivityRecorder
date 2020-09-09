using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Text;

public class Statistics_Daily_Record : MonoBehaviour
{
    [SerializeField] Text text_date;
    [SerializeField] Text text_record;

    const string f_date = "MM.dd.({0})";

    public void Show_Daily_Record(DateTime _date, string _big_cat = "", string _category = "", int _activity_index = -1)
    {
        if (!DataController.instance.records.ContainsKey(_date))
        {
            Debug.Log("Null Key: 해당 날짜에 활동기록이 존재하지 않음");
            return;
        }

        var record = DataController.instance.records[_date];

        if(record.Count < 1)
        {
            Debug.Log("Count 0: 해당 날짜에 활동기록이 존재하지 않음");
            return;
        }

        text_date.text = string.Format(
            _date.ToString(f_date), 
            _Functions.Get_Day_of_Week_Kor((int)_date.DayOfWeek));

        StringBuilder sb = new StringBuilder();
        
    }
}

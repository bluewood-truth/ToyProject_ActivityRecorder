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
    const string f_italic = "<i>{0}</i>\n";
    const string f_count = "    <size=30>- {0}{1}</size>\n";

    public void Show_Daily_Record(DateTime _date, Filtering_Data _data)
    {
        if (!DataController.instance.records.ContainsKey(_date))
        {
            Debug.Log("Null Key: 해당 날짜에 활동기록이 존재하지 않음");
            return;
        }

        var day_records = DataController.instance.Get_Records_by_Filter(_date, _data);

        if(day_records.Length < 1)
        {
            Debug.Log("Count 0: 해당 날짜에 활동기록이 존재하지 않음");
            return;
        }

        text_date.text = string.Format(
            _date.ToString(f_date), 
            _Functions.Get_Day_of_Week_Kor((int)_date.DayOfWeek));

        StringBuilder sb = new StringBuilder();
        string prev_activity_name = string.Empty;
        for (int i = 0; i < day_records.Length; i++)
        {
            var record = day_records[i];
            if(prev_activity_name != record.activity.name)
                sb.Append(string.Format(f_italic, record.activity.name));
            sb.Append(string.Format(f_count, record.count, record.activity.count_unit));
            prev_activity_name = record.activity.name;
        }
        text_record.text = sb.ToString();

        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)text_record.transform.parent);
    }
}

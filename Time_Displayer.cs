using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Globalization;

public class Time_Displayer : MonoBehaviour
{
    [SerializeField] Text text_date;
    [SerializeField] Text text_time;

    // 상수
    const string f_date = "MM월 dd일 ddd요일";
    const string f_time = "HH:mm";
    const string f_time_sec_added = "{0}<size=128>:{1:00}</size>";

    WaitForSeconds wait_one_second = new WaitForSeconds(1);
    Coroutine cor_time_display;
    CultureInfo kor = CultureInfo.CreateSpecificCulture("ko-KR");

    private void Awake()
    {
        cor_time_display = StartCoroutine(c_Time_Display());
    }

    IEnumerator c_Time_Display()
    {
        DateTime time = DateTime.Now;
        Update_Date_Text(time);
        Update_Time_Text(time);

        while (true)
        {
            int day = time.Day;
            time = time.AddSeconds(1);

            // 날짜가 바뀌었으면 날짜 텍스트를 업데이트
            if (day != time.Day)
                Update_Date_Text(time);

            // 시간 텍스트를 업데이트
            Update_Time_Text(time);

            // 1초 대기
            yield return wait_one_second;
        }
    }

    void Update_Date_Text(DateTime _input)
    {
        text_date.text = _input.ToString(f_date, kor);
    }

    void Update_Time_Text(DateTime _input)
    {
        text_time.text = string.Format(f_time_sec_added, _input.ToString(f_time), _input.Second);
    }
}

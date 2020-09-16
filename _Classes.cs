using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


// 앱에 필요한 데이터를 정의하는 파일


public struct Activity
{
    public string name;
    public string category;
    public string count_unit;
    public Activity(string name, string category, string count_unit)
    {
        this.name = name;
        this.category = category;
        this.count_unit = count_unit;
    }
}

public enum Count_Unit
{
    없음=0, 회=1, 초=2, 분=3, 시간=4, m=5, km=6
}

public class Record
{
    public Activity activity;
    public int count;
    public DateTime datetime;
}


public class Timer_Setting
{
    public string name;
    public List<int> timesets;
    public int repeat;

    public int[] Get_All_Timesets()
    {
        var result = new List<int>();
        int n = 0;
        while(n < repeat)
        {
            result.AddRange(timesets);
            n += 1;
        }

        return result.ToArray();
    }
}


public class Todo
{
    public string name;
    public Activity[] activities;
    public bool[] activity_done;

    public Term term;

    // 특정 요일
    public bool[] term_weekday = new bool[7];

    // n일에 1회
    public DateTime term_start_date;
    public int term_day;

    public enum Term
    {
        특정요일, n일당1회
    }

    // 리셋 체크용 변수
    DateTime prev_day;


    // 표시 여부 
    public bool is_Display()
    {
        Next_Term_Check();
        DateTime today = DateTime.Today;

        // n일에 한 번
        if (term == Term.n일당1회)
            return true;
        // 특정 요일
        else
        {
            if (term_weekday[(int)today.DayOfWeek])
                return true;
        }

        return false;
    }

    // 한 주기가 끝났는지 체크하고 끝났으면 리셋
    void Next_Term_Check()
    {
        DateTime today = DateTime.Today;
        
        if(today != prev_day)
        {
            // 특정 요일
            if (term == Term.특정요일)
                Reset();
            // n일에 한 번
            else
            {
                int differ = Get_Day_Differ();
                if (differ % term_day == 0)
                    Reset();
            }
        }
        prev_day = today;
    }

    int Get_Day_Differ()
    {
        DateTime today = DateTime.Today;

        long tick_differ = today.Ticks - term_start_date.Ticks;
        TimeSpan differ = new TimeSpan(tick_differ);

        return differ.Days;
    }

    void Reset()
    {
        for (int i = 0; i < activity_done.Length; i++)
            activity_done[i] = false;
    }
}


public class Filtering_Data
{
    public string big_cat = "";
    public string category = "";
    public int activity_index = -1;

    public Filtering_Data(string _big_cat = "", string _category = "", int _activity_index = -1)
    {
        big_cat = _big_cat;
        category = _category;
        activity_index = _activity_index;
    }
}


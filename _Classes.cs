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
        if(repeat == 0)
        {
            for (int i = 0; i < 100; i++)
            {
                result.AddRange(timesets);
            }
        }

        return result.ToArray();
    }
}


public class Todo
{
    public bool display = true; // 메인화면에 표시할지

    public string name;
    public Activity[] activities;
    public bool[] activity_done;
    public int done_count {
        get {
            int count = 0;
            for (int i = 0; i < activity_done.Length; i++) {
                count += activity_done[i] ? 1 : 0;
            }
            return count;
        }
    }

    public Term term;

    // 특정 요일
    public bool[] term_weekday = new bool[7];

    // n일에 1회
    public DateTime term_start_date;
    public int term_day;

    public enum Term
    {
        특정요일=0, 일일=1, 기간=2
    }

    // 리셋 체크용 변수
    DateTime prev_day;

    // 메인에 표시 여부 
    public bool is_Display()
    {
        if (!display)
            return false;

        Next_Term_Check();
        DateTime today = DateTime.Today;


        switch (term)
        {
            case Term.특정요일:
                if (term_weekday[(int)today.DayOfWeek])
                    return true;
                break;
            case Term.일일:
                int differ = Get_Day_Differ();
                if (differ % term_day == 0)
                    return true;
                break;
            case Term.기간:
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
            switch (term)
            {
                case Term.특정요일:
                    Reset();
                    break;
                case Term.일일:
                    Reset();
                    break;
                case Term.기간:
                    int differ = Get_Day_Differ();
                    if (differ % term_day == 0)
                        Reset();
                    break;
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

    public int Get_Deadline_Day()
    {
        Next_Term_Check();
        int differ = Get_Day_Differ();
        int d_day = term_day - (differ % term_day) - 1;

        if (term == Term.일일)
            d_day += 1;
        
        if (d_day == term_day)
            d_day = 0;
        return d_day;
    }

    void Reset()
    {
        for (int i = 0; i < activity_done.Length; i++)
            activity_done[i] = false;
    }
}


public class Filtering_Data
{
    public string big_cat;
    public string category;
    public int activity_index;

    public Filtering_Data(string _big_cat = "", string _category = "", int _activity_index = -1)
    {
        big_cat = _big_cat;
        category = _category;
        activity_index = _activity_index;
    }
}


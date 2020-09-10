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
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
    public List<int> time_sets;
    public int repeat;

    public int[] Get_All_Time_Sets()
    {
        var result = new List<int>();
        int n = 0;
        while(n < repeat)
        {
            result.AddRange(time_sets);
            n += 1;
        }

        return result.ToArray();
    }
}
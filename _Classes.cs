using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
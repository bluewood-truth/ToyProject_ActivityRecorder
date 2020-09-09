using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

// 자주 사용되는 함수들을 static으로 모아둠
public static class _Functions
{
    public static void Clear(this Transform _input)
    {
        for(int i = 0; i < _input.childCount; i++)
        {
            _input.GetChild(i).gameObject.SetActive(false);
        }   
    }

    public static GameObject Get_Child_From_Pool(this Transform _container, GameObject _prefab, bool _as_first_child = false)
    {
        // 루프를 통해 비활성화된 child를 만나면 리턴
        for(int i = 0; i < _container.childCount; i++)
        {
            var child = _container.GetChild(i);
            if (!child.gameObject.activeSelf)
            {
                if (_as_first_child)
                    child.SetAsFirstSibling();
                return child.gameObject;
            }
        }

        // 비활성화된 child가 없다면 새로운 인스턴스를 생성
        var result = UnityEngine.Object.Instantiate(_prefab);
        result.transform.SetParent(_container);
        if (_as_first_child)
            result.transform.SetAsFirstSibling();
        return result;
    }

    public static bool Close_Check(this Dropdown _input)
    {
        if(_input.gameObject.activeSelf && _input.transform.childCount > 3)
        {
            _input.Hide();
            return false;
        }

        return true;
    }

    public static bool Close_Check(this GameObject _input)
    {
        if (_input.gameObject.activeSelf)
        {
            _input.SetActive(false);
            return false;
        }

        return true;
    }

    public static void Colored_Object_Caching(GameObject[] _colored_objects)
    {
        for (int i = 0; i < _colored_objects.Length; i++)
        {
            var c_object = _colored_objects[i];

            var img = c_object.GetComponent<Image>();
            if (img)
            {
                DataController.instance.colored_image.Add(img);
                img.color = DataController.instance.color;
                continue;
            }

            var txt = c_object.GetComponent<Text>();
            if (txt)
            {
                DataController.instance.colored_text.Add(txt);
                txt.color = DataController.instance.color;
                continue;
            }
        }
    }


    // 타이머 창을 닫아도 타이머가 계속 돌아가게 하기 위해
    public static void SetEnable(this GameObject _object, bool is_on)
    {
        CanvasGroup cg = _object.GetComponent<CanvasGroup>();
        if(cg == null)
        {
            Debug.Log("캔버스그룹이 없음:" + _object.name);
        }

        cg.alpha = is_on ? 1 : 0;
        cg.blocksRaycasts = is_on;
    }


    public static DateTime Get_Firstday_of_Month(DateTime _input)
    {
        var day = new DateTime(_input.Year, _input.Month, _input.Day);
        return day.AddDays(1 - day.Day);
    }

    static string[] day_of_week = new string[7] { "일", "월", "화", "수", "목", "금", "토" };
    public static string Get_Day_of_Week_Kor(int _day_of_week)
    {
        return day_of_week[_day_of_week];
    }
}

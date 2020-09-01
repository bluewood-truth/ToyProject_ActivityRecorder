using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        var result = Object.Instantiate(_prefab);
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
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Toggle_Text_Color : MonoBehaviour
{
    GameObject target;

    private void Start()
    {
        Init();
    }   

    void Init()
    {
        var toggle = GetComponent<Toggle>();
        var prefab_text = GetComponentInChildren<Text>().gameObject;

        target = Instantiate(prefab_text);
        target.transform.SetParent(transform);

        target.GetComponent<RectTransform>().localPosition = Vector2.zero;
        target.GetComponent<Text>().color = DataController.instance.color_white;

        toggle.onValueChanged.AddListener((bool is_on) =>
        {
            target.SetActive(is_on);
        });
        toggle.onValueChanged.Invoke(toggle.isOn);
    }
}

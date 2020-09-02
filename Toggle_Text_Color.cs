using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Toggle_Text_Color : MonoBehaviour
{
    Text target_text;

    private void Start()
    {
        Init();
    }   

    void Init()
    {
        var toggle = GetComponent<Toggle>();
        var prefab_text = GetComponentInChildren<Text>().gameObject;

        var target = Instantiate(prefab_text);
        target.transform.SetParent(transform);
        target.GetComponent<RectTransform>().localPosition = Vector2.zero;
        target_text = target.GetComponent<Text>();

        toggle.onValueChanged.AddListener((bool is_on) =>
        {
            target_text.color = DataController.instance.color_white;
            target_text.gameObject.SetActive(is_on);
        });
        toggle.onValueChanged.Invoke(toggle.isOn);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Button_Timeset : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] Timer_Manage parent;
    [SerializeField] bool is_up;
    [SerializeField] bool is_min;

    Coroutine cor_value_change;

    float delay = .5f;
    WaitForSeconds change_term = new WaitForSeconds(.08f);

    string f_value = "{0:00}";

    IEnumerator c_Value_Change()
    {
        float tmp = 0;
        while(tmp < delay)
        {
            tmp += Time.deltaTime;
            yield return null;
        }

        while (true)
        {
            Value_Change();
            yield return change_term;
        }
    }

    void Value_Change()
    {
        if (is_min)
            parent.timeset_min += is_up ? 1 : -1;
        else
            parent.timeset_sec += is_up ? 1 : -1;

        parent.Update_Timeset_Text(is_min);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Value_Change();
        cor_value_change = StartCoroutine(c_Value_Change());
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        StopCoroutine(cor_value_change);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlertHandler : MonoBehaviour
{
    [SerializeField] CanvasGroup toast;
    [SerializeField] Text text;

    Coroutine alert_display;
    float fade_time = .2f;
    bool is_playing = false;

    private void Awake()
    {
        Alert.handler = this;
    }

    public void Alert_Display(string _message, Wait _time)
    {
        if (is_playing)
            return;

        text.text = _message;
        toast.gameObject.SetActive(true);
        is_playing = true;

        switch (_time)
        {
            case Wait.Long:
                alert_display = StartCoroutine(c_Alert_Display(3f));
                break;
            case Wait.Short:
                alert_display = StartCoroutine(c_Alert_Display(1.5f));
                break;
        }
    }

    IEnumerator c_Alert_Display(float _t)
    {
        float alpha = 1;
        toast.alpha = alpha;

        yield return new WaitForSeconds(_t);

        while (alpha > 0)
        {
            toast.alpha = alpha;
            alpha -= Time.deltaTime / fade_time;
            yield return null;
        }

        toast.gameObject.SetActive(false);
        is_playing = false;
    }

    public enum Wait
    {
        Short, Long
    }
}



public static class Alert
{
    public static AlertHandler handler;

    public static void Show(string _message, AlertHandler.Wait _wait = AlertHandler.Wait.Short)
    {
        handler.Alert_Display(_message, _wait);
    }
}
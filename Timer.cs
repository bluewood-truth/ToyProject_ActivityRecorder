using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField] Dropdown select_timer;
    [SerializeField] Text text_time;
    [SerializeField] Text text_time_prev;
    [SerializeField] Text text_time_next;
    [SerializeField] Button btn_manage;
    [SerializeField] Button button_stop;

    [Space(10)]

    [SerializeField] GameObject[] colored_objects;

    const string f_time = "{0:00}:{1:00}<size=128>:{2:00}</size>";
    const string STOPWATCH = "<i>스톱워치</i>";

    int[] time_sets; // 연속적으로 실행될 타이머들의 배열
    int time_set_index = 0; // 타이머 배열의 인덱스

    float time_tms; // 현재 타이머에서 실제로 변화하는 시간 (10밀리초 단위)

    Coroutine cor_timer;

    public void Enable()
    {
        gameObject.SetActive(true);
        gameObject.SetEnable(true);
    }

    private void Start()
    {
        Init();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (select_timer.Close_Check())
                gameObject.SetEnable(false);
        }
    }

    void Init()
    {
        select_timer.onValueChanged.AddListener(Select_Timer);
        Update_Select_TImer_Options();
        Select_Timer(select_timer.value);

        _Functions.Colored_Object_Caching(colored_objects);
    }

    public void Update_Select_TImer_Options()
    {
        List<string> options = new List<string>();
        options.Add(STOPWATCH);
        for(int i = 0; i < DataController.instance.timer_settings.Count; i++)
        {
            options.Add(DataController.instance.timer_settings[i].name);
        }
        select_timer.ClearOptions();
        select_timer.AddOptions(options);
    }

    void Select_Timer(int _value)
    {
        if(_value == 0)
        {
            time_sets = null;
            Set_Stopwatch_Text();
            time_tms = 0;
        }
        else
        {
            var timer_now = DataController.instance.timer_settings[_value - 1];
            time_sets = timer_now.Get_All_Time_Sets();
            Set_Timer_Text(true);
            time_tms = time_sets[0];
        }
    }

    void Set_Stopwatch_Text()
    {
        text_time.text = Get_Time_Format_Text(0, false);
        text_time_next.text = string.Empty;
        text_time_prev.text = string.Empty;
    }

    void Set_Timer_Text(bool _reset = false)
    {
        if(_reset)
            time_set_index = 0;

        text_time.text = Get_Time_Format_Text(time_sets[time_set_index], false);

        if (time_set_index > 0)
            text_time_prev.text = Get_Time_Format_Text(time_sets[time_set_index - 1], false);
        else
            text_time_prev.text = string.Empty;

        if (time_sets.Length > time_set_index + 1)
            text_time_next.text = Get_Time_Format_Text(time_sets[time_set_index + 1], false);
        else
            text_time_next.text = string.Empty;

    }

    int tmp_time_tms = -100;
    void Update_Timer_Text(int _tms)
    {
        if (_tms < 0)
            _tms = 0;

        // 연산이 많아져 타이머가 느려지는 것을 방지하기 위해 띄엄띄엄 update시킴
        int abs = tmp_time_tms > _tms ? tmp_time_tms - _tms : _tms - tmp_time_tms;
        if (abs < 3)
            return;
        
        text_time.text = Get_Time_Format_Text(_tms);
        tmp_time_tms = _tms;
    }


    IEnumerator c_Stopwatch()
    {
        while (true)
        {
            time_tms += Time.deltaTime;
            Update_Timer_Text((int)(time_tms * 100));
            yield return null;
        }
    }

    IEnumerator c_Timer()
    {
        while (true)
        {
            // 현재 타이머가 종료됐으면 다음 타이머로 교체
            if (time_tms < 0)
            {
                time_set_index += 1;
                if (time_sets.Length <= time_set_index)
                {
                    // 정지
                    button_stop.onClick.Invoke();
                    break;
                }
                else
                {
                    time_tms = time_sets[time_set_index];
                    Set_Timer_Text();
                }
            }

            time_tms -= Time.deltaTime;
            Update_Timer_Text((int)(time_tms * 100));

            yield return null;
        }
    }


    public void Btn_Start()
    {
        tmp_time_tms = -100;
        select_timer.interactable = false;
        btn_manage.interactable = false;

        // time_sets가 null이면 스톱워치
        if(time_sets == null)
        {
            cor_timer = StartCoroutine(c_Stopwatch());
        }
        // 아니면 타이머
        else
        {
            cor_timer = StartCoroutine(c_Timer());
        }
    }

    public void Btn_Stop()
    {
        Select_Timer(select_timer.value);
        select_timer.interactable = true;
        btn_manage.interactable = true;
    }

    public void Btn_Pause()
    {
        StopCoroutine(cor_timer);
    }

    public void Btn_Continue()
    {
        if (time_sets == null)
        {
            cor_timer = StartCoroutine(c_Stopwatch());
        }
        else
        {
            cor_timer = StartCoroutine(c_Timer());
        }
    }

    string Get_Time_Format_Text(int _time, bool _is_tms = true)
    {
        // 10밀리초 단위가 아닐 경우 100을 곱해 단위를 맞춤
        if (!_is_tms)
            _time *= 100;

        int tms = _time % 100;
        int sec = ((_time - tms) % 6000) / 100;
        int min = (_time - tms - sec) / 6000;

        return string.Format(f_time, min, sec, tms);
    }
}


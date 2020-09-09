using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using UnityEngine.Events;

public class Timer_Manage : MonoBehaviour
{
    [SerializeField] InputField input_timer_name;

    [Space(10)]

    public Text text_min;
    public Text text_sec;
    [SerializeField] Transform container_timeset;
    [SerializeField] InputField input_timeset_repeat;
    GameObject prefab_timeset;

    [Space(10)]

    [SerializeField] Transform container_timer;
    GameObject prefab_timer;

    [Space(10)]

    [SerializeField] GameObject[] colored_objects;

    const string f_value = "{0:00}";
    const string f_timeset = "{0}m{1:00}s";
    const string f_timeset_sec_only = "{0}s";
    const string f_timer_name = "<size=45>{0}</size>\n  ";
    const string f_repeat = "\n  x {0}회 반복";
    const string REPEAT_PERMANENTLY = "\n  x 계속 반복";
    const string ONE = "1";

    List<int> timesets;
    int m_timeset_min;
    int m_timeset_sec;
    public int timeset_min
    {
        get { return m_timeset_min; }
        set
        {
            if (value > 99) value = 99;
            else if (value < 0) value = 0;
            m_timeset_min = value;
        }
    }
    public int timeset_sec
    {
        get { return m_timeset_sec; }
        set
        {
            if (value > 59) value = 59;
            else if (value < 0) value = 0;
            m_timeset_sec = value;
        }
    }
    const int timeset_limit = 5;

    public UnityAction action_update_timer;

    private void Start()
    {
        Init();
    }

    void Init()
    {
        prefab_timeset = container_timeset.GetChild(0).gameObject;
        prefab_timer = container_timer.GetChild(0).gameObject;

        Btn_Reset();
        Update_Timer_Container();

        _Functions.Colored_Object_Caching(colored_objects);
    }



    public void Update_Timeset_Text(bool _is_min)
    {
        Text text = _is_min ? text_min : text_sec;
        int v = _is_min ? timeset_min : timeset_sec;
        text.text = string.Format(f_value, v);
    }

    public void Btn_Add_Timeset()
    {
        if (timesets.Count >= timeset_limit)
        {
            Alert.Show(string.Format("추가할 수 있는 시간은 {0}개까지입니다.", timeset_limit));
            return;
        }

        int tmp_min = int.Parse(text_min.text);
        int tmp_sec = int.Parse(text_sec.text);

        if (tmp_min == 0 && tmp_sec == 0)
        {
            Alert.Show("추가할 시간을 설정해주세요.");
            return;
        }

        timesets.Add(tmp_min * 60 + tmp_sec);

        Update_Timeset_Container();
        Reset_Timeset_Texts();
    }

    void Update_Timeset_Container()
    {
        container_timeset.Clear();

        for(int i = 0; i < timesets.Count; i++)
        {
            var child = container_timeset.Get_Child_From_Pool(prefab_timeset);

            child.GetComponentInChildren<Text>().text = Get_Timeset_Str(timesets[i]);
    
            var btn_remove = child.GetComponentInChildren<Button>();
            btn_remove.onClick.RemoveAllListeners();
            int index = i;
            btn_remove.onClick.AddListener(() =>
            {
                timesets.RemoveAt(index);
                Update_Timeset_Container();
            });

            child.SetActive(true);
        }
    }

    string Get_Timeset_Str(int _input)
    {
        int tmp_min = _input / 60;
        int tmp_sec = _input - tmp_min * 60;
        
        string result = tmp_min < 1 ? 
            string.Format(f_timeset_sec_only, tmp_sec) : 
            string.Format(f_timeset, tmp_min, tmp_sec);

        return result;
    }


    void Reset_Timeset_Texts()
    {
        timeset_min = 0;
        timeset_sec = 0;
        Update_Timeset_Text(true);
        Update_Timeset_Text(false);
    }

    public void Btn_Reset()
    {
        Reset_Timeset_Texts();

        timesets = new List<int>();
        Update_Timeset_Container();

        input_timer_name.text = string.Empty;
        input_timeset_repeat.text = ONE;
    }

    public void Btn_Add_Timer()
    {
        if (!Timer_Input_Valid_Check())
            return;

        var timer = new Timer_Setting();
        timer.name = input_timer_name.text;
        timer.repeat = int.Parse(input_timeset_repeat.text);
        timer.timesets = timesets;

        DataController.instance.Add_Timer(timer);

        Update_Timer_Container();
        Btn_Reset();
    }

    bool Timer_Input_Valid_Check()
    {
        if (input_timer_name.text == string.Empty)
        {
            Alert.Show("타이머명을 입력해주세요.");
            return false;
        }
        if (timesets.Count < 1)
        {
            Alert.Show("타이머의 시간을 추가해주세요.");
            return false;
        }
        if (input_timeset_repeat.text == string.Empty)
        {
            Alert.Show("반복횟수를 입력해주세요.");
            return false;
        }
        return true;
    }

    void Update_Timer_Container()
    {
        container_timer.Clear();
        StringBuilder sb = new StringBuilder();

        for (int i = 0; i < DataController.instance.timer_settings.Count; i++)
        {
            var timer = DataController.instance.timer_settings[i];
            var child = container_timer.Get_Child_From_Pool(prefab_timer);

            sb.Append(string.Format(f_timer_name, timer.name));
            for(int j = 0; j < timer.timesets.Count; j++)
            {
                if (j != 0)
                    sb.Append('+');
                string timeset = Get_Timeset_Str(timer.timesets[j]);
                sb.Append(timeset);
            }

            sb.Append(Get_Repeat_Str(timer.repeat));

            child.GetComponentInChildren<Text>().text = sb.ToString();
            sb = new StringBuilder();

            var btn_remove = child.GetComponentInChildren<Button>();
            btn_remove.onClick.RemoveAllListeners();
            int index = i;
            btn_remove.onClick.AddListener(() =>
            {
                DataController.instance.Remove_Timer(index);
                Update_Timer_Container();
            });

            child.SetActive(true);
        }

        action_update_timer?.Invoke();
    }
        

    string Get_Repeat_Str(int _input)
    {
        if (_input == 1)
            return string.Empty;
        else if (_input == 0)
            return REPEAT_PERMANENTLY;
        else
            return string.Format(f_repeat, _input);
    }
}

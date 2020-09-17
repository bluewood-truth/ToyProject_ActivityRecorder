using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class Statistics_Calendar : MonoBehaviour
{
    [SerializeField] InputField input_year;
    [SerializeField] InputField input_month;
    [SerializeField] Transform container_days;
    [SerializeField] Text text_filtering;

    [Space(10)]

    [SerializeField] Statistics_Daily_Record daily_record;

    [Space(10)]

    Color[] color_filled;
    Dictionary<string, Color[]> color_filled_category;
    [SerializeField] Color color_none;
    [SerializeField] Color color_disabled;


    Toggle[] days;

    const string EMPTY = "empty";
    const string ALL = "(전체 표시)";
    const string f_activity = "(활동: {0})";
    const string f_category = "(분류: {0})";
    const string f_big_cat = "(대분류: {0})";
    const string f_num = "{0:00}";

    DateTime month;
    Filtering_Data data;

    private void Awake()
    {
        days = container_days.GetComponentsInChildren<Toggle>();

        input_year.onEndEdit.AddListener(Input_Year);
        input_month.onEndEdit.AddListener(Input_Month);
    }

    private void OnEnable()
    {
        Set_Filled_Color_by_Alpha(DataController.instance.color);
        Set_Filled_Category_Color();
        Update_Calendar(System.DateTime.Today, new Filtering_Data());
    }

    public bool Check_Daily_Window_Close()
    {
        for(int i = 0; i < days.Length; i++)
        {
            if (days[i].isOn)
            {
                days[i].isOn = false;
                return false;
            }
        }

        return true;
    }

    public void Update_Calendar(DateTime _month = default, Filtering_Data _data = null)
    {
        if (_data == null)
        {
            if (data == null)
                data = new Filtering_Data();
        }
        else
        {
            data = _data;
        }

        DateTime date;
        if(_month != default)
        {
            date = _Functions.Get_Firstday_of_Month(_month);
            month = date;
        }
        else
        {
            date = _Functions.Get_Firstday_of_Month(month);
        }
        int first_weekday = (int)date.DayOfWeek;
        var today = DateTime.Today;

        for (int i = 0; i < container_days.childCount; i++)
        {
            var child = container_days.GetChild(i);
            if(i < first_weekday || date.Month > month.Month)
            {
                Set_Empty_Date(child);
            }
            else if (date > today)
            {
                Set_Disabled_Date(child);
                date = date.AddDays(1);
            }
            else
            {
                Set_Date(child, date);
                date = date.AddDays(1);
            }
        }

        text_filtering.text = Get_Filtering_Text();

        input_year.text = string.Format(f_num, month.Year % 2000);
        input_month.text = string.Format(f_num, month.Month);
    }

    string Get_Filtering_Text()
    {        
        if(data.activity_index != -1)
        {
            return string.Format(f_activity, DataController.instance.activities[data.activity_index].name);
        }
        else if (data.category != string.Empty)
        {
            return string.Format(f_category, data.category);
        }
        else if (data.big_cat != string.Empty)
        {
            return string.Format(f_big_cat, data.big_cat);
        }
        else
        {
            return ALL;
        }
    }

    void Set_Filled_Color_by_Alpha(Color _origin)
    {
        color_filled = new Color[4] {
            Get_Alpha_Color(_origin, .4f),
            Get_Alpha_Color(_origin, .6f),
            Get_Alpha_Color(_origin, .8f),
            Get_Alpha_Color(_origin, 1.0f)
        };
    }

    void Set_Filled_Category_Color()
    {
        color_filled_category = new Dictionary<string, Color[]>();
        for(int i = 0; i < DataController.instance.categories.Count; i++)
        {
            string category = DataController.instance.categories[i];
            Color color = DataController.instance.Get_Category_Color(category);
            color_filled_category.Add(category, new Color[4]
                {
                    color,
                    Get_More_Deep_Color(color,2),
                    Get_More_Deep_Color(color,3),
                    Get_More_Deep_Color(color,4)
                });
        }
    }

    Color Get_Alpha_Color(Color _origin, float _alpha)
    {
        return new Color(_origin.r, _origin.g, _origin.b, _alpha);
    }

    Color Get_More_Deep_Color(Color _origin, int _mult)
    {
        float[] rgb = new float[3] { _origin.r, _origin.g, _origin.b};
        float max = Mathf.Max(rgb);
        bool[] is_max = new bool[3] {
            max == rgb[0],
            max == rgb[1],
            max == rgb[2] };

        float[] altered_rgb = new float[3];
        for(int i = 0; i < altered_rgb.Length; i++)
        {
            float differ = 1 - rgb[i];
            altered_rgb[i] = is_max[i] ? rgb[i] : 1 - (differ * _mult);
        }

        return new Color(altered_rgb[0], altered_rgb[1], altered_rgb[2]);
    }

    void Set_Empty_Date(Transform _child)
    {
        _child.name = EMPTY;
        _child.GetComponent<Toggle>().enabled = false;
        _child.GetChild(0).gameObject.SetActive(false);
        _child.GetComponent<Image>().enabled = false;
    }

    void Set_Disabled_Date(Transform _child)
    {
        _child.name = EMPTY;
        _child.GetComponent<Toggle>().enabled = false;
        _child.GetChild(0).gameObject.SetActive(false);
        _child.GetComponent<Image>().color = color_disabled;
    }

    void Set_Date(Transform _child, DateTime _date)
    {
        _child.name = _date.Day.ToString();
        var btn = _child.GetComponent<Toggle>();
        var img = _child.GetComponent<Image>();
        
        img.enabled = true;
        btn.onValueChanged.RemoveAllListeners();
        if (DataController.instance.records.ContainsKey(_date))
        {
            var record = DataController.instance.Get_Records_by_Filter(_date, data);

            if(record.Length > 0)
            {
                btn.enabled = true;
                _child.GetChild(0).gameObject.SetActive(true);
                Color[] color_palette;
                if (data.category != string.Empty)
                    color_palette = color_filled_category[data.category];
                else
                    color_palette = color_filled;

                if (record.Length < 1)
                {
                    img.color = color_none;
                }
                else if (record.Length < 4)
                {
                    img.color = color_palette[record.Length - 1];
                }
                else
                {
                    img.color = color_palette[3];
                }

                btn.onValueChanged.AddListener((bool is_on) =>
                {
                    daily_record.gameObject.SetActive(is_on);
                    if (is_on)
                        daily_record.Show_Daily_Record(_date, data);
                });
                return;
            }
        }
        img.color = color_none;
        btn.enabled = false;
        _child.GetChild(0).gameObject.SetActive(false);
    }


    public void Btn_Month_Move(bool _is_add)
    {
        int next_year = month.AddMonths(_is_add ? 1 : -1).Year;
        if (next_year > 2099 || next_year < 2000)
        {
            Alert.Show("유효하지 않은 날짜입니다.");
            return;
        }

        month = month.AddMonths(_is_add ? 1 : -1);
        Update_Calendar(month);
    }

    void Input_Year(string _input)
    {
        int year = int.Parse(_input);
        input_year.text = string.Format(f_num, year);

        month = month.AddYears((2000 + year) - month.Year);
        Update_Calendar(month);
    }

    void Input_Month(string _input)
    {
        int mon = int.Parse(_input);
        input_year.text = string.Format(f_num, mon);

        month = month.AddMonths((mon) - month.Month);
        Update_Calendar(month);
    }
}

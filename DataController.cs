using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using Newtonsoft.Json;
using System;
using UnityEngine.Events;

public class DataController : MonoBehaviour
{
    public static DataController instance = null;
    void Singleton()
    {
        if (instance == null)
            instance = this;

        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    private void Awake()
    {
        Singleton();
        Colored_Object_Caching();
        Load_Data();
    }


    void Load_Data()
    {
        categories = Load<List<string>>(CATEGORY);
        if (categories == null)
            categories = new List<string>();

        activities = Load<List<Activity>>(ACTIVITY);
        if (activities == null)
            activities = new List<Activity>();

        records = Load<Dictionary<DateTime, List<Record>>>(RECORD);
        if (records == null)
            records = new Dictionary<DateTime, List<Record>>();

        timer_settings = Load<List<Timer_Setting>>(TIMER);
        if (timer_settings == null)
            timer_settings = new List<Timer_Setting>();

        todo_lists = Load<List<Todo>>(TODO);
        if (todo_lists == null)
            todo_lists = new List<Todo>();
    }

    void Colored_Object_Caching()
    {
        var colored_objects = GameObject.FindGameObjectsWithTag(COLORED);
        _Functions.Colored_Object_Caching(colored_objects);
    }

    void Change_Color(Color _color)
    {
        for(int i = 0; i < colored_text.Count; i++)
        {
            colored_text[i].color = _color;
        }
        for (int i = 0; i < colored_image.Count; i++)
        {
            colored_image[i].color = _color;
        }
    }


    // 데이터들
    [HideInInspector] public List<string> categories;
    [HideInInspector] public List<Activity> activities;
    [HideInInspector] public Dictionary<DateTime, List<Record>> records;
    [HideInInspector] public List<Timer_Setting> timer_settings;
    [HideInInspector] public List<Todo> todo_lists;
    public List<Record> Get_Today_Records()
    {
        var today = DateTime.Today;
        if (!records.ContainsKey(today))
            records.Add(today, new List<Record>());
        return records[today];
    }

    public string[] Get_Big_Categories()
    {
        List<string> big_categories = new List<string>();
        for(int i = 0; i < categories.Count; i++)
        {
            string category = categories[i];
            if (category.Contains(DASH))
            {
                var big_cat = category.Split('-')[0];
                if (!big_categories.Contains(big_cat))
                    big_categories.Add(big_cat);
            }
        }

        return big_categories.ToArray();
    }
    
    public string[] Get_Categories_By_Big_Cat(string _big_cat)
    {
        List<string> result = new List<string>();
        string tmp = string.Format(f_dash, _big_cat);
        for (int i = 0; i < categories.Count; i++)
        {
            string category = categories[i];
            if (category.StartsWith(tmp) || category == _big_cat)
                result.Add(category);
        }
        return result.ToArray();
    }

    public Record[] Get_Records_by_Filter(DateTime _date, Filtering_Data _data)
    {
        if (!records.ContainsKey(_date))
            return new Record[0];

        var result = new List<Record>();
        var day_record = records[_date];

        // 활동으로 필터링
        if (_data.activity_index != -1)
        {
            for (int i = 0; i < day_record.Count; i++)
            {
                if(day_record[i].activity.Equals(activities[_data.activity_index]))
                {
                    result.Add(day_record[i]);
                }
            }
            return result.ToArray();
        }
        // 분류로 필터링
        else if (_data.category != string.Empty)
        {
            for (int i = 0; i < day_record.Count; i++)
            {
                if (day_record[i].activity.category == _data.category)
                {
                    result.Add(day_record[i]);
                }
            }
            return result.ToArray();
        }
        // 대분류로 필터링
        else if (_data.big_cat != string.Empty)
        {
            string tmp = string.Format(f_dash, _data.big_cat);
            for (int i = 0; i < day_record.Count; i++)
            {
                if (day_record[i].activity.category.StartsWith(tmp) || day_record[i].activity.category == _data.big_cat)
                {
                    result.Add(day_record[i]);
                }
            }
            return result.ToArray();
        }
        else
        {
            return records[_date].ToArray();
        }
    }

    [Space(10)]

    public Color color;
    public Color color_white;
    [SerializeField] Color[] category_colors;
    public Color Get_Category_Color(string _category)
    {
        int cat_index = categories.IndexOf(_category) % category_colors.Length;
        return category_colors[cat_index];
    }

    public List<Text> colored_text = new List<Text>();
    public List<Image> colored_image = new List<Image>();



    // 상수
    public const string COLORED = "Colored";
    const string SAVE = "SAVE";
    const string CATEGORY = "CATEGORY";
    const string ACTIVITY = "ACTIVITY";
    const string RECORD = "RECORD";
    const string TIMER = "TIMER";
    const string TODO = "TODO";
    const string DASH = "-";

    const string f_json = "{0}.json";
    public const string f_dash = "{0}-";


    public void Add_Category(string _input)
    {
        if (!categories.Contains(_input))
        {
            categories.Add(_input);
            Save(categories, CATEGORY);
        }
    }

    public void Remove_Category(int _index)
    {
        try
        {
            categories.RemoveAt(_index);
            Save(categories, CATEGORY);
        }
        catch
        {
            Debug.Log("Remove_Category: 존재하지 않는 인덱스 " + _index);
        }
    }

    public void Add_Activity(Activity _input)
    {
        if (!activities.Contains(_input))
        {
            activities.Add(_input);
            Save(activities, ACTIVITY);
        }
    }

    public void Remove_Activity(int _index)
    {
        try
        {
            activities.RemoveAt(_index);
            Save(activities, ACTIVITY);
        }
        catch
        {
            Debug.Log("Remove_Activity: 존재하지 않는 인덱스 " + _index);
        }
    }

    public UnityAction<Activity> action_todo_done;
    public void Add_Record(Record _input)
    {
        action_todo_done?.Invoke(_input.activity);
        records[DateTime.Today].Add(_input);
        Save(records, RECORD);
    }

    public void Remove_Record(int _index)
    {
        try
        {
            records[DateTime.Today].RemoveAt(_index);
            Save(records, RECORD);
        }
        catch
        {
            Debug.Log("Remove_Record: 존재하지 않는 인덱스 " + _index);
        }
    }

    public void Add_Timer(Timer_Setting _input)
    {
        timer_settings.Add(_input);
        Save(timer_settings, TIMER);
    }

    public void Remove_Timer(int _index)
    {
        try
        {
            timer_settings.RemoveAt(_index);
            Save(timer_settings, TIMER);
        }
        catch
        {
            Debug.Log("Remove_Timer: 존재하지 않는 인덱스 " + _index);
        }
    }

    public void Add_Todo(Todo _input = null)
    {
        if(_input != null)
            todo_lists.Add(_input);
        Save(todo_lists, TODO);
        Update_TodoList();
    }

    public void Update_Todo(int _index, Todo _todo)
    {
        todo_lists[_index] = _todo;
        Save(todo_lists, TODO);
        Update_TodoList();
    }

    public void Remove_Todo(int _index)
    {
        try
        {
            todo_lists.RemoveAt(_index);
            Save(todo_lists, TODO);
            Update_TodoList();
        }
        catch
        {
            Debug.Log("Remove_Todo: 존재하지 않는 인덱스 " + _index);
        }
    }

    public void Toggle_Todo(int _index, bool _toggle)
    {
        todo_lists[_index].display = _toggle;
        Save(todo_lists, TODO);
        Update_TodoList(false);
    }

    public UnityAction action_update_todolist;
    public UnityAction action_update_main_todolist;
    void Update_TodoList(bool _update_list = true)
    {
        if(_update_list)
            action_update_todolist?.Invoke();

        action_update_main_todolist?.Invoke();

        // 메인화면 업데이트 추가;
    }


    void Save(object _data, string _filename)
    {
        string json_data = JsonConvert.SerializeObject(_data);
        string path = Path.Combine(Application.persistentDataPath, SAVE);

        // 세이브 폴더가 없으면 만든다
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        path = Path.Combine(path, string.Format(f_json, _filename));
        File.WriteAllText(path, json_data, System.Text.Encoding.UTF8);
    }

    public T Load<T>(string _filename)
    {
        string path = Path.Combine(Application.persistentDataPath, SAVE, string.Format(f_json, _filename));

        // 파일이 없으면 null을 반환
        if (!File.Exists(path))
        {
            return default;
        }

        string json_data = File.ReadAllText(path);
        return JsonConvert.DeserializeObject<T>(json_data);
    }
}

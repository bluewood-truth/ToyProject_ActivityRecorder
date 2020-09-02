using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using Newtonsoft.Json;

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

    public Color color;
    public Color color_white;
    public List<Text> colored_text = new List<Text>();
    public List<Image> colored_image = new List<Image>();



    // 상수
    public const string COLORED = "Colored";
    const string SAVE = "SAVE";
    const string CATEGORY = "CATEGORY";
    const string ACTIVITY = "ACTIVITY";

    const string f_json = "{0}.json";


    
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

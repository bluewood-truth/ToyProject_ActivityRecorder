using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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



    // 상수
    const string SAVE = "SAVE";
    const string CATEGORY = "CATEGORY";
    const string ACTIVITY = "ACTIVITY";

    const string f_json = "{0}.json";


    // 데이터들
    public List<string> categories;
    public List<Activity> activities;



    
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

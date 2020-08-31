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
    }



    // 상수
    const string SAVE = "SAVE";
    const string f_json = "{0}.json";





    void Save(object _data, string _filename)
    {
        string jsonData = JsonConvert.SerializeObject(_data);
        string path = Path.Combine(Application.persistentDataPath, SAVE);

        // 세이브 폴더가 없으면 만든다
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        path = Path.Combine(path, string.Format(f_json, _filename));
        File.WriteAllText(path, jsonData, System.Text.Encoding.UTF8);
    }

    public T Load<T>(string _filename)
    {
        string path = Path.Combine(Application.persistentDataPath, SAVE, string.Format(f_json, _filename));
        string jsondata = File.ReadAllText(path);
        return JsonConvert.DeserializeObject<T>(jsondata);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Record_Activity : MonoBehaviour
{
    [SerializeField] Dropdown_Category select_category;
    [SerializeField] Dropdown_Activity select_activity;
    [SerializeField] InputField input_count;
    [SerializeField] Text text_unit;

    [Space(10)]

    [SerializeField] Transform container_record;
    GameObject prefab_record;

    [Space(10)]

    [SerializeField] GameObject[] colored_objects;


    const string f_time = "HH:mm";
    const string f_record = "<i><size=30><color=#{4}>  [{0}]</color></size>\n{1}</i> {2}{3}";


    private void Start()
    {
        Init();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (select_category.dropdown.Close_Check() && select_activity.dropdown.Close_Check())
                gameObject.SetActive(false);
        }
    }


    void Init()
    {
        prefab_record = container_record.GetChild(0).gameObject;

        select_category.Update_Options();
        select_activity.Update_Options();
        Update_Record_Container();

        select_activity.dropdown.onValueChanged.AddListener((int value) =>
        {
            if(value == 0)
            {
                text_unit.text = string.Empty;
            }
            else
            {
                text_unit.text = select_activity.Get_Value().count_unit;
            }
        });

        _Functions.Colored_Object_Caching(colored_objects);
    }


    void Reset_Inputs()
    {
        // select_activity.dropdown.value = 0;
        // select_category.dropdown.value = 0;
        input_count.text = string.Empty;
    }

    void Update_Record_Container()
    {
        container_record.Clear();
        var today_records = DataController.instance.Get_Today_Records();

        for(int i = 0; i < today_records.Count; i++)
        {
            var record = today_records[i];

            var child = _Functions.Get_Child_From_Pool(container_record, prefab_record, true);
            var child_texts = child.GetComponentsInChildren<Text>();
            var cat_color = DataController.instance.Get_Category_Color(record.activity.category);

            child_texts[0].text = string.Format(f_record,
                record.activity.category, record.activity.name,
                record.count, record.activity.count_unit,
                ColorUtility.ToHtmlStringRGB(cat_color));

            child_texts[1].text = record.datetime.ToString(f_time);

            var child_remove_btn = child.GetComponentInChildren<Button>();
            child_remove_btn.onClick.RemoveAllListeners();

            int index = i;
            child_remove_btn.onClick.AddListener(() =>
            {
                DataController.instance.Remove_Record(index);
                Update_Record_Container();
            });

            child.SetActive(true);
        }
    }

    public void Btn_Add_Record()
    {
        if (!Record_Input_Valid_Check())
            return;

        var record = new Record();

        record.activity = select_activity.Get_Value();
        record.count = int.Parse(input_count.text);
        record.datetime = System.DateTime.Now;

        DataController.instance.Add_Record(record);
        Reset_Inputs();
        Update_Record_Container();

    }

    bool Record_Input_Valid_Check()
    {
        if (select_category.dropdown.value == 0 || select_activity.dropdown.value == 0)
        {
            Alert.Show("분류와 활동 선택해주세요.");
            return false;
        }
        if (input_count.text == string.Empty)
        {
            Alert.Show("활동량을 입력해주세요.");
            return false;
        }
        
        return true;
    }
}

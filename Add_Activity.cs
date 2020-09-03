using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Add_Activity : MonoBehaviour
{

    [SerializeField] Toggle toggle_category;
    [SerializeField] Toggle toggle_activity;

    [Space(10)]

    [SerializeField] InputField input_category;
    [SerializeField] Dropdown_Category select_category;
    [SerializeField] Dropdown select_unit;
    [SerializeField] InputField input_activity;

    [Space(10)]

    [SerializeField] Transform category_container;
    [SerializeField] Transform activity_container;

    GameObject prefab_category_child;
    GameObject prefab_activity_child;

    [Space(10)]

    [SerializeField] GameObject[] colored_objects;


    bool is_category_page = true; // 분류 추가 화면인지
    const string IS_CAT_PAGE = "IS_CAT_PAGE";
    const string f_activity = "<i><size=36>[{0}]</size>\n{1}</i> <size=36>({2})</size>";


    private void Start()
    {
        Init();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(select_category.dropdown.Close_Check() && select_unit.Close_Check())
            gameObject.SetActive(false);
        }
    }

    void Init()
    {
        prefab_category_child = category_container.GetChild(0).gameObject;
        prefab_activity_child = activity_container.GetChild(0).gameObject;

        Update_Category_Container();
        Update_Activity_Container();

        if (PlayerPrefs.HasKey(IS_CAT_PAGE))
            is_category_page = PlayerPrefs.GetInt(IS_CAT_PAGE) == 1 ? true : false;

        if (is_category_page)
            toggle_category.isOn = true;
        else
            toggle_activity.isOn = true;
        toggle_category.onValueChanged.AddListener((bool isOn) => { PlayerPrefs.SetInt(IS_CAT_PAGE, isOn ? 1 : 0); });

        _Functions.Colored_Object_Caching(colored_objects);
    }


    public void Btn_Add_Cateogory()
    {
        if (!Category_Input_Valid_Check())
            return;

        string input = input_category.text;
        DataController.instance.Add_Category(input);

        input_category.text = string.Empty;
        Update_Category_Container();
    }

    bool Category_Input_Valid_Check()
    {
        string input = input_category.text;
        if (input == string.Empty)
        {
            return false;
        }
        if (DataController.instance.categories.Contains(input))
        {
            return false;
        }

        return true;
    }

    void Update_Category_Container()
    {
        category_container.Clear();

        var categories = DataController.instance.categories;
        for (int i = 0; i < categories.Count; i++)
        {
            var child = category_container.Get_Child_From_Pool(prefab_category_child);
            child.GetComponentInChildren<Text>().text = categories[i];

            var child_remove_btn = child.GetComponentInChildren<Button>();
            child_remove_btn.onClick.RemoveAllListeners();

            int index = i;
            child_remove_btn.onClick.AddListener(() =>
            {
                DataController.instance.Remove_Category(index);
                Update_Category_Container();
            });

            child.SetActive(true);
        }

        select_category.Update_Options();
    }


    public void Btn_Add_Activity()
    {
        if (!Activity_Input_Vaild_Check())
            return;

        DataController.instance.Add_Activity(new Activity(input_activity.text, select_category.Get_Value(), ((Count_Unit)select_unit.value).ToString()));

        input_activity.text = string.Empty;
        Update_Activity_Container();
    }

    bool Activity_Input_Vaild_Check()
    {
        if(select_category.Get_Value() == string.Empty || select_unit.value == 0)
        {
            return false;
        }
        string input = input_activity.text;
        if (input == string.Empty)
        {
            return false;
        }
        if (DataController.instance.activities.Contains(new Activity(input, select_category.Get_Value(), ((Count_Unit)select_unit.value).ToString() )))
        {
            return false;
        }

        return true;
    }

    void Update_Activity_Container()
    {
        activity_container.Clear();

        var activities = DataController.instance.activities;
        for (int i = 0; i < activities.Count; i++)
        {
            var child = activity_container.Get_Child_From_Pool(prefab_activity_child);
            var activity = activities[i];
            child.GetComponentInChildren<Text>().text = string.Format(f_activity, activity.category, activity.name, activity.count_unit);

            var child_remove_btn = child.GetComponentInChildren<Button>();
            child_remove_btn.onClick.RemoveAllListeners();

            int index = i;
            child_remove_btn.onClick.AddListener(() =>
            {
                DataController.instance.Remove_Activity(index);
                Update_Activity_Container();
            });

            child.SetActive(true);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Todo_List_Write : MonoBehaviour
{
    [SerializeField] InputField input_todo_name;
    [SerializeField] Dropdown_Category select_category;
    [SerializeField] Dropdown_Activity select_activity;
    [SerializeField] Transform container_activity;
    GameObject prefab_activity;

    [Space(10)]
    [SerializeField] Dropdown select_term;
    [SerializeField] Transform[] container_term;
    [Space(5)]
    Toggle[] toggle_term_weekday;
    [Space(5)]
    [SerializeField] InputField input_term_start;
    [SerializeField] InputField input_term_day;
    [Space(5)]
    [SerializeField] InputField input_period_start;
    [SerializeField] InputField input_period;
    [Space(10)]

    [SerializeField] GameObject[] colored_objects;


    int update_index = -1; // 수정할 todo의 인덱스. -1이면 신규작성.

    List<Activity> tmp_activities = new List<Activity>();

    const string f_activity = "<i><size=30><color=#{2}>[{0}]</color></size>\n{1}</i>";
    const string f_date = "yyyy.MM.dd";

    void Init()
    {
        prefab_activity = container_activity.GetChild(0).gameObject;
        toggle_term_weekday = container_term[(int)Todo.Term.특정요일].GetComponentsInChildren<Toggle>();
        
        Update_Activity_Container();

        _Functions.Colored_Object_Caching(colored_objects);
    }

    public bool Close_Check()
    {
        return select_category.dropdown.Close_Check() && 
            select_activity.dropdown.Close_Check() && 
            select_term.Close_Check() && gameObject.Close_Check();
    }

    public void Show(int _update_index = -1)
    {
        if (prefab_activity == null)
            Init();

        update_index = _update_index;
        gameObject.SetActive(true);
        Btn_Reset();
    }

    public void Btn_Reset()
    {
        for (int i = 0; i < toggle_term_weekday.Length; i++)
            toggle_term_weekday[i].isOn = false;
        input_term_start.text = DateTime.Today.ToString(f_date);
        input_term_day.text = string.Empty;
        input_period_start.text = DateTime.Today.ToString(f_date);
        input_period.text = string.Empty;

        input_todo_name.text = string.Empty;

        select_category.dropdown.value = 0;
        select_activity.dropdown.value = 0;
        select_term.value = 0;
        tmp_activities = new List<Activity>();

        if (update_index != -1)
        {
            Todo todo = DataController.instance.todo_lists[update_index];
            input_todo_name.text = todo.name;
            tmp_activities = new List<Activity>(todo.activities);

            select_term.value = (int)todo.term + 1;

            switch (todo.term)
            {
                case Todo.Term.특정요일:
                    for (int i = 0; i < todo.term_weekday.Length; i++)
                    {
                        toggle_term_weekday[i].isOn = todo.term_weekday[i];
                    }
                    break;
                case Todo.Term.일일:
                    input_term_start.text = todo.term_start_date.ToString(f_date);
                    input_term_day.text = todo.term_day.ToString();
                    break;
                case Todo.Term.기간:
                    input_period_start.text = todo.term_start_date.ToString(f_date);
                    input_period.text = todo.term_day.ToString();
                    break;
            }
        }

        Update_Activity_Container();
    }

    public void Btn_Add_Activity()
    {
        if(select_activity.dropdown.value == 0)
        {
            Alert.Show("활동을 선택해주세요.");
            return;
        }

        tmp_activities.Add(select_activity.Get_Value());
        Update_Activity_Container();
    }

    void Update_Activity_Container()
    {
        container_activity.Clear();
        for(int i = 0; i < tmp_activities.Count; i++)
        {
            var child = _Functions.Get_Child_From_Pool(container_activity, prefab_activity);
            var activity = tmp_activities[i];

            int index = i;
            var cat_color = DataController.instance.Get_Category_Color(activity.category);
            child.GetComponentInChildren<Text>().text = string.Format(f_activity, activity.category, activity.name, ColorUtility.ToHtmlStringRGB(cat_color));

            var btn_remove = child.GetComponentInChildren<Button>();
            btn_remove.onClick.RemoveAllListeners();
            btn_remove.onClick.AddListener(() =>
            {
                tmp_activities.RemoveAt(index);
                Update_Activity_Container();
            });

            child.transform.localScale = Vector2.one;
            child.SetActive(true);
        }
    }

    public void Select_Term(int _value)
    {
        for(int i = 0; i < container_term.Length; i++)
        {
            container_term[i].gameObject.SetActive(false);
        }
        if(_value != 0)
        {
            container_term[_value - 1].gameObject.SetActive(true);
        }
    }

    public void Btn_Add_Todo()
    {
        if (!Todo_Input_Valid_Check())
            return;

        Todo new_todo = new Todo();
        new_todo.name = input_todo_name.text;
        new_todo.activities = tmp_activities.ToArray();
        new_todo.activity_done = new bool[new_todo.activities.Length];
        new_todo.term = (Todo.Term)select_term.value - 1;

        switch (new_todo.term)
        {
            case Todo.Term.특정요일:
                for (int i = 0; i < toggle_term_weekday.Length; i++)
                {
                    new_todo.term_weekday[i] = toggle_term_weekday[i].isOn;
                }
                break;
            case Todo.Term.일일:
                new_todo.term_start_date = DateTime.ParseExact(input_term_start.text, f_date, null);
                new_todo.term_day = int.Parse(input_term_day.text);
                break;
            case Todo.Term.기간:
                new_todo.term_start_date = DateTime.ParseExact(input_period_start.text, f_date, null);
                new_todo.term_day = int.Parse(input_period.text);
                break;
        }

        if(update_index == -1)
        {
            DataController.instance.Add_Todo(new_todo);
        }
        else
        {
            DataController.instance.Update_Todo(update_index, new_todo);
        }

        gameObject.SetActive(false);
    }

    bool Todo_Input_Valid_Check()
    {
        if(input_todo_name.text == string.Empty)
        {
            Alert.Show("스케줄명을 입력해주세요.");
            return false;
        }

        if(tmp_activities.Count == 0)
        {
            Alert.Show("최소한 하나 이상의 활동이 입력되어야 합니다.");
            return false;
        }

        if(select_term.value == 0)
        {
            Alert.Show("기간을 선택해주세요.");
            return false;
        }
        // 특정 요일
        else if(select_term.value == 1)
        {
            bool is_on = false;
            for(int i = 0; i < toggle_term_weekday.Length; i++)
            {
                if (toggle_term_weekday[i].isOn)
                {
                    is_on = true;
                    break;
                }
            }
            if (!is_on)
            {
                Alert.Show("하나 이상의 요일을 체크해주세요.");
                return false;
            }
        }
        // n일에 한 번
        else if (select_term.value == 2)
        {
            if(input_term_start.text == string.Empty || input_term_day.text == string.Empty)
            {
                Alert.Show("시작일과 기간을 입력해주세요.");
                return false;
            }

            try
            {
                DateTime tmp = DateTime.ParseExact(input_term_start.text, f_date, null);
            }
            catch
            {
                Alert.Show("날짜 형식이 올바르지 않습니다. (yyyy.mm.dd)");
                return false;
            }
        }

        return true;
    }
}

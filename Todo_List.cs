using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Todo_List : MonoBehaviour
{
    [SerializeField] Transform container_todo;
    GameObject prefab_todo;
    [SerializeField] Todo_List_Write write;

    [Space(10)]
    
    [SerializeField] GameObject[] colored_objects;

    const string f_todo = "{0} ({1}/{2})\n<size=32>{3}</size>";
    const string f_term = "{0}일에 한 번 (다음 기한: {1})";
    const string f_d_day = "{0}일 뒤";
    const string TODAY = "오늘";
    const string TOMORROW = "내일";

    private void Awake()
    {
        Init();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (write.Close_Check())
            {
                gameObject.SetActive(false);
            }
        }
    }

    void Init()
    {
        prefab_todo = container_todo.GetChild(0).gameObject;

        DataController.instance.action_update_todolist += Update_Todo_List;
        Update_Todo_List();

        _Functions.Colored_Object_Caching(colored_objects);
    }

    void Update_Todo_List()
    {
        container_todo.Clear();

        for(int i = 0; i < DataController.instance.todo_lists.Count; i++)
        {
            var child = container_todo.Get_Child_From_Pool(prefab_todo);
            var todo = DataController.instance.todo_lists[i];

            var toggle = child.transform.GetChild(0).GetComponent<Toggle>();
            var text = child.transform.GetChild(1).GetComponent<Text>();
            var btn_edit = child.transform.GetChild(2).GetComponent<Button>();
            var btn_remove = child.transform.GetChild(3).GetComponent<Button>();
            int index = i;

            toggle.onValueChanged.RemoveAllListeners();
            toggle.isOn = todo.display;
            toggle.onValueChanged.AddListener((bool is_on) => { DataController.instance.Toggle_Todo(index, is_on); });

            text.text = string.Format(f_todo, todo.name, todo.done_count, todo.activity_done.Length, Get_Term_Text(todo));

            btn_edit.onClick.RemoveAllListeners();
            btn_edit.onClick.AddListener(() => { write.Show(index); });
            btn_remove.onClick.RemoveAllListeners();
            btn_remove.onClick.AddListener(() => { DataController.instance.Remove_Todo(index); });

            child.SetActive(true);
        }
    }

    public static string Get_Term_Text(Todo _todo)
    {
        var sb = new System.Text.StringBuilder();

        if(_todo.term == Todo.Term.특정요일)
        {
            for(int i = 0; i < _todo.term_weekday.Length; i++)
            {
                if (i != 0)
                    sb.Append(' ');

                int index = i + 1;
                if (index == 7)
                    index = 0;

                if (_todo.term_weekday[index])
                {
                    sb.Append(_Functions.Get_Day_of_Week_Kor(index));
                }
            }
        }
        else if (_todo.term == Todo.Term.n일당1회)
        {
            string deadline;
            int d_day = _todo.Get_Deadline_Day();
            switch (d_day)
            {
                case 0:
                    deadline = TODAY;
                    break;
                case 1:
                    deadline = TOMORROW;
                    break;
                default:
                    deadline = string.Format(f_d_day, d_day);
                    break;
            }

            sb.Append(string.Format(f_term, _todo.term_day, deadline));
        }

        return sb.ToString();
    }
}

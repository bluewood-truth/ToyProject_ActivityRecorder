using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Main_TodoList : MonoBehaviour
{
    [SerializeField] Transform container_todo;
    GameObject prefab_todo;
    [SerializeField] Todo_Detail todo_detail;

    [SerializeField] Color gray;

    const string f_todo = "[{0}] {1} ({2}/{3})";
    const string f_todo_done = "[{0}] {1} ✓";
    const string f_term = "D-{0}";
    const string DAILY = "일일";  

    private void Start()
    {
        Init();
    }

    void Init()
    {
        prefab_todo = container_todo.GetChild(0).gameObject;

        DataController.instance.action_update_main_todolist += Update_Todo_List;
        DataController.instance.action_todo_done += Todo_Done;

        Update_Todo_List();
    }

    void Update_Todo_List()
    {
        container_todo.Clear();

        for (int i = 0; i < DataController.instance.todo_lists.Count; i++)
        {
            var todo = DataController.instance.todo_lists[i];
            if (!todo.is_Display())
                continue;
            var child = container_todo.Get_Child_From_Pool(prefab_todo);

            string term;

            if (todo.term == Todo.Term.특정요일)
            {
                term = DAILY;
            }
            else
            {
                term = string.Format(f_term, todo.Get_Deadline_Day());
            }

            var text = child.GetComponentInChildren<Text>();
            bool is_done = todo.done_count == todo.activities.Length;
            text.text = is_done ?
                string.Format(f_todo_done, term, todo.name) :
                string.Format(f_todo, term, todo.name, todo.done_count, todo.activities.Length);
            text.color = is_done ? DataController.instance.color : gray;

            int index = i;
            var btn = child.GetComponent<Button>();
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(() => { todo_detail.Show(index); });

            child.SetActive(true);
        }
    }


    void Todo_Done(Activity _input)
    {
        for (int i = 0; i < DataController.instance.todo_lists.Count; i++)
        {
            var todo = DataController.instance.todo_lists[i];
            if (!todo.is_Display())
                continue;

            for (int j = 0; j < todo.activities.Length; j++)
            {
                if (_input.Equals(todo.activities[j]) && !todo.activity_done[j])
                {
                    todo.activity_done[j] = true;
                    break;
                }
            }
        }
        DataController.instance.Add_Todo();
    }



}

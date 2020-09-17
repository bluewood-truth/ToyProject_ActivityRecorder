using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Todo_Detail : MonoBehaviour
{
    [SerializeField] Text text_todo_detail;
    const string f_todo_detail = "{0} ({1}/{2})\n<size=40>{3}\n\n{4}</size>";
    const string f_item = " - {0}";
    const string f_gray = "<color=#fff9>{0}</color>";
    const string f_done = "{0} ✓";

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            gameObject.SetActive(false);
        }
    }

    public void Show(int _todo_index)
    {
        var todo = DataController.instance.todo_lists[_todo_index];

        var sb = new System.Text.StringBuilder();
        for(int i = 0; i < todo.activities.Length; i++)
        {
            string item = string.Format(f_item, todo.activities[i].name);
            item = string.Format(todo.activity_done[i] ? f_done : f_gray, item);
            sb.Append(item);
            sb.Append('\n');
        }

        text_todo_detail.text = string.Format(f_todo_detail,
            todo.name, todo.done_count, todo.activities.Length,
            Todo_List.Get_Term_Text(todo), sb.ToString());

        gameObject.SetActive(true);
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)text_todo_detail.transform);
    }
}

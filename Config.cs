using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Config : MonoBehaviour
{
    [SerializeField] Transform container_palette;
    GameObject prefab_color;

    public const string COLOR = "Color";

    private void Awake()
    {
        Init();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            gameObject.SetActive(false);
        }
    }

    void Init()
    {
        Init_Color_Selector();
    }

    void Init_Color_Selector()
    {
        prefab_color = container_palette.GetChild(0).gameObject;

        container_palette.Clear();

        for (int i = 0; i < DataController.instance.color_palette.Length; i++)
        {
            var child = container_palette.Get_Child_From_Pool(prefab_color);
            var toggle = child.GetComponent<Toggle>();
            var color = DataController.instance.color_palette[i];

            int index = i;
            child.transform.Find(COLOR).GetComponent<Image>().color = color;
            toggle.isOn = color == DataController.instance.color;
            toggle.onValueChanged.AddListener((bool is_on) =>
            {
                if (is_on)
                {
                    DataController.instance.Change_Color(color);
                    PlayerPrefs.SetInt(COLOR, index);
                }
            });

            child.SetActive(true);
        }
    }
}

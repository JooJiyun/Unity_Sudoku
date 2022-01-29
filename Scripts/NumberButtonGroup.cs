using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NumberButtonGroup : MonoBehaviour
{
    private int stage_level;
    private int level;
    private bool notemode = false;

    public GameObject NumberButton_prefab;
    private GameObject[] NumberButton_object;

    public GameObject CellsContent_object;

    private int not_note_value;
    private bool[] note_check;
    void Start()
    {
        stage_level = PlayerPrefs.GetInt("Level", 3);
        level = stage_level * stage_level;

        if (stage_level >= 5)
        {
            transform.GetComponent<GridLayoutGroup>().cellSize = new Vector2(70, 70);
            transform.GetComponent<GridLayoutGroup>().constraintCount = 3;
        }


        NumberButton_object = new GameObject[level];
        for (int i = 0; i < level; i++)
        {
            NumberButton_object[i] = Instantiate(NumberButton_prefab) as GameObject;
            NumberButton_object[i].transform.SetParent(transform, false);
            NumberButton_object[i].GetComponent<NumberButton>().SetIdx(i);
        }
    }
    public bool ToggleMode()
    {
        notemode = notemode ? false : true;
        for (int i = 0; i < level; i++)
        {
            NumberButton_object[i].GetComponent<NumberButton>().ChangeModeButton(notemode);
        }
        SetNumberButtons();
        return gameObject.activeSelf;
    }
    public void SetAllValues(int value, bool[] in_note)
    {
        gameObject.SetActive(true);
        not_note_value = value;
        note_check = in_note;
        SetNumberButtons();
    }

    public void SetNumberButtons()
    {
        for (int i = 0; i < level; i++)
        {
            NumberButton_object[i].GetComponent<NumberButton>().SetInitColor();
        }
        if (notemode)
        {
            for (int i = 0; i < level; i++)
            {
                if (note_check[i]) NumberButton_object[i].GetComponent<NumberButton>().ChangeToggleColor();
            }
        }
        else
        {
            if (not_note_value <= 0) return;
            NumberButton_object[not_note_value - 1].GetComponent<NumberButton>().ChangeToggleColor();
        }
    }

    public void ClickNumberButton(int value)
    {
        if (!notemode)
        {
            if (not_note_value > 0)
            {
                NumberButton_object[not_note_value - 1].GetComponent<NumberButton>().ChangeToggleColor();
            }
            not_note_value = value;
        }
        NumberButton_object[value - 1].GetComponent<NumberButton>().ChangeToggleColor();
        CellsContent_object.GetComponent<CellsContent>().InsertValueToCell(value, notemode);
    }
}

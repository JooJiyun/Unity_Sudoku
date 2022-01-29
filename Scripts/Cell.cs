using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cell : MonoBehaviour
{

    public Text Value;

    public Text Note_object;

    public GameObject Right_bar, Left_bar, Bottom_bar, Top_bar;
    public GameObject Background;

    public bool[] in_note;
    private bool touchable;

    private int idx_x, idx_y;
    private int value_by_user = 0;
    public bool correct_value = false;

    public void Init(int stage_level, int pos_x, int pos_y)
    {
        touchable = true;
        idx_x = pos_x;
        idx_y = pos_y;

        in_note = new bool[(stage_level * stage_level) + 1];
        for (int i = 1; i < in_note.Length; i++)
        {
            in_note[i] = false;
        }
    }

    public int GetValue()
    {
        return value_by_user;
    }
    public void ShowWrong()
    {
        if (value_by_user < 1) return;
        correct_value = false;
        if (!transform.GetComponentInParent<CellsContent>().AssistantMode) return;
        Value.color = Color.red;
    }
    public void BlindWrong()
    {
        if (value_by_user < 1) return;
        correct_value = true;
        Value.color = Color.black;
    }

    public void SetValueByComputer(int v)
    {
        touchable = false;
        Value.text = v.ToString();
        Value.color = Color.gray;
        value_by_user = -1;
        correct_value = true;
    }
    public void SetValueByPlayer(int v)
    {
        Value.text = v.ToString();
        Value.color = Color.black;
        value_by_user = v;
    }
    public void SetValueInit()
    {
        Value.text = "";
        value_by_user = 0;
    }

    public void SetToggleNote(int v)
    {
        in_note[v] = in_note[v] ? false : true;

        Note_object.text = "";
        for (int i = 1; i < in_note.Length; i++)
        {
            if (in_note[i]) Note_object.text = Note_object.text + (i).ToString() + " ";
        }
    }

    public void OnClick()
    {
        if (!touchable) return;

        Background.GetComponent<Image>().color = Color.cyan;

        transform.GetComponentInParent<CellsContent>().ClickCell(idx_x, idx_y, value_by_user, in_note);
    }
    public void EndClick()
    {
        Background.GetComponent<Image>().color = Color.white;
    }

    public void ShowRightBar()
    {
        Right_bar.SetActive(true);
    }
    public void ShowLeftBar()
    {
        Left_bar.SetActive(true);
    }
    public void ShowBottomBar()
    {
        Bottom_bar.SetActive(true);
    }
    public void ShowTopBar()
    {
        Top_bar.SetActive(true);
    }
}

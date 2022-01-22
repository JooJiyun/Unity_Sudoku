using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NumberButton : MonoBehaviour
{
    public Text Value_text;
    private int value;
    private bool checked_flg = false;

    public void SetIdx(int idx)
    {
        value = idx + 1;
        Value_text.text = value.ToString();
    }
    public void OnClick()
    {
        transform.GetComponentInParent<NumberButtonGroup>().ClickNumberButton(value);
    }

    public void ChangeToggleColor()
    {
        checked_flg = checked_flg ? false : true;
        if (checked_flg) 
        {
            transform.GetComponent<Image>().color = Color.cyan;
        }
        else
        {
            transform.GetComponent<Image>().color = Color.white;
        }
    }

    public void ChangeModeButton(bool notemode)
    {
        if (notemode)
        {
            ColorUtility.TryParseHtmlString("#9088FF", out Color color);
            transform.GetComponent<Outline>().effectColor =  color;
            Value_text.color = Color.gray;
        }
        else
        {
            transform.GetComponent<Outline>().effectColor = Color.yellow;
            Value_text.color = Color.black;
        }
    }

    public void SetInitColor()
    {
        transform.GetComponent<Image>().color = Color.white;
        checked_flg = false;
    }
}

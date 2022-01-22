using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModeButton : MonoBehaviour
{
    private bool notemode = false;
    public GameObject NumberButtonGroup_object;

    public void OnClick()
    {
        notemode = notemode ? false : true;
        if (!NumberButtonGroup_object.GetComponent<NumberButtonGroup>().ToggleMode()) return;
        if (notemode)
        {
            transform.GetComponent<Image>().color = Color.gray;
        }
        else
        {
            transform.GetComponent<Image>().color = Color.white;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HintButton : MonoBehaviour
{
    private bool hintmode = true;
    public GameObject CellsContents_object;

    public void OnClick()
    {
        hintmode = hintmode ? false : true;
        CellsContents_object.GetComponent<CellsContent>().ToggleAssistantMode();
        if (hintmode)
        {
            transform.GetComponent<Image>().color = Color.white;
        }
        else
        {
            transform.GetComponent<Image>().color = Color.gray;
        }
    }
}

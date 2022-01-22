using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuCanvas : MonoBehaviour
{
    public GameObject Background;
    public GameObject[] BackgroundCell;
    private int[] cell_sign;

    public Dropdown Level_object;
    public Text Hint_object;
    
    void Start()
    {
        cell_sign = new int[BackgroundCell.Length];
        for(int i = 0; i < BackgroundCell.Length; i++)
        {
            Color tmp = BackgroundCell[i].GetComponent<Image>().color;
            tmp.a = Random.Range(0f, 0.5f);
            BackgroundCell[i].GetComponent<Image>().color = tmp;
            cell_sign[i] = 1;
        }
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        Color.RGBToHSV(Background.GetComponent<Image>().color, out float h, out float s, out float v);
        h += Time.deltaTime * 0.01f;
        if (h > 1) h -= 1;
        Background.GetComponent<Image>().color = Color.HSVToRGB(h, s, v);

        for (int i = 0; i < BackgroundCell.Length; i++)
        {
            Color tmp = BackgroundCell[i].GetComponent<Image>().color;
            if (tmp.a > 0.5) cell_sign[i] = -1;
            else if (tmp.a < 0.01) cell_sign[i] = 1;
            tmp.a += cell_sign[i] * Time.deltaTime * 0.1f;
            BackgroundCell[i].GetComponent<Image>().color = tmp;
        }
    }

    public void HintUp()
    {
        Hint_object.text = (int.Parse(Hint_object.text) + 1).ToString();
        Hint_object.color = Color.black;
        int level = Level_object.value + 3;
        level *= level;
        if (int.Parse(Hint_object.text) >= ((level * level)-1))
        {
            Hint_object.text = ((level * level) - 1).ToString();
            Hint_object.color = Color.red;
        }
    }
    public void HintDown()
    {
        Hint_object.text = (int.Parse(Hint_object.text) - 1).ToString();
        Hint_object.color = Color.black;
        if (int.Parse(Hint_object.text) <= 1)
        {
            Hint_object.text = "1";
            Hint_object.color = Color.red;
        }
    }

    public void LoadGameScene()
    {
        PlayerPrefs.SetInt("Level", Level_object.value + 3);
        PlayerPrefs.SetInt("Hint", int.Parse(Hint_object.text));
        SceneManager.LoadScene(1);
    }
}

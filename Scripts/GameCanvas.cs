using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameCanvas : MonoBehaviour
{
    public GameObject QuitGameCanvas;
    public GameObject EndGameCanvas;
    public Text Score; 

    public GameObject Timer_object;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            QuitGameCanvas.gameObject.SetActive(true);
            Timer_object.GetComponent<Timer>().On_flg = false;
        }
    }

    public void BlindQuitGameCanvas()
    {
        QuitGameCanvas.gameObject.SetActive(false);
        Timer_object.GetComponent<Timer>().On_flg = true;
    }

    public void CompleteGame()
    {
        EndGameCanvas.gameObject.SetActive(true);
        Timer_object.GetComponent<Timer>().On_flg = false;
        Score.text = Timer_object.GetComponent<Text>().text;
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}

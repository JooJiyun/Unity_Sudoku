using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public float time = 0;
    public bool On_flg = true;

    void Update()
    {
        if (On_flg)
        {
            time += Time.deltaTime;
            float tmp = time;
            string hour = ((int)(tmp / 3600)).ToString();
            if (hour.Length == 1) hour = "0" + hour;
            tmp %= 3600;
            string min = ((int)(tmp / 60)).ToString();
            if (min.Length == 1) min = "0" + min;
            tmp %= 60;
            string sec = ((int)tmp).ToString();
            if (sec.Length == 1) sec = "0" + sec;
            transform.GetComponent<Text>().text = hour+":"+min+":"+sec;
        }
    }
}

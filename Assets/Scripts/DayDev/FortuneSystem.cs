using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.Audio;
using TMPro;

public class FortuneSystem : MonoBehaviour
{
    public static Root data = new Root();
    private string json;
    private string filePath;
    private int today;
    private int thisMonth;
    private int thisWeek;
    private int daySelected;
    private int typeSelected;
    private string[] headText = { "พื้นดวงชีวิต", "การงาน", "การเงิน", "ความรัก", "สุขภาพ" };
    [SerializeField] private GameObject firstPanel;
    [SerializeField] private GameObject dayPanel;
    [SerializeField] private GameObject typePanel;
    [SerializeField] private GameObject btnFortune;
    [SerializeField] private GameObject textFortune;
    [SerializeField] private GameObject btnBack;
    [SerializeField] private GameObject vdoPanel;
    [SerializeField] private Text txtValue;
    [SerializeField] private VideoPlayer vdo;
    [SerializeField] private AudioClip[] audios;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private Text txtHead;
    [SerializeField] private Text txtDebug;
    
    void Start()
    {
        filePath = Application.streamingAssetsPath + "/DataFortune.json";
        txtDebug.text = Application.streamingAssetsPath + "/DataFortune.json";
        StartCoroutine(FetchDataWithFilepath(filePath));
        CheckDate();
        vdo.url = Application.streamingAssetsPath + "/vdo.mp4";

    }
        public void Clicked_Fortune()
    {
        typePanel.SetActive(true);
        btnFortune.SetActive(false);
        firstPanel.SetActive(false);
    }
    public void Clicked_Back()
    {
        firstPanel.SetActive(false);
        dayPanel.SetActive(false);
        typePanel.SetActive(false);
        btnFortune.SetActive(true);
        textFortune.SetActive(false);
        btnBack.SetActive(false);
        vdoPanel.SetActive(false);
        /*switch (valueBack)
        {
            case 0:
                btnBack.SetActive(false);
                dayPanel.SetActive(false);
                btnFortune.SetActive(true);
                break;
            case 1:
                valueBack = 0;
                dayPanel.SetActive(true);
                typePanel.SetActive(false);
                break;
            case 2:
                valueBack = 1;
                textFortune.SetActive(false);
                typePanel.SetActive(true);
                break;
        }*/
    }
    public void Clicked_Days(int value)
    {
        daySelected = value;
        dayPanel.SetActive(false);
        SetTextValue();
    }
    public void Clicked_GotoWebSite()
    {
        Application.OpenURL("http://siamphithee.com/");
    }
    public void Clicked_TypeSelected(int value)
    {
        typePanel.SetActive(false);
        typeSelected = value;
        dayPanel.SetActive(true);
        txtHead.text = data.Head[value].value;
    }
    private void CheckDate()
    {
        today = DateTime.Today.Day;
        thisMonth = DateTime.Today.Month - 1;
        if (today >= 0 || today <= 7)
        {
            thisWeek = 0;
        }
        else if (today >= 7 || today <= 13)
        {
            thisWeek = 1;
        }
        else if (today >= 14 || today <= 20)
        {
            thisWeek = 2;
        }
        else
            thisWeek = 3;
        
        Debug.Log(today + " " + thisWeek + "" + thisMonth);
    }
    public void SetTextValue()
    {
        //vdoPanel.SetActive(true);
        print(data.Month[thisMonth].Week[thisWeek].day[daySelected].data[typeSelected].value);
        txtValue.text = data.Month[thisMonth].Week[thisWeek].day[daySelected].data[typeSelected].value;
        audioSource.PlayOneShot(audios[1]);
        textFortune.SetActive(true);
        btnBack.SetActive(true);
        //StartCoroutine(PlayVideo());
    }
    IEnumerator PlayVideo()
    {
        vdo.Play();
        audioSource.volume = 0;
        textFortune.SetActive(true);
        btnBack.SetActive(true);
        yield return new WaitForSeconds(13.0f);
        audioSource.volume = 1;
        vdoPanel.SetActive(false);
        audioSource.PlayOneShot(audios[1]);
    }

    IEnumerator FetchDataWithFilepath(string filePath)
    {
        if (filePath.Contains("://") || filePath.Contains(":///"))
        {
            WWW www = new WWW(filePath);
            yield return www;
            json = www.text;
            //JSONNode jsonNode = JSON.Parse(json);
            JsonUtility.FromJsonOverwrite(json, data);
            //JsonConvert.SerializeObject(data, Formatting.Indented);
            Debug.Log(data.Month[2].Week[0].day[0]);
        }
        else
        {
            json = File.ReadAllText(filePath);
            Debug.Log(json);
            JsonUtility.FromJsonOverwrite(json, data);
            Debug.Log(data.Month[3].Week[0].day[0].data[0].value);
        }
    }

}
//DATA JSON
[Serializable]
public class Data
{
    public string value;
}
[Serializable]
public class Day
{
    public List<Data> data;
}
[Serializable]
public class Week
{
    public List<Day> day;
}
[Serializable]
public class Month
{
    public List<Week> Week;
}
[Serializable]
public class Head
{
    public string value;
}
[Serializable]
public class Root
{
    public List<Month> Month;
    public List<Head> Head;
}

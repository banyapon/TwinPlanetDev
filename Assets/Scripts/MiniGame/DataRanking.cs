using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class DataRanking : MonoBehaviour
{
    private string json;
    private string urlLoad = "https://twinplanetonline.com/api/highscoresongkran.php";
    public RankData data;
    public static DataRanking instance;
    public Text currentRank;
    private string _name, _email;

    [SerializeField] private GameObject scoreLabel;

    private void Start()
    {
        instance = this;
        StartCoroutine(LoadData(urlLoad));
        _email = PlayerPrefs.GetString("email");
        _name = PlayerPrefs.GetString("nickname");
    }

    public void SetScore(int score)
    {
        StartCoroutine(SaveData(score.ToString()));
    }
    private void ShowData()
    {
        foreach (var item in data.response)
        {
            if (item.email == _email)
            {
                currentRank.text = item.rank.ToString();
            }
        }
        if (data.response.Count >= scoreLabel.transform.childCount)
        {
            for (int i = 0; i < scoreLabel.transform.childCount; i++)
            {
                scoreLabel.transform.GetChild(i).GetChild(1).GetComponent<Text>().text = data.response[i].fullname;
                scoreLabel.transform.GetChild(i).GetChild(2).GetComponent<Text>().text = data.response[i].score;
            }
        }
        else
        {
            for (int i = 0; i < data.response.Count; i++)
            {
                scoreLabel.transform.GetChild(i).GetChild(1).GetComponent<Text>().text = data.response[i].fullname;
                scoreLabel.transform.GetChild(i).GetChild(2).GetComponent<Text>().text = data.response[i].score;
            }
        }
    }
    IEnumerator LoadData(string filePath = "")
    {
        data = new RankData();
        if (filePath.Contains("://") || filePath.Contains(":///"))
        {
            WWW www = new WWW(filePath);
            yield return www;
            json = www.text;
            //JSONNode jsonNode = JSON.Parse(json);
            JsonUtility.FromJsonOverwrite(json, data);
            //JsonConvert.SerializeObject(data, Formatting.Indented);
            //Debug.Log(data.Month[2].Week[0].day[0]);
            Debug.Log(data.response[0].fullname);
            ShowData();
            StopCoroutine(LoadData());
        }
        else
        {
            json = File.ReadAllText(filePath);
            JsonUtility.FromJsonOverwrite(json, data);
            Debug.Log(data.response[0].fullname);
            //Debug.Log(data.Month[3].Week[0].day[0].data[0].value);
            StopCoroutine(LoadData());
        }
    }
    /*IEnumerator submitMyName()
    {
        json = JsonUtility.ToJson(data);
        WWWForm form = new WWWForm();
        form.AddField("status_code", "200");
        form.AddField("response", json);

        WWW www = new WWW("https://twinplanetonline.com/api/highscoresongkran.php?limit=10", form);
        yield return www;

        int maxUser = userData.Count;
        for (int i = 0; i < maxUser; i++)
        {
            Debug.Log(userData[i]["name"]);
        }
        StopCoroutine(submitMyName());
    }*/
    IEnumerator SaveData(string score = "")
    {
        string url = "https://twinplanetonline.com/api/submitscore.php?email=" + _email + "&score=" + score + "&fullname=" + _name + "&api_key=twinplanet0x156xxee011";
        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            yield return www.SendWebRequest();
            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("Form upload complete!");
                Debug.Log(www.downloadHandler.text);
                StartCoroutine(LoadData(urlLoad));
                StopCoroutine(SaveData());
            }
        }
    }
}
[System.Serializable]
public class RankData
{
    public int status_code;
    public List<Response> response;

}
[System.Serializable]
public class Response
{
    public int rank;
    public string fullname;
    public string score;
    public string email;
}



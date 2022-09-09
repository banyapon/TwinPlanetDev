using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using SimpleJSON;

public class ContestList : MonoBehaviour
{
    public const string getContestList = "http://147.50.231.95:8001/api/RequestContest.ashx";
    public const string getContestByID = "http://147.50.231.95:8001/api/RequestContestByID.ashx?ID=";
    public const string getContestImage = "http://147.50.231.95:8001/api/DownloadImageContest.ashx?";

    [Header("CONTEST INFO")]
    public int userCount;
    public List<string> userID = new List<string>();
    public List<string> userCode = new List<string>();
    public List<string> contestName = new List<string>();
    public List<Sprite> sprite = new List<Sprite>();

    [Header("USER INTERFACE")]
    [SerializeField] private GameObject[] _modelList;
    [SerializeField] private int _index;

    void Start()
    {
        for (int i = 0; i < 4; i++)
        {
            Image img = _modelList[i].transform.Find("Frame/Image").GetComponent<Image>();
            Text text = _modelList[i].transform.Find("Tag/Text").GetComponent<Text>();

            img.sprite = null;
            text.text = "loading..";
        }

        StartCoroutine(GetContestList());
    }

    public void ChangePage(bool isNext)
    {
        for (int i = 0; i < 4; i++)
        {
            Image img = _modelList[i].transform.Find("Frame/Image").GetComponent<Image>();
            Text text = _modelList[i].transform.Find("Tag/Text").GetComponent<Text>();

            img.sprite = null;
            text.text = "loading..";
        }

        StartCoroutine(GetContestImage(isNext));
    }

    IEnumerator GetContestList()
    {
        UnityWebRequest requestContestList = UnityWebRequest.Get(getContestList);

        yield return requestContestList.SendWebRequest();

        if (requestContestList.isNetworkError || requestContestList.isHttpError)
        {
            Debug.LogError(requestContestList.error);
            yield break;
        }

        JSONNode contestUserListInfo = JSON.Parse(requestContestList.downloadHandler.text);
        Debug.Log(contestUserListInfo);
        JSONNode _userCount = contestUserListInfo["Message"];

        for (int i = 0; i < _userCount.Count; i++) //IMPORT ALL USER ID & USER CODE
        {
            userID.Add(_userCount[i]["docno"].ToString().Replace('"', ' ').Trim());
            userCode.Add(_userCount[i]["uscode"].ToString().Replace('"', ' ').Trim());

            if (_userCount[i]["image1"].ToString() == "null") //REMOVE USER WHO DIDN'T IMPORT CONTEST IMAGE
            {
                userID.RemoveRange(userID.Count - 1, 1);
                userCode.RemoveRange(userCode.Count - 1, 1);
            }
        }
        userCount = userID.Count;

        Debug.Log("Loaded Contest List");
        StartCoroutine(GetContestImage(true));
    }

    IEnumerator GetContestImage(bool isNext)
    {
        for (int i = 0; i < 4; i++)
        {
            //GET IMAGE
            UnityWebRequest requestContestImage1 = UnityWebRequestTexture.GetTexture(getContestImage + "ID=" + userID[_index] + "&CODE=" + 3);
            yield return requestContestImage1.SendWebRequest();
            if (requestContestImage1.isNetworkError || requestContestImage1.isHttpError)
            {
                Debug.LogError(requestContestImage1.error);
                yield break;
            }
            Texture2D image1 = ((DownloadHandlerTexture)requestContestImage1.downloadHandler).texture;
            sprite[i] = Sprite.Create(image1, new Rect(0, 0, image1.width, image1.height), Vector2.zero);

            //GET CONTEST NAME
            UnityWebRequest requestContestID = UnityWebRequest.Get(getContestByID + userID[_index]);
            yield return requestContestID.SendWebRequest();
            if (requestContestID.isNetworkError || requestContestID.isHttpError)
            {
                Debug.LogError(requestContestID.error);
                yield break;
            }
            JSONNode contestInfoList = JSON.Parse(requestContestID.downloadHandler.text);
            JSONNode contestInfo = contestInfoList["Message"][0];
            contestName[i] = contestInfo["teamname"].ToString().Replace('"', ' ').Trim();

            if (isNext)
            {
                if (_index >= userID.Count - 1) _index = 0;
                else _index++;
            }
            else
            {                
                if (_index <= 0) _index = userID.Count - 1;
                else _index--;
            }
        }

        for (int i = 0; i < 4; i++)
        {           
            Image img = _modelList[i].transform.Find("Frame/Image").GetComponent<Image>();
            Text text = _modelList[i].transform.Find("Tag/Text").GetComponent<Text>();

            int x = i;
            if (!isNext) x = 4 - i - 1;

            img.sprite = sprite[x];
            text.text = contestName[x];
        }
    }
}

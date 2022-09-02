using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using SimpleJSON;

public class ContestWindow : MonoBehaviour
{
    [Header("BILLBOARD UI")]
    [SerializeField] private Image _contestImg;
    [SerializeField] private Sprite _sprite1, _sprite2;
    [SerializeField] private Text _teamName, _contestName, _institution;
    [SerializeField] private int _index;
    [SerializeField] private Button[] _buttons;

    [Header("TERMS & CONDITIONS")]
    [SerializeField] private GameObject _conditionCanvas;
    [SerializeField] private Image _confirmWaitButton;
    [SerializeField] private Button _confirmButton;

    [Header("CONTEST API")]
    public const string getContestList = "http://147.50.231.95:8001/api/RequestContest.ashx";
    public const string getContestByID = "http://147.50.231.95:8001/api/RequestContestByID.ashx?ID=";
    public const string getContestByUserCode = "http://147.50.231.95:8001/api/RequestContestByUsercode.ashx?CODE=";
    public const string getContestImage = "http://147.50.231.95:8001/api/DownloadImageContest.ashx?";

    [Header("CONTEST INFO")]
    [SerializeField] private int userCount;
    [SerializeField] private List<string> userID = new List<string>();
    [SerializeField] private List<string> userCode = new List<string>();

    void Start()
    {
        _conditionCanvas.SetActive(true);
        _confirmButton.interactable = false;
        StartCoroutine(ConditionCooldown());

        _contestImg.sprite = null;
        _sprite1 = null;
        _sprite2 = null;
        //_teamName.text = "";
        _contestName.text = "";
        //_institution.text = "";

        StartCoroutine(GetContestList());
    }

    #region TERMS AND CONDITIONS

    IEnumerator ConditionCooldown()
    {
        float waitTime = 3f;
        float counter = waitTime;

        while (counter > 0)
        {
            counter -= Time.deltaTime;
            _confirmWaitButton.fillAmount = counter / waitTime;
            yield return null;
        }

        _confirmButton.interactable = true;
    }

    public void CloseCondition()
    {
        _conditionCanvas.SetActive(false);
    }

    #endregion

    #region BILLBOARD MANAGER

    public void Show()
    {
        _contestImg.sprite = null;
        _sprite1 = null;
        _sprite2 = null;
        //_teamName.text = "Loading..";
        _contestName.text = "ชื่อผลงาน : Loading..";
        //_institution.text = "สถาบัน : Loading..";

        StartCoroutine(GetContestInfo(_index));
    }

    public void OnClickNext()
    {
        if (_index >= userCount - 1) _index = 0;
        else _index++;

        Show();
    }

    public void OnClickBack()
    {
        if (_index <= 0) _index = userCount - 1;
        else _index--;

        Show();
    }

    public void OnClickPDF()
    {
        Application.OpenURL(getContestImage + "ID=" + userID[_index] + "&CODE=" + 1);
    }

    public void OnClickClose()
    {
        FindObjectOfType<ContestBoard>().ToggleContestWindow(false);
    }

    public void ShowImage(int index)
    {
        if (index == 1)
            _contestImg.sprite = _sprite1;
        else
            _contestImg.sprite = _sprite2;
    }

    private void ButtonInteractable(bool isActive)
    {
        for (int i = 0; i < _buttons.Length; i++)
        {
            _buttons[i].interactable = isActive;
        }
    }

    #endregion

    #region API WEB REQUEST
    IEnumerator GetContestList()
    {
        UnityWebRequest requestContestList = UnityWebRequest.Get(getContestList);

        yield return requestContestList.SendWebRequest();

        if(requestContestList.isNetworkError || requestContestList.isHttpError)
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
        Show();
    }

    IEnumerator GetContestInfo(int index)
    {
        ButtonInteractable(false);
        UnityWebRequest requestContestID = UnityWebRequest.Get(getContestByID + userID[index]);

        yield return requestContestID.SendWebRequest();

        if(requestContestID.isNetworkError || requestContestID.isHttpError)
        {
            Debug.LogError(requestContestID.error);
            yield break;
        }

        JSONNode contestInfoList = JSON.Parse(requestContestID.downloadHandler.text);
        JSONNode contestInfo = contestInfoList["Message"][0];

        //GET STRING FROM CONTEST API
        string contestName = contestInfo["contestname"].ToString().Replace('"', ' ').Trim();
        string teamName = contestInfo["teamname"].ToString().Replace('"',' ').Trim();        
        string institution = contestInfo["institution"].ToString().Replace('"', ' ').Trim();

        _contestName.text = "ชื่อผลงาน : " + contestName;
        //_teamName.text = "ชื่อทีม : " + teamName;
        //_institution.text = "สถาบัน : " + institution;

        StartCoroutine(GetContestImage(index));
        ButtonInteractable(true);
    }

    IEnumerator GetContestImage(int index)
    {
        //REQUEST IMAGE 1
        UnityWebRequest requestContestImage1 = UnityWebRequestTexture.GetTexture(getContestImage + "ID=" + userID[index] + "&CODE=" + 2);   
        yield return requestContestImage1.SendWebRequest();
        if(requestContestImage1.isNetworkError || requestContestImage1.isHttpError)
        {
            Debug.LogError(requestContestImage1.error);
            yield break;
        }

        //REQUEST IMAGE 2
        UnityWebRequest requestContestImage2 = UnityWebRequestTexture.GetTexture(getContestImage + "ID=" + userID[index] + "&CODE=" + 3);
        yield return requestContestImage2.SendWebRequest();
        if(requestContestImage2.isNetworkError || requestContestImage2.isHttpError)
        {
            Debug.LogError(requestContestImage2.error);
            yield break;
        }

        Texture2D image1 = ((DownloadHandlerTexture)requestContestImage1.downloadHandler).texture;
        Texture2D image2 = ((DownloadHandlerTexture)requestContestImage2.downloadHandler).texture;
        _sprite1 = Sprite.Create(image1, new Rect(0, 0, image1.width, image1.height), Vector2.zero);
        _sprite2 = Sprite.Create(image2, new Rect(0, 0, image2.width, image2.height), Vector2.zero);

        ShowImage(2);
    }
    #endregion   
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using SimpleJSON;

public class ContestWindow : MonoBehaviour
{
    [Header("BILLBOARD UI")]
    [SerializeField] private GameObject _contestWindow;
    [SerializeField] private GameObject _contestList;
    [SerializeField] private Image _contestImg;
    [SerializeField] private Sprite _sprite1, _sprite2;
    [SerializeField] private Text _teamName, _contestName, _institution, _voteStatus;
    [SerializeField] private Button _voteButton;
    [SerializeField] private int _index;

    [Header("TERMS & CONDITIONS")]
    [SerializeField] private GameObject _conditionCanvas;
    [SerializeField] private Image _confirmWaitButton;
    [SerializeField] private Button _confirmButton;

    [Header("VOTE API")]
    public const string voteApi = "https://twinplanetonline.com/api/fashion/vote.php?email=";
    public const string apiKey = "twinplanet0x156xxee011";
    [HideInInspector] public string characterImage;
    [HideInInspector] public string designImage;
    [HideInInspector] public string inspirePdf;
    [HideInInspector] public string contestID;

    [Header("CONTEST INFO")]
    [SerializeField] private string userID;

    void Start()
    {        
        StartCoroutine(ConditionCooldown());

        _contestImg.sprite = null;
        _sprite1 = null;
        _sprite2 = null;
        //_teamName.text = "";
        _contestName.text = "";
        //_institution.text = "";

        //StartCoroutine(GetContestList());
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Alpha1))
            Debug.Log("Press");
    }

    #region TERMS AND CONDITIONS

    IEnumerator ConditionCooldown()
    {
        _conditionCanvas.SetActive(true);
        _confirmButton.interactable = false;
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
    public void Show(string image1, string image2, string pdf, string name, string id)
    {
        _contestWindow.SetActive(true);
        characterImage = image1;
        designImage = image2;
        inspirePdf = pdf;
        contestID = id;
        _contestName.text = name;

        StartCoroutine(GetContestImage());
        Debug.Log("Showed");
    }

    public void OnClickPDF()
    {
        Application.OpenURL(inspirePdf);
    }

    public void OnClickClose()
    {
        _contestImg.sprite = null;
        _sprite1 = null;
        _sprite2 = null;
        //_teamName.text = "";
        _contestName.text = "";
        _voteStatus.text = "";
        characterImage = null;
        designImage = null;
        inspirePdf = null;
        _voteButton.interactable = true;

        _contestWindow.SetActive(false);
        _contestList.SetActive(true);
    }

    public void ShowImage(int index)
    {
        if (index == 1)
            _contestImg.sprite = _sprite1;
        else
            _contestImg.sprite = _sprite2;
    }

    public void Vote()
    {
        _voteButton.interactable = false;
        StartCoroutine(GetVoteCheck());
    }

    public void VoteResult(bool isSuccess)
    {
        if (isSuccess)
        {
            _voteStatus.color = Color.green;
            _voteStatus.text = "โหวตสำเร็จ!";
        }
        else
        {
            _voteStatus.color = Color.red;
            _voteStatus.text = "ไม่สามารถโหวตได้อีก";
        }
    }

    #endregion

    #region API WEB REQUEST
    IEnumerator GetContestImage()
    {
        //REQUEST IMAGE 1
        UnityWebRequest requestContestImage1 = UnityWebRequestTexture.GetTexture(characterImage);   
        yield return requestContestImage1.SendWebRequest();
        if(requestContestImage1.isNetworkError || requestContestImage1.isHttpError)
        {
            Debug.LogError(requestContestImage1.error);
            yield break;
        }

        //REQUEST IMAGE 2
        UnityWebRequest requestContestImage2 = UnityWebRequestTexture.GetTexture(designImage);
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

    IEnumerator GetVoteCheck()
    {
        UnityWebRequest requestVoteAPI = UnityWebRequest.Get(voteApi + PlayerPrefs.GetString("email") + "&code=" + contestID + "&api_key=" + apiKey);
        yield return requestVoteAPI.SendWebRequest();
        if (requestVoteAPI.isNetworkError || requestVoteAPI.isHttpError)
        {
            Debug.LogError(requestVoteAPI.error);
            yield break;
        }

        JSONNode voteData = JSON.Parse(requestVoteAPI.downloadHandler.text);
        string check = voteData["message"].ToString().Replace('"', ' ');
        Debug.Log(check);

        if (check == " already voted ")
        {
            VoteResult(false);
        }
        else if (check == " submit success ")
        {
            VoteResult(true);
        }
        else
        {
            Debug.LogError("vote failed..");
        }
    }
    #endregion   
}

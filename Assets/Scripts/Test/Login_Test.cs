using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class Login_Test : MonoBehaviour
{
    public string base_url = "https://daydev.tech/api/twin/";
    public GameObject window_register, window_login, window_alert;
    public int gender_value = 1;
    public Toggle genders;
    public Dropdown gender_dropdown;
    public InputField input_email_login, input_name_login;
    public InputField input_email, input_password, input_nickname, input_bio, input_fullname;
    public string jsonData;
    private string str_email, str_name, str_nickname, str_bio, str_fullname;
    public string authen_code;
    public GameObject[] tabs;

    void Start()
    {
        window_register.SetActive(false);
        window_login.SetActive(true);
        window_alert.SetActive(false);
        authen_code = PlayerPrefs.GetString("authen_code");
        string param_authen, api_url_authen;
        param_authen = "authen_code=" + authen_code;
        api_url_authen = base_url + "user.php" + "?" + param_authen;
        if (genders != null)
        {
            genders.onValueChanged.AddListener((value) =>
            {
                HandleGenders(value);
            });
        }
        //UnityWebRequest www = new UnityWebRequest(api_url_authen);
        //StartCoroutine(FetchAuthentication(api_url_authen));
    }

    public void RegisterUser()
    {
        str_email = input_email.text;
        str_name = input_password.text;
        str_nickname = input_nickname.text;
        str_fullname = input_fullname.text;
        str_bio = input_bio.text;
        if (str_email == "" || str_name == "" || str_nickname == "" || str_fullname == "")
        {
            ShowAlert();
        }
        else
        {
            string param, api_url;
            param = "email=" + str_email + "&password=" + str_name + "&nickname=" + str_nickname + "&fullname=" + str_fullname + "&bio=" + str_bio + "&gender=" + gender_value;
            api_url = base_url + "register.php" + "?" + param;
            UnityWebRequest www = new UnityWebRequest(api_url);
            StartCoroutine(FetchResponseUser(api_url));
        }
    }

    public void HandleGenders(bool isOn)
    {
        if (isOn) gender_value = 1;
        else gender_value = 2;
    }

    public void DoLogin()
    {
        str_email = input_email_login.text;
        str_name = input_name_login.text;
        
        if (str_email == "" || str_name == "")
        {
            //ShowAlert();           
        }
        else
        {
            LoginUser(str_email, str_name);
        }
    }

    public void LoginUser(string _email, string _password)
    {
        print(_email);
        print(_password);
        string api_url;
        api_url = base_url + "login.php";
        WWWForm form = new WWWForm();
        form.AddField("email", _email);
        form.AddField("password", _password);
        StartCoroutine(FetchData(api_url, form));
    }

    public void LoginUserGet()
    {
        str_email = input_email_login.text != ""? input_email_login.text : "twinplanet_test@gmail.com";
        str_name = input_name_login.text != ""? input_name_login.text : "Test";
        gender_value = gender_dropdown.value;

        if (str_email == "" || str_name == "")
        {
            //ShowAlert();
        }

        PlayerPrefs.SetString("authen_code", authen_code);
        PlayerPrefs.SetString("email", str_email);
        PlayerPrefs.SetString("nickname", str_name);
        PlayerPrefs.SetString("gender", gender_value.ToString());
        PlayerPrefs.SetString("fullname", "Twinplanet Test");
        PlayerPrefs.SetString("bio", "Welcome to Twinplanet");
        PlayerPrefs.Save();
        //Bypass
        print(PlayerPrefs.GetString("gender"));
        if (PlayerPrefs.GetString("gender") == "1") StartCoroutine(LoadSceneObject("CharacterM"));
        else StartCoroutine(LoadSceneObject("CharacterF"));
    }

    public void LoginUserGetPassingParam(string str_email, string str_password)
    {
        if (str_email == "" || str_password == "")
        {
            ShowAlert();
        }
        else
        {
            string param, api_url;
            param = "email=" + str_email + "&password=" + str_password;
            api_url = base_url + "loginget.php" + "?" + param;
            UnityWebRequest www = new UnityWebRequest(api_url);
            StartCoroutine(FetchResponseLogin(api_url));
        }
    }

    IEnumerator FetchData(string URL, WWWForm form)
    {
        UnityWebRequest www = UnityWebRequest.Post(URL, form);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log("Success!");
            jsonData = www.downloadHandler.text;
            JSONNode jsonNode = SimpleJSON.JSON.Parse(jsonData);
            Debug.Log(jsonData);
            //StartCoroutine(LoadSceneObject("MainMenu"));
        }
    }

    IEnumerator FetchResponseLogin(string URL)
    {
        UnityWebRequest www = UnityWebRequest.Get(URL);
        yield return www.SendWebRequest();
        if (www.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log(www.error);
        }
        else
        {
            jsonData = www.downloadHandler.text;
            JSONNode jsonNode = SimpleJSON.JSON.Parse(jsonData);
            string status_code;
            status_code = jsonNode["status_code"];
            if (status_code == "200")
            {
                PlayerPrefs.SetString("authen_code", jsonNode["authen_code"]);
                PlayerPrefs.SetString("email", "twinplanet_test@gmail.com");
                PlayerPrefs.SetString("nickname", "test");
                PlayerPrefs.SetString("gender", jsonNode["gender"]);
                PlayerPrefs.SetString("fullname", "Twinplanet Test");
                PlayerPrefs.SetString("bio", "Welcome to Twinplanet");
                PlayerPrefs.Save();
                //Bypass
                print(PlayerPrefs.GetString("gender"));
                if (PlayerPrefs.GetString("gender") == "1") StartCoroutine(LoadSceneObject("CharacterM"));
                else StartCoroutine(LoadSceneObject("CharacterF"));
            }
            else
            {
                Debug.Log("Failed");
                ShowAlert();
            }
        }
    }

    IEnumerator FetchResponseUser(string URL)
    {
        UnityWebRequest www = UnityWebRequest.Get(URL);
        yield return www.SendWebRequest();
        if (www.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log(www.error);
        }
        else
        {
            jsonData = www.downloadHandler.text;
            JSONNode jsonNode = SimpleJSON.JSON.Parse(jsonData);
            string status_code;
            status_code = jsonNode["status_code"];
            if (status_code == "200")
            {
                PlayerPrefs.SetString("authen_code", jsonNode["authen_code"]);
                PlayerPrefs.SetString("email", jsonNode["email"]);
                PlayerPrefs.SetString("nickname", jsonNode["nickname"]);
                PlayerPrefs.SetString("gender", jsonNode["gender"]);
                PlayerPrefs.SetString("fullname", jsonNode["fullname"]);
                PlayerPrefs.Save();
                LoginUserGetPassingParam(input_email.text, input_password.text);
            }
            else
            {
                Debug.Log("Failed");
                ShowAlert();
            }
        }
    }

    public IEnumerator LoadSceneObject(string sceneName)
    {
        AsyncOperation async = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
        async.allowSceneActivation = false;
        while (!async.isDone)
        {
            float progress = Mathf.Clamp01(async.progress / 0.9f);
            if (progress == 1f)
            {
                async.allowSceneActivation = true;
            }
            yield return null;
        }
    }

    IEnumerator FetchAuthentication(string URL)
    {
        UnityWebRequest www = UnityWebRequest.Get(URL);
        yield return www.SendWebRequest();
        if (www.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log(www.error);
        }
        else
        {
            jsonData = www.downloadHandler.text;
            JSONNode jsonNode = SimpleJSON.JSON.Parse(jsonData);
            string status_code;
            status_code = jsonNode["status_code"];
            if (status_code == "200")
            {
                if (jsonNode["gender"] == "1") StartCoroutine(LoadSceneObject("CharacterM"));
                else StartCoroutine(LoadSceneObject("CharacterF"));
            }
            else
            {
                Debug.Log("Failed");
                ShowAlert();
            }
        }
    }

    public void ToggleLogin()
    {
        window_register.SetActive(false);
        window_login.SetActive(true);
    }
    public void ToggleRegister()
    {
        window_register.SetActive(true);
        window_login.SetActive(false);
        NextTab(0);
    }

    public void ShowAlert()
    {
        window_alert.SetActive(true);
    }

    public void closeAlertWindows()
    {
        window_alert.SetActive(false);
    }

    public void OpenWeb(string website)
    {
        Application.OpenURL(website);
    }

    public void NextTab(int tabNo)
    {
        foreach (GameObject go in tabs)
        {
            go.SetActive(false);
        }
        tabs[tabNo].SetActive(true);
    }
}

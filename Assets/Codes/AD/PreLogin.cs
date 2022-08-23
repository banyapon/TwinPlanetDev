using AD.JS;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class PreLogin : MonoBehaviour
{
	private string baseUrl = "https://twin-planet.com/cms/";
    public string mytoken;
	public string jsonString;
	public bool saveUser = false;
	public A5ListUserData listuser;

	void Start()
    {
		UserData.Instance.token = "8F_O3F4_h18vf92s6pmvY-mauM8yOpojGs8dSuZoSVl.Qf2UDM3kDO4QjNxojIwhXZiwiN1ATNwMjN0YTM6ICdhlmIsETM6ICZpJye.9JCVXpkI6ICc5RnIsIiN1IzUIJiOicGbhJye";
#if UNITY_WEBGL && !UNITY_EDITOR
        UserData.Instance.token = Cookie.Token();
#endif
        mytoken = UserData.Instance.token;
    }

    void Update()
    {
        if (mytoken != null && saveUser == false) 
        {
            print("Process Token");
			saveUser = true;
			StartCoroutine(PostMe());           
        }
        //print(Reverse(mytoken));
    }

    public static string Reverse(string s)
    {
        char[] charArray = s.ToCharArray();
        Array.Reverse(charArray);
        return new string(charArray);
    }

	public void MeResponse(string data) 
	{
		print(data);
		// process other
		A5User user = JsonUtility.FromJson<A5User>(data);
		if (user != null) 
		{
			UserData.Instance.me = user;
			UserData.Instance.userId = user.id;
			print(UserData.Instance.userId);
			print("Process Get User by ID : " + user.id);
			StartCoroutine(PostUser());
		}

	}

	public void UserResponse(string data)
	{
		print(data);

		listuser = JsonUtility.FromJson<A5ListUserData>(data);
		if (listuser != null) 
		{
			print(listuser.data[0]);
			UserData.Instance.user = listuser.data[0];
			if (UserData.Instance.user.attributes.gender == "Male")
			{
				PlayerPrefs.SetString("gender", "1");
			}
			else 
			{
				PlayerPrefs.SetString("gender", "2");
			}
			PlayerPrefs.SetString("authen_code", Reverse(mytoken));
			PlayerPrefs.SetString("email", UserData.Instance.me.email);
			PlayerPrefs.SetString("nickname", UserData.Instance.me.username);
			PlayerPrefs.SetString("img", UserData.Instance.user.attributes.profile_image_url);
			//PlayerPrefs.SetString("gender", jsonNode["gender"]);
			PlayerPrefs.SetString("fullname", "");
			PlayerPrefs.SetString("bio", UserData.Instance.user.attributes.biography);
			PlayerPrefs.Save();
			if (PlayerPrefs.GetString("gender") == "1") StartCoroutine(LoadSceneObject("CharacterM"));
			else StartCoroutine(LoadSceneObject("CharacterF"));

		}

	}

	public IEnumerator PostMe()
	{
		string token = Reverse(mytoken);
		UnityWebRequest www = UnityWebRequest.Get(baseUrl + "api/users/me");
		www.SetRequestHeader("Authorization", "Bearer " + token);
		www.SetRequestHeader("Content-Type", "application/json");
		yield return www.SendWebRequest();


		// yield return www.SendWebRequest();

		if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
		{
			Debug.Log(www.error);
			Debug.Log(www.downloadHandler.text);
		}
		else
		{
			// Debug.Log(www.downloadHandler.text);
			jsonString = www.downloadHandler.text;
			MeResponse(www.downloadHandler.text);
		}
	}

	public IEnumerator PostUser()
	{
		string token = Reverse(mytoken);

		UnityWebRequest www = UnityWebRequest.Get(baseUrl + "api/user-informations?populate=user&filters[user][id][$eq]=" + UserData.Instance.userId);
		www.SetRequestHeader("Authorization", "Bearer " + token);
		www.SetRequestHeader("Content-Type", "application/json");
		yield return www.SendWebRequest();


		// yield return www.SendWebRequest();

		if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
		{
			Debug.Log(www.error);
			Debug.Log(www.downloadHandler.text);
		}
		else
		{
			// Debug.Log(www.downloadHandler.text);
			jsonString = www.downloadHandler.text;
			UserResponse(www.downloadHandler.text);
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
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserData
{
    public string version = "1.00";
    private static UserData instance;
    public static UserData Instance
    {
        get
        {
            //PlayerPrefs.DeleteAll();
            if (instance == null) instance = new UserData();
            return instance;
        }
    }
    public string token;
    public int userId;
    public A5User me;
    public A5UserData user;
    public int genderId;
}
//{"id":3,"username":"strikergtr","email":"cotton989@gmail.com","provider":"local","confirmed":true,"blocked":false,"createdAt":"2022-02-28T06:35:04.820Z","updatedAt":"2022-02-28T06:35:04.820Z"}
[System.Serializable]
public class A5User
{
    public int id;
    public string username;
    public string email;
}

[System.Serializable]
public class A5UserData
{
    public int id;
    public A5Personal attributes;

}

[System.Serializable]
public class A5Personal
{
    public string gender;
    public string glb_url;
    public string profile_image_url;
    public string biography;

}

[System.Serializable]
public class A5ListUserData
{
    public List<A5UserData> data;
}

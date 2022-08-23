using AD;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultSelect : MonoBehaviour
{
    public CharacterEditor characterEditor;
    public GenderSelect gender;
    public enum GenderSelect { Male, Female }

    void Start()
    {
        if (gender == GenderSelect.Male)
        {
            characterEditor.SetGroup(0);
        }
        else 
        {
            characterEditor.SetGroup(0);
        }
    }

    void Update()
    {
        
    }
}

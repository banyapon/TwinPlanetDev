using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu_CharacterPage : MonoBehaviour
{
    [SerializeField] private GameObject[] _page;
    private int _index = 0;

    void Start()
    {
        Show(_index);
    }

    public void NextPage()
    {
        if (_index >= _page.Length - 1)
            _index = 0;
        else
            _index++;
        Show(_index);
    }

    public void BackPage()
    {
        if (_index <= 0)
            _index = _page.Length - 1;
        else
            _index--;
        Show(_index);
    }

    void Show(int index)
    {
        for (int i = 0; i < _page.Length; i++)
        {
            _page[i].SetActive(false);
        }
        _page[index].SetActive(true);
    }
}

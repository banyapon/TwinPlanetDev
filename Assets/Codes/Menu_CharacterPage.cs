using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu_CharacterPage : MonoBehaviour
{
    [SerializeField] private GameObject[] _page;
    [SerializeField] private GameObject _pageView;
    [SerializeField] private Sprite[] _onPage;
    [SerializeField] private Transform _parentPos;
    [SerializeField] private int _index = 0;
    private List<Image> _indexPageImg = new List<Image>();

    void Awake()
    {
        for (int i = 0; i < _page.Length; i++)
        {
            GameObject GO = Instantiate(_pageView.gameObject, _parentPos);
            _indexPageImg.Add(GO.GetComponent<Image>());
        }
    }

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
            _indexPageImg[i].sprite = _onPage[0];
        }
        _page[index].SetActive(true);
        _indexPageImg[index].sprite = _onPage[1];
    }
}

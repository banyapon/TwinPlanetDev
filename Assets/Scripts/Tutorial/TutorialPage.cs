using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialPage : MonoBehaviour
{
    public static bool isFirstLogin = true;

    [SerializeField] private Image _tutorialPageImage;
    [SerializeField] private Sprite[] _page;

    int index = 0;

    void Start()
    {
        if (isFirstLogin) OnShowTutorial();
    }

    public void OnShowTutorial()
    {
        isFirstLogin = false;
        _tutorialPageImage.gameObject.SetActive(true);
        Show(index);
    }

    public void OnCloseTutorial()
    {
        _tutorialPageImage.gameObject.SetActive(false);
    }

    public void ChangePage(bool isNext)
    {
        if (isNext)
        {
            if (index >= _page.Length - 1) OnCloseTutorial();
            else index++;
        }
        else
        {
            if (index <= 0) return;
            else index--;
        }
        Show(index);
    }

    void Show(int _index)
    {
        _tutorialPageImage.sprite = _page[_index];
    }

}

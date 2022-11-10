using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using ReadyPlayerMe;

public class CharacterCreate : MonoBehaviour
{
    private const string TAG = nameof(WebAvatarLoader);   
    [SerializeField] private string avatarUrl = "";

    [Header("Player Character")]
    [SerializeField] private Transform _playerPos;

    [Header("Animator")]
    [SerializeField] private Animator _animator;
    [SerializeField] private RuntimeAnimatorController _animatorController;

    private GameObject avatar;

    [SerializeField] private bool isEdit;

    private void Start()
    {
        PartnerSO partner = Resources.Load<PartnerSO>("Partner");
        WebInterface.SetupRpmFrame(partner.Subdomain);

        if (isEdit) OnWebViewAvatarGenerated(avatarUrl);
    }

    public void OnWebViewAvatarGenerated(string generatedUrl)
    {
        var avatarLoader = new AvatarLoader();
        avatarUrl = generatedUrl;
        avatarLoader.OnCompleted += OnAvatarLoadCompleted;
        avatarLoader.OnFailed += OnAvatarLoadFailed;
        avatarLoader.LoadAvatar(avatarUrl);
        PlayerPrefs.SetString("Avatar", avatarUrl);
    }

    public void LoadAvatar(string avatarUrl) => OnWebViewAvatarGenerated(avatarUrl);

    private void OnAvatarLoadCompleted(object sender, CompletionEventArgs args)
    {
        if (avatar) Destroy(avatar);
        avatar = args.Avatar;

        //Set to parent position
        avatar.transform.SetParent(_playerPos);
        avatar.transform.localPosition = Vector3.zero;
        avatar.transform.localEulerAngles = Vector3.zero;

        //Remove animator component
        Destroy(avatar.GetComponent<Animator>());
        _animator.runtimeAnimatorController = _animatorController;
    }

    private void OnAvatarLoadFailed(object sender, FailureEventArgs args)
    {
        SDKLogger.Log(TAG, $"Avatar Load failed with error: {args.Message}");
    }
}

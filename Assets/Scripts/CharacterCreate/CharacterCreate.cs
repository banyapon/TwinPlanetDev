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
    [SerializeField] private RuntimeAnimatorController _animatorController;
    [SerializeField] private Avatar _animatorAvatar;

    private GameObject avatar;

    private void Start()
    {
        PartnerSO partner = Resources.Load<PartnerSO>("Partner");
        WebInterface.SetupRpmFrame(partner.Subdomain);

        OnWebViewAvatarGenerated(avatarUrl);
    }

    public void OnWebViewAvatarGenerated(string generatedUrl)
    {
        var avatarLoader = new AvatarLoader();
        avatarUrl = generatedUrl;
        avatarLoader.OnCompleted += OnAvatarLoadCompleted;
        avatarLoader.OnFailed += OnAvatarLoadFailed;
        avatarLoader.LoadAvatar(avatarUrl);
    }

    private void OnAvatarLoadCompleted(object sender, CompletionEventArgs args)
    {
        if (avatar) Destroy(avatar);
        avatar = args.Avatar;
        Animator animator = avatar.GetComponent<Animator>();

        //Set to parent position
        avatar.transform.SetParent(_playerPos);
        avatar.transform.localPosition = Vector3.zero;
        avatar.transform.localEulerAngles = Vector3.zero;

        //Assign animator component
        animator.runtimeAnimatorController = _animatorController;
        animator.avatar = _animatorAvatar;
    }

    private void OnAvatarLoadFailed(object sender, FailureEventArgs args)
    {
        SDKLogger.Log(TAG, $"Avatar Load failed with error: {args.Message}");
    }
}

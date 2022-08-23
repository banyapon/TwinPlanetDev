// Gameplay System V1.4 By VF
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.EventSystems;
using TMPro;
using Photon.Pun;
using UnityEngine.UI;
using AD.Network;
using UnityEngine.SceneManagement;
using UnityEngine.AI;
using System;

namespace AD
{
    public class GameplaySystem : MonoBehaviour
    {
        public static GameplaySystem main;
        public RectTransform menu;
        public GameObject popup;
        public TextMeshProUGUI popupOld;
        public Text popupTMP;
        public Text NamepopupTMP;
        private string privateMap = "VIPRoom";
        public Chair[] chairs;
        public GameObject roomMasterPanel;

        //Profile Select
        public Text targetName;
        public Text targetFull;
        public Text targetBio;

        private bool done = false;

        public GameObject panelWarpOff, panelWarpOn, panelEmoOff, panelEmoOn, panelInfo;

        [HideInInspector] public CharacterControl characterControl;
        [HideInInspector] CharacterControl menuCharacterControl;
        [HideInInspector] CharacterControl contactCharacterControl;
        [HideInInspector] CharacterControl.Command contactCommand;
        [HideInInspector] int contactData;
        [HideInInspector] string contactName;

        public bool isOverUI => EventSystem.current != null ? EventSystem.current.IsPointerOverGameObject(-1) : false;

        private void Awake()
        {
            if (main == null) main = this;
            else Destroy(gameObject);
            PhotoboothController.OnPlayerInRoom += HandlePlayerInRoom;
            NetworkManager.OnPlayerIsMaster += HandlePlayerIsMaster;
            PhotoboothController.OnBgChanged += HandleBGChanged;
            ChatM.OnSentMessage += HandleOnMessage;
        }
        private void OnDestroy()
        {
            PhotoboothController.OnPlayerInRoom -= HandlePlayerInRoom;
            NetworkManager.OnPlayerIsMaster -= HandlePlayerIsMaster;
            PhotoboothController.OnBgChanged -= HandleBGChanged;
            ChatM.OnSentMessage -= HandleOnMessage;
        }
        private void HandleBGChanged(int index)
        {
            characterControl.photonView.RPC("ChangeBackgroundPhotobooth", RpcTarget.All, index);
        }
        private void HandlePlayerIsMaster(bool value)
        {
            roomMasterPanel.SetActive(value);
            print(characterControl);
            if (value)
            {
                Vector3 myPos = new Vector3(1.39f, 0, -0.57f);
                Vector3 myEulerAngles = new Vector3(0, -25.498f, 0);
                characterControl.photonView.RPC("CharacterTelepot", RpcTarget.All, myPos, myEulerAngles);
            }
            else
            {
                Vector3 myPos = new Vector3(0.46f, 0, -0.6f);
                Vector3 myEulerAngles = new Vector3(0, 2.46f, 0);
                characterControl.photonView.RPC("CharacterTelepot", RpcTarget.All, myPos, myEulerAngles);
            }
        }
        private void HandlePlayerInRoom()
        {
            print("do");
            NetworkManager.main.CheckMaster();
        }
        private void HandleOnMessage(string newMessage){
            Debug.Log(newMessage);
            characterControl.photonView.RPC("SentMessage",RpcTarget.All, characterControl.nickname, newMessage);
            Debug.Log("OnMessage");
        }
        public void FullScreen()
        {
            Screen.fullScreen = !Screen.fullScreen;
        }
        public void Teleport(string mapname)
        {
            //Time.timeScale = 1;
            NetworkManager.main.spawnPoint = Vector3.zero;
            NetworkManager.main.Join(mapname);
        }
        private void Update()
        {
            if (Input.GetButtonDown("Fire1")&&!miniGame) if (!isOverUI) HideMenu();
            if (miniGame) CountTime();
        }
        public void Start()
        {
            /*panelWarpOff.SetActive(true);
            panelWarpOn.SetActive(false);
            panelEmoOff.SetActive(true);
            panelEmoOn.SetActive(false);
            panelInfo.SetActive(false);*/
        }
        public void ToggleEmotion(bool stage) 
        {
            if (stage)
            {
                panelEmoOff.SetActive(false);
                panelEmoOn.SetActive(true);
            }

            else 
            {
                panelEmoOff.SetActive(true);
                panelEmoOn.SetActive(false);
            }
        }
        public void ToggleWarp(bool stage)
        {
            if (stage)
            {
                panelWarpOff.SetActive(false);
                panelWarpOn.SetActive(true);
            }

            else
            {
                panelWarpOff.SetActive(true);
                panelWarpOn.SetActive(false);
            }
        }
        public void ToggleInfo(bool stage)
        {
            if (stage)
            {
                panelInfo.SetActive(true);
            }

            else
            {
                panelInfo.SetActive(false);
            }
            HideMenu();
        }
        public void ShowMenu(int viewID)
        {
            menuCharacterControl = FindCharacterControl(viewID);
            menu.position = Input.mousePosition;
            menu.gameObject.SetActive(true);
        }
        public void ClickPrivateRoom() => ClickCommand(CharacterControl.Command.PrivateRoom, UnityEngine.Random.Range(10000, 99999));
        public void ClickPhotoBooth() => ClickCommand(CharacterControl.Command.Photobooth, 1, menuCharacterControl.nickname);
        public void ClickMarry() => ClickCommand(CharacterControl.Command.Marry);
        public void ClickHug() => ClickCommand(CharacterControl.Command.Hug);
        public void ClickCommand(CharacterControl.Command command, int data = 0,string name = "")
        { 
            menuCharacterControl.photonView.RPC("Request", RpcTarget.All, characterControl.photonView.ViewID, command, data,name);
            HideMenu();
        }
        public void ClickProfile()
        {
            targetName.text = menuCharacterControl.nickname;
            targetFull.text = menuCharacterControl.fullname;
            targetBio.text = menuCharacterControl.bio;
            HideMenu();
        }
        public void WarpToPhotobooth()
        {
            Vector3 myPos = new Vector3(1.39f, 0, -500.57f);
            Quaternion myEulerAngles = new Quaternion(0, 3.723f, 0, 0);
            characterControl.photonView.RPC("CharacterTelepot", RpcTarget.All, myPos, myEulerAngles);
        }
        public void HideMenu()
        {
            if (menu.gameObject.activeSelf)
            {
                menu.gameObject.SetActive(false);
            }
        }
        public void RequestReceive(int fromViewID, CharacterControl.Command command, int data,string name)
        {
            if (contactCharacterControl != null) return;
            contactCharacterControl = FindCharacterControl(fromViewID);
            contactCommand = command;
            contactData = data;
            contactName = name;
            NamepopupTMP.text = contactCharacterControl.nameTMP.text;
            switch (command)
            {
                case CharacterControl.Command.PrivateRoom:                    
                    popupTMP.text = "ต้องการชวนคุณเข้าห้องส่วนตัว";
                    break;
                case CharacterControl.Command.Marry:
                    popupTMP.text = "ต้องการขอแต่งงานกับคุณ";
                    break;
                case CharacterControl.Command.Hug:
                    popupTMP.text = "ต้องการขอกอดกับคุณ";
                    break;
                case CharacterControl.Command.Photobooth:
                    popupTMP.text = "ต้องการชวนคุณไปถ่ายรูป";
                    break;
            }
            popup.SetActive(true);
        }
        public void RequestAccept()
        {
            if (contactCharacterControl == null) return;
            if (characterControl.animationNow == CharacterControl.AnimationName.Sit) return;
            if (contactCharacterControl.animationNow == CharacterControl.AnimationName.Sit) return;
            if (characterControl.characterEditor.GetTopIndex() == 10) return;
            if (contactCharacterControl.characterEditor.GetTopIndex() == 10) return;

            switch (contactCommand)
            {
                case CharacterControl.Command.PrivateRoom:
                    contactCharacterControl.photonView.RPC("CharacterTelepotToMap", RpcTarget.Others, privateMap, contactData);
                    Network.NetworkManager.main.Join(privateMap, 1, contactName, 10);
                    break;
                case CharacterControl.Command.Marry:
                    PlayCoupleAnimation(CharacterControl.AnimationName.Marry, 1.5f);
                    break;
                case CharacterControl.Command.Hug:
                    //PhotoBooth(CharacterControl.AnimationName.Hug, 0.2f);
                    PlayCoupleAnimation(CharacterControl.AnimationName.Hug, 0.4f);
                    break;
                case CharacterControl.Command.Photobooth:
                    print(contactData);
                    contactCharacterControl.photonView.RPC("CharacterTelepotToMap", RpcTarget.Others, "Photobooth", 1, contactName);
                    StartCoroutine(waitsec());
                    //PhotoBooth();
                    break;
            }
            RequestCancel();
        }
        IEnumerator waitsec()
        {
            yield return new WaitForSeconds(1.0f);
            Network.NetworkManager.main.Join("Photobooth", 1, contactName, 2);
        }
        public void RequestCancel()
        {
            contactCharacterControl = null;
            contactCommand = CharacterControl.Command.None;
            contactData = 0;
            popup.SetActive(false);
        }
        public void PlayAnimationExcited() => characterControl.PlayAnimationExcited();
        public void PlayAnimationHug() => characterControl.PlayAnimationHug();
        public void PlayAnimationSit() => characterControl.PlayAnimationSit();
        public void PlayAnimationGiving() => characterControl.PlayAnimationGiving();
        public void PlayAnimationLaugh() => characterControl.PlayAnimationLaugh();
        public void PlayAnimationHi() => characterControl.PlayAnimationHi();
        public void PlayAnimationMarry() => characterControl.PlayAnimationMarry();
        private void PlayCoupleAnimation(CharacterControl.AnimationName animationName, float distance = 1)
        {
            Vector3 myEulerAngles = LookAt(characterControl.transform.position, contactCharacterControl.transform.position);
            Vector3 contactEulerAngles = LookAt(contactCharacterControl.transform.position, characterControl.transform.position);
            contactCharacterControl.photonView.RPC("CharacterTelepot", RpcTarget.All, contactCharacterControl.transform.position, contactEulerAngles);
            characterControl.photonView.RPC("CharacterTelepot", RpcTarget.All, contactCharacterControl.transform.position + (contactCharacterControl.transform.forward * distance), myEulerAngles);
            contactCharacterControl.photonView.RPC("PlayAnimation", RpcTarget.All, animationName);
            characterControl.photonView.RPC("PlayAnimation", RpcTarget.All, animationName);
            if (animationName == CharacterControl.AnimationName.Marry) 
            {
                StartCoroutine(DefaultCloth(characterControl, characterControl.characterEditor.GetBottomIndex()));
                StartCoroutine(DefaultCloth(contactCharacterControl, contactCharacterControl.characterEditor.GetBottomIndex()));
                contactCharacterControl.photonView.RPC("ChangeClothSet", RpcTarget.All, 10);
                characterControl.photonView.RPC("ChangeClothSet", RpcTarget.All, 10);
                
            }
        }
        IEnumerator DefaultCloth(CharacterControl characterControl, int groupIndex)
        {
            yield return new WaitForSeconds(13);
            characterControl.photonView.RPC("ChangeClothSet", RpcTarget.All, groupIndex);

        }
        public void LeaveRoom()
        {
            NetworkManager.main.Clicked_Leaveroom();
        }
        public Chair FindChair(int index) => chairs[index];
        public int FindChairIndex(Chair chair)
        {
            for (int i = 0; i < chairs.Length; i++)
            {
                if (chair == chairs[i]) return i;
            }
            return -1;
        }
        public CharacterControl FindCharacterControl(int viewID)
        {
            GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("Player");
            foreach (GameObject go in gameObjects)
            {
                CharacterControl characterControl = go.GetComponent<CharacterControl>();
                if (characterControl.photonView.ViewID == viewID) return characterControl;
            }
            return null;
        }
        private Vector3 LookAt(Vector3 from, Vector3 to)
        {
            Vector3 lookPosition = to - from;
            lookPosition.y = 0;
            return Quaternion.LookRotation(lookPosition).eulerAngles;
        }

        //Minigame---------------------------------------------------------------------------------------------------------------------------
        [SerializeField] private GameObject oldCamera;
        [SerializeField] private GameObject heart;
        [SerializeField] public Transform heartParent;
        [SerializeField] private Text txt_time;
        [SerializeField] private GameObject endPanel;
        [SerializeField] private Text ui_score;
        [SerializeField] private Text ui_scoreEnd;
        [SerializeField] private GameObject panelRespawn;
        public static Action OnPlayerDead = delegate { };
        public int score;
        private bool miniGame;
        private float timer = 180f;
        private GameObject newCam;

        public void TeleportToMinigame()
        {
            NetworkManager.main.spawnPoint = Vector3.zero;
            NetworkManager.main.Join("MiniGame", 0, "", 10);
        }
        private void CountTime()
        {
            if (timer > 0)
            {
                timer -= Time.deltaTime;
                txt_time.text = Convert.ToInt32(timer).ToString();
            }
            else
            {
                EndGame();
            }
        }
        public void EndGame()
        {
            if (!done)
            {
                done = true;
                endPanel.SetActive(true);
                ui_scoreEnd.text = ui_score.text;
                Cursor.lockState = CursorLockMode.None;
                //newCam.transform.GetChild(1).gameObject.SetActive(false);
                DataRanking.instance.SetScore(score);
                OnPlayerDead.Invoke();
            }

            /*characterControl.GetComponent<CharacterControl>().enabled = true;
            characterControl.GetComponent<NavMeshAgent>().enabled = true;
            characterControl.GetComponent<PlayerShoot>().enabled = false;
            characterControl.GetComponent<PlayerController>().enabled = false;
            characterControl.GetComponent<Rigidbody>().useGravity = false;
            characterControl.GetComponent<CharacterController>().enabled = false;*/
            //Time.timeScale = 0;
        }
        public void GetScore(int getscore)
        {
            score += getscore;
            ui_score.text = score.ToString();
        }
        public void Respawn(bool panel)
        {
            if (panel)
            {
                for (int i = 0; i < 15; i++)
                {
                    heartParent.GetChild(i).gameObject.SetActive(true);
                }
            }
            panelRespawn.SetActive(panel);
        }
        public void SetupMiniGame(bool myCamera)
        {
            miniGame = true;
            txt_time.gameObject.SetActive(true);
            ui_score.gameObject.SetActive(true);
            heartParent.gameObject.SetActive(true);
            characterControl.gameObject.layer = LayerMask.NameToLayer("HIDE");
            characterControl.GetComponent<PlayerShoot>().enabled = true;
            characterControl.GetComponent<NavMeshAgent>().enabled = false;
            characterControl.GetComponent<Rigidbody>().useGravity = true;
            characterControl.GetComponent<CharacterController>().enabled = true;
            characterControl.photonView.RPC("SetupMiniGame", RpcTarget.All);
            if (oldCamera != null)
            {
                oldCamera.SetActive(false);
            }
            for (int i = 0; i < characterControl.transform.childCount; i++)
            {
                if (characterControl.transform.GetChild(i).GetComponent<Camera>() != null)
                {
                    characterControl.transform.GetChild(i).gameObject.SetActive(myCamera);
                    newCam = characterControl.transform.GetChild(i).gameObject;
                }
            }
            if (characterControl.GetComponent<PhotonView>().IsMine)
            {
                SetLayerRecursively(characterControl.gameObject, LayerMask.NameToLayer("HIDE"));
            }
        }
        void SetLayerRecursively(GameObject obj, int newLayer)
        {
            if (null == obj)
            {
                return;
            }

            obj.layer = newLayer;

            foreach (Transform child in obj.transform)
            {
                if (null == child)
                {
                    continue;
                }
                if (!child.GetComponent<Camera>())
                {
                    SetLayerRecursively(child.gameObject, newLayer);
                }
            }
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(GameplaySystem), true)]
    public class GameplaySystemEditor : Editor
    {
        protected GameplaySystem gameplaySystem;
        private void OnEnable() => gameplaySystem = (GameplaySystem)target;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("SetupChairs")) { SetupChairs(); }
        }

        private void SetupChairs()
        {
            GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("Chair");
            gameplaySystem.chairs = new Chair[gameObjects.Length];
            for (int i = 0; i < gameObjects.Length; i++) { gameplaySystem.chairs[i] = gameObjects[i].GetComponent<Chair>(); }
        }
    }
#endif
}
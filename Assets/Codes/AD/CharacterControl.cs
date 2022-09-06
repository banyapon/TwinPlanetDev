// Character Control V1.7 By VF
// Character Control Plus Sound By ST
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using Photon.Chat;
using UnityEngine.EventSystems;
using FrostweepGames.VoicePro;
using FrostweepGames.Plugins.Native;
using System;
using UnityEngine.SceneManagement;
using Photon.Pun.Demo.PunBasics;

namespace AD
{
    public class CharacterControl : MonoBehaviourPunCallbacks
    {
        public TextMeshPro nameTMP;
        public Animator animator;
        public NavMeshAgent agent;
        public new PhotonView photonView; //
        public CharacterEditor characterEditor;
        [HideInInspector] public GameObject flower;
        [HideInInspector] public string micName;
        [HideInInspector] public ToggleMic playerMicToggle;
        private new Camera camera;
        [HideInInspector] public AnimationName animationNow = AnimationName.Idle;
        private int animationCount;
        private Chair chair;
        private Vector2 mouseDownPosition;
        private bool holdingFlower;
        private ChatClient chatClient;
        [HideInInspector] public Recorder recorder;
        [HideInInspector] public Listener listener;
        [HideInInspector] public string nickname, gender, fullname, bio;
        public static Action<int> OnBackgroundChange = delegate { };


        #region NEW PLAYER CONTROLLER FROM AEAMETAVERSE
        FixedJoystick _joystick;
        private CharacterController _characterController;
        private Vector3 _playerVelocity;
        private float playerSpeed = 3.0f;
        [Range(0.1f,0.5f)] [SerializeField] private float _turnSmoothTime = 0.2f;
        private float _turnSmoothVelocity;
        private float gravityValue = 10f;
        [SerializeField] private GameObject _orientation;
        [SerializeField] private bool isWalk = false;
        private bool isSit = false;
        [HideInInspector] public bool isStop = false;
        public enum CameraType { OLD_CAMERA, NEW_CAMERA };
        public CameraType cameraType;
        #endregion

        public enum AnimationName { Idle, Walk, Excited, Giving, Hug, Sit, Laugh, Hi, Marry }

        void Awake()
        {
            if (!PhotonNetwork.InRoom) { this.enabled = false; return; }
            camera = Camera.main;
            if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("MiniGame")) //MINI GAME CONTROLLER
            {
                GetComponent<PlayerController>().enabled = true;
                GameplaySystem.main.characterControl = this;
                if (photonView.IsMine)
                { 
                    GameplaySystem.main.SetupMiniGame(true);
                    LoadPlayerData();
                }
                else GameplaySystem.main.SetupMiniGame(false);
                Cursor.lockState = CursorLockMode.Locked;
                
                this.enabled = false;
            }
            else if (camera == null) camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        }

        private void Start()
        {
            _characterController = gameObject.GetComponent<CharacterController>();
            if (photonView.IsMine)
            {
                gameObject.AddComponent<AudioListener>();
                if(cameraType == CameraType.OLD_CAMERA) camera.GetComponent<CameraControl>().Setup(transform);
                recorder = GameObject.FindObjectOfType<Recorder>();
                listener = GameObject.FindObjectOfType<Listener>();
                RefreshMicrophonesButtonOnClickHandler();
                recorder.StopRecord();
                playerMicToggle = GameObject.FindObjectOfType<ToggleMic>();

                #region AEAMETAVERSE
                _characterController.enabled = true;
                if (SystemInfo.deviceType == DeviceType.Handheld)
                {
                    if (_joystick == null)
                    {
                        GameObject joystick = GameObject.FindGameObjectWithTag("joystick") as GameObject;
                        _joystick = joystick.GetComponent<FixedJoystick>();
                    }
                }
                //CAMERA WORK ASSIGN
                #endregion

                //nameTMP.text = "" + photonView.ViewID;
                LoadPlayerData();               
            }
            else
            {
                _characterController.enabled = false;
            }

            if(cameraType == CameraType.NEW_CAMERA)
            {
                var _cameraWork = gameObject.GetComponent<CameraWork>();
                if (_cameraWork != null)
                {
                    if (photonView.IsMine)
                    {
                        _cameraWork.OnStartFollowing();
                    }
                }
                else
                {
                    Debug.LogError("<Color=Red><b>Missing</b></Color> CameraWork Component on player Prefab.", this);
                }
            }
        }

        public void LoadPlayerData()
        {
            nickname = PlayerPrefs.GetString("nickname");
            fullname = PlayerPrefs.GetString("fullname");
            gender = PlayerPrefs.GetString("gender");
            bio = PlayerPrefs.GetString("bio");
            nameTMP.text = nickname; 
            characterEditor.Load();
            photonView.RPC("CharacterData", RpcTarget.OthersBuffered, nickname, fullname, gender, bio, characterEditor.GetHatIndex(), characterEditor.GetFaceIndex(), characterEditor.GetGlassesIndex(), characterEditor.GetBodyIndex(), characterEditor.GetTopIndex(), characterEditor.GetBottomIndex(), characterEditor.GetFootwearIndex());
        }

        void Update()
        {
            if (photonView.IsMine)
            {
                //CharactorInput();
                NewCharacterInput(); //AEA METAVERSE VERSION
                ClickInput();
                StandUp();
            }
            CharactorAnimation();
        }

        private void LateUpdate() => nameTMP.transform.rotation = camera.transform.rotation;

        private void GetMicStage()
        {
            ToggleMic(playerMicToggle.toggleStage);
        }

        private void CharactorInput()
        {
            if (playerMicToggle != null) GetMicStage();
            if (Input.GetButtonDown("Fire1")) mouseDownPosition = Input.mousePosition;
            if (Input.GetButtonUp("Fire1"))
            {
                if (GameplaySystem.main.isOverUI) return;
                if (Vector2.Distance(mouseDownPosition, Input.mousePosition) > 70) return;
                RaycastHit hit;
                Ray ray = camera.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.transform.tag == "Player")
                    {
                        Debug.Log("hit");
                        if (Vector3.Distance(transform.position, hit.transform.position) < 1.5f)
                        {
                            PhotonView view = hit.transform.GetComponent<PhotonView>();
                            if (view.ViewID != photonView.ViewID)
                            {
                                GameplaySystem.main.ShowMenu(view.ViewID);
                            }
                        }
                    }
                    else if (hit.transform.tag == "Chair")
                    {
                        if (Vector3.Distance(transform.position, hit.transform.position) < 2f)
                        {
                            if (animationNow != AnimationName.Sit)
                            {

                                Chair chair = hit.transform.GetComponent<Chair>();
                                if (chair.characterControl == null)
                                {
                                    if (animationNow != AnimationName.Idle && animationNow != AnimationName.Walk) return;
                                    photonView.RPC("SitAt", RpcTarget.All, GameplaySystem.main.FindChairIndex(chair));
                                }
                            }

                        }
                        else
                        {
                            MoveTo(hit.point);
                            photonView.RPC("MoveTo", RpcTarget.Others, hit.point);
                        }
                    }
                    else
                    {
                        MoveTo(hit.point);
                        photonView.RPC("MoveTo", RpcTarget.Others, hit.point);
                    }

                }
            }

            if (Input.GetKeyDown(KeyCode.P))
            {
                //characterEditor.Default();
                //characterEditor.Save();
                characterEditor.SetGroup(10);
                photonView.RPC("CharacterData", RpcTarget.OthersBuffered, nameTMP.text, characterEditor.GetHatIndex(), characterEditor.GetFaceIndex(), characterEditor.GetGlassesIndex(), characterEditor.GetBodyIndex(), characterEditor.GetTopIndex(), characterEditor.GetBottomIndex(), characterEditor.GetFootwearIndex());
            }
            if (animationNow != AnimationName.Idle && animationNow != AnimationName.Walk) return;
            /*         if (Input.GetKeyDown(KeyCode.Alpha1)) PlayAnimationExcited();
                     if (Input.GetKeyDown(KeyCode.Alpha2)) PlayAnimationSit();
                     if (Input.GetKeyDown(KeyCode.Alpha3)) PlayAnimationMarry();
                     if (Input.GetKeyDown(KeyCode.Alpha4)) PlayAnimationGiving();
                     if (Input.GetKeyDown(KeyCode.Alpha5)) PlayAnimationLaugh();
                     if (Input.GetKeyDown(KeyCode.Alpha6)) PlayAnimationHi();
                     if (Input.GetKeyDown(KeyCode.Alpha7)) PlayAnimationHug();
                     if (Input.GetKeyDown(KeyCode.F)) { holdingFlower = !holdingFlower; flower.SetActive(holdingFlower); }
                     if (Input.GetKeyDown(KeyCode.Space)) ToggleMic(true);
                     if (Input.GetKeyUp(KeyCode.Space)) ToggleMic(false);*/
        }

        private void NewCharacterInput()
        {
            if (photonView.IsMine)
            {
                if (isSit || isStop) return;
                Vector3 move;
                
                if (Input.GetAxis("Vertical") != 0.0f || Input.GetAxis("Horizontal") != 0.0f) //INPUT
                {                  
                    move = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
                }
                else
                {
                    if (_joystick == null) return;
                    move = new Vector3(_joystick.Horizontal, 0, _joystick.Vertical).normalized;
                }

                if (move != Vector3.zero)
                {
                    //CHARACTER ROTATION
                    float targetAngle = Mathf.Atan2(move.x, move.z) * Mathf.Rad2Deg + camera.transform.eulerAngles.y;
                    float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _turnSmoothVelocity, _turnSmoothTime);
                    transform.rotation = Quaternion.Euler(0f, angle, 0f);
                    Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

                    //MOVEMENT
                    _characterController.Move(moveDir.normalized * Time.deltaTime * playerSpeed);

                    //gameObject.transform.forward = _moveDirection;
                    isWalk = true;
                }
                else
                {
                    isWalk = false;
                }

                _playerVelocity.y -= gravityValue * Time.deltaTime;
                _characterController.Move(_playerVelocity * Time.deltaTime);
            }
        }

        private void ClickInput()
        {
            if (Input.GetButtonDown("Fire1")) mouseDownPosition = Input.mousePosition;
            if (Input.GetButtonUp("Fire1"))
            {
                if (GameplaySystem.main.isOverUI) return;
                if (Vector2.Distance(mouseDownPosition, Input.mousePosition) > 70) return;
                RaycastHit hit;
                Ray ray = camera.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.transform.tag == "Player")
                    {
                        Debug.Log("hit");
                        if (Vector3.Distance(transform.position, hit.transform.position) < 1.5f)
                        {
                            PhotonView view = hit.transform.GetComponent<PhotonView>();
                            if (view.ViewID != photonView.ViewID)
                            {
                                GameplaySystem.main.ShowMenu(view.ViewID);
                            }
                        }
                    }
                    else if (hit.transform.tag == "Chair")
                    {
                        if (Vector3.Distance(transform.position, hit.transform.position) < 2f)
                        {
                            if (animationNow != AnimationName.Sit)
                            {

                                Chair chair = hit.transform.GetComponent<Chair>();
                                if (chair.characterControl == null)
                                {
                                    if (animationNow != AnimationName.Idle && animationNow != AnimationName.Walk) return;
                                    photonView.RPC("SitAt", RpcTarget.All, GameplaySystem.main.FindChairIndex(chair));
                                }
                            }

                        }                       
                    }
                }
            }
        }

        public void StandUp()
        {
            if (!isSit) return;
            if ((Input.GetAxisRaw("Vertical") > 0f) || (Input.GetKey(KeyCode.Escape)))
            {
                photonView.RPC("StandUp", RpcTarget.All);
                this._characterController.enabled = true;
            }
            else if(_joystick != null)
            {
                if (_joystick.Vertical > 0f)
                {
                    photonView.RPC("StandUp", RpcTarget.All);
                    this._characterController.enabled = true;
                }                  
            }
        }

        public void ToggleMic(bool mute)
        {
            if (mute && NetworkRouter.Instance.ReadyToTransmit && micName != "No Devices")
            {
                //print("Ready To Call");
                print("Mic Name :" + micName);
                //checkStatus.isRecord.text = "Record With : " + micName;
                nameTMP.text = nickname + " <sprite index=0>";
                recorder.StartRecord();
            }
            else
            {
                //print("Failed to Call");
                nameTMP.text = nickname;
                RefreshMicrophonesButtonOnClickHandler();
                //checkStatus.isRecord.text = "Mute With : " + micName;
                recorder.StopRecord();
            }

        }

        public void PlayAnimation(int index) => PlayAnimation((AnimationName)index);
        public void PlayAnimation(AnimationName animationName) 
        {
            if (animationNow == AnimationName.Sit)
            {
                if (animationName == AnimationName.Excited) return;
                if (animationName == AnimationName.Giving) return;
                if (animationName == AnimationName.Hi) return;
                if (animationName == AnimationName.Laugh) return;
            }
            photonView.RPC("PlayAnimation", RpcTarget.All, animationName);
        }

        public void PlayAnimationExcited() => PlayAnimation(AnimationName.Excited);
        public void PlayAnimationHug() => PlayAnimation(AnimationName.Hug);
        public void PlayAnimationSit() => PlayAnimation(AnimationName.Sit);
        public void PlayAnimationGiving() => PlayAnimation(AnimationName.Giving);
        public void PlayAnimationLaugh() => PlayAnimation(AnimationName.Laugh);
        public void PlayAnimationHi() => PlayAnimation(AnimationName.Hi);
        public void PlayAnimationMarry() 
        {
             PlayAnimation(AnimationName.Marry);
        } 

        private void CharactorAnimation()
        {          
            if (animationNow != AnimationName.Idle && animationNow != AnimationName.Walk) return;

            /*if (Vector3.Distance(Vector3.zero, agent.velocity) > 0.3f) ChangeAnimation(AnimationName.Walk);
            else ChangeAnimation(AnimationName.Idle);*/

            if (photonView.IsMine)
            {
                if (isWalk)
                {
                    if (animationNow == AnimationName.Walk) return;
                    PlayAnimation(AnimationName.Walk);
                }
                else
                {
                    if (animationNow == AnimationName.Idle) return;
                    PlayAnimation(AnimationName.Idle);
                }
            }
        }

        private void ChangeAnimation(AnimationName animationName)
        {
            if (animationNow == animationName) return;
            animationNow = animationName;
            animationCount++;
            switch (animationNow)
            {
                case AnimationName.Idle: animator.CrossFadeInFixedTime((holdingFlower ? "H" : "") + animationNow.ToString(), 0.3f); isStop = false; break;
                case AnimationName.Walk: animator.CrossFadeInFixedTime((holdingFlower ? "H" : "") + animationNow.ToString(), 0.3f); isStop = false; break;
                case AnimationName.Sit: animator.CrossFadeInFixedTime((holdingFlower ? "H" : "") + animationNow.ToString(), 0.4f); isStop = true; isWalk = false; break;
                case AnimationName.Excited: animator.CrossFadeInFixedTime(animationNow.ToString(), 0.3f); StartCoroutine(PlayAnimationIdle(animationCount)); isStop = true; isWalk = false; break;
                case AnimationName.Laugh: animator.CrossFadeInFixedTime(animationNow.ToString(), 0.3f); StartCoroutine(PlayAnimationIdle(animationCount)); isStop = true; isWalk = false; break;
                case AnimationName.Hi: animator.CrossFadeInFixedTime(animationNow.ToString(), 0.3f); StartCoroutine(PlayAnimationIdle(animationCount)); isStop = true; isWalk = false; break;
                default: animator.CrossFadeInFixedTime(animationNow.ToString(), 0.2f); StartCoroutine(PlayAnimationIdle(animationCount)); isStop = true; isWalk = false; break;
            }
            Debug.Log(animationNow);
        }

        IEnumerator PlayAnimationIdle(int count)
        {
            yield return new WaitForSeconds(0.2f);
            yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1);
            if (count == animationCount)
            { 
                ChangeAnimation(AnimationName.Idle);
            }
                
        }

        public void MoveTo(Vector3 position)
        {
            if (this.chair != null)
            {
                this.chair.characterControl = null;
                this.chair = null;
            }
            agent.enabled = true;
            agent.isStopped = false;
            agent.SetDestination(position);
            ChangeAnimation(AnimationName.Walk);
        }

        private void RefreshMicrophonesButtonOnClickHandler()
        {
            recorder.RefreshMicrophones();
            if (CustomMicrophone.HasConnectedMicrophoneDevices())
            {
                micName = CustomMicrophone.devices[0];
                recorder.SetMicrophone(CustomMicrophone.devices[0]);
            }
            else micName = "No Devices";
        }


        [PunRPC]
        public void MoveTo(Vector3 position, PhotonMessageInfo info) { if (!photonView.IsMine) MoveTo(position); }

        [PunRPC]
        public void PlayAnimation(AnimationName animationName, PhotonMessageInfo info)
        {
            if (agent.enabled == true) if (animationName != AnimationName.Walk) agent.isStopped = true;
            ChangeAnimation(animationName);
        }

        [PunRPC]
        public void SitAt(int index, PhotonMessageInfo info)
        {
            agent.enabled = false;
            this._characterController.enabled = false;
            Chair chair = GameplaySystem.main.FindChair(index);
            chair.collider.enabled = false;                      
            ChangeAnimation(AnimationName.Sit);           
            if (this.chair != null)
            {
                if (this.chair.characterControl != this) this.chair.characterControl.MoveTo(chair.transform.position);
                else
                {
                    this.chair.characterControl = null;
                    this.chair = null;
                }
            }
            chair.characterControl = this;
            this.chair = chair;
            this.transform.eulerAngles = chair.sitPoint.eulerAngles;
            this.transform.position = chair.sitPoint.position;
            this.isSit = true;
        }

        [PunRPC]
        public void StandUp(PhotonMessageInfo info)
        {            
            Chair chair = this.chair;
            Vector3 chairPos = chair.transform.position;
            this.transform.position = new Vector3(transform.position.x + UnityEngine.Random.Range(0.5f, 1f), transform.position.y, transform.position.z + UnityEngine.Random.Range(0.5f, 1f));
            chair.collider.enabled = true;
            chair.characterControl = null;
            this.chair = null;
            ChangeAnimation(AnimationName.Idle);
            this.isSit = false;            
        }

        [PunRPC]
        public void CharacterTelepot(Vector3 position, Vector3 rotation, PhotonMessageInfo info)
        {
            agent.enabled = false;
            transform.position = position;
            transform.eulerAngles = rotation;
        }

        [PunRPC]
        public void CharacterStage(Vector3 position, Vector3 rotation, Vector3 moveTo, AnimationName animationName, PhotonMessageInfo info)
        {
            if (photonView.IsMine) return;
            agent.enabled = false;
            if (Vector3.Distance(transform.position, position) > 1f) transform.position = position;
            if (animationName != AnimationName.Sit)
            {
                agent.enabled = true;
                if (Vector3.Distance(transform.position, moveTo) > 1f) agent.SetDestination(moveTo);
            }
            transform.eulerAngles = rotation;
            ChangeAnimation(animationName);
        }

        [PunRPC]
        public void CharacterData(string rnickname, string rfullname, string rgender, string rbio, int hatIndex, int faceIndex, int glassesIndex, int bodyIndex, int topIndex, int bottomIndex, int footwearIndex, PhotonMessageInfo info)
        {
            if (photonView.IsMine) return;
            nameTMP.text = rnickname;
            nickname = rnickname;
            fullname = rfullname;
            gender = rgender;
            bio = rbio;
            characterEditor.Load(hatIndex, faceIndex, glassesIndex, bodyIndex, topIndex, bottomIndex, footwearIndex);
        }

        [PunRPC]
        public void ChangeClothSet(int groupIndex, PhotonMessageInfo info)
        {
            characterEditor.SetGroup(groupIndex);
            characterEditor.Show();
        }

        [PunRPC]
        public void ChangeBackgroundPhotobooth(int index, PhotonMessageInfo info)
        {
            OnBackgroundChange.Invoke(index);
        }
      
        [PunRPC]
        public void Request(int fromViewID, Command command, int data,string name, PhotonMessageInfo info)
        {
            if (!photonView.IsMine) return;
            print(fromViewID);

            print(command);
            print(data);
            GameplaySystem.main.RequestReceive(fromViewID, command, data, name);
        }
        public enum Command { None, PrivateRoom, Giving, Hug, Marry, Photobooth };

        [PunRPC]
        public void CharacterTelepotToMap(string map, int data,string name, PhotonMessageInfo info)
        {
            if (!photonView.IsMine) return;
            Network.NetworkManager.main.Join(map, data, name, 100);
        }

        [PunRPC]
        public void SentMessage(string fromName,string newMessage)
        {
            ChatM.instance.Created_NewMessage(fromName,newMessage);
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            if (!photonView.IsMine) return;
            if (chair != null) photonView.RPC("SitAt", RpcTarget.All, GameplaySystem.main.FindChairIndex(chair));
            else photonView.RPC("CharacterStage", RpcTarget.Others, transform.position, transform.eulerAngles, agent.destination, animationNow);
        }
    }
}


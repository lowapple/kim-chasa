using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Chasa
{
    public class SceneManager : MonoBehaviour
    {
        public static SceneManager instance = null;

        public SoulChatManager soulChatManager;
        public StoreUI storeUI;
        public MissionUI missionUI;
        public MissionUIManager missionUIManager;
        public InventoryManager inventoryManager;
        public PoolObjects poolObjects;
        public SoundManager soundManager;
        public OptionUIManager optionUIManager;
        public ItemManager itemManager;
        public GameStateManager gameStateManager;
        public TutorialManager tutorialManager;

        public GameObject others;
        public GameObject playerSoul;
        public Text playerSoulText;

        public ChasaPlayerUnit character;
        public FreeLookCam cam;

        public GameObject pressF;
        public Text getItemText;

        public bool isCursor;

        [Range(0, 1)]
        public float bgmScale;
        public Slider bgmSlider;
        [Range(0, 1)]
        public float effectScale;
        public Slider effectSlider;

        public bool isOnotherScene = false;

        private bool isToggle = false;
        public GameObject[] toggleUIs;

        public void Awake()
        {
            if (instance != null)
            {
                Destroy(this.gameObject);
                return;
            }

            Screen.SetResolution(1280, 720, true);

            instance = this;
            DontDestroyOnLoad(this);
        }

        private void Start()
        {
            HideCharacterSoul();
        }

        public void HideCharacterSoul()
        {
            playerSoul.SetActive(false);
        }
        public void ShowCharacterSoul()
        {
            if(!isToggle)
                playerSoul.SetActive(true);
        }

        public void CharacterMove(Transform pos)
        {
            character.chasaCharacter.m_Rigidbody.isKinematic = true;
            character.chasaCharacter.m_Rigidbody.useGravity = false;

            character.gameObject.transform.localPosition = new Vector3(0, 0, 0);
            character.gameObject.transform.transform.localRotation = Quaternion.Euler(0, 0, 0);

            cam.gameObject.transform.localPosition = new Vector3(0, 0, 0);
            cam.gameObject.transform.localRotation = Quaternion.Euler(0, 0, 0);

            character.enabled = false;
            character.chasaCharacter.enabled = false;
            character.chasaControl.enabled = false;
            character.chasaCombat.enabled = false;

            character.transform.parent.transform.position = pos.position;
            character.transform.parent.transform.localRotation = pos.localRotation;

            character.enabled = true;
            character.chasaCharacter.enabled = true;
            character.chasaControl.enabled = true;
            character.chasaCombat.enabled = true;

            character.chasaCharacter.m_Rigidbody.isKinematic = false;
            character.chasaCharacter.m_Rigidbody.useGravity = true;

        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.T))
            {
                for (int i = 0; i < toggleUIs.Length; i++)
                {
                    toggleUIs[i].gameObject.SetActive(false);
                }
                isToggle = true;
            }
        }

        private void LateUpdate()
        {
            bgmScale = bgmSlider.value;
            effectScale = effectSlider.value;
        }

        public void ShowCursor()
        {
            isCursor = true;

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        public void HideCursor()
        {
            isCursor = false;

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Chasa
{
    public class BookDoor : MonoBehaviour
    {
        public Book book;
        public GameObject door;
        public Animator missionDoor;

        // Name
        public GameObject Names;
        public Text[] Name;

        public GameObject NameSelection;

        private string nameBackup;
        private int nameIdx;
        private bool isBookDoorReady = false;

        private void Start()
        {
            missionDoor.gameObject.SetActive(false);

            Names.SetActive(false);

            nameIdx = 0;
            nameBackup = Name[nameIdx].text;
            Name[nameIdx].text = "▶" + Name[nameIdx].text;

            NameSelection.gameObject.SetActive(false);
        }

        public void OnTriggerEnter(Collider other)
        {
            if (other.transform.CompareTag("Player"))
            {
                SceneManager.instance.pressF.SetActive(true);
                isBookDoorReady = true;
            }
        }

        public void OnTriggerExit(Collider other)
        {
            if (other.transform.CompareTag("Player"))
            {
                SceneManager.instance.pressF.SetActive(false);
                isBookDoorReady = false;
            }
        }

        public void NameUp()
        {
            if (nameIdx <= 0)
                return;
            else
            {
                Name[nameIdx].text = nameBackup;
                nameIdx--;
                nameBackup = Name[nameIdx].text;
                Name[nameIdx].text = "▶" + Name[nameIdx].text;
            }
        }

        public void NameDown()
        {
            if (nameIdx >= Name.Length - 1)
                return;
            else
            {
                Name[nameIdx].text = nameBackup;
                nameIdx++;
                nameBackup = Name[nameIdx].text;
                Name[nameIdx].text = "▶" + Name[nameIdx].text;
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F) && isBookDoorReady)
            {
                if (!book.IsOpen())
                {
                    book.Open_Book();
                    SceneManager.instance.soundManager.PlayEffect("Page");

                    book.covercallback.pageturncallback = CoverOpenCallback;

                    SceneManager.instance.pressF.SetActive(false);

                    SceneManager.instance.character.chasaCharacter.Move(Vector3.zero, false, false);
                    SceneManager.instance.character.chasaCharacter.m_Animator.SetFloat("Forward", 0.0f);
                    SceneManager.instance.character.chasaCharacter.m_Animator.SetFloat("Turn", 0.0f);
                    SceneManager.instance.character.chasaControl.enabled = false;

                    NameSelection.gameObject.SetActive(true);
                }
                else
                {
                    book.Close_Book();
                    SceneManager.instance.soundManager.PlayEffect("Page");

                    book.covercallback.pageturncallback = CoverCloseCallback;

                    SceneManager.instance.pressF.SetActive(true);
                    SceneManager.instance.character.chasaCharacter.Move(Vector3.zero, false, false);
                    SceneManager.instance.character.chasaCharacter.m_Animator.SetFloat("Forward", 0.0f);
                    SceneManager.instance.character.chasaCharacter.m_Animator.SetFloat("Turn", 0.0f);
                    SceneManager.instance.character.chasaControl.enabled = true;

                    NameSelection.gameObject.SetActive(false);
                }
            }

            // 책 페이지 넘김
            if (book.IsOpen())
            {
                if (Input.GetKeyDown(KeyCode.D))
                {
                    if (book.CanTurnPage())
                    {
                        // 페이지 넘기기 전
                        switch (book.pageCount)
                        {
                            case 0:
                                {
                                    Names.SetActive(false);
                                }
                                break;
                        }

                        book.Turn_Page();
                    }
                }
                if (Input.GetKeyDown(KeyCode.A))
                {
                    if (book.CanGoBackAPage())
                    {
                        // 페이지 넘기기 전
                        switch (book.pageCount)
                        {
                            case 1:
                                {
                                    Names.SetActive(true);
                                }
                                break;
                        }

                        book.Turn_BackPage();
                    }
                }
            }

            // 이름 선택
            if (book.IsOpen())
            {
                if (Input.GetKeyDown(KeyCode.W))
                {
                    NameUp();
                }
                if (Input.GetKeyDown(KeyCode.S))
                {
                    NameDown();
                }
            }

            // 씬으로 이동
            if (book.IsOpen())
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    book.covercallback.pageturncallback = CoverCloseCallback;
                    book.DoorOpen();
                    NameSelection.gameObject.SetActive(false);
                    Invoke("MissionDoorOpen", 3f);
                }
            }
        }

        public void CoverOpenCallback()
        {
            Names.SetActive(true);
        }
        public void CoverCloseCallback()
        {
            Names.SetActive(false);
        }

        public void MissionDoorOpen()
        {
            missionDoor.gameObject.SetActive(true);
            missionDoor.Play("MissionDoorOpen");
            SceneManager.instance.character.chasaCharacter.Move(Vector3.zero, false, false);
            SceneManager.instance.character.chasaCharacter.m_Animator.SetFloat("Forward", 0.0f);
            SceneManager.instance.character.chasaCharacter.m_Animator.SetFloat("Turn", 0.0f);
            SceneManager.instance.character.chasaControl.enabled = true;
            book.gameObject.SetActive(false);
        }
    }
}
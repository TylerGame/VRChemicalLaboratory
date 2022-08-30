using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


namespace VRChemiLab.Sava.Script
{
    public class UIManager : MonoBehaviour
    {

        public GameObject chemiReaction;
        public GameObject questionButton;
        public bool isOpendFormulaWindow = false;

        public GameObject topDownCamera;
        public GameObject detailWindow;
        private bool isShowingDetail = false;

        private static UIManager instance;
        public static UIManager Instance
        {
            get
            {
                return instance;
            }
        }

        void Awake()
        {
            if (instance != null)
            {
                Destroy(gameObject);
            }
            else
            {
                instance = this;
            }
        }
        // Start is called before the first frame update
        void Start()
        {
            // chemiReaction = GameObject.Find("TargetChemiReaction");
            // questionButton = GameObject.Find("QuestionButton");

            // chemiReaction.SetActive(false);
            // questionButton.SetActive(true);

            isOpendFormulaWindow = false;
            topDownCamera.SetActive(false);
            detailWindow.transform.localScale = new Vector3(0.2f, 0f, 1f);
            detailWindow.SetActive(false);
            isShowingDetail = false;
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetMouseButton(0))
            {
                topDownCamera.SetActive(true);
            }
            else
            {
                topDownCamera.SetActive(false);
            }
        }

        public void ShowDetail(bool s, string str)
        {
            if (s)
            {
                if (!isShowingDetail)
                {
                    isShowingDetail = true;
                    detailWindow.SetActive(true);
                    detailWindow.transform.DOScaleY(1f, 0.2f).OnComplete(() =>
                    {
                        detailWindow.GetComponentInChildren<TMPro.TMP_Text>().text = str;
                        detailWindow.transform.DOScaleX(1f, 0.2f).OnComplete(() =>
                        {
                            // detailWindow.GetComponentInChildren<TMPro.TMP_Text>().text = str;
                        });
                    });


                }
            }
            else
            {
                if (isShowingDetail)
                {
                    isShowingDetail = false;
                    detailWindow.transform.DOScaleX(0.2f, 0.2f).OnComplete(() =>
                          {
                              detailWindow.transform.DOScaleY(0f, 0.2f).OnComplete(() =>
                              {
                                  detailWindow.SetActive(false);
                              });
                          });


                }


            }
        }

        public void OnClick_QuestionButton()
        {
            // questionButton.SetActive(false);
            // chemiReaction.SetActive(true);
            // chemiReaction.transform.DOScaleY(1f, 0.2f).OnComplete(() =>
            // {
            //     chemiReaction.transform.DOScaleX(1f, 0.2f);
            // });

            // isOpendFormulaWindow = true;
        }


        public void OnClick_CloseButton()
        {
            // chemiReaction.transform.DOScaleX(0.3f, 0.2f).OnComplete(() =>
            // {
            //     chemiReaction.transform.DOScaleY(0f, 0.2f).OnComplete(() =>
            //     {
            //         questionButton.SetActive(true);
            //         chemiReaction.SetActive(false);
            //         isOpendFormulaWindow = false;
            //     });
            // });
        }
    }

}

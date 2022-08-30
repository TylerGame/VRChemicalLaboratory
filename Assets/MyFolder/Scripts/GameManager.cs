using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRChemiLab.Sava.Script
{
    public class GameManager : MonoBehaviour
    {



        public enum GameState { PickupWater, PickupNa, TakingReaction }
        public GameState gs = GameState.PickupWater;
        private float repeatAudioTime = 20f;
        private float t = 0f;
        private static GameManager instance;
        public static GameManager Instance
        {
            get
            {
                return instance;
            }
        }

        void Awake()
        {

            if (instance == null)
                instance = this;
            else
                Destroy(gameObject);
        }
        // Start is called before the first frame update
        void Start()
        {
            t = Time.time;
            Invoke("PlayAudio", 3f);
        }

        // Update is called once per frame
        void Update()
        {
            if (Time.time - t > repeatAudioTime)
            {
                t = Time.time;
                PlayAudio();
            }

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.name == "waterbeaker")
                {
                    UIManager.Instance.ShowDetail(true, "H<size=30>2</size>O");
                }
                else if (hit.collider != null && hit.collider.name.Contains("Na"))
                {
                    UIManager.Instance.ShowDetail(true, "Na");
                }
                else
                {
                    UIManager.Instance.ShowDetail(false, "Na");
                }
            }

        }

        public void PlayAudio()
        {
            switch (gs)
            {
                case GameState.PickupWater:
                    SoundManager.Instance.PlaySound("pickupwater");
                    break;
                case GameState.PickupNa:
                    SoundManager.Instance.PlaySound("pickupnatirum");
                    break;
                case GameState.TakingReaction:
                    SoundManager.Instance.PlaySound("reactiontakingplace");
                    break;
            }
        }
    }
}


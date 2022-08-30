using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace VRChemiLab.Sava.Script
{
    public class ChemiReactionBeakcer : MonoBehaviour
    {

        public GameObject liqidModel;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        void OnTriggerStay(Collider other)
        {
            if (other.gameObject.name == "waterbeaker")
            {
                liqidModel.transform.localScale = new Vector3(liqidModel.transform.localScale.x, liqidModel.transform.localScale.y + Time.deltaTime * 0.1f, liqidModel.transform.localScale.z);
                if (liqidModel.transform.localScale.y > 0.5f)
                {
                    GameManager.Instance.gs = GameManager.GameState.PickupNa;
                    GameManager.Instance.PlayAudio();
                }
                SoundManager.Instance.PlaySound("water fallen");
            }
            // if (other.gameObject.name.Contains("Na"))
            // {
            //     GameManager.Instance.gs = GameManager.GameState.TakingReaction;
            //     GameManager.Instance.PlayAudio();
            //     smokeEffect.SetActive(true);
            //     Invoke("StopSmokeEffect", 10f);
            // }
        }


    }
}


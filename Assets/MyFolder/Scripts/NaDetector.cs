using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRChemiLab.Sava.Script
{
    public class NaDetector : MonoBehaviour
    {

        public GameObject smokeEffect;
        // Start is called before the first frame update
        void Start()
        {
            smokeEffect.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {

        }

        void OnTriggerStay(Collider other)
        {
            if (other.gameObject.name.Contains("Na"))
            {
                GameManager.Instance.gs = GameManager.GameState.TakingReaction;
                GameManager.Instance.PlayAudio();
                smokeEffect.SetActive(true);
                Invoke("StopSmokeEffect", 10f);
                other.gameObject.GetComponent<Collider>().enabled = false;
            }
        }

        void StopSmokeEffect()
        {
            smokeEffect.SetActive(false);
        }
    }
}

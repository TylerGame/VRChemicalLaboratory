using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace VRChemiLab.Sava.Script
{

    public class SoundEffect : MonoBehaviour
    {

        public string fallenSoundName;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        void OnCollisionEnter(Collision other)
        {
            Debug.Log(other.collider.tag);
            if (other.collider.tag != "Respawn" && Time.time > 3f)
            {
                if (SoundManager.Instance != null)
                    SoundManager.Instance.PlaySound(fallenSoundName);
            }
        }
    }
}


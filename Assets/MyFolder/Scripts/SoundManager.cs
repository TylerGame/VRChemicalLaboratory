using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRChemiLab.Sava.Script
{
    public class SoundManager : MonoBehaviour
    {
        Dictionary<string, AudioClip> soundPacks = new Dictionary<string, AudioClip>();
        AudioSource speacker;

        private static SoundManager _instance;
        public static SoundManager Instance
        {
            get
            {
                return _instance;
            }
        }
        void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            speacker = Camera.main.GetComponent<AudioSource>();
            AudioClip[] clips = Resources.LoadAll<AudioClip>("Sounds");
            foreach (AudioClip clip in clips)
            {
                soundPacks.Add(clip.name, clip);
            }
        }

        // Update is called once per frame
        void Update()
        {

        }


        public void PlaySound(string name)
        {
            AudioClip clip;
            bool ret = soundPacks.TryGetValue(name, out clip);
            if (ret && !speacker.isPlaying)
            {
                speacker.clip = clip;
                speacker.Play();
            }
        }

    }


}

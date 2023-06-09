using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Insomnia.Defines;

namespace Insomnia {

    [RequireComponent(typeof(AudioSource))]
    public class Speaker : Observer {
        [Header("Components")]
        [SerializeField] private AudioSource m_audio = null;

        [Header("Settings")]
        [SerializeField] private SoundType m_type = SoundType.Master;

        [Header("Audio Clips")]
        [SerializeField] private AudioClip[] m_clips;

        private void Awake() {
            m_audio = GetComponent<AudioSource>();
        }

        #region Observer Functions
        public sealed override void Notify(object noti) {
            SoundNotiData data = (SoundNotiData)noti;
            if(default(SoundNotiData).Equals(data))
                return;

            if(m_audio == null)
                return;

            m_audio.volume = data.volumes[(int)SoundType.Master] * data.volumes[(int)m_type];
        }

        public sealed override void OnActivate() {
            VolumeController subject = VolumeController.Instance;
            if(subject == null)
                return;

            subject.Subscribe(this);
        }

        public sealed override void OnDeactivate() {
            VolumeController subject = VolumeController.Instance;
            if(subject == null)
                return;

            subject.Unsubscribe(this);
        }

        #endregion

        public void Play(int clipIndex, bool isLoop = false, float delay = -1f) {
            if(m_clips.Length <= clipIndex)
                return;

            if(m_audio == null)
                return;

            if(isLoop && m_audio.clip == m_clips[clipIndex])
                return;

            if(m_audio.isPlaying) 
                m_audio.Stop();

            m_audio.loop = isLoop;
            m_audio.clip = m_clips[clipIndex];

            if(delay < 0f)
                m_audio.Play();
            else
                m_audio.PlayDelayed(delay);
        }

        public void PlayOneShot(int clipIndex) {
            if(m_clips.Length >= clipIndex)
                return;

            m_audio.PlayOneShot(m_clips[clipIndex]);
        }
    }
}


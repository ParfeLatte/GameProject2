using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Insomnia.Defines;

namespace Insomnia {

    [RequireComponent(typeof(AudioSource))]
    public abstract class Speaker : Observer {
        [Header("Speaker: Components")]
        [HideInInspector] protected AudioSource m_audio = null;

        [Header("Speaker: Settings")]
        [SerializeField] protected SoundType m_type = SoundType.Master;
        [SerializeField] private bool m_isGlobal = false;
        [SerializeField] private int m_cameraWidth = -1;

        [Header("Speaker: Audio Clips")]
        [SerializeField] protected AudioClip[] m_clips;

        protected virtual void Awake() {
            m_audio = GetComponent<AudioSource>();
        }

        private void Update() {
            if(m_isGlobal)
                return;

            if(m_cameraWidth <= 0)
                return;

            float xPosInCam = Camera.main.WorldToScreenPoint(transform.position).x;
            float panStereoValue = (xPosInCam - (m_cameraWidth / 2)) / (m_cameraWidth / 2);
            m_audio.panStereo = Mathf.Clamp(panStereoValue, -1f, 1f);
        }

        protected abstract void Reset();

        protected virtual void OnValidate() {
            m_audio = GetComponent<AudioSource>();
            if(m_audio == null)
                return;

            m_audio.playOnAwake = false;
            m_audio.loop = false;
        }

        #region Observer Functions
        public override void Notify(object noti) {
            SoundNotiData data = (SoundNotiData)noti;
            if(default(SoundNotiData).Equals(data))
                return;

            if(m_audio == null)
                return;

            m_audio.volume = data.volumes[(int)SoundType.Master] * data.volumes[(int)m_type];
        }

        public sealed override void OnStart() {
            VolumeController subject = VolumeController.Instance;
            if(subject == null)
                return;

            subject.Subscribe(this);
            OnSpeakerStart();
            m_cameraWidth = Camera.main.pixelWidth;
        }

        public sealed override void OnEnd() {
#if UNITY_EDITOR == false
            VolumeController subject = VolumeController.Instance;
            if(subject == null)
                return;

            subject.Unsubscribe(this);
            OnSpeakerEnd();
#endif
        }

#endregion

        public virtual void OnSpeakerStart() { }
        public virtual void OnSpeakerEnd() { }

        public virtual void Play(int clipIndex, bool isLoop = false, float delay = -1f) {
            if(m_clips.Length <= clipIndex)
                return;

            if(m_clips[clipIndex] == null)
                return;

            if(m_audio == null)
                return;

            //if(isLoop && m_audio.clip == m_clips[clipIndex] && m_audio.isPlaying)
            //    return;

            if(m_audio.clip == m_clips[clipIndex] && m_audio.isPlaying)
                return;

            m_audio.loop = isLoop;
            m_audio.clip = m_clips[clipIndex];

            if(delay < 0f)
                m_audio.Play();
            else
                m_audio.PlayDelayed(delay);
        }

        public virtual void PlayOneShot(int clipIndex) {
            if(m_clips.Length <= clipIndex)
                return;

            m_audio.PlayOneShot(m_clips[clipIndex]);
        }

        public virtual void Stop() {
            if(m_audio.isPlaying == false)
                return;

            m_audio.loop = false;
            m_audio.Stop();
        }
    }
}


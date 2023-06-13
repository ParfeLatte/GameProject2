using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static Insomnia.Defines;

namespace Insomnia{
    public class BGM_Speaker : Speaker {
        #region Simple Singleton
        private static BGM_Speaker m_instance;
        
        public static BGM_Speaker Instance { get => m_instance; }
        #endregion

        public enum BGMSounds {
            BGM = 3,
            Wave = 7,
        }

        [Header("BGM_Speaker: Status")]
        [SerializeField] private float m_maxVolume = 0f;
        private Array m_BgmSoundsValues = Enum.GetValues(typeof(BGMSounds));

        [Header("BGM_Speaker: Settings")]
        [SerializeField, Range(0.1f, 10f)] private float m_volumeFluctuationTime = 1f;

        [Header("BGM_Speaker: Events")]
        [SerializeField]
        private Queue<Action> onMusicStop = new Queue<Action>();

        protected override void Awake() {
            base.Awake();
            m_instance = this;
        }

        protected override void OnValidate() {
            m_audio = GetComponent<AudioSource>();

            if(m_audio == null)
                return;

            m_audio.loop = false;
            m_audio.volume = 0f;
        }

        public sealed override void Notify(object noti) {
            SoundNotiData data = (SoundNotiData)noti;
            if(default(SoundNotiData).Equals(data))
                return;

            if(m_audio == null)
                return;

            if(m_audio.isPlaying) 
                m_audio.volume = data.volumes[(int)SoundType.Master] * data.volumes[(int)m_type];
            m_maxVolume = data.volumes[(int)SoundType.Master] * data.volumes[(int)m_type];
        }

        public override void Play(int bgmType, bool isLoop = false, float delay = -1f) {
            //TODO: �������� �����ϱ�
            int startIndex = 0;
            int lastIndex = bgmType;

            foreach(int bgmCount in m_BgmSoundsValues) {
                if(bgmCount == bgmType)
                    break;

                startIndex += bgmCount;
            }

            int randomPlay = UnityEngine.Random.Range(startIndex, lastIndex);

            if(m_clips.Length <= randomPlay)
                return;

            if(m_clips[randomPlay] == null)
                return;

            if(m_audio == null)
                return;

            if(m_audio.isPlaying) {
                if(onMusicStop.Count >= 1)
                    return;

                onMusicStop.Enqueue(() => { Play(bgmType, isLoop, delay); });
                Stop();
                return;
            }

            m_audio.loop = isLoop;
            m_audio.clip = m_clips[randomPlay];

            if(delay < 0f) 
                m_audio.Play();
            else
                m_audio.PlayDelayed(delay);

            StartCoroutine(CoDragVolume(true, delay));
        }

        public override void PlayOneShot(int clipIndex) {
            base.PlayOneShot(clipIndex);
        }

        public override void Stop() {
            if(m_audio.isPlaying == false)
                return;

            StartCoroutine(CoDragVolume(false));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="onoff">true if procedural volume up, false down</param>
        /// <returns></returns>
        private IEnumerator CoDragVolume(bool onoff, float delay = -1f) {
            while(true) {
                if(delay <= 0f)
                    break;

                delay -= Time.deltaTime;
                yield return null;
            }

            float targetVolume = onoff ? m_maxVolume : 0f;
            float curTick = 0f;
            float curVolume = m_audio.volume;

            while(true) {
                curTick += Time.deltaTime;
                m_audio.volume = Mathf.Lerp(curVolume, targetVolume, curTick / m_volumeFluctuationTime);

                yield return null;
                if(curTick >= m_volumeFluctuationTime && Mathf.Abs(m_audio.volume - targetVolume) <= 0.1f) {
                    m_audio.volume = targetVolume;
                    break;
                }
            }

            if(onoff == false)
                m_audio.Stop();

            while(onMusicStop.Count > 0) {
                onMusicStop.Dequeue().Invoke();
            }

            yield break;
        }

        private void OnDestroy() {
            m_instance = null;
        }

        protected override void Reset() {
            if(m_clips == null) {
                m_clips = new AudioClip[(int)m_BgmSoundsValues.GetValue(m_BgmSoundsValues.Length - 1)];
                return;
            }

            if(m_clips.Length <= 0) {
                m_clips = new AudioClip[(int)m_BgmSoundsValues.GetValue(m_BgmSoundsValues.Length - 1)];
                return;
            }
        }
    }
}
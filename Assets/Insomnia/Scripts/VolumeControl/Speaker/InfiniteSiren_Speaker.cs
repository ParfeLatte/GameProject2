using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Insomnia.Siren_Speaker;

namespace Insomnia{
    public class InfiniteSiren_Speaker : Speaker {
        public enum InfiniteSirenSounds {
            Alarm = 0,

        }

        protected override void Reset() {
            m_isGlobal = true;

            if(m_clips == null) {
                m_clips = new AudioClip[Enum.GetNames(typeof(InfiniteSirenSounds)).Length];
                return;
            }

            if(m_clips.Length <= 0) {
                m_clips = new AudioClip[Enum.GetNames(typeof(InfiniteSirenSounds)).Length];
                return;
            }
        }
    }
}

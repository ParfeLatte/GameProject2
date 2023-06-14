using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Insomnia.NormalMob_Speaker;

namespace Insomnia{
    public class GiantMob_Speaker : Speaker {
        public enum GiantMobSounds {
            GiantMob_Sleep = 0,
            GiantMob_Walk,
            GiantMob_Attack,
            GiantMob_Damaged,
            GiantMob_Dead,
        }
        protected override void Reset() {
            if(m_clips == null) {
                m_clips = new AudioClip[Enum.GetNames(typeof(GiantMobSounds)).Length];
                return;
            }

            if(m_clips.Length <= 0) {
                m_clips = new AudioClip[Enum.GetNames(typeof(GiantMobSounds)).Length];
                return;
            }
        }
    }
}

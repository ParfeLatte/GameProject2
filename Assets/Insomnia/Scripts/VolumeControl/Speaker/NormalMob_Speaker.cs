using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Insomnia.Player_Speaker;

namespace Insomnia {
    public class NormalMob_Speaker : Speaker {
        public enum NormalMobSounds {
            NormalMob_Sleep = 0,
            NormalMob_Walk,
            MormalMob_Attack,
            MormalMob_Damaged,
            MormalMob_Dead,
        }

        protected sealed override void Reset() {
            if(m_clips == null) {
                m_clips = new AudioClip[Enum.GetNames(typeof(NormalMobSounds)).Length];
                return;
            }

            if(m_clips.Length <= 0) {
                m_clips = new AudioClip[Enum.GetNames(typeof(NormalMobSounds)).Length];
                return;
            }
        }
    }
}
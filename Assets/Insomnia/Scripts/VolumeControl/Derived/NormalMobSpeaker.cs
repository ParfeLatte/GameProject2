using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Insomnia.PlayerSpeaker;

namespace Insomnia {
    public class NormalMobSpeaker : Speaker {
        public enum NormalMobSounds {
            NormalMob_Sleep = 0,
            NormalMob_Walk,
            MormalMob_Attack,
            MormalMob_Damaged,
            MormalMob_Dead,
        }

        protected override void OnValidate() {
            base.OnValidate();

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
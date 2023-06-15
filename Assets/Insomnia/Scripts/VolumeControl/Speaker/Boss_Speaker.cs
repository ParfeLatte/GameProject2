using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Insomnia.NormalMob_Speaker;

namespace Insomnia{
    public class Boss_Speaker : Speaker {
        public enum BossSounds {
            BossMob_Sleep = 0,
            BossMob_Walk,
            BossMob_Attack,
            BossMob_Damaged,
            BossMob_Dead,
        }

        protected override void Reset() {
            if(m_clips == null) {
                m_clips = new AudioClip[Enum.GetNames(typeof(BossSounds)).Length];
                return;
            }

            if(m_clips.Length <= 0) {
                m_clips = new AudioClip[Enum.GetNames(typeof(BossSounds)).Length];
                return;
            }
        }
    }
}

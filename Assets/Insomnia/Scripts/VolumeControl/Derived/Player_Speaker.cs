using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Insomnia.NormalMob_Speaker;

namespace Insomnia {
    public class Player_Speaker : Speaker {
        public enum PlayerSounds {
            PlayerWalk = 0,
            PlayerAttack_Normal,
            PlayerAttack_Giant,
            PlayerAttack_Boss,
            PlayerDamage,
        }

        protected override void OnValidate() {
            base.OnValidate();

            if(m_clips == null) {
                m_clips = new AudioClip[Enum.GetNames(typeof(PlayerSounds)).Length];
                return;
            }

            if(m_clips.Length <= 0) {
                m_clips = new AudioClip[Enum.GetNames(typeof(PlayerSounds)).Length];
                return;
            }
        }
    }
}
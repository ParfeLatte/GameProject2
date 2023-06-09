using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Insomnia.NormalMobSpeaker;

namespace Insomnia {
    public class PlayerSpeaker : Speaker {
        public enum PlayerSounds {
            PlayerWalk = 0,
            PlayerAttack_Normal,
            PlayerAttack_Giant,
            PlayerAttack_Boss,
            PlayerDamage,
        }

        private void OnValidate() {
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
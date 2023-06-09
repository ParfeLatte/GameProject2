using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Insomnia.PlayerSpeaker;

namespace Insomnia {
    public class ElevatorSpeaker : Speaker {
        public enum ElevatorSounds {
            Open = 0,
            Close,
            Move,
            Stop,
        }

        private void OnValidate() {
            if(m_clips == null) {
                m_clips = new AudioClip[Enum.GetNames(typeof(ElevatorSounds)).Length];
                return;
            }

            if(m_clips.Length <= 0) {
                m_clips = new AudioClip[Enum.GetNames(typeof(ElevatorSounds)).Length];
                return;
            }
        }
    }
}


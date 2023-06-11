using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Insomnia.PlayerSpeaker;

namespace Insomnia {
    public class ElevatorSpeaker : Speaker {
        public enum ElevatorSounds {
            Close = 0,
            Open,
            Move,
            Stop,
        }

        protected override void OnValidate() {
            base.OnValidate();

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


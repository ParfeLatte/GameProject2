using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Insomnia.Player_Speaker;

namespace Insomnia {
    public class Elevator_Speaker : Speaker {
        public enum ElevatorSounds {
            Close = 0,
            Open,
            Move,
            Stop,
        }

        protected sealed override void Reset() {
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


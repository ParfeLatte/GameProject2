using System;
using UnityEngine;
using static Insomnia.Player_Speaker;

namespace Insomnia {
    public class Door_Speaker : Speaker {
        public enum DoorSounds {
            DoorClose = 0,
            DoorOpen,

        }

        protected sealed override void Reset() {
            if(m_clips == null) {
                m_clips = new AudioClip[Enum.GetNames(typeof(DoorSounds)).Length];
                return;
            }

            if(m_clips.Length <= 0) {
                m_clips = new AudioClip[Enum.GetNames(typeof(DoorSounds)).Length];
                return;
            }
        }
    }
}
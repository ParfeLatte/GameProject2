using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Insomnia.PlayerSpeaker;

namespace Insomnia {
    public class DoorSpeaker : Speaker {
        public enum DoorSounds {
            DoorClose = 0,
            DoorOpen,

        }

        protected override void OnValidate() {
            base.OnValidate();

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
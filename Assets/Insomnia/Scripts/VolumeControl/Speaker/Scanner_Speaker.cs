using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Insomnia.Boss_Speaker;

namespace Insomnia{
    public class Scanner_Speaker : Speaker {
        public enum ScannerSounds {
            OnScanning = 0,
            OnScanFinished
        }

        protected override void Reset() {
            if(m_clips == null) {
                m_clips = new AudioClip[Enum.GetNames(typeof(ScannerSounds)).Length];
                return;
            }

            if(m_clips.Length <= 0) {
                m_clips = new AudioClip[Enum.GetNames(typeof(ScannerSounds)).Length];
                return;
            }
        }
    }
}

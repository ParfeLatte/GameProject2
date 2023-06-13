using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Insomnia.BGM_Speaker;

namespace Insomnia{
	public class BGMTest : MonoBehaviour {
		public void PlayBGM() {
			BGM_Speaker.Instance.Play((int)BGMSounds.BGM, true);
		}

		public void PlayWave() {
            BGM_Speaker.Instance.Play((int)BGMSounds.Wave, true);
        }
	}
}

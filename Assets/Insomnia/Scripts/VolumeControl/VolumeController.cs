using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Insomnia.Defines;

namespace Insomnia {
    public class VolumeController : ImmortalSubject<VolumeController, Speaker> {
        [SerializeField, Range(0f, 1f)] private float[] m_volumes= new float[3];

        protected override void Notify(Speaker speaker) {
            SoundNotiData noti = new SoundNotiData(){volumes = m_volumes};
            speaker.Notify(noti);
        }
    }
}

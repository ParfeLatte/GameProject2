using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Insomnia.Defines;

namespace Insomnia {
    public class VolumeController : ImmortalSubject<VolumeController, Speaker> {
        [SerializeField, Range(0f, 1f)] private float[] m_volumes= new float[3]{ 1f, 1f, 1f };

        #region Properties
        public float Volume_Master { get => m_volumes[0];
            set {
                if(m_volumes.Length <= 0)
                    return;

                m_volumes[0] = value;
                OnUpdate();
            }
        }
        public float Volume_BGM { get => m_volumes[1]; 
            set {
                if(m_volumes.Length <= 1)
                    return;

                m_volumes[1] = value;
                OnUpdate();
            }
        }
        public float Volume_SFX { get => m_volumes[2]; 
            set {
                if(m_volumes.Length <= 2)
                    return;

                m_volumes[2] = value;
                OnUpdate();
            }
        }

        #endregion

        #region IDataIO
        public void LoadData() {
            m_volumes[0] = PlayerPrefs.HasKey("Volume_Master")  ? PlayerPrefs.GetFloat("Volume_Master") : 1f;
            m_volumes[1] = PlayerPrefs.HasKey("Volume_BGM")     ? PlayerPrefs.GetFloat("Volume_BGM")    : 1f;
            m_volumes[2] = PlayerPrefs.HasKey("Volume_SFX")     ? PlayerPrefs.GetFloat("Volume_SFX")    : 1f;
        }

        public void RemoveData() {
            PlayerPrefs.DeleteKey("Volume_Master");
            PlayerPrefs.DeleteKey("Volume_BGM");
            PlayerPrefs.DeleteKey("Volume_SFX");
        }

        public void SaveData() {
            PlayerPrefs.SetFloat("Volume_Master", m_volumes[0]);
            PlayerPrefs.SetFloat("Volume_BGM", m_volumes[1]);
            PlayerPrefs.SetFloat("Volume_SFX", m_volumes[2]);
        }

        #endregion

        #region Unity Event Functions
        protected override void Awake() {
            base.Awake();

            LoadData();
        }

        #endregion

        #region Subject Functions

        public override void OnUpdate() {
            if(m_observers == null)
                return;

            SaveData();
            SoundNotiData noti = new SoundNotiData(){volumes = m_volumes};

            for(int i = 0; i < m_observers.Count; i++) {
                if(m_observers[i] == null)
                    continue;

                m_observers[i].Notify(noti);
            }
        }

        protected override void Notify(Speaker observer) {
            SoundNotiData noti = new SoundNotiData(){volumes = m_volumes};
            observer.Notify(noti);
        }

        #endregion
    }
}

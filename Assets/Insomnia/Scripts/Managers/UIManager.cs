using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Insomnia {
    public class UIManager : ImmortalSingleton<UIManager> {
        private FadeUI_SceneChangeEffect m_fade = null;
        public static FadeUI_SceneChangeEffect Fade { get => _instance.m_fade; }

        protected override void Awake() {
            base.Awake();
            m_fade = GetComponentInChildren<FadeUI_SceneChangeEffect>();

        }
    }
}


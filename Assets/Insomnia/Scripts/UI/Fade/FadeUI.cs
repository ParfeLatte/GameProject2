using Insomnia;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**********************************************************
 *                                                        *
 *              Fade In : True / Fade Out : False         *
 *                                                        *
 **********************************************************/

namespace Insomnia {
    public class FadeUI : SceneChangeEffect {
        [Header("FadeUI: Components")]
        [SerializeField] private Image m_fadeImage = null;

        [Header("FadeUI: Settings")]
        [SerializeField] private Color m_fadeInColor;
        [SerializeField] private Color m_fadeOutColor;
        [SerializeField, Range(0.1f, 10f), Tooltip("효과의 초당 변화량")] 
        private float m_fadeDuration = 0.1f;

        protected override void Awake_Child() {
            m_fadeImage = GetComponentInChildren<Image>();
        }

        protected override void Init_Child() {
            if(m_fadeImage == null)
                return;

            m_fadeImage.color = m_fadeInColor;
            m_fadeImage.raycastTarget = false;

            StartCoroutine(CoEffect(m_fadeOutColor));
        }

        public override void StartEffect() {
            StopAllCoroutines();
            StartCoroutine(CoEffect(m_fadeInColor));
        }

        public override void FinishEffect() {
            StopAllCoroutines();
            StartCoroutine(CoEffect(m_fadeOutColor));
        }

        protected override IEnumerator CoEffect(object targetValue) {
            m_curEffectFinished = false;
            Color target = (Color)targetValue;
            Color prevColor = m_fadeImage.color;
            float curTick = 0f;

            while(true) {
                curTick += Time.deltaTime;
                m_fadeImage.color = Color.Lerp(prevColor, target, curTick / m_fadeDuration);
                if(Mathf.Abs(target.a - m_fadeImage.color.a) <= 0.1f)
                    break;

                yield return null;
            }

            m_fadeImage.color = target;
            m_curEffectFinished = true;
            yield break;
        }

        protected override void ForceInit() {
            m_fadeImage.color = m_fadeOutColor;
        }
    }
}


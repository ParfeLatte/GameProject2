using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Insomnia{
	public class AlertUI : MortalSingleton<AlertUI> {
		[Header("AlertUI: Components")]
		[SerializeField] private Image m_alertImage = null;

		[Header("AlertUI: Status")]
		[SerializeField] private bool m_showingAlert = false;
		[SerializeField] private GameObject m_alertCaller = null;

		[Header("AlertUI: Settings")]
		[SerializeField] private Color m_alertMax;
		[SerializeField] private Color m_alertMin;
		[SerializeField, Range(1f, 3f)] private float m_alertDuration = 1.0f;
		private int m_targetValue = 1;

        #region Properties
		public bool ShowingAlert { get => m_showingAlert; }

        #endregion

        private void Reset() {
			Color redColor = Color.red;
			redColor.a = 0.2f;
			m_alertMax = redColor;
			redColor.a = 0f;
			m_alertMin = redColor;

			m_alertImage = GetComponentInChildren<Image>();
			m_alertImage.raycastTarget = false;
        }

        protected override void Awake() {
			base.Awake();

            Color redColor = Color.red;
            redColor.a = 0.2f;
            m_alertMax = redColor;
            redColor.a = 0f;
            m_alertMin = redColor;
            m_alertImage.raycastTarget = false;
        }

		public void TriggerAlert(GameObject caller) {
			if(m_showingAlert) {
				if(ReferenceEquals(m_alertCaller, caller) == false)
					return;
			}

			if(m_alertCaller == null)
				m_alertCaller = caller;

            m_showingAlert = !m_showingAlert;
			if(m_showingAlert)
				StartCoroutine(CoStartAlert());
        }

		private IEnumerator CoStartAlert() {
			Color startColor = m_alertImage.color;
			while(m_showingAlert) {
				startColor.a = Mathf.Clamp(startColor.a + m_targetValue * Time.deltaTime, 0f, 0.2f);
				if(startColor.a <= 0f || startColor.a >= 0.2f)
					m_targetValue *= -1;

				m_alertImage.color = startColor;
				yield return null;
			}

			m_alertImage.color = m_alertMin;
			m_targetValue = 1;
			yield break;
		}
    }
}

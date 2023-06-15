using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Insomnia{
	[RequireComponent(typeof(BoxCollider2D))]
	public class Snitcher : MonoBehaviour {
		[Header("Snitcher: Components")]
		[SerializeField] private BoxCollider2D m_snitchArea = null;

        [Header("Snitcher: External References")]
        [SerializeField] private static InfiniteSiren m_infSiren = null;

        [Header("Snitcher: Settings")]
        [SerializeField] private int m_snitchFloor = -1;

        private void Awake() {
            m_snitchArea = GetComponent<BoxCollider2D>();
            if(m_infSiren == null)
                m_infSiren = GetComponentInParent<InfiniteSiren>();
        }

        private void OnTriggerEnter2D(Collider2D collision) {
            if(m_infSiren == null)
                return;

            if(m_snitchFloor == -1)
                return;

            if(collision.gameObject.CompareTag("Player") == false)
                return;

            m_infSiren.Snitch(m_snitchFloor);
        }
    }
}

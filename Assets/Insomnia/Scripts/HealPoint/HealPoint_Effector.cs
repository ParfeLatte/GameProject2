using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Insomnia{
    public class HealPoint_Effector : EffectorBase {
        [Header("HealPoint: Components")]
        [SerializeField] private SpriteRenderer m_renderer = null;

        [Header("HealPoint: Settings")]
        [SerializeField, Range(10f, 50f)] private float m_healAmount = 10f;
        [SerializeField, Range(1, 10)] private int m_healLimit = 2;

        #region EffectorBase Functions
        protected override void Awake() {
            base.Awake();
            m_renderer = GetComponentInChildren<SpriteRenderer>();
        }

        protected override void Use(Interactor user) {
            if(m_healLimit <= 0)
                return;

            Player player = user.GetComponent<Player>();
            if(player == null)
                return;

            if(player.isDead)
                return;

            player.RestoreHealth(m_healAmount);
            return;
        }

        #endregion
    }
}

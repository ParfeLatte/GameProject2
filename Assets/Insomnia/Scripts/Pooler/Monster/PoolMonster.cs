using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Insomnia.GiantMob_Speaker;

namespace Insomnia{
	public class PoolMonster : Monster {
		[Header("PoolMonster : External References")]
		[SerializeField] private static MonsterPooler m_pooler = null;

        protected override void Destroy() {
            if(m_pooler != null)
                m_pooler.Release(this);
            base.Destroy();
        }

        protected override void OnWake() {
            if(YDist > 20) {
                Die();
                return;
            }
                
            base.OnWake();
        }
    }
}

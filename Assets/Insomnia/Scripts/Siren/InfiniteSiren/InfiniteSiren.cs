using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Insomnia.BGM_Speaker;
using static Insomnia.InfiniteSiren_Speaker;

namespace Insomnia{
	public class InfiniteSiren : MonoBehaviour {
		[Header("InfiniteSiren: Components")]
		[SerializeField] private InfiniteSiren_Speaker m_speaker = null;

		[Header("InfiniteSiren: Internal References")]
		[SerializeField] private Snitcher[] m_playerSnitchers = null;

		[Header("InfiniteSiren: External References")]
		[SerializeField] private Player m_player = null;
		[SerializeField] private ObjectPooler<Monster> m_pooler = null;
		[SerializeField] private Transform[] m_spawnPoints = null;
		[SerializeField] private List<Monster> m_monsters = new List<Monster>();

		[Header("InfiniteSiren: Status")]
		[SerializeField] private Transform[] m_targetToSpawn = null;

		[Header("InfiniteSiren: Settings")]
		[SerializeField] private int m_spawnDivision = 3;
		[SerializeField, Range(0.2f, 3f)] private float m_avgSpawnInterval = 0.5f;
		[SerializeField, Range(0.1f, 1f)] private float m_diffSpawnInterval = 0.1f;


        private void OnValidate() {
			if(m_spawnPoints == null || m_playerSnitchers == null)
				return;

			if(m_playerSnitchers.Length == 0)
				return;

            m_spawnDivision = m_spawnPoints.Length / m_playerSnitchers.Length;
        }

        public void Snitch(int snitchFloor) {
			if(snitchFloor < 0)
				return;

			if(m_playerSnitchers.Length <= snitchFloor)
				return;

			m_targetToSpawn = new ArraySegment<Transform>(m_spawnPoints, m_spawnDivision * snitchFloor, m_spawnDivision).ToArray();
		}

        private void Awake() {
			m_speaker = GetComponent<InfiniteSiren_Speaker>();
        }

        private void Start() {
			m_pooler = MonsterPooler.Instance;
			if(m_pooler == null)
				return;

			m_monsters = m_pooler.Get(100);
			if(m_monsters.Count == 0)
				return;

			if(m_player == null) {
				Debug.LogError("m_player not exists");
				return;
			}

			if(m_speaker == null)
				return;

			if(AlertUI.Instance == null)
				return;

			AlertUI.Instance.TriggerAlert(gameObject);
			m_speaker.Play((int)InfiniteSirenSounds.Alarm, true);
			BGM_Speaker.Instance.Play((int)BGMSounds.Wave, true);
			StartCoroutine(CoStartInfiniteSiren());
        }

		private IEnumerator CoStartInfiniteSiren() {
			Debug.Log("Starting Infinite Siren.");
            float curTick = 0f;

            while(true) {
				if(m_targetToSpawn == null) {
					yield return null;
					continue;
				}

				if(m_targetToSpawn.Length <= 0) {
					yield return null;
					continue;
				}

                yield return null;

                Monster monster = null;
				for(int i = 0; i < m_monsters.Count; i++) {
					if(m_monsters[i].gameObject.activeSelf == false)
						monster = m_monsters[i];
				}

				if(monster == null) {
					if(m_pooler != null)
						m_monsters.AddRange(m_pooler.Get(100));
				}

				yield return null;

                int spawnMustbeFaster = UnityEngine.Random.Range(0, 100) > 50 ? 1 : -1;
                float spawnInterval = m_avgSpawnInterval + (spawnMustbeFaster * m_diffSpawnInterval);

                yield return null;

                if(curTick < spawnInterval) {
                    while(true) {
                        yield return null;

                        curTick += Time.deltaTime;
                        if(curTick >= spawnInterval)
                            break;
                    }
                }
				curTick = 0f;

				Transform toSpawn = null;
				while(true) {
					if(m_targetToSpawn[UnityEngine.Random.Range(0, m_spawnDivision)] != null) {
                        toSpawn = m_targetToSpawn[UnityEngine.Random.Range(0, m_spawnDivision)];
						break;
                    }
						
					yield return null;
                }

				if(monster == null) {
					yield return null;
                    continue;
                }
					
				monster.player = m_player;
				monster.Player = m_player.gameObject;
				monster.transform.position = toSpawn.position;
				monster.gameObject.SetActive(true);
				monster.MonsterAwake();
				yield return null;
            }
		}
    }
}

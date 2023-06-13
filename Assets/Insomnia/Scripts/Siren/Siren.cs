using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using static Insomnia.Siren_Speaker;

namespace Insomnia {
    public class Siren : MonoBehaviour {
        [Header("Siren: Internal References")]
        [SerializeField] private BoxCollider2D m_sirenRange = null;
        [SerializeField] private Siren_Speaker m_speaker = null;

        [Header("Siren: External References")]
        [SerializeField] private Interactable m_caller = null;
        private List<Monster> m_monstersInArea = new List<Monster>();
        private List<Monster> m_monstersSpawn = new List<Monster>();
        private Player m_target = null;

        [Header("Siren: Settings")]
        [SerializeField] private Transform m_spawnArea = null;
        [SerializeField, Range(10, 100)] private int m_spawnCount = 10;
        [SerializeField] private float m_spawnIntervalMinimum = 0.1f;
        [SerializeField] private float m_spawnIntervalMaximum = 10f;
        [SerializeField, Range(0f, 5f)] private float m_spawnIntervalAverage = 0.6f;
        [SerializeField, Range(0f, 5f)] private float m_spawnIntervalThreshold = 1f;

        private void Awake() {
            m_speaker = GetComponentInChildren<Siren_Speaker>();
        }

        private void OnTriggerEnter2D(Collider2D collision) {
            Monster monster = collision.gameObject.GetComponent<Monster>();
            if(monster == null)
                return;

            if(monster.isDead)
                return;

            if(m_monstersInArea.Contains(monster))
                return;

            if(m_monstersSpawn.Contains(monster)) 
                return;

            m_monstersInArea.Add(monster);
        }

        public void OnAlarmed(Interactable caller) {
            if(caller == null)
                return;

            if(caller.User == null)
                return;

            m_caller = caller;

            StartCoroutine(CoStartSpawnMonster());

            for(int i = 0; i < m_monstersInArea.Count; i++) {
                m_monstersInArea[i].MonsterAwake();
                //TODO: 몬스터에게 플레이어 전달하기
            }

            m_speaker.Play((int)SirenSounds.Alarm, true);
            //TODO: 배경음악 점진적 시작 요청하기
            StartCoroutine(CoStartCheckAllDead());
        }

        private IEnumerator CoStartCheckAllDead() {
            while(true) {
                bool allDead = m_monstersInArea.All(x => x.isDead);
                yield return null;
                allDead &= m_monstersSpawn.All(x => x.isDead);

                if(allDead)
                    break;

                yield return null;
            }
            m_speaker.Stop();
            m_caller.InteractConditionSolved(null);
            //TODO: 배경음악 점진적종료 요청하기
            yield break;
        }

        private IEnumerator CoStartSpawnMonster() {
            float curTick = 0f;
            float timeToSpawn = Mathf.Clamp(m_spawnIntervalAverage + Random.Range(m_spawnIntervalThreshold * -1, m_spawnIntervalThreshold), m_spawnIntervalMinimum, m_spawnIntervalMaximum);

            for(int i = 0; i < m_monstersSpawn.Count; i++) {
                while(true) {
                    if(curTick < timeToSpawn) {
                        curTick += Time.deltaTime;
                        yield return null;
                        continue;
                    }

                    curTick = 0;
                    timeToSpawn = Mathf.Clamp(m_spawnIntervalAverage + Random.Range(m_spawnIntervalThreshold * -1, m_spawnIntervalThreshold), m_spawnIntervalMinimum, m_spawnIntervalMaximum);
                    break;
                }

                m_monstersSpawn[i].transform.position = m_spawnArea.position;
                m_monstersSpawn[i].gameObject.SetActive(true);
                m_monstersSpawn[i].MonsterAwake();
                yield return null;
            }

            yield break;
        }
    }
}

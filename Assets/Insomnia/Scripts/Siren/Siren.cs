using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using static Insomnia.Siren_Speaker;
using static Insomnia.BGM_Speaker;

namespace Insomnia {
    public class Siren : MonoBehaviour {
        [Header("Siren: Internal References")]
        [SerializeField] private Siren_Speaker m_speaker = null;

        [Header("Siren: External References")]
        [SerializeField] private Interactable m_caller = null;
        [SerializeField] private List<Monster> m_monstersInArea = new List<Monster>();
        [SerializeField] private List<Monster> m_monstersSpawn = new List<Monster>();
        private Player m_player = null;
        private static AlertUI m_alertUI = null;

        [Header("Siren: Status")]
        [SerializeField] private bool m_onAlarmed = false;

        [Header("Siren: Settings")]
        [SerializeField] private Transform m_spawnArea = null;
        [SerializeField, Range(10, 100)] private int m_spawnCount = 10;
        [SerializeField] private float m_spawnIntervalMinimum = 0.1f;
        [SerializeField] private float m_spawnIntervalMaximum = 10f;
        [SerializeField, Range(0f, 5f)] private float m_spawnIntervalAverage = 0.6f;
        [SerializeField, Range(0f, 5f)] private float m_spawnIntervalThreshold = 1f;

        private void Awake() {
            m_speaker = GetComponentInChildren<Siren_Speaker>();
            m_player = FindObjectOfType<Player>();
        }

        private void Start() {
            for(int i = 0; i < m_monstersSpawn.Count; i++) {
                m_monstersSpawn[i].Player = m_player.gameObject;
                m_monstersSpawn[i].player = m_player;
            }

            if(AlertUI.Instance == null) 
                gameObject.SetActive(false);

            m_alertUI = AlertUI.Instance;
        }

        public void OnAlarmed(Interactable caller) {
            if(caller == null)
                return;

            if(caller.User == null)
                return;

            m_onAlarmed = true;
            m_caller = caller;

            StartCoroutine(CoStartSpawnMonster());

            for(int i = 0; i < m_monstersInArea.Count; i++) {
                m_monstersInArea[i].MonsterAwake();
            }

            m_speaker.Play((int)SirenSounds.Alarm, true);
            BGM_Speaker.Instance.Play((int)BGMSounds.Wave, true);
            if(m_alertUI != null)
                m_alertUI.TriggerAlert(gameObject);

            StartCoroutine(CoStartCheckAllDead());
        }

        private IEnumerator CoStartCheckAllDead() {
            while(true) {
                bool allDead = m_monstersInArea.All(x => x.isDead);
                allDead &= m_monstersSpawn.All(x => x.isDead);
                
                if(allDead)
                    break;

                yield return null;
            }

            m_onAlarmed = false;
            m_speaker.Stop();
            m_caller.InteractConditionSolved(null);
            BGM_Speaker.Instance.Play((int)BGMSounds.BGM, true);

            Invoke("SetActiveFalseAllMonsters", 1.5f);

            if(m_alertUI != null)
                m_alertUI.TriggerAlert(gameObject);
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
                m_monstersSpawn[i].Player = m_player.gameObject;
                m_monstersSpawn[i].player = m_player;
                m_monstersSpawn[i].MonsterAwake();
                yield return null;
            }

            yield break;
        }

        private void SetActiveFalseAllMonsters() {
            if(m_monstersInArea != null) {
                for(int i = 0; i < m_monstersInArea.Count; i++) {
                    m_monstersInArea[i].gameObject.SetActive(false);
                }
            }

            if(m_monstersSpawn != null) {
                for(int i = 0; i < m_monstersSpawn.Count; i++) {
                    m_monstersSpawn[i].gameObject.SetActive(false);
                }
            }
        }
    }
}

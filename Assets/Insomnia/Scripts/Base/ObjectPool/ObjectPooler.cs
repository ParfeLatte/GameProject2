using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace Insomnia{
	public class ObjectPooler<T> : MortalSingleton<ObjectPooler<T>> where T : MonoBehaviour{
        [Header("ObjectPooler: Internal References")]
        [SerializeField] private GameObject m_poolPrefab = null;
		[SerializeField] private List<T> m_pool = new List<T>();

        [Header("ObjectPooler: Status")]
        [SerializeField, Tooltip("현재 풀링 가능한 오브젝트의 갯수")] private int m_poolableCount = 0;

        [Header("ObjectPooler: Settings")]
        [SerializeField, Tooltip("Generate()시 생성할 오브젝트의 갯수")] private int m_generateCount = 10;

        protected override void Awake() {
            base.Awake();

            for(int i = 0; i < transform.childCount; i++) {
                GameObject child = transform.GetChild(i).gameObject;
                child.SetActive(false);

                T pool = child.GetComponent<T>();
                if(pool == null)
                    continue;

                m_pool.Add(pool);
            }

            m_poolableCount = m_pool.Count;
        }

        /// <summary>
        /// <seealso cref="m_generateCount"/>의 크기 만큼 pool오브젝트를 생성하는 함수.
        /// </summary>
        private void Generate() {
            if(m_poolPrefab == null)
                return;

            if(m_generateCount == 0)
                return;

            T comp = m_poolPrefab.GetComponent<T>();
            if(comp == null)
                return;

            for(int i = 0; i < m_generateCount; i++) {
                GameObject obj = Instantiate(m_poolPrefab);
                obj.SetActive(false);

                T pool = obj.GetComponent<T>();
                m_pool.Add(pool);
                m_poolableCount++;
            }
        }

        /// <summary>
        /// 단일 pool오브젝트를 가져오는 함수.
        /// </summary>
        /// <returns></returns>
        public T Get() {
            if(m_poolableCount <= 0)
                Generate();

            T obj = m_pool.FirstOrDefault(x => x.gameObject.activeSelf == false);
            if(obj != null)
                m_poolableCount--;

            return obj;
        }

        /// <summary>
        /// 여러 개의 pool오브젝트를 가져오는 함수.
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<T> Get(int count) {
            while(true) {
                if(m_poolableCount <= count)
                    Generate();
                else
                    break;
            }

            List<T> ret = new List<T>();
            ret.AddRange(m_pool.Where(x => x.gameObject.activeSelf == false).Take(10).ToList());
            m_poolableCount -= count;

            return ret;
        }

        /// <summary>
        /// 단일 pool오브젝트를 환원하는 함수
        /// </summary>
        /// <param name="obj"></param>
        public void Release(T obj) {
            if(obj == null)
                return;

            if(m_pool.Contains(obj) == false)
                return;

            obj.gameObject.SetActive(false);
            m_poolableCount++;
            return;
        }

        /// <summary>
        /// 여러 개의 pool오브젝트를 환원하는 함수
        /// </summary>
        /// <param name="list"></param>
        public void Release(List<T> list) {
            if(list == null)
                return;

            for(int i = 0; i < list.Count; i++) {
                if(m_pool.Contains(list[i]) == false)
                    continue;

                list[i].gameObject.SetActive(false);
                m_poolableCount++;
            }

            return;
        }
    }
}

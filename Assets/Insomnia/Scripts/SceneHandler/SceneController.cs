using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Insomnia {
    public class SceneController : Singleton<SceneController> {
        private AsyncOperation m_loadNextScene = null;
        private Queue<Action> m_completed = new Queue<Action>();
        private SceneChangeEffect m_changeEffect = null;
        private bool m_isEffectTemp = false;

        #region Properties
        public float Progress {
            get {
                if(m_loadNextScene == null)
                    return 0f;

                return m_loadNextScene.progress + 0.1f;


            }
        }

        public static Queue<Action> LoadSceneCompleted { get => _instance.m_completed; }

        #endregion

        public bool AddSceneChangeEffect(SceneChangeEffect effect) {
            if(m_changeEffect != null)
                return false;

            m_changeEffect = effect;
            m_isEffectTemp = true;
            effect.transform.SetParent(transform);
            return true;
        }

        public bool ChangeSceneTo(string sceneName, bool autoSceneChange = true) {
            if(sceneName == null || sceneName == string.Empty)
                return false;

            StartCoroutine(CoStartLoadScene(sceneName, autoSceneChange));
            return true;
        }

        private IEnumerator CoStartLoadScene(string sceneName, bool autoSceneChange = true) {
            #region Loading Scene
            //이전 씬 가져오기
            Scene prevScene = SceneManager.GetActiveScene();

            //씬 전환 시작 효과
            if(m_changeEffect != null) {
                //있다면 씬 전환 효과 시작.
                m_changeEffect.StartEffect();

                while(true) {
                    yield return null;

                    //씬 전환 효과가 끝났는지 계속 체크. Polling
                    if(m_changeEffect.EffectFinished)
                        break;
                }
            }

            //로딩 씬 로드
            AsyncOperation loading = SceneManager.LoadSceneAsync("Loading", LoadSceneMode.Additive);
            loading.allowSceneActivation = true;

            while(loading.isDone == false) {
                yield return null;
            }

            //Test씬 해제
            AsyncOperation unloadPrevScene = SceneManager.UnloadSceneAsync(prevScene);

            while(unloadPrevScene.isDone == false)
                yield return null;

            prevScene = default;
            yield return null;

            #endregion

            #region 다음 씬 로드
            //로딩씬을 가져온다.
            prevScene = SceneManager.GetActiveScene();

            //씬 전환 종료 효과
            if(m_changeEffect != null) {
                //씬 전환 종료 효과 재생
                m_changeEffect.FinishEffect();

                while(true) {
                    yield return null;

                    if(m_changeEffect.EffectFinished)
                        break;
                }
            }

            //다음 씬 로딩
            m_loadNextScene = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
            m_loadNextScene.allowSceneActivation = false;

            float fakeLoading = 0f;
            while(m_loadNextScene.progress < 0.9f || fakeLoading <= 5f) {
                yield return null;
                fakeLoading += Time.deltaTime;
            }

            //씬 전환 시작 효과
            if(m_changeEffect != null) {
                //있다면 씬 전환 효과 시작.
                m_changeEffect.StartEffect();

                while(true) {
                    yield return null;

                    //씬 전환 효과가 끝났는지 계속 체크. Polling
                    if(m_changeEffect.EffectFinished)
                        break;
                }
            }

            m_loadNextScene.allowSceneActivation = true;

            //for(int i = 0; i < m_completed.Count; i++) {
            //    m_completed.Dequeue().Invoke();
            //    yield return null;
            //}

            AsyncOperation unloadLoading = SceneManager.UnloadSceneAsync(prevScene);

            if(unloadLoading != null) {
                while(unloadLoading.isDone == false)
                    yield return null;
            }

            //씬 전환 종료 효과
            if(m_changeEffect != null) {
                m_changeEffect.FinishEffect();

                while(true) {
                    yield return null;

                    if(m_changeEffect.EffectFinished)
                        break;
                }

                //만약 일시적 효과였을 경우 제거한다.
                if(m_changeEffect.IsTemporal) {
                    m_changeEffect.gameObject.SetActive(false);
                    m_changeEffect.transform.SetParent(null);
                    m_changeEffect = null;
                }
            }

            yield break;
            #endregion
        }
    }
}


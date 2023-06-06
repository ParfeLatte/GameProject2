using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using static Insomnia.Defines;

namespace Insomnia {
    public class SceneController : ImmortalSingleton<SceneController> {
        private AsyncOperation m_loadNextScene = null;
        private Queue<Action> m_completed = new Queue<Action>();
        private SceneChangeEffect m_changeEffect = null;
        private SceneChangeEffect m_waitingEffect = null;
        private bool m_isLoading = false;

        #region Properties
        public float Progress {
            get {
                if(m_loadNextScene == null)
                    return 0f;

                return m_loadNextScene.progress + 0.1f;


            }
        }
        public bool IsLoading { get => m_isLoading; }

        public SceneChangeEffect Effect {
            get => m_changeEffect;
            set {
                if(value == null) {
                    m_changeEffect = m_waitingEffect;
                    m_waitingEffect = null;
                    if(m_changeEffect != null)
                        m_changeEffect.gameObject.SetActive(true);
                }
                else {
                    if(m_changeEffect == null) 
                        m_changeEffect = value;
                    else 
                        m_waitingEffect = value;
                }
            }
        }

        public static Queue<Action> LoadSceneCompleted { get => _instance.m_completed; }

        #endregion

        public bool AddSceneChangeEffect(SceneChangeEffect effect) {
            Effect = effect;
            effect.transform.SetParent(transform);

            return ReferenceEquals(Effect, effect);
        }

        public bool ChangeSceneTo(string sceneName, bool skipLoadingScene = false, bool autoSceneChange = true) {
            if(sceneName == null || sceneName == string.Empty)
                return false;

            StartCoroutine(CoStartLoadScene(sceneName, skipLoadingScene, autoSceneChange));
            return true;
        }

        private IEnumerator CoStartLoadScene(string sceneName, bool skipLoadingScene = false, bool autoSceneChange = true) {
            m_isLoading = true;
            Scene prevScene = default;

            #region Loading Scene
            if(skipLoadingScene == false) {
                prevScene = SceneManager.GetActiveScene();

                //�� ��ȯ ȿ�� ����
                if(Effect != null) {
                    //�ִٸ� �� ��ȯ ȿ�� ����.
                    Effect.StartEffect();

                    while(true) {
                        yield return null;

                        //�� ��ȯ ȿ���� �������� ��� üũ. Polling
                        if(Effect.EffectFinished)
                            break;
                    }
                }

                //���� �� �ε�
                AsyncOperation loading = SceneManager.LoadSceneAsync("Loading", LoadSceneMode.Single);
                loading.allowSceneActivation = false;

                while(loading.isDone == false) {
                    if(loading.progress >= 0.9f) {
                        loading.allowSceneActivation = true;
                        break;
                    }
                    yield return null;
                }

                //���� �� ����
                AsyncOperation unloadPrevScene = SceneManager.UnloadSceneAsync(prevScene);

                if(unloadPrevScene != null) {
                    while(unloadPrevScene.isDone == false)
                        yield return null;
                }

                //�� ��ȯ ȿ�� ����
                if(Effect != null) {

                    Effect.FinishEffect();

                    while(true) {
                        yield return null;

                        if(Effect.EffectFinished)
                            break;
                    }
                }
            }
            #endregion

            #region ���� �� �ε�

            prevScene = SceneManager.GetActiveScene();

            //���� �� �ε�
            m_loadNextScene = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
            m_loadNextScene.allowSceneActivation = false;

            float fakeLoading = 0f;
            while(m_loadNextScene.progress < 0.9f || fakeLoading <= 5f) {
                if(m_loadNextScene.progress >= 0.9f && fakeLoading > 5f)
                    break;

                yield return null;
                fakeLoading += Time.deltaTime;
            }

            //�� ��ȯ ȿ�� ����
            if(Effect != null) {
                //�ִٸ� �� ��ȯ ȿ�� ����.
                Effect.StartEffect();

                while(true) {
                    yield return null;

                    //�� ��ȯ ȿ���� �������� ��� üũ. Polling
                    if(Effect.EffectFinished)
                        break;
                }
            }

            m_loadNextScene.allowSceneActivation = true;

            //���� �� ����
            AsyncOperation unloadLoading = SceneManager.UnloadSceneAsync(prevScene);

            if(unloadLoading != null) {
                while(unloadLoading.isDone == false) {
                    if(unloadLoading.progress >= 0.9f)
                        break;
                }
            }

            while(m_completed.Count > 0) {
                Action action = m_completed.Dequeue();
                action?.Invoke();
            }

            m_completed.Clear();

            //�� ��ȯ ȿ�� ����
            if(Effect != null) {
                Effect.FinishEffect();

                while(true) {
                    yield return null;

                    if(m_changeEffect.EffectFinished)
                        break;
                }

                //���� �Ͻ��� ȿ������ ��� �����Ѵ�.
                if(Effect.IsTemporal) {
                    Destroy(Effect.gameObject);
                    Effect = null;
                }
            }

            m_isLoading = false;
            yield break;
            #endregion
        }
    }
}


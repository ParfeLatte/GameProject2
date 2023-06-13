using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Insomnia {
    public abstract class ImmortalSubject<SingletonType, ObserverType> : ImmortalSingleton<SingletonType>
            where SingletonType : Component
            where ObserverType : Observer {
        [SerializeField] protected List<ObserverType> m_observers = new List<ObserverType>();

        public bool Subscribe(ObserverType observer) {
            if(observer == null)
                return false;

            if(m_observers.Contains(observer))
                return false;

            m_observers.Add(observer);
            Notify(observer);
            return true;
        }

        public bool Unsubscribe(ObserverType observer) {
            if(observer == null)
                return false;

            if(m_observers.Contains(observer) == false)
                return false;

            m_observers.Remove(observer);
            return true;
        }

        public virtual void OnUpdate() {
            if(m_observers == null)
                return;

            for(int i = 0; i < m_observers.Count; i++) {
                if(m_observers[i] == null)
                    continue;

                Notify(m_observers[i]);
            }
        }

        protected abstract void Notify(ObserverType observer);
    }

    public abstract class MortalSubject<SingletonType, ObserverType> : MortalSingleton<SingletonType>
        where SingletonType : Component
        where ObserverType : Observer {
        protected List<ObserverType> m_observers = new List<ObserverType>();

        public bool Subscribe(ObserverType observer) {
            if(observer == null)
                return false;

            if(m_observers.Contains(observer))
                return false;

            m_observers.Add(observer);
            Notify(observer);
            return true;
        }

        public bool Unsubscribe(ObserverType observer) {
            if(observer == null)
                return false;

            if(m_observers.Contains(observer) == false)
                return false;

            m_observers.Remove(observer);
            return true;
        }

        public void OnUpdate() {
            if(m_observers == null)
                return;

            if(m_observers.Count <= 0)
                return;

            for(int i = 0; i < m_observers.Count; i++) {
                if(m_observers[i] == null)
                    continue;

                Notify(m_observers[i]);
            }
        }

        protected virtual void OnDestroy() {
            m_observers.Clear();
            m_observers = null;
        }

        protected abstract void Notify(ObserverType observer);
    }
}


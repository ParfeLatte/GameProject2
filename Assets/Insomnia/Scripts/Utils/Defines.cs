using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Insomnia {
    [Serializable]
    public enum SceneType {
        Main,
        Tutorial,
        Lab,
        Loading,
        EscapeFailed,
    }


    public class Defines : MonoBehaviour {
        #region Enums
        public enum ObjectType {
            None,
            Terminal,       //터미널
            Security,       //보안문?
            Resources,      //순수 아이템
            Passage,        //일반 문?
            Storage,        //아이템 보관함
        }

        public enum StatusType {
            None,
            Normal,

        }

        public enum CommandError {
            Success = 0,
            Failed = 1,
            Loading = 2,
            SyntaxError = 3,
            SyntaxError_ExceptionalParam = 4,
            NoDataFound = 5,
            InvalidID = 6,

        }

        public enum SoundType : int {
            Master = 0,
            BGM,
            SFX,
        }

        public enum ControllerSignResult : int { Negative = -1, Positive = 1 }

        #endregion

        #region Structs
        public struct ObjectData {
            public string ID;
            public string Location;
            public string Description;
            public Vector3 Position;
            public ObjectType ObjectType;
            public StatusType Status;
        }

        public struct SoundNotiData{
            public float[] volumes;
        }

        #endregion

        #region Interfaces
        public interface ISceneChangeEffector {
            void OnSceneChangeStart(IEnumerator<Action> coroutine);
            void OnSceneChangeFinish();
        }

        public interface IDataIO {
            public void SaveData();
            public void LoadData();
            public void RemoveData();
        }

        #endregion


    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Insomnia {
    public class Defines : MonoBehaviour {
        #region Enums
        public enum ObjectType {
            None,
            Terminal,       //�͹̳�
            Security,       //���ȹ�?
            Resources,      //���� ������
            Passage,        //�Ϲ� ��?
            Storage,        //������ ������
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

        #endregion

        #region Interfaces
        public interface ISceneChangeEffector {
            void OnSceneChangeStart(IEnumerator<Action> coroutine);
            void OnSceneChangeFinish();
        }

        #endregion







    }
}
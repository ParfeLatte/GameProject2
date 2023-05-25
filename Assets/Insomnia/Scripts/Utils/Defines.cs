using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Insomnia {
    public class Defines : MonoBehaviour {
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

        public struct ItemData {
            public string ID;
            public string Location;
            public string Description;
            public Vector3 Position;
            public ObjectType ObjectType;
            public StatusType Status;
        }
    }
}
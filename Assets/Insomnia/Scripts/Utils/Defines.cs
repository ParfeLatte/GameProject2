using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Insomnia {
    public class Defines : MonoBehaviour {
        public enum ObjectType {
            Terminal,
            Security,
            Resources,
            Passage,
            Storage,

        }
        public enum StatusType {
            Normal,

        }
        public struct ItemData {
            public string ID;
            public string Location;
            public Vector3 Pos;
            public ObjectType ObjectType;
            public StatusType Status;
        }
    }
}
using Insomnia;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Insomnia.Defines;

namespace Insomnia {
    public class Item : SearchableBase {

        private void Start() {
            ItemManager.Instance.AddSearchable(this);
        }
    }
}
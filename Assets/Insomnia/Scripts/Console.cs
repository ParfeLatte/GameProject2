using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Insomnia {
    public class Console : Interactable {
        [SerializeField] private SpriteRenderer _lapTop = null;
        [SerializeField] private Sprite[] _lapTopSprites = null;

        protected override void Awake() {
            base.Awake();
            _lapTop = GetComponentInChildren<SpriteRenderer>();
        }

        public override void StandbyInteract() {
            _lapTop.sprite = _lapTopSprites[1];
        }

        public override void ReleaseInteract() {
            _lapTop.sprite = _lapTopSprites[0];
        }

        public override void OnInteractStart() {
            //TODO: UI���� ����ֱ�
        }

        public override void OnInteractEnd() {
            //TODO: ������ ���ϱ�
        }
    }
}

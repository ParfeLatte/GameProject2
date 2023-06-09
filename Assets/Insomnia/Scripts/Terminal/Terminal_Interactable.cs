using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Insomnia {
    public class Terminal_Interactable : Interactable {
        [SerializeField] private SpriteRenderer _lapTop = null;
        [SerializeField] private Sprite[] _lapTopSprites = null;
        [SerializeField] private TerminalUI m_terminalUI = null;

        protected override void Awake() {
            base.Awake();
        }

        public override void StandbyInteract() {
            _lapTop.sprite = _lapTopSprites[1];
        }

        public override void ReleaseInteract() {
            _lapTop.sprite = _lapTopSprites[0];
        }

        public override bool OnInteractStart() {
            m_terminalUI.OpenTerminal();
            onInteractStart?.Invoke();
            return false;
        }

        public override void OnInteractEnd() {
            m_terminalUI.CloseTerminal();
            onInteractEnd?.Invoke();
        }
    }
}


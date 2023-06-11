using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Insomnia {
    public class Terminal_Interactable : Interactable {
        [Header("Terminal_Interactable: Components")]
        [SerializeField] private SpriteRenderer _lapTop = null;
        [SerializeField] private Sprite[] _lapTopSprites = null;

        [Header("Terminal_Interactable: References")]
        [SerializeField] private TerminalUI m_terminalUI = null;

        protected override void Awake() {
            base.Awake();
        }

        public override void StandbyInteract() {
            if(m_canInteract == false)
                return;

            _lapTop.sprite = _lapTopSprites[1];
        }

        public override void ReleaseInteract() {
            if(m_canInteract == false)
                return;

            _lapTop.sprite = _lapTopSprites[0];
        }

        public override bool OnInteractStart() {
            if(m_canInteract == false)
                return true;

            m_terminalUI.OpenTerminal();
            onInteractStart?.Invoke(this);
            return false;
        }

        public override void OnInteractEnd() {
            m_terminalUI.CloseTerminal();
            onInteractEnd?.Invoke(this);
        }
    }
}


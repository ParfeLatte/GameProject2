using Insomnia;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Terminal : SearchableBase {
    [SerializeField] private TerminalUI m_terminalUI = null;
    [SerializeField] private Terminal_Interactable m_interactable = null;

    public TerminalUI UI { get => m_terminalUI; }
    public Terminal_Interactable Interactable { get => m_interactable; }

    protected override void Awake() {
        base.Awake();
        m_terminalUI.SetTerminal(this);
    }

    protected override void Start() {
        base.Start();
    }
}

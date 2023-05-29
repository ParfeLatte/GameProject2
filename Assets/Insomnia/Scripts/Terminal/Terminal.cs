using Insomnia;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Terminal : SearchableBase {
    [SerializeField] private TerminalUI m_terminalUI = null;

    public TerminalUI UI { get => m_terminalUI; }

    protected override void Awake() {
        base.Awake();
        m_terminalUI.SetTerminal(this);
    }

    private void Start() {
        ItemManager.Instance.AddSearchable(this);
    }
}

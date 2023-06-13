using Insomnia;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Insomnia.Defines;

public class Terminal_Searchable : SearchableBase {
    [Header("Terminal: External References")]
    [SerializeField] private TerminalUI m_terminalUI = null;
    [SerializeField] private Terminal_Interactable m_interactable = null;

    public TerminalUI UI { get => m_terminalUI; }
    public Terminal_Interactable Interactable { get => m_interactable; }

    protected override void Reset() {
        m_IDFormat = "TERMINAL";
        m_Location = "ZONE_";
        m_ObjectType = ObjectType.Terminal;
        m_Status = StatusType.Normal;
    }

    protected override void Awake() {
        base.Awake();
        m_terminalUI.SetTerminal(this);
    }

    protected override void Start() {
        base.Start();
    }
}

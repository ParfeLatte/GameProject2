using Insomnia;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Insomnia {
    public abstract class TerminalCommand : ScriptableObject {

        [SerializeField] protected string m_command = "";
        [SerializeField] protected float m_loadingTime = 0;
        [SerializeField, Multiline(10)] protected string m_Description = string.Empty;
        protected List<string> m_commandResult = new List<string>();

        [SerializeField] private UnityEvent onCommandSuccess = null;

        public string Command { get => m_command; }
        public float LoadingTime { get => m_loadingTime; }
        public string Description { get => m_Description; }

        public virtual bool ContainsKeyword(string comparand) {
            return m_command.Contains(comparand);
        }

        /// <summary>
        /// �Է��� Ŀ�ǵ��� ù��° ��ū�� ���ϴ� �Լ�.
        /// </summary>
        /// <param name="command">splitted[0] ���� �ʼ�</param>
        /// <returns>return true if command is valid. else false.</returns>
        public virtual bool CheckCommand(string splitted) {
            return m_command.Equals(splitted);
        }

        /// <summary>
        /// �Է��� Ŀ�ǵ� ��ü�� �о� ����� �����ϴ� �Լ�.
        /// </summary>
        /// <param name="command">�Էµ� ��ɾ� ��ü.</param>
        /// <returns>returns <see cref="m_loadingTime"/> : �ε� �ð� ��ȯ.</returns>
        public abstract IEnumerator<KeyValuePair<float, List<string>>> RunCommand(Terminal terminal, string command);
    }
}
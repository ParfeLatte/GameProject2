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

        /// <summary>
        /// �Է��� Ŀ�ǵ��� ù��° ��ū�� ���ϴ� �Լ�.
        /// </summary>
        /// <param name="command">splitted[0] ���� �ʼ�</param>
        /// <returns>return true if command is valid. else false.</returns>
        public virtual bool CheckCommand(string splitted) {
            return m_command.Equals(splitted.ToUpper());
        }

        /// <summary>
        /// Ŀ�ǵ忡 ���� ������ ��ȯ�ϴ� �Լ�.
        /// </summary>
        /// <returns></returns>
        public virtual string GetDescription() {
            return m_Description;
        }

        /// <summary>
        /// �Է��� Ŀ�ǵ� ��ü�� �о� ����� �����ϴ� �Լ�.
        /// </summary>
        /// <param name="command">�Էµ� ��ɾ� ��ü.</param>
        /// <returns>returns <see cref="m_loadingTime"/> : �ε� �ð� ��ȯ.</returns>
        public abstract KeyValuePair<float, List<string>> RunCommand(TerminalUI console, string command);
    }
}
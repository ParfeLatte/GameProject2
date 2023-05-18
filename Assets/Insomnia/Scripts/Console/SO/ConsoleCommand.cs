using Insomnia;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Insomnia {
    public abstract class ConsoleCommand : ScriptableObject {

        [SerializeField] protected string m_command = "";
        [SerializeField] protected float m_loadingTime = 0;
        protected List<string> m_commandResult = new List<string>();

        /// <summary>
        /// �Է��� Ŀ�ǵ��� ù��° ��ū�� ���ϴ� �Լ�.
        /// </summary>
        /// <param name="command">splitted[0] ���� �ʼ�</param>
        /// <returns>return true if command is valid. else false.</returns>
        public virtual bool CheckCommand(string splitted) {
            return m_command.Equals(splitted.ToUpper());
        }

        /// <summary>
        /// �Է��� Ŀ�ǵ� ��ü�� �о� ����� �����ϴ� �Լ�.
        /// </summary>
        /// <param name="command">�Էµ� ��ɾ� ��ü.</param>
        /// <returns>returns <see cref="m_loadingTime"/> : �ε� �ð� ��ȯ.</returns>
        public abstract KeyValuePair<float, List<string>> RunCommand(TerminalUI console, string command);
    }
}
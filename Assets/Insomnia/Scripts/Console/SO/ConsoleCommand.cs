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
        /// 입력한 커맨드의 첫번째 토큰을 비교하는 함수.
        /// </summary>
        /// <param name="command">splitted[0] 전달 필수</param>
        /// <returns>return true if command is valid. else false.</returns>
        public abstract bool CheckCommand(string splitted);

        /// <summary>
        /// 입력한 커맨드 전체를 읽어 명령을 실행하는 함수.
        /// </summary>
        /// <param name="command">입력된 명령어 전체.</param>
        /// <returns>returns <see cref="m_loadingTime"/> : 로딩 시간 반환.</returns>
        public abstract KeyValuePair<float, List<string>> RunCommand(ConsoleUI console, string command);
    }
}
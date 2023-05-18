using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Insomnia {
    enum ProcessErrorType {
        Success = 0,
        Failed = 1,
        Loading = 2,
        SyntaxError = 3,
    }

    public class TerminalUI : MonoBehaviour {
        #region Elements
        private TMP_InputField m_consoleInput = null;
        private TMP_Text m_consoleWindow = null;

        #endregion

        #region Variables
        [SerializeField] private List<ConsoleCommand> m_commands = new List<ConsoleCommand>();
        private List<string> commandContainer = new List<string>();
        private readonly int MaxCommandLines = 32;
        private int m_selectedIndex = 0;
        private int m_maxIndex = 0;
        private bool m_loading = false;

        private WaitForSeconds m_loadingWait = new WaitForSeconds(0.2f);
        #endregion

        #region Command Formats
        

        #endregion

        private void Awake() {
            m_consoleInput = GetComponentInChildren<TMP_InputField>();
            m_consoleWindow = GetComponentInChildren<TMP_Text>();
        }

        private void OnEnable() {
            m_consoleInput.ActivateInputField();
        }

        public void OnKeyDown_Enter() {
            string command = m_consoleInput.text;
            m_consoleInput.text = null;
            m_consoleInput.ActivateInputField();

            //TODO: 텍스트 리스트에 넣어주기
            commandContainer.Add(command);
            CalculateLastIndex();

            ProcessErrorType success = ProcessCommand(command);
            if(success == ProcessErrorType.SyntaxError) commandContainer.Add($"<color=red>Syntax Error: Check the Command - {command.Split('\n')[0]}</color>");
            if(success == ProcessErrorType.Loading    ) commandContainer.Add($"<color=yellow>Sequence Error: Wait for the Signal : </color>");
            if(success == ProcessErrorType.Failed     ) commandContainer.Add($"<color=red>Command Failed: ^%!$@#!^%@$#%^!$@#</color>");

            DisplayCommand();
        }

        /// <summary>
        /// 가리킬 커맨드의 인덱스와 전체 커맨드의 마지막 인덱스를 초기화한다.
        /// </summary>
        private void CalculateLastIndex() {
            m_selectedIndex = m_maxIndex = commandContainer.Count - 1;
        }

        /// <summary>
        /// 현재까지 입력된 커맨드중 제일 최근 내용 32개를 출력하는 함수
        /// </summary>
        private void DisplayCommand() {
            if(commandContainer.Count <= 0) {
                m_consoleWindow.text = "";
                return;
            }

            int index = Mathf.Max(0, commandContainer.Count - MaxCommandLines);
            int count = commandContainer.Count < MaxCommandLines ? commandContainer.Count : MaxCommandLines;

            string res = "";

            for(int i = index; i < index + count; i++) {
                res += commandContainer[i] + '\n';
            }

            m_consoleWindow.text = res;
        }

        /// <summary>
        /// 위쪽 방향키가 입력되었을 때 실행되는 함수. 이전 커맨드를 불러온다.
        /// 최초 커맨드에서 더 이상 움직이지 않는다.
        /// </summary>
        public void OnKeyDown_ArrowUp() {
            if(m_selectedIndex <= 0)
                return;


        }

        /// <summary>
        /// 아래쪽 방향키가 입력되었을 때 실행되는 함수. 지금보다 이후의 커맨드를 불러온다.
        /// 커맨드가 없을 경우 공란으로 둔다.
        /// </summary>
        public void OnKeyDown_ArrowDown() {
            if(m_selectedIndex == m_maxIndex)
                return;


        }

        /// <summary>
        /// 입력받은 명령어를 처리한다.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        private ProcessErrorType ProcessCommand(string command) {
            bool syntaxError = false;
            string[] splitted = command.ToUpper().Split(' ');

            if(m_loading && splitted[0] != "/EXIT")
                return ProcessErrorType.Loading;

            if(splitted[0].StartsWith('/')) {
                switch(splitted[0]) {
                    case "/COMMAND": {
                        Debug.Log("command list");
                    }
                    break;
                    case "/EXIT": {
                        Debug.Log("exit command");
                        CloseConsole();
                    }
                    break;
                    default: return ProcessErrorType.SyntaxError;
                }
            }
            else {
                ConsoleCommand activeCommand = m_commands.Single(x => x.CheckCommand(splitted[0]) == true);
                if(activeCommand == null)
                    return ProcessErrorType.SyntaxError;

                KeyValuePair<float, List<string>> result = activeCommand.RunCommand(this, command);
                StartCoroutine(CoLoadData(result));
            }

            return ProcessErrorType.Success;
        }

        /// <summary>
        /// 로딩이 필요한 커맨드들에 로딩효과를 적용해주는 함수
        /// </summary>
        /// <param name="loadingTime"></param>
        /// <returns></returns>
        private IEnumerator CoLoadData(KeyValuePair<float, List<string>> result) {
            float processInterval = 0;
            int loadingCommand = m_maxIndex;
            string commandOrigin = new string(commandContainer[loadingCommand]);
            m_loading = true;

            while(processInterval <= result.Key) {
                yield return null;
                processInterval += Time.deltaTime;
                commandContainer[loadingCommand] = MakeProcessVisual(commandOrigin, (int)Mathf.Round(processInterval));
                DisplayCommand();
            }
            if(result.Value != null)
                commandContainer.AddRange(result.Value);
            commandContainer[loadingCommand] = commandOrigin;
            DisplayCommand();
            m_loading = false;
            Debug.Log("Command Finished");
            yield break;
        }

        /// <summary>
        /// 제일 최근 커맨드의 뒤에 로딩 효과를 적용하는 함수
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="percentage"></param>
        /// <returns></returns>
        //private string MakeProcessVisual(string origin, int percentage) {
        //    char[] val = new char[10];
        //    for(int i = 0; i < val.Length; i++) {
        //        if(percentage / ( ( i + 1 ) * 10 ) != 0)
        //            val[i] = '#';
        //        else
        //            val[i] = '.';
        //    }

        //    return origin + $" - [{val.ArrayToString()}]";
        //}

        private string MakeProcessVisual(string origin, int processInterval) {
            char[] loadingIcon = { '\\', '|', '/', '―'};
            return origin + $" - {loadingIcon[processInterval % loadingIcon.Length]}";
        }

        public void OpenConsole() {
            gameObject.SetActive(true);
        }

        public void CloseConsole() {
            gameObject.SetActive(false);
        }
    }
}


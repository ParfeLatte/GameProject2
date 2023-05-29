using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Insomnia.Defines;

namespace Insomnia {
    public class TerminalUI : MonoBehaviour {
        #region Components
        private Terminal m_parentTerminal = null;

        #endregion

        #region Elements
        private TMP_InputField m_consoleInput = null;
        private TMP_Text m_consoleWindow = null;

        #endregion

        #region Variables
        [SerializeField] private List<TerminalCommand> m_commands = new List<TerminalCommand>();
        private List<string> commandContainer = new List<string>();
        private readonly int MaxCommandLines = 32;
        private int m_selectedIndex = 0;
        private int m_maxIndex = 0;
        private bool m_loading = false;
        #endregion

        #region Properties
        public List<TerminalCommand> Commands { get => m_commands; }
        public List<string> Container { get => commandContainer; }

        #endregion

        private void Awake() {
            m_consoleInput = GetComponentInChildren<TMP_InputField>();
            m_consoleWindow = GetComponentInChildren<TMP_Text>();
        }

        public void SetTerminal(Terminal terminal) {
            if(terminal == null)
                return;

            m_parentTerminal = terminal;
        }

        public void OnKeyDown_Enter() {
            string command = m_consoleInput.text.ToUpper();
            m_consoleInput.text = null;
            m_consoleInput.ActivateInputField();

            //TODO: 텍스트 리스트에 넣어주기
            commandContainer.Add(command);
            CalculateLastIndex();

            CommandError success = ProcessCommand(command);
            if(success == CommandError.SyntaxError) commandContainer.Add($"<color=red>Syntax Error: Check the Command - {command.Split(' ')[0]}</color>");
            if(success == CommandError.Loading    ) commandContainer.Add($"<color=yellow>Command Timeout: Wait for the Signal : </color>");
            if(success == CommandError.Failed     ) commandContainer.Add($"<color=red>Command Failed: ^%!$@#!^%@$#%^!$@#</color>");

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

            int startIndex = Mathf.Max(0, commandContainer.Count - MaxCommandLines);
            int count = Mathf.Clamp(commandContainer.Count, 0, MaxCommandLines);

            string res = "";

            for(int i = startIndex; i < startIndex + count; i++) {
                res += (commandContainer[i] + '\n');
            }
            string[] test = res.Split('\n');
            int skipCount = Mathf.Clamp(test.Length - MaxCommandLines, 0, MaxCommandLines);
            string[] skipped = test.Skip(skipCount).ToArray();
            res = "";
            for(int i = 0; i < skipped.Length; i++) {
                res += skipped[i] + '\n';
            }
            m_consoleWindow.text = res;
        }

        /// <summary>
        /// 입력받은 명령어를 처리한다.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        private CommandError ProcessCommand(string command) {
            string[] splitted = command.Split(' ');
            if(splitted[0].Equals("EXIT")) {
                StopAllCoroutines();
                m_loading = false;
            }

            if(m_loading && splitted[0].Equals("EXIT") == false)
                return CommandError.Loading;

            TerminalCommand validCommand = m_commands.SingleOrDefault(x => x.CheckCommand(splitted[0]));
            //Single은 무조건 하나가 있어야 되지만 SingleOrDefault는 하나만 있거나 없을 수 있다. 없을 경우 null을 return;

            if(command == string.Empty) 
                return CommandError.Success;

            if(validCommand == null && command != string.Empty)
                return CommandError.SyntaxError;

            IEnumerator<KeyValuePair<float, List<string>>> result = validCommand.RunCommand(m_parentTerminal, command);

            StartCoroutine(CoLoadData(result));                

            return CommandError.Success;
        }

        /// <summary>
        /// 로딩이 필요한 커맨드들에 로딩효과를 적용해주는 함수
        /// </summary>
        /// <param name="loadingTime"></param>
        /// <returns></returns>
        private IEnumerator CoLoadData(IEnumerator<KeyValuePair<float, List<string>>> result) {
            float processInterval = 0f;
            int loadingCommand = m_maxIndex;
            string commandOrigin = new string(commandContainer[loadingCommand]);

            while(true) {
                if(result.MoveNext() == false)
                    break;

                KeyValuePair<float, List<string>> current = result.Current;
                processInterval = 0f;
                if(current.Key >= 0.1f)
                    m_loading = true;


                while(processInterval <= current.Key) {
                    yield return null;
                    processInterval += Time.deltaTime;
                    if(commandContainer.Count != 0)
                        commandContainer[loadingCommand] = MakeProcessVisual(commandOrigin, (int)Mathf.Round(processInterval));
                    DisplayCommand();
                }

                if(current.Value != null) {
                    if(current.Value.Count > 0)
                        commandContainer.AddRange(current.Value);
                }

                DisplayCommand();
            }

            if(commandContainer.Count != 0)
                commandContainer[loadingCommand] = commandOrigin;
            DisplayCommand();
            m_loading = false;
        }

        private string MakeProcessVisual(string origin, int processInterval) {
            char[] loadingIcon = { '\\', '|', '/', '―'};
            return origin + $" - {loadingIcon[processInterval % loadingIcon.Length]}";
        }

        public void OpenTerminal() {
            gameObject.SetActive(true);
            m_consoleInput.ActivateInputField();
        }

        public void CloseTerminal() {
            gameObject.SetActive(false);
        }
    }
}


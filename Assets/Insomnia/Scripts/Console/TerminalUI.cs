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
        [SerializeField] private List<TerminalCommand> m_commands = new List<TerminalCommand>();
        private List<string> commandContainer = new List<string>();
        private readonly int MaxCommandLines = 32;
        private int m_selectedIndex = 0;
        private int m_maxIndex = 0;
        private bool m_loading = false;
        #endregion

        private void Awake() {
            m_consoleInput = GetComponentInChildren<TMP_InputField>();
            m_consoleWindow = GetComponentInChildren<TMP_Text>();
        }

        public void OnKeyDown_Enter() {
            string command = m_consoleInput.text;
            m_consoleInput.text = null;
            m_consoleInput.ActivateInputField();

            //TODO: �ؽ�Ʈ ����Ʈ�� �־��ֱ�
            commandContainer.Add(command);
            CalculateLastIndex();

            ProcessErrorType success = ProcessCommand(command);
            if(success == ProcessErrorType.SyntaxError) commandContainer.Add($"<color=red>Syntax Error: Check the Command - {command.Split(' ')[0]}</color>");
            if(success == ProcessErrorType.Loading    ) commandContainer.Add($"<color=yellow>Command Timeout: Wait for the Signal : </color>");
            if(success == ProcessErrorType.Failed     ) commandContainer.Add($"<color=red>Command Failed: ^%!$@#!^%@$#%^!$@#</color>");

            DisplayCommand();
        }

        /// <summary>
        /// ����ų Ŀ�ǵ��� �ε����� ��ü Ŀ�ǵ��� ������ �ε����� �ʱ�ȭ�Ѵ�.
        /// </summary>
        private void CalculateLastIndex() {
            m_selectedIndex = m_maxIndex = commandContainer.Count - 1;
        }

        /// <summary>
        /// ������� �Էµ� Ŀ�ǵ��� ���� �ֱ� ���� 32���� ����ϴ� �Լ�
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
        /// �Է¹��� ��ɾ ó���Ѵ�.
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

                        for(int i = 0; i < m_commands.Count; i++) {
                            commandContainer.Add(m_commands[i].GetDescription());
                        }
                        DisplayCommand();
                    } break;
                    case "/EXIT": {
                        Debug.Log("exit command");
                        CloseTerminal();
                    } break;
                    case "/CLEAR": {
                        commandContainer.Clear();
                        DisplayCommand();
                    }break;
                    default: return ProcessErrorType.SyntaxError;
                }
            }
            else {
                TerminalCommand validCommand = m_commands.SingleOrDefault(x => x.CheckCommand(splitted[0]));
                //Single�� ������ �ϳ��� �־�� ������ SingleOrDefault�� �ϳ��� �ְų� ���� �� �ִ�. ���� ��� null�� return;
                if(validCommand == null)
                    return ProcessErrorType.SyntaxError;

                Debug.Assert(validCommand != null, "ValidCommand is null");
                KeyValuePair<float, List<string>> result = validCommand.RunCommand(this, command);
                StartCoroutine(CoLoadData(result));
            }

            return ProcessErrorType.Success;
        }

        /// <summary>
        /// �ε��� �ʿ��� Ŀ�ǵ�鿡 �ε�ȿ���� �������ִ� �Լ�
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

        private string MakeProcessVisual(string origin, int processInterval) {
            char[] loadingIcon = { '\\', '|', '/', '��'};
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


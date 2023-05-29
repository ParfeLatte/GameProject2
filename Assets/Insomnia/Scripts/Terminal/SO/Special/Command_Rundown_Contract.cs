using Insomnia;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Insomnia {
    [CreateAssetMenu(menuName ="Command/Special/Rundown_Contract", fileName ="Command_Rundown_Contract")]
    public class Command_Rundown_Contract : TerminalCommand {
        private static string m_rundown_invalidRundownError = "<color=red>Error: Invalid Rundown - There is no Rundown /{{0}/}.</color>";
        private static string m_rundown_toomuchKeyError = "<color=red>Error: Too Much Key - Key should be one.</color>";
        private static string m_rundown_lackofKeyError = "<color=red>Error: Lack Of Key.</color>";
        private static string m_rundown_Loading = "Loading for {0}...";

        public override IEnumerator<KeyValuePair<float, List<string>>> RunCommand(Terminal terminal, string command) {
            m_commandResult.Clear();

            string[] keys = command.Split(' ');
            if(keys.Length <= 1) {
                m_commandResult.Add(string.Format(m_rundown_lackofKeyError));
                yield return new KeyValuePair<float, List<string>>(0f, m_commandResult);
                yield break;
            }

            if(keys.Length >= 3) {
                m_commandResult.Add(string.Format(m_rundown_toomuchKeyError));
                yield return new KeyValuePair<float, List<string>>(0f, m_commandResult);
                yield break;
            }

            terminal.UI.CloseTerminal();

            SceneController controller = SceneController.Instance;
            if(controller == null) {
                yield break;
            }

            string key = "";
            if(keys[1] == "LAB")
                key = "Lab";

            controller.ChangeSceneTo(key);
            yield break;

            //m_commandResult.Add(string.Format())
            yield return new KeyValuePair<float, List<string>>(0f, m_commandResult);

            yield return default;
        }
    }

}

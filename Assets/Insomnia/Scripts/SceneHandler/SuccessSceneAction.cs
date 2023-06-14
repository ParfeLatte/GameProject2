using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Insomnia{
	public class SuccessSceneAction : MonoBehaviour {
		public void OnClick_ReturnToMain() {
			SceneController.Instance.ChangeSceneTo("Main");
		}
	}
}

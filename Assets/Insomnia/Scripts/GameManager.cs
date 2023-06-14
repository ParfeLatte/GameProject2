using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Insomnia{
	public class GameManager : ImmortalSingleton<GameManager> {
		public static bool IsPause { get => Time.timeScale <= 0.1f; }

	}
}

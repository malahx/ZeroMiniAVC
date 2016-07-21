/* 
ZeroMiniAVC
Copyright 2016 Malah

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  If not, see <http://www.gnu.org/licenses/>. 
*/

using System.IO;
using UnityEngine;

namespace ZeroMiniAVC {
	[KSPAddon (KSPAddon.Startup.Instantly, true)]
	public class ZeroMiniAVC : MonoBehaviour {

		static ZeroMiniAVC Instance;

		void Awake() {
			if (Instance != null) {
				Destroy (this);
				return;
			}
			Instance = this;
			DontDestroyOnLoad (Instance);
			Debug.LogWarning ("ZeroMiniAVC started ...");
			ScreenMessages.PostScreenMessage ("ZeroMiniAVC started...", 10);
			AssemblyLoader.LoadedAssembyList _assemblies = AssemblyLoader.loadedAssemblies;
			for (int _i = _assemblies.Count - 1; _i >= 0; --_i) {
				AssemblyLoader.LoadedAssembly _assembly = _assemblies[_i];
				if (_assembly.name == "MiniAVC") {
					// Remove/Unload MiniAVC here seems to bug mods which use assemblies.
					Debug.LogWarning ("MiniAVC is disabled and will be pruned at the shutdown of KSP.");
					ScreenMessages.PostScreenMessage ("MiniAVC will be pruned at the shutdown of KSP.", 10);
					return;
				}
			}
			Debug.LogWarning ("ZeroMiniAVC destroyed...");
			ScreenMessages.PostScreenMessage ("ZeroMiniAVC destroyed...", 10);
			Destroy (this);
		}

		void OnApplicationQuit() {
			Debug.LogWarning ("ZeroMiniAVC prune all MiniAVC ...");
			ScreenMessages.PostScreenMessage ("ZeroMiniAVC prune all MiniAVC ...", 10);
			AssemblyLoader.LoadedAssembyList _assemblies = AssemblyLoader.loadedAssemblies;
			for (int _i = _assemblies.Count - 1; _i >= 0; --_i) {
				AssemblyLoader.LoadedAssembly _assembly = _assemblies[_i];
				if (_assembly.name == "MiniAVC") {
					if (File.Exists (_assembly.path + ".pruned")) {
						File.Delete (_assembly.path + ".pruned");
					}
					File.Move (_assembly.path, _assembly.path + ".pruned");
					string[] _path = _assembly.path.Split (new char[2] { '/', '\\' });
					string _mod = _path[_path.IndexOf ("GameData") + 1];
					Debug.LogWarning ("MiniAVC pruned for " + _mod);
					ScreenMessages.PostScreenMessage ("MiniAVC pruned for " + _mod, 10);
				}
			}
			Debug.LogWarning ("ZeroMiniAVC ended.");
			ScreenMessages.PostScreenMessage ("ZeroMiniAVC ended.", 10);
		}
	}
}
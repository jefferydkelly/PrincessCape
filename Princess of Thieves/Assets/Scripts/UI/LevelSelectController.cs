using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LevelSelectController : MenuController{
	[SerializeField]
	TextAsset levelList;
	string[] levels;
	string [] levelNames;
	int numButtons = 4;
	int startIndex = 0;
	// Use this for initialization
	void Start () {
		Setup ();
		string[] lines = levelList.text.Split ('\n');
		levels = new string[lines.Length];
		levelNames = new string[lines.Length];
		int l = 0;
		foreach (string line in lines) {
			string[] splitLine = line.Split (',');
			levels[l] = splitLine [0].Trim();
			levelNames [l] = splitLine [1].Trim();
			l++;
		}
		for (int i = 0; i < numButtons; i++) {
			Button button = buttons [i];
			string level = levels [i];
			button.GetComponentInChildren<Text> ().text = levelNames[i];
			button.onClick.AddListener (delegate() {
				ChangeScene(level);
			});
		}
	}

	void UpdateButtons() {
		for (int i = 0; i < numButtons; i++) {
			Button button = buttons [i];
			int levInd = (startIndex + i) % levels.Length;
			string level = levels [levInd];
			button.GetComponentInChildren<Text> ().text = levelNames[levInd];
			button.onClick.RemoveAllListeners ();
			button.onClick.AddListener (delegate() {
				ChangeScene(level);
			});
		}
	}

	public void IncIndex() {
		startIndex = (startIndex + 1) % levelNames.Length;
		UpdateButtons ();
	}

	public void DecIndex() {
		startIndex -= 1;

		while (startIndex < 0) {
			startIndex += levels.Length;
		}
		UpdateButtons ();
	}
}

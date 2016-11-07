using UnityEngine;
using System.Collections;

public class EntranceGate : GateController {
    public TextAsset cutscene;
    protected override void OnClose()
    {
        GameManager.Instance.StartCutscene(cutscene);
    }
}

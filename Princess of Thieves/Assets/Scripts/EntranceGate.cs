using UnityEngine;
using System.Collections;

public class EntranceGate : GateController {

    protected override void OnClose()
    {
        GameManager.Instance.StartCutscene("EntranceCutscene");
    }
}

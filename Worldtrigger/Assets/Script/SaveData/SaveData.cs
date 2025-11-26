using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SaveData
{
    public List<KeyBinding> KeyBindings = new List<KeyBinding>();
}

[Serializable]
public class KeyBinding
{
    public string actionName;  // 例: "Jump", "Reload"
    public KeyCode key;        // 割り当てるキー
}

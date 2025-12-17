using System;
using System.Collections.Generic;

[Serializable]
public class KeybindData
{
    public string action;
    public string key;
}

[Serializable]
public class SaveData
{
    public List<KeybindData> binds = new();
}

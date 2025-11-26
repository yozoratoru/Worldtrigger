using System.IO;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;

    [HideInInspector] public SaveData data;
    private string filepath;
    private string fileName = "Data.json";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        filepath = Application.dataPath + "/" + fileName;

        if (!File.Exists(filepath))
        {
            data = new SaveData();

            // 初期キーバインド
            data.KeyBindings.Add(new KeyBinding(){ actionName="Jump", key = KeyCode.Space });
            data.KeyBindings.Add(new KeyBinding(){ actionName="Fire", key = KeyCode.Mouse0 });
            data.KeyBindings.Add(new KeyBinding(){ actionName="Aim", key = KeyCode.Mouse1 });
            data.KeyBindings.Add(new KeyBinding(){ actionName="Reload", key = KeyCode.R });
            data.KeyBindings.Add(new KeyBinding(){ actionName="Ability", key = KeyCode.Q });

            Save(data);
        }
        else
        {
            data = Load(filepath);
        }
    }

    public void Save(SaveData data)
    {
        string json = JsonUtility.ToJson(data, true); // 見やすい整形
        File.WriteAllText(filepath, json);
    }

    public SaveData Load(string path)
    {
        string json = File.ReadAllText(path);
        return JsonUtility.FromJson<SaveData>(json);
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Save(data);
        }
    }
}

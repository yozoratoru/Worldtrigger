using UnityEngine;
using System.Collections.Generic;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;

    public Dictionary<string, bool> actionStates = new Dictionary<string, bool>();

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
    }

    void Update()
    {
        var binds = DataManager.Instance.data.binds;

        // すべてのアクションをfalseにリセット
        foreach (var bind in binds)
        {
            if (!actionStates.ContainsKey(bind.action))
            {
                actionStates[bind.action] = false;
            }
            else
            {
                actionStates[bind.action] = false;
            }
        }

        // 押されているキーをチェックしてtrueにセット
        foreach (var bind in binds)
        {
            KeyCode keyCode;
            if (System.Enum.TryParse(bind.key, out keyCode))
            {
                if (Input.GetKey(keyCode))
                {
                    actionStates[bind.action] = true;
                }
            }
            else
            {
                Debug.LogWarning("Invalid key: " + bind.key);
            }
        }
    }
}
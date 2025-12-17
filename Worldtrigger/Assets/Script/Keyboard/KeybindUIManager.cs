using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class KeybindUIManager : MonoBehaviour
{
    public Transform content;      // ScrollView の Content
    public GameObject rowPrefab;   // 行Prefab

    void Start()
    {
        // LayoutをVerticalに変更
        var horizontalLayout = content.GetComponent<HorizontalLayoutGroup>();
        if (horizontalLayout != null)
        {
            Destroy(horizontalLayout);
        }
        var verticalLayout = content.GetComponent<VerticalLayoutGroup>();
        if (verticalLayout == null)
        {
            verticalLayout = content.gameObject.AddComponent<VerticalLayoutGroup>();
            verticalLayout.childAlignment = TextAnchor.UpperLeft;
            verticalLayout.spacing = 10f; // 適当な間隔
        }

        Debug.Log("KeybindUIManager Start: binds count = " + DataManager.Instance.data.binds.Count);
        foreach (var bind in DataManager.Instance.data.binds)
        {
            Debug.Log("Processing bind: action=" + bind.action + ", key=" + bind.key);
            var row = Instantiate(rowPrefab, content);

            // Action名を設定
            var actionText = row.transform.Find("ActionNameText");
            if (actionText != null)
            {
                var tmp = actionText.GetComponent<TextMeshProUGUI>();
                if (tmp != null)
                {
                    tmp.text = bind.action;
                    Debug.Log("Set action text to: " + bind.action);
                }
                else
                {
                    Debug.LogError("TextMeshProUGUI not found on ActionNameText");
                }
            }
            else
            {
                Debug.LogError("ActionNameText not found in rowPrefab");
            }

            // ボタン表示のText
            var btn = row.transform.Find("KeyButton");
            if (btn != null)
            {
                var btnText = btn.GetComponentInChildren<TextMeshProUGUI>();
                if (btnText != null)
                {
                    btnText.text = bind.key;
                    Debug.Log("Set key text to: " + bind.key);
                }
                else
                {
                    Debug.LogError("TextMeshProUGUI not found in KeyButton children");
                }

                // ボタンを押したらキー待ち開始
                btn.GetComponent<Button>().onClick.AddListener(() =>
                {
                    StartCoroutine(WaitKey(bind, btnText));
                });
            }
            else
            {
                Debug.LogError("KeyButton not found in rowPrefab");
            }
        }
    }

    System.Collections.IEnumerator WaitKey(KeybindData bind, TextMeshProUGUI display)
    {
        display.text = "Press any key...";

        bool waiting = true;
        while (waiting)
        {
            foreach (KeyCode key in System.Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(key))
                {
                    bind.key = key.ToString();
                    display.text = bind.key;
                    DataManager.Instance.Save(DataManager.Instance.data);
                    waiting = false;
                    break;
                }
            }
            yield return null;
        }
    }
}

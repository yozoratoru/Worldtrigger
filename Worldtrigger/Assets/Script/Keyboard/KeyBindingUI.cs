using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class KeyBindingUI : MonoBehaviour
{
    public Transform content;
    public GameObject itemPrefab;

    private Button waitingButton = null;
    private string waitingAction = "";

    void Start()
    {
        LoadUI();
    }

    void LoadUI()
    {
        foreach (Transform t in content)
        {
            Destroy(t.gameObject);
        }

        foreach (var kb in DataManager.Instance.data.KeyBindings)
        {
            GameObject obj = Instantiate(itemPrefab, content);
            obj.transform.Find("ActionName").GetComponent<Text>().text = kb.actionName;

            Button keyBtn = obj.transform.Find("KeyButton").GetComponent<Button>();
            Text keyText = keyBtn.transform.Find("Text").GetComponent<Text>();
            keyText.text = kb.key.ToString();

            keyBtn.onClick.AddListener(() =>
            {
                waitingButton = keyBtn;
                waitingAction = kb.actionName;
                keyText.text = "Press key...";
                StartCoroutine(WaitForKey());
            });
        }
    }

    IEnumerator WaitForKey()
    {
        while (true)
        {
            foreach (KeyCode code in System.Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(code))
                {
                    ApplyNewKey(waitingAction, code);
                    waitingButton.transform.Find("Text").GetComponent<Text>().text = code.ToString();

                    // JSON 保存
                    DataManager.Instance.Save(DataManager.Instance.data);

                    waitingButton = null;
                    waitingAction = "";
                    yield break;
                }
            }
            yield return null;
        }
    }

    void ApplyNewKey(string action, KeyCode newKey)
    {
        foreach (var kb in DataManager.Instance.data.KeyBindings)
        {
            if (kb.actionName == action)
            {
                kb.key = newKey;
                return;
            }
        }
    }
}

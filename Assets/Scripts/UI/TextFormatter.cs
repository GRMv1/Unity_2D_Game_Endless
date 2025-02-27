using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TMPro.TextMeshProUGUI))]
public class TextFormatter : MonoBehaviour
{
    private TMPro.TextMeshProUGUI text;

    private static int DEFAULT_FONT_SIZE = 60;
    private static int ALTERNATIVE_FONT_SIZE = 35;
    void Start()
    {
        text = GetComponent<TMPro.TextMeshProUGUI>();
        GameManager.Instance.OnSettingChange += SwapFont;
    }

    public void SwapFont(bool isDefault)
    {
        text.fontSize = isDefault ? DEFAULT_FONT_SIZE : ALTERNATIVE_FONT_SIZE;
    }

    private void OnDestroy()
    {
        GameManager.Instance.OnSettingChange -= SwapFont;
    }
}

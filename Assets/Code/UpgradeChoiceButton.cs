using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public sealed class UpgradeChoiceButton : MonoBehaviour
{
    [Header("UI Refs")]
    [SerializeField] private Button button;
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI descriptionText;

    private object payload;
    private Action<object> onClick;

    public void Bind(object payload, Sprite sprite, string title, string description, Action<object> onClick)
    {
        this.payload = payload;
        this.onClick = onClick;

        if (icon != null) icon.sprite = sprite;
        if (nameText != null) nameText.text = title ?? "";
        if (descriptionText != null) descriptionText.text = description ?? "";

        if (button != null)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => this.onClick?.Invoke(this.payload));
        }
    }
}
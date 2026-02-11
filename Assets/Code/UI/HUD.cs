using TMPro;
using UnityEngine;

public class HUD : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI timerText;
    [SerializeField]
    FillbarUI hpBar,
        xpBar;

    public TextMeshProUGUI TimerText => timerText;
    public FillbarUI HPBar => hpBar;
    public FillbarUI XPBar => xpBar;

    private void FixedUpdate()
    {
        if (timerText != null)
            timerText.text = Utils.TimeFormat(GameManager.Instance.CurrentGameTime);
    }

    public void SetHP(float value, float maxValue) =>
        hpBar?.SetFill(value, maxValue);

    public void SetXP(float value, float maxValue) =>
        xpBar?.SetFill(value, maxValue);
}

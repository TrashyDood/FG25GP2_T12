using System;
using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
public class FillbarUI : MonoBehaviour
{
    [SerializeField]
    FillArea[] fillAreas;

    [field: SerializeField, Range(0, 1)]
    public float FillAmount { get; set; }

    public void SetFill(float value, float maxValue) => FillAmount = value / maxValue;

    private void Update()
    {
        if (fillAreas == null)
            return;

        FillArea fillArea;
        for (int i = 0; i < fillAreas.Length; i++)
        {
            fillArea = fillAreas?[i];
            fillArea.Fill(FillAmount, Time.deltaTime);
        }
    }

    [Serializable]
    class FillArea
    {
        public Image image;
        public float speed;
        public float fillAmount;

        public float Fill(float target, float deltaTime)
        {
            fillAmount = Mathf.Lerp(fillAmount, target, speed * deltaTime);

            if (image != null)
                image.fillAmount = fillAmount;

            return fillAmount;
        }
    }
}
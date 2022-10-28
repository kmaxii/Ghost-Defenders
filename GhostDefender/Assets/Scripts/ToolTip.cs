using System;
using TMPro;
using UnityEngine;

public class ToolTip : MonoBehaviour
{

    [SerializeField] private Camera uiCamera;
    [SerializeField] private RectTransform parent;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private RectTransform background;
    
    [SerializeField] private float padding = 8f;

    
    private static ToolTip Instance;


    private void Start()
    {
        Instance = this;
        HideToolTip();
    }

    private void Update()
    {
        transform.localPosition = Input.mousePosition;
    }
    

    private void ShowTooltipPrivate(string showString)
    {
        gameObject.SetActive(true);

        text.text = showString;

        Vector2 backgroundSize = new Vector2(text.preferredWidth + padding * 2f, text.preferredHeight + padding * 2f);
        background.sizeDelta = backgroundSize;
    }
    
    private void HideTooltipPrivate()
    {
        gameObject.SetActive(false);

    }

    public static void ShowToolTip(string toolTipText)
    {
        Instance.ShowTooltipPrivate(toolTipText);
    }
    
    public static void HideToolTip()
    {
        Instance.HideTooltipPrivate();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetSelectionSize : MonoBehaviour
{
    public RectTransform selectionOneRT;
    public RectTransform selectionTwoRT;

    void Start()
    {
        RectTransform wholePanelRT = transform.GetComponent<RectTransform>();
        selectionOneRT.sizeDelta = new Vector2(wholePanelRT.rect.width / 2, selectionOneRT.sizeDelta.y);
        selectionTwoRT.sizeDelta = new Vector2(wholePanelRT.rect.width / 2, selectionTwoRT.sizeDelta.y);
    }

    void Update()
    {
        
    }
}
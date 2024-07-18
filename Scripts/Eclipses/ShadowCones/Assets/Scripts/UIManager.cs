using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    
    private float currentControlPanelScale = 0;
    private float targetControlPanelScale = 0;
    public float animationSpeed = 7;
    public RectTransform controlPanel;
    float t = 0;
    private SystemManager system;

    public TMP_InputField timeScaleField;
    public Slider timeScaleSlider;

    // Start is called before the first frame update
    void Start()
    {
        system = FindObjectsOfType<SystemManager>()[0];
    }

    // Update is called once per frame
    void Update()
    {
        if (t > 1) {
            controlPanel.localScale = new Vector3(targetControlPanelScale, targetControlPanelScale, 1);
            return;
        }
        
        float scale = Mathf.Lerp(currentControlPanelScale, targetControlPanelScale, t);
        controlPanel.localScale = new Vector3(scale, scale, 1);
        t += animationSpeed * Time.deltaTime;
    }

    public void ClickHelpButton()
    {
        currentControlPanelScale = controlPanel.localScale.x;
        targetControlPanelScale = 1 - targetControlPanelScale;
        t = 0;
    }

    public void OnTimeScaleChanged(float timeScale)
    {
        system.SetTimeScale(timeScale);
        timeScaleField.text = timeScale.ToString("F3");
    }

    public void OnTimeScaleFieldChanged(string input)
    {
        if (float.TryParse(input, out float timeScale))
        {
            timeScaleSlider.value = timeScale;
        }
        else
        {
            timeScaleField.text = timeScaleSlider.value.ToString("F3");
        }
    }

    public void ResetTimeScale()
    {
        timeScaleSlider.value = 1;
    }
}

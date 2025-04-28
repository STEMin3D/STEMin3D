using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR;

[System.Serializable]
public class PrimaryButtonEvent : UnityEvent<bool> { }

[System.Serializable]
public class SecondaryButtonEvent : UnityEvent<bool> { }

[System.Serializable]
public class TriggerButtonEvent : UnityEvent<bool> { }

[System.Serializable]
public class GripButtonEvent : UnityEvent<bool> { }

public class VRControls : MonoBehaviour
{
    public GameObject stars;
    public GameObject constellations;
    public GameObject coordinates;
    public GameObject boundaries;
    public GameObject labels;
    public GameObject sun;

    public GameObject pauseMenu;

    public PrimaryButtonEvent primaryButtonPress;
    public SecondaryButtonEvent secondaryButtonPress;
    public TriggerButtonEvent triggerButtonPress;
    public GripButtonEvent gripButtonPress;

    private List<InputDevice> devicesWithPrimaryButton;
    private List<InputDevice> devicesWithSecondaryButton;
    private List<InputDevice> devicesWithTriggerButton;
    private List<InputDevice> devicesWithGripButton;

    private void Awake()
    {
        if (primaryButtonPress == null)
        {
            primaryButtonPress = new PrimaryButtonEvent();
        }
        if (secondaryButtonPress == null)
        {
            secondaryButtonPress = new SecondaryButtonEvent();
        }
        if (triggerButtonPress == null)
        {
            triggerButtonPress = new TriggerButtonEvent();
        }
        if (gripButtonPress == null)
        {
            gripButtonPress = new GripButtonEvent();
        }


        devicesWithPrimaryButton = new List<InputDevice>();
        devicesWithSecondaryButton = new List<InputDevice>();
        devicesWithTriggerButton = new List<InputDevice>();
        devicesWithGripButton = new List<InputDevice>();
    }

    private void Start()
    {
        labels.SetActive(false);
        constellations.SetActive(false);
        pauseMenu.SetActive(false);
    }

    void OnEnable()
    {
        List<InputDevice> allDevices = new List<InputDevice>();
        InputDevices.GetDevices(allDevices);
        foreach (InputDevice device in allDevices)
            InputDevices_deviceConnected(device);

        InputDevices.deviceConnected += InputDevices_deviceConnected;
        InputDevices.deviceDisconnected += InputDevices_deviceDisconnected;
    }

    private void OnDisable()
    {
        InputDevices.deviceConnected -= InputDevices_deviceConnected;
        InputDevices.deviceDisconnected -= InputDevices_deviceDisconnected;
        devicesWithPrimaryButton.Clear();
        devicesWithSecondaryButton.Clear();
        devicesWithTriggerButton.Clear();
        devicesWithGripButton.Clear();
    }

    private void InputDevices_deviceConnected(InputDevice device)
    {
        bool discardedValue;
        if (device.TryGetFeatureValue(CommonUsages.primaryButton, out discardedValue))
        {
            devicesWithPrimaryButton.Add(device); // Add any devices that have a primary button.
        }
        if (device.TryGetFeatureValue(CommonUsages.secondaryButton, out discardedValue))
        {
            devicesWithSecondaryButton.Add(device);
        }
        if (device.TryGetFeatureValue(CommonUsages.triggerButton, out discardedValue))
        {
            devicesWithTriggerButton.Add(device);
        }
        if (device.TryGetFeatureValue(CommonUsages.gripButton, out discardedValue))
        {
            devicesWithGripButton.Add(device);
        }
    }

    private void InputDevices_deviceDisconnected(InputDevice device)
    {
        if (devicesWithPrimaryButton.Contains(device))
            devicesWithPrimaryButton.Remove(device);

        if (devicesWithSecondaryButton.Contains(device))
            devicesWithSecondaryButton.Remove(device);

        if (devicesWithTriggerButton.Contains(device))
            devicesWithTriggerButton.Remove(device);

        if (devicesWithGripButton.Contains(device))
            devicesWithGripButton.Remove(device);
    }

    private bool isPrimary1Pressed = false;
    private bool isPrimary2Pressed = false;

    private bool isSecondary1Pressed = false;
    private bool isSecondary2Pressed = false;

    private bool isTrigger1Pressed = false;
    private bool isTrigger2Pressed = false;
    private bool isSunSimming = false;

    private bool isGrip1Pressed = false;
    private bool isGrip2Pressed = false;
    void Update()
    {
        foreach (var device in devicesWithPrimaryButton)
        {
            bool primaryButtonState = false;
            bool isLeftController = false;
            if (device.characteristics.ToString().Contains("Left"))
            {
                isLeftController = true;
            }
            device.TryGetFeatureValue(CommonUsages.primaryButton, out primaryButtonState);
            if (primaryButtonState) {
                if (isLeftController)
                {
                    //left controller has menu button
                    if (!isPrimary1Pressed)
                    {
                        isPrimary1Pressed = true;
                    }
                }
                else
                {
                    //right controller has no menu button
                    if (!isPrimary2Pressed)
                    {
                        isPrimary2Pressed = true;
                    }
                }
            }
            else
            {
                if (isLeftController)
                {
                    //left controller has menu button
                    if (isPrimary1Pressed)
                    {
                        stars.SetActive(!stars.activeSelf);
                        isPrimary1Pressed = false;
                    }
                }
                else
                {
                    if (isPrimary2Pressed)
                    {
                        coordinates.SetActive(!coordinates.activeSelf);
                        isPrimary2Pressed = false;
                    }
                }
            }
        }

        foreach (var device in devicesWithSecondaryButton)
        {
            bool secondaryButtonState = false;
            bool isLeftController = false;
            if (device.characteristics.ToString().Contains("Left"))
            {
                isLeftController = true;
            }
            device.TryGetFeatureValue(CommonUsages.secondaryButton, out secondaryButtonState);
            if (secondaryButtonState)
            {
                if (isLeftController)
                {
                    //left controller has menu button
                    if (!isSecondary1Pressed)
                    {
                        isSecondary1Pressed = true;
                    }
                }
                else
                {
                    //right controller has no menu button
                    if (!isSecondary2Pressed)
                    {
                        isSecondary2Pressed = true;
                    }
                }
            }
            else
            {
                if (isLeftController)
                {
                    //left controller has menu button
                    if (isSecondary1Pressed)
                    {
                        isSecondary1Pressed = false;
                        constellations.SetActive(!constellations.activeSelf);
                    }
                }
                else
                {
                    if (isSecondary2Pressed)
                    {
                        isSecondary2Pressed = false;
                        boundaries.SetActive(!boundaries.activeSelf);
                    }
                }
            }
        }

        foreach (var device in devicesWithTriggerButton)
        {
            bool triggerButtonState = false;
            bool isLeftController = false;
            if (device.characteristics.ToString().Contains("Left"))
            {
                isLeftController = true;
            }
            device.TryGetFeatureValue(CommonUsages.triggerButton, out triggerButtonState);
            if (triggerButtonState)
            {
                if (isLeftController)
                {
                    //left controller has menu button
                    if (!isTrigger1Pressed)
                    {
                        isTrigger1Pressed = true;
                    }
                }
                else
                {
                    //right controller has no menu button
                    if (!isTrigger2Pressed)
                    {
                        isTrigger2Pressed = true;
                    }
                }
            }
            else
            {
                if (isLeftController)
                {
                    //left controller has menu button
                    if (isTrigger1Pressed)
                    {
                        isTrigger1Pressed = false;
                        labels.SetActive(!labels.activeSelf);
                    }
                }
                else
                {
                    if (isTrigger2Pressed)
                    {
                        isTrigger2Pressed = false;
                        isSunSimming = !isSunSimming;
                    }
                }
            }
        }

        if (isSunSimming)
        {
            sun.transform.RotateAround(sun.transform.position, Vector3.right, Time.deltaTime * 10f);
        }

        foreach (var device in devicesWithGripButton)
        {
            bool gripButtonState = false;
            bool isLeftController = false;
            if (device.characteristics.ToString().Contains("Left"))
            {
                isLeftController = true;
            }
            device.TryGetFeatureValue(CommonUsages.gripButton, out gripButtonState);
            if (gripButtonState)
            {
                if (isLeftController)
                {
                    //left controller has menu button
                    if (!isGrip1Pressed)
                    {
                        isGrip1Pressed = true;
                    }
                }
                else
                {
                    //right controller has no menu button
                    if (!isGrip2Pressed)
                    {
                        isGrip2Pressed = true;
                    }
                }
            }
            else
            {
                if (isLeftController)
                {
                    //left controller has menu button
                    if (isGrip1Pressed)
                    {
                        isGrip1Pressed = false;
                        pauseMenu.SetActive(!pauseMenu.activeSelf);
                        if (pauseMenu.activeSelf)
                        {
                            Time.timeScale = 0.0f;
                        }
                        else
                        {
                            Time.timeScale = 1.0f;
                        }
                    }
                }
                else
                {
                    if (isGrip2Pressed)
                    {
                        isGrip2Pressed = false;
                    }
                }
            }
        }
    }
}

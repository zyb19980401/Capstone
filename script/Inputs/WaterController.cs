using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class WaterController : MonoBehaviour
{
    // A static variable in Unity is a variable that is shared by all instances of a class.
    public static float WaterDepth = 0.0f;
    public static float WaterLevelChangeSpeed = 0.0f;
    public static float autoModeSpeed = 0.0f;

    public GameObject Plane;
    public GameObject WaterSurface;
    public GameObject UnderWaterCanvas;


    [Tooltip("[Under Development] Allow user to Adjust water level by VR (righthand) controller thumbstick")]
    public bool AdjustMode = false;
    public float AdjustModeBaseSpeed = 0.1f;
    public float AutoSpeed = 0.05f;

    PlayerControls controls;

    private float AdjustModeSpeed = 0.0f;
    private bool isUnderWater = false;

    // public delegate void UnderWater();
    // public static event UnderWater UnderWater;

    // Commands inside the Awake() method will always be called before the Start() method, Start is called before the first frame update
    void Awake()
    {
        controls = new PlayerControls();

        if (AdjustMode) // Control by VR (righthand) controller thumbstick
        {
            controls.Player.AdjustWaterLevel.performed += ctx => adjustWaterLevel(ctx.ReadValue<Vector2>());
            controls.Player.AdjustWaterLevel.canceled += ctx => adjustWaterLevel(new Vector2(0,0));
        }

        // Trigger by right-hand primary (increasing) and secondary (decreasing) buttons
        controls.Player.IncreaseWaterLevel.performed += ctx => autoChangeWaterLevel(1);
        controls.Player.DecreaseWaterLevel.performed += ctx => autoChangeWaterLevel(-1);
        controls.Player.StopChangingWaterLevel.performed += ctx => stopChangingWaterLevel();
    }

    private void adjustWaterLevel(Vector2 move)
    {
        Debug.Log("AdjustMode: Thumb-stick coordinates = " + move);
        WaterLevelChangeSpeed -= AdjustModeSpeed;
        AdjustModeSpeed = AdjustModeBaseSpeed * move.y;
        WaterLevelChangeSpeed += AdjustModeSpeed;
    }

    private void autoChangeWaterLevel(int direction)
    {
        WaterLevelChangeSpeed -= autoModeSpeed;
        if (autoModeSpeed == 0.0f)
        {
            autoModeSpeed = direction * AutoSpeed;
            Debug.Log("autoMode: Start to change, speed = " + autoModeSpeed.ToString("0.00") + "m/s");
        }
        else
        {
            autoModeSpeed = 0.0f;
            Debug.Log("autoMode: Stop changing.");
        }
        WaterLevelChangeSpeed += autoModeSpeed;
    }

    private void stopChangingWaterLevel()
    {
        float WaterLevelChangeSpeedBefore = WaterLevelChangeSpeed;
        if (WaterLevelChangeSpeed > 0.0f)
        {
            Debug.Log("StopChangingWaterLevel: Stop increasing.");
        }
        else if (WaterLevelChangeSpeed < 0.0f)
        {
            Debug.Log("StopChangingWaterLevel: Stop decreasing.");
        }
        else
        {
            Debug.Log("StopChangingWaterLevel: Remain freezing.");
        }
        WaterLevelChangeSpeed = 0.0f;
        autoModeSpeed = 0.0f;
        RainController.RainingWaterRisingSpeed = 0.0f;
        ExportRecords.Instance.exportRecords();
    }

    private void StartUnderWaterEffect()
    {
        UnderWaterCanvas.SetActive(true);
    }

    private void EndUnderWaterEffect()
    {
        UnderWaterCanvas.SetActive(false);
    }

    private void OnEnable()
    {
        controls.Player.Enable();
    }
    private void OnDisable()
    {
        controls.Player.Disable();
    }

    // Update is called once per frame, FixedUpdate is called after a certain time period
    void FixedUpdate()
    {
        Vector3 movement;
        float CameraHight = XROriginController.Origins[XROriginController.activateOriginID].transform.GetChild(0).transform.GetChild(0).transform.position[1];
        float WaterSurfaceHeight = WaterSurface.transform.position[1];

        if (CameraHight < WaterSurfaceHeight)
        {
            if (!isUnderWater)
            {
                isUnderWater = true;
                StartUnderWaterEffect();
            }
        }
        else
        {
            if (isUnderWater)
            {
                isUnderWater = false;
                EndUnderWaterEffect();
            }
        }

        WaterDepth = WaterSurfaceHeight - Plane.transform.position[1];
        UIDisplayController.Instance.WaterDepthArea.transform.GetChild(0).GetComponent<TMP_Text>().text = "Water Depth\n" + WaterDepth.ToString("0.00") + 'm';
        Debug.Log("Water Depth\n" + WaterDepth.ToString("0.00") + 'm');
        movement = new Vector3(0.0f, 1.0f, 0.0f) * WaterLevelChangeSpeed * Time.deltaTime;

        WaterSurface.transform.Translate(movement, Space.World);
    }
}

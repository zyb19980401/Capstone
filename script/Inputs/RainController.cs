using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RainController : MonoBehaviour
{
    // A static variable in Unity is a variable that is shared by all instances of a class.
    public static int IsRaining = 0;
    public static float RainingWaterRisingSpeed = 0.0f;


    public GameObject Rain;
    public GameObject Water;
    public float waterRisingSpeed = 0.02f;

    PlayerControls controls;

    // Commands inside the Awake() method will always be called before the Start() method, Start is called before the first frame update
    void Awake()
    {
        controls = new PlayerControls();

        controls.Player.Rain.performed += ctx => RainControl();
    }

    private void OnEnable()
    {
        controls.Player.Enable();
    }

    private void OnDisable()
    {
        controls.Player.Disable();
    }
  
    private void RainControl()
    {
        WaterController.WaterLevelChangeSpeed -= RainingWaterRisingSpeed;
        if (Rain.activeSelf)
        {
            Rain.SetActive(false);
            IsRaining = 0;
            RainingWaterRisingSpeed = 0.0f;
            Debug.Log("Stop to rain");
        }
        else
        {
            Rain.SetActive(true);
            IsRaining = 1;
            RainingWaterRisingSpeed = waterRisingSpeed;
            Debug.Log("Start to rain");
        }
        WaterController.WaterLevelChangeSpeed += RainingWaterRisingSpeed;
    }

    // Update is called once per frame
    void Update()
    {

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DayNightController : MonoBehaviour
{
    // A static variable in Unity is a variable that is shared by all instances of a class.
    // public static int DayNight = 1;             // 1 means day, 0 means night.
    public static string DayNight = "Day";


    public GameObject Day;
    public GameObject Night;

    public Material SkyboxNight;
    public Material SkyboxDay;
    public Color FogColorNight;
    public Color FogColorDay;

    PlayerControls controls;

    // Commands inside the Awake() method will always be called before the Start() method, Start is called before the first frame update
    void Awake()
    {
        controls = new PlayerControls();
        controls.Player.DayNight.performed += ctx => SwitchDayNight();
    }

    private void OnEnable()
    {
        controls.Player.Enable();
    }

    private void OnDisable()
    {
        controls.Player.Disable();
    }
  
    private void SwitchDayNight()
    {
        if (Day.activeSelf)
        {
            RenderSettings.skybox = SkyboxNight;
            RenderSettings.fogColor = FogColorNight;
            Night.SetActive(true);
            Day.SetActive(false);
            DayNight = "Night";
            Debug.Log("Switch to night");
        }
        else
        {
            RenderSettings.skybox = SkyboxDay;
            RenderSettings.fogColor = FogColorDay;
            Night.SetActive(false);
            Day.SetActive(true);
            DayNight = "Day";
            Debug.Log("Switch to day");
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}

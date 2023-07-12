using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIDisplayController : MonoBehaviour
{
    public static UIDisplayController Instance {get; private set;}

    public bool DisplayWaterDepth = false;
    public bool DisplayFPS = false;
    public bool PauseWhenDisplayInstructions = true;
    public GameObject WaterDepthArea;
    public GameObject FPSDisplayArea;
    public GameObject[] Instructions;
    public float[] InstructionDisplayDepth;

    private bool[] InstructionsDisplayed = new bool[] {false, false, false, false};
    // private int ActivatedInstructionID = -1;
    private int InstructionsCount = 0;

    PlayerControls controls;

    // Commands inside the Awake() method will always be called before the Start() method, Start is called before the first frame update
    void Awake()
    {
        InstructionsCount = Instructions.Length;
        controls = new PlayerControls();
        if (DisplayWaterDepth)
        {
            if (!WaterDepthArea.activeSelf)
            {
                WaterDepthArea.SetActive(true);
            }
            controls.Player.DisplayWaterDepth.performed += ctx => displayWaterDepth();
        }
        else
        {
            if (WaterDepthArea.activeSelf)
            {
                WaterDepthArea.SetActive(false);
            }
        }

        if (DisplayFPS)
        {
            if (!FPSDisplayArea.activeSelf)
            {
                FPSDisplayArea.SetActive(true);
            }
        }
        else
        {
            if (FPSDisplayArea.activeSelf)
            {
                FPSDisplayArea.SetActive(false);
            }
        }

        // keep a static reference to the class itself.
        Instance = this;
    }

    private void displayWaterDepth()
    {
        if (WaterDepthArea.activeSelf)
        {
            WaterDepthArea.SetActive(false);
        }
        else
        {
            WaterDepthArea.SetActive(true);
        }
    }

    private void displayInstructions(GameObject instruction)
    {
        // // Stop increasing water level
        // WaterController.WaterLevelChangeSpeed = 0.0f;
        // WaterController.autoModeSpeed = 0.0f;
        // RainController.RainingWaterRisingSpeed = 0.0f;

        if (!instruction.activeSelf)
        {
            instruction.SetActive(true);
        }
        if (PauseWhenDisplayInstructions)
        {
           // PauseController.Instance.pause();
        }
    }

    private void removeInstructions(GameObject instruction)
    {
        if (instruction.activeSelf)
        {
            instruction.SetActive(false);
        }
    }

    private void OnEnable()
    {
        controls.Player.Enable();
    }
    private void OnDisable()
    {    
        controls.Player.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < InstructionsCount; i ++)
        {
            if (WaterController.WaterDepth >= InstructionDisplayDepth[i] && !InstructionsDisplayed[i])
            {
                displayInstructions(Instructions[i]);
                InstructionsDisplayed[i] = true;
            }
            if (WaterController.WaterDepth >= InstructionDisplayDepth[i] + 0.01 && InstructionsDisplayed[i])
            {
                removeInstructions(Instructions[i]);
            }
        }
    }
}

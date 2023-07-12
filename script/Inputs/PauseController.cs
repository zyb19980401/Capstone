using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseController : MonoBehaviour
{
    public static PauseController Instance {get; private set;}

    // A static variable in Unity is a variable that is shared by all instances of a class.
    public static bool gameIsPaused = false;

    public GameObject GamePausedDisplay;

    PlayerControls controls;

    AudioSource[] sources;

    // Commands inside the Awake() method will always be called before the Start() method, Start is called before the first frame update
    void Awake()
    {
        controls = new PlayerControls();
        controls.Player.PauseGame.performed += ctx => pauseControl();

        // keep a static reference to the class itself.
        Instance = this;
    }

    private void OnEnable()
    {
        controls.Player.Enable();
    }
    private void OnDisable()
    {    
        controls.Player.Disable();
    }

    void pauseControl()
    {
        if (gameIsPaused)
        {
            resume();
        }
        else
        {
            pause();
        }
    }

    public void resume()
    {
        // find all ACTIVE audio source objects
        sources = FindObjectsOfType(typeof(AudioSource)) as AudioSource[];
        // Resume Audio
        foreach(AudioSource audioSource in sources)
        {
            Debug.Log("Resume Audio: " + audioSource.name);
            audioSource.Play();
        }

        Time.timeScale = 1f;
        gameIsPaused = false;
        GamePausedDisplay.SetActive(false);
        Debug.Log("Game Resumed");
    }

    public void pause()
    {
        // find all ACTIVE audio source objects
        sources = FindObjectsOfType(typeof(AudioSource)) as AudioSource[];
        // Pause Audio
        foreach(AudioSource audioSource in sources)
        {
            Debug.Log("Pause Audio: " + audioSource.name);
            audioSource.Pause();
        }

        Time.timeScale = 0f;
        gameIsPaused = true;
        GamePausedDisplay.SetActive(true);
        Debug.Log("Game Paused");
    }

    // Update is called once per frame
    void Update()
    {

    }
}

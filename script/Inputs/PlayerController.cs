using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public GameObject Camera;

    [Tooltip("Origin movement will not be controlled by VR controllers if true")]
    public bool FixedOrigin;

    [Tooltip("Origin movement on Y-axis will not be controlled by VR controllers if true")]
    public bool FixedOriginHight;

    public float speed = 1;

    PlayerControls controls;
    Vector2 move;

    // Commands inside the Awake() method will always be called before the Start() method, Start is called before the first frame update
    void Awake()
    {
        controls = new PlayerControls();
        controls.Player.Move.performed += ctx => SendMessage(ctx.ReadValue<Vector2>());
        controls.Player.Move.performed += ctx => move = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += ctx => move = Vector2.zero;
        controls.Player.Up.performed += ctx => move = ctx.ReadValue<Vector2>();
    }

    private void OnEnable()
    {
        controls.Player.Enable();
    }

    private void OnDisable()
    {    
        controls.Player.Disable();
    }

    void SendMessage(Vector2 coordinates)
    {
        Debug.Log("Move input: " + coordinates);
        Debug.Log("Forward direction: " + Camera.transform.forward);
    }

    // Update is called once per frame, FixedUpdate is called after a certain time period
    void FixedUpdate()
    {
        Vector3 forward = Camera.transform.forward;

        // The projection of the forward vector onto a horizontal plane.
        // .normalized makes sure the speed is constant however the pitch angle changes when move.magnitude == 1.
        Vector3 forwardHorizontal = new Vector3(forward.x, 0.0f, forward.z).normalized;
        Vector3 rightwardHorizontal = new Vector3(forwardHorizontal.z, 0.0f, -forwardHorizontal.x);
        
        Vector3 movingDirection;
        Vector3 movement;

        if (FixedOrigin)
        {
            movingDirection = new Vector3(0,0,0);
        }
        else if (FixedOriginHight)
        {
            movingDirection = forwardHorizontal * move.y + rightwardHorizontal * move.x;
        }
        else
        {
            movingDirection = forward * move.y + rightwardHorizontal * move.x;
        }

        movement = movingDirection * speed * Time.deltaTime;
        transform.Translate(movement, Space.World);
        
        // if (movingDirection != new Vector3(0,0,0))
        // {
        //     Debug.Log("Moving direction: " + movingDirection);
        // }
    }
}

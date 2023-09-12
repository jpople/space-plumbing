using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class PlayerMovement : MonoBehaviour
{
    const float MOVE_SPEED = 3f;
    Vector2 movementInput;
    Rigidbody2D rb;
    Animator anim;
    LevelInteractable terminal;
    bool interactThisFrame = false;
    public UnityEvent onInteract;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        HandleMovement();

        if (interactThisFrame)
        {
            HandleInteractInput();
            interactThisFrame = false;
        }

        // RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.up, 0.75f, LayerMask.GetMask("Interactable"));
        // if (hit.collider != null) {
        //     terminal = hit.collider.GetComponent<LevelInteractable>();
        //     terminal.SetInteractable(true);
        // }
        // else {
        //     if (terminal != null) {
        //         terminal.SetInteractable(false);
        //     }
        //     terminal = null;
        // }
    }

    private void HandleMovement()
    {
        if (movementInput.x > 0)
        {
            anim.CrossFade("Player_Walk_Right", 0f);
        }
        else if (movementInput.x < 0)
        {
            anim.CrossFade("Player_Walk_Left", 0f);
        }
        else
        {
            if (movementInput.y > 0)
            {
                anim.CrossFade("Player_Walk_Up", 0f);
            }
            else if (movementInput.y < 0)
            {
                anim.CrossFade("Player_Walk_Down", 0f);
            }
            else
            {
                anim.CrossFade("Player_Idle", 0f);
            }
        }
    }

    void FixedUpdate()
    {
        Vector2 newPosition = rb.position += movementInput;
        rb.MovePosition(newPosition);
    }

    public void OnMoveInput(InputAction.CallbackContext c)
    {
        movementInput = c.ReadValue<Vector2>().normalized;
        movementInput *= MOVE_SPEED * Time.fixedDeltaTime;
    }

    public void OnInteractInput(InputAction.CallbackContext c)
    {
        if (c.started)
        {
            interactThisFrame = true;
        }
    }

    private void HandleInteractInput()
    {
        // if (terminal != null)
        // {
        //     terminal.Interact();
        // }
        // else
        // {
        //     Debug.Log("tried interacting to no avail!");
        // }
        onInteract.Invoke();
    }

    // public void SetTerminal(LevelInteractable newTerminal)
    // {
    //     terminal = newTerminal;
    // }
}

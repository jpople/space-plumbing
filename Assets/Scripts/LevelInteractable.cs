using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class EnterInteractionRadiusEvent : UnityEvent<int>
{
    // nothing
}

[System.Serializable]
public class ExitInteractionRadiusEvent : UnityEvent<int>
{
    // nothing
}

public class LevelInteractable : MonoBehaviour
{
    public SpriteRenderer highlight;

    public int id = -1; // if this ever remains -1, we're having a bad time
    public EnterInteractionRadiusEvent enter;
    public ExitInteractionRadiusEvent exit;

    PlayerMovement player;

    void Start()
    {
        highlight = transform.GetChild(0).GetComponent<SpriteRenderer>();
        SetInteractable(false);
    }

    public void Interact()
    {
        Debug.Log("interacted!");
    }

    public void SetInteractable(bool newState)
    {
        highlight.gameObject.SetActive(newState);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        player = other.transform.GetComponent<PlayerMovement>();
        if (player != null)
        {
            Debug.Log("entered!");
            SetInteractable(true);
            enter.Invoke(id);
            // player.SetTerminal(this);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log("exited!");
        // player.SetTerminal(null);
        SetInteractable(false);
        exit.Invoke(id);
    }

    public void OnInteract()
    {
        Debug.Log($"interacted with interactable {id}!");
    }
}

using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections;
using System.Collections.Generic;

public class LevelInteractableManager : MonoBehaviour
{
    private List<LevelInteractable> allInteractables;
    public List<LevelInteractable> eligibleInteractables;
    private LevelInteractable interactionTarget;
    private PlayerMovement player;

    private void Start()
    {
        // subscribe to events from interactables
        allInteractables = new List<LevelInteractable>(FindObjectsOfType<LevelInteractable>());
        foreach (var (item, index) in allInteractables.WithIndex())
        {
            item.id = index;
            item.enter.AddListener(AddInteractableToEligible);
            item.exit.AddListener(RemoveInteractableFromEligible);

            player = FindObjectOfType<PlayerMovement>();
        }

        // subscribe to events from player
        player.onInteract.AddListener(HandleInteract);
    }

    private void Update()
    {
        if (eligibleInteractables.Count == 1)
        {
            interactionTarget = eligibleInteractables[0];
        }
        else if (eligibleInteractables.Count == 0)
        {
            interactionTarget = null;
        }
        else
        {
            // calculate the closest one
        }
    }

    private void AddInteractableToEligible(int id)
    {
        eligibleInteractables.Add(allInteractables[id]);
    }

    private void RemoveInteractableFromEligible(int id)
    {
        eligibleInteractables.Remove(allInteractables[id]);
    }

    private void HandleInteract()
    {
        if (interactionTarget != null)
        {
            interactionTarget.OnInteract();
        }
        else
        {
            Debug.Log("no interaction target found!");
        }
    }
}
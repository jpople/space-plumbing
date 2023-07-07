using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAudioManager : MonoBehaviour
{
    AudioSource source;
    [SerializeField] AudioClip rotateSound;
    [SerializeField] AudioClip editSound;
    [SerializeField] AudioClip resetSound;
    [SerializeField] AudioClip submitSound;
    [SerializeField] AudioClip errorSound;

    private void Awake() {
        source = GetComponent<AudioSource>();
    }

    public void Rotate() {
        source.PlayOneShot(rotateSound);
    }

    public void Edit() {
        source.PlayOneShot(editSound);
    }

    public void Reset() {
        source.PlayOneShot(resetSound);
    }

    public void Submit() {
        source.PlayOneShot(submitSound);
    }

    public void Error() {
        source.PlayOneShot(errorSound);
    }

}

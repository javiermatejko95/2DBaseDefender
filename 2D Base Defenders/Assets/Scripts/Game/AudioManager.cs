using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    #region STATIC
    public static AudioManager Instance = null;
    #endregion

    #region EXPOSED_FIELDS
    [SerializeField] private AudioSource effectSource = null;
    [SerializeField] private AudioSource musicSource = null;
    #endregion

    #region UNITY_CALLS
    private void Start()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    #region PUBLIC_METHODS
    public void PlaySound(AudioClip clip)
    {
        effectSource.PlayOneShot(clip);
    }
    #endregion
}

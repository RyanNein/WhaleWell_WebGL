using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MenuVolume : MonoBehaviour, IMenuButton
{
    TextMeshProUGUI text;

    float volume;
    const float minVolumeDb = -80f;
    const float maxVolumeDb = 0f;
    const float maxVolumeDisplay = 100f;
    const float volumeStep = 10f;
    const float minLinearVolume = 0.0001f;

    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        volume = AudioManager.Instance.GetGroupVolume(AudioManager.AudioGroups.MasterGroup);
        UpdateVolumeText();
    }

    public void DoClick()
    {
        float normalizedVolume = NormalizeVolume(volume);
        normalizedVolume += volumeStep;
        if (normalizedVolume > maxVolumeDisplay)
        {
            normalizedVolume = 0f;
        }

        volume = DenormalizeVolume(normalizedVolume);
        AudioManager.Instance.SetGroupVolume(AudioManager.AudioGroups.MasterGroup, volume);
        UpdateVolumeText();
    }

    public void Highlight(bool _on)
    {
        var alpha = _on ? 1f : 0.5f;
        text.color = new Color(1f, 1f, 1f, alpha);
    }

    void UpdateVolumeText()
    {
        float normalizedVolume = NormalizeVolume(volume);
        text.text = "Volume " + normalizedVolume.ToString("F0");
    }

    float NormalizeVolume(float volumeDb)
    {
        float linearVolume = Mathf.Pow(10f, volumeDb / 20f);
        return Mathf.Lerp(0, maxVolumeDisplay, Mathf.InverseLerp(minLinearVolume, 1f, linearVolume));
    }

    float DenormalizeVolume(float normalizedVolume)
    {
        float linearVolume = Mathf.Lerp(minLinearVolume, 1f, normalizedVolume / maxVolumeDisplay);
        return 20f * Mathf.Log10(linearVolume);
    }
}

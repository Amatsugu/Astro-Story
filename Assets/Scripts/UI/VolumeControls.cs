using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeControls : MonoBehaviour
{
	public AudioMixer mixer;
	public Slider sfxSlider;
	public Slider musicSlider;

	private void Start()
	{
		if (mixer.GetFloat("SFX", out var sfx))
			sfxSlider.value = sfx;

		if (mixer.GetFloat("Music", out var music))
			musicSlider.value = music;
	}

	private void FixedUpdate()
	{
		mixer.SetFloat("SFX", sfxSlider.value);
		mixer.SetFloat("Music", musicSlider.value);
	}
}
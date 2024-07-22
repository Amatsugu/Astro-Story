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

	private void FixedUpdate()
	{
		mixer.SetFloat("SFX", sfxSlider.value);
		mixer.SetFloat("Music", musicSlider.value);
	}
}
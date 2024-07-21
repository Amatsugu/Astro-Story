using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.Audio;

public class MusicManager : MonoBehaviour
{
	public static MusicManager Instance { get; private set; }
	public AudioClip mainBGM;
	public GameObject playbackObject;
	public AudioMixerGroup mixer;
	public float fadeDuration = 1;

	private AudioSource[] _sources;
	private int _curSource;

	private bool _isFading;
	private float[] _fadeTime;
	private float[] _fadeStart;
	private float[] _fadeStop;

	[Serializable]
	public struct Music
	{
		public YarnManager.Planet planet;
		public AudioClip clip;
	}

	public List<Music> planetMusic;

	private void Awake()
	{
		Instance = this;
		_sources = new AudioSource[2];
		_fadeTime = new float[2];
		_fadeStart = new float[2];
		_fadeStop = new float[2];

		_sources[0] = playbackObject.AddComponent<AudioSource>();
		_sources[1] = playbackObject.AddComponent<AudioSource>();

		foreach (var source in _sources)
		{
			source.outputAudioMixerGroup = mixer;
			source.loop = true;
		}
	}

	private void Start()
	{
		Play(mainBGM);
	}

	private void Update()
	{
		if (_isFading)
			PerformFade();
		
		for (int i = 0; i < _sources.Length; i++)
		{
			var t = _fadeTime[i] / fadeDuration;
			_sources[i].volume = Mathf.Lerp(_fadeStart[i], _fadeStop[i], t);
		}

	}

	private void PerformFade()
	{
		for (int i = 0; i < _fadeTime.Length; i++)
		{
			_fadeTime[i] += Time.deltaTime;
			if (_fadeTime[i] > fadeDuration)
			{
				_fadeTime[i] = fadeDuration;
				_isFading = false;
			}
		}
	}

	public AudioClip GetMusicForPlanet(YarnManager.Planet planet)
	{
		var clip = planetMusic.FirstOrDefault(m => m.planet == planet).clip;
		if (clip == null)
			return mainBGM;
		return clip;
	}

	public void Play(AudioClip clip)
	{
		var curSource = _sources[_curSource];

		if (curSource.isPlaying)
		{
			CrossFade(clip, _curSource, (_curSource + 1) % _sources.Length, fadeDuration);
			return;
		}

		curSource.clip = clip;
		curSource.Play();
		_fadeTime[_curSource] = fadeDuration;
		_fadeStop[_curSource] = 1;
	}

	private void CrossFade(AudioClip clip, int from, int to, float duration)
	{
		var fromSource = _sources[from];
		var toSource = _sources[to];


		_curSource = to;
		toSource.clip = clip;
		toSource.Play();

		_isFading = true;

		_fadeStart[from] = fromSource.volume;
		_fadeStop[from] = 0;

		_fadeStart[to] = toSource.volume;
		_fadeStop[to] = 1;

		_fadeTime[from] = 0;
		_fadeTime[to] = 0;
	}

	public static void PlayThemeFor(YarnManager.Planet planet)
	{
		var music = Instance.GetMusicForPlanet(planet);
		Instance.Play(music);
	}

	public static void PlayMainTheme()
	{
		Instance.Play(Instance.mainBGM);
	}
}
using System.Collections;
using System.Collections.Generic;

using TMPro;

using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class ResourceHUD : MonoBehaviour
{
	public Resource resource;

	private TextMeshProUGUI _text;
	private void Start()
	{
		_text = GetComponent<TextMeshProUGUI>();
	}

	private void FixedUpdate()
	{
		_text.SetText($"{ResourceTracker.GetResouceCount(resource)}");
	}
}

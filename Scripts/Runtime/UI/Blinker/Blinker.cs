using Extenity.BeyondAudio;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Extenity.UIToolbox
{

	public class Blinker : MonoBehaviour
	{
		[Header("Timing")]
		public float BlinkInterval = 0.4f;
		public float BlinkOnPercentage = 0.8f;
		public float Duration = 0f;
		[Header("Audio")]
		public string BlinkAudio = "";
		public string HideAudio = "";
		[Header("Color")]
		public Color OnColor = Color.white;
		public Color OffColor = Color.grey;
		[Header("Controlled Objects")]
		public MonoBehaviour ActivatedObject;
		public CanvasGroup ActivatedCanvasGroup;
		public SpriteRenderer ColoredSprite;
		public Image ColoredImage;
		public TextMeshProUGUI ColoredTextMeshProUGUI;

		public float BlinkOnDuration { get { return BlinkInterval * BlinkOnPercentage; } }
		public float BlinkOffDuration { get { return BlinkInterval * (1f - BlinkOnPercentage); } }

		private bool IsBlinking;
		private bool BlinkState;
		private float StartTime = -1f;
		private float NextActionTime = -1f;

		private void Start()
		{
			StopBlinking();
		}

		private void FixedUpdate()
		{
			if (!IsBlinking)
				return;

			var now = Time.time;
			if (NextActionTime > now)
				return;

			if (Duration > 0f && now - StartTime > Duration)
			{
				StopBlinking();
				return;
			}

			BlinkState = !BlinkState;
			SwitchState(BlinkState);
			NextActionTime = now + (BlinkState ? BlinkOnDuration : BlinkOffDuration);
		}

		public void StartBlinking()
		{
			IsBlinking = true;
			BlinkState = false;
			SwitchState(BlinkState);
			StartTime = Time.time;
			NextActionTime = StartTime + BlinkOnDuration;
		}

		public void StopBlinking()
		{
			IsBlinking = false;
			BlinkState = false;
			SwitchState(BlinkState);
			StartTime = -1f;
			NextActionTime = -1f;
		}

		private void SwitchState(bool blinkState)
		{
			if (ActivatedObject)
				ActivatedObject.enabled = blinkState;
			if (ActivatedCanvasGroup)
			{
				if (blinkState)
				{
					ActivatedCanvasGroup.alpha = 1f;
				}
				else
				{
					ActivatedCanvasGroup.alpha = 0f;
				}
			}
			if (ColoredSprite)
				ColoredSprite.color = blinkState ? OnColor : OffColor;
			if (ColoredImage)
				ColoredImage.color = blinkState ? OnColor : OffColor;
			if (ColoredTextMeshProUGUI)
				ColoredTextMeshProUGUI.color = blinkState ? OnColor : OffColor;

			if (blinkState)
			{
				AudioManager.Play(HideAudio);
			}
			else
			{
				AudioManager.Play(BlinkAudio);
			}
		}
	}

}
using System;
using DG.Tweening;
using Extenity.MathToolbox;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Extenity.UIToolbox
{

	public class Bar : MonoBehaviour
	{
		[BoxGroup("Setup")]
		public RectTransform BarMask;
		[BoxGroup("Setup")]
		public RectTransform BarIncreaseMask;
		[BoxGroup("Setup")]
		public CanvasGroup BarIncreaseMaskCanvasGroup;
		[BoxGroup("Setup")]
		public RectTransform BarDecreaseMask;
		[BoxGroup("Setup")]
		public CanvasGroup BarDecreaseMaskCanvasGroup;
		[BoxGroup("Setup")]
		public float BarSize = 100;
		[BoxGroup("Setup")]
		[Tooltip("An initial value below zero means the bar won't be initialized.")]
		public float InitialValue = -1f;

		[BoxGroup("Animation")]
		public Ease AnimationEasing = Ease.OutCubic;
		[Tooltip("Duration of 0 means no animation.")]
		[Range(0f, 10f)]
		[HorizontalGroup("Animation/AnimationDurationGroup")]
		public float AnimationDuration = 0f;
#if UNITY_EDITOR
		[Button("Disable Animation")]
		[DisableIf(nameof(AnimationDuration), 0f)]
		[HorizontalGroup("Animation/AnimationDurationGroup")]
		private void _DisableAnimation()
		{
			AnimationDuration = 0f;
		}
#endif
		[Range(0f, 10f)]
		[BoxGroup("Animation")]
		public float AnimationDelay = 0f;

		private Tweener Animation;
		private float AnimationEndValue = float.NaN;

		private void Awake()
		{
			if (InitialValue >= 0)
			{
				SetValue(InitialValue, true);
			}
			HideIncreaseDecreaseMasksAndEndAnimation();
		}

		private void OnDestroy()
		{
			if (Animation != null)
			{
				Animation.Kill(false);
				Animation = null;
			}
		}

		public void SetValue(float percentage)
		{
			SetValueUnclamped(Mathf.Clamp01(percentage), false);
		}

		public void SetValue(float percentage, bool skipAnimation)
		{
			SetValueUnclamped(Mathf.Clamp01(percentage), skipAnimation);
		}

		public void SetValueUnclamped(float percentage)
		{
			SetValueUnclamped(percentage, false);
		}

		public void SetValueUnclamped(float percentage, bool skipAnimation)
		{
			var currentSize = BarMask.sizeDelta;
			var newSize = currentSize;
			newSize.x = BarSize * percentage;

			// Check if the new value is the same as previously set new value
			if ((!float.IsNaN(AnimationEndValue) && newSize.x.IsAlmostEqual(AnimationEndValue)) || // This line checks for the target value of ongoing animation.
				newSize.x.IsAlmostEqual(currentSize.x)) // This line checks for the current size of bar mask. Doing this covers the cases for animationless bars.
			{
				return;
			}

			if (AnimationDuration > 0f && !skipAnimation
#if UNITY_EDITOR
				&& Application.isPlaying
#endif
				)
			{
				AnimationEndValue = newSize.x;

				if (Animation == null)
				{
					Animation = DOTween.To(TweenGetterX, TweenSetterX, newSize.x, AnimationDuration)
						.SetDelay(AnimationDelay)
						.SetEase(AnimationEasing)
						.SetAutoKill(false)
						.SetUpdate(UpdateType.Late)
						.SetTarget(this)
						.OnComplete(HideIncreaseDecreaseMasksAndEndAnimation);
				}
				else
				{
					Animation.ChangeValues(currentSize.x, newSize.x, AnimationDuration);
					Animation.Restart(true);
				}

				{
					if (BarIncreaseMaskCanvasGroup && newSize.x > currentSize.x)
					{
						if (BarDecreaseMaskCanvasGroup) // Disable the other mask first
							BarDecreaseMaskCanvasGroup.alpha = 0f;
						var sizeX = newSize.x - currentSize.x;
						BarIncreaseMask.anchoredPosition = new Vector2(BarMask.anchoredPosition.x + currentSize.x, BarIncreaseMask.anchoredPosition.y);
						BarIncreaseMask.sizeDelta = new Vector2(sizeX, BarIncreaseMask.sizeDelta.y);
						BarIncreaseMaskCanvasGroup.alpha = 1f;
					}
					else if (BarDecreaseMaskCanvasGroup && newSize.x < currentSize.x)
					{
						if (BarIncreaseMaskCanvasGroup) // Disable the other mask first
							BarIncreaseMaskCanvasGroup.alpha = 0f;
						var sizeX = currentSize.x - newSize.x;
						BarDecreaseMask.anchoredPosition = new Vector2(BarMask.anchoredPosition.x + currentSize.x - sizeX, BarDecreaseMask.anchoredPosition.y);
						BarDecreaseMask.sizeDelta = new Vector2(sizeX, BarDecreaseMask.sizeDelta.y);
						BarDecreaseMaskCanvasGroup.alpha = 1f;
					}
				}
			}
			else
			{
				BarMask.sizeDelta = newSize;
				HideIncreaseDecreaseMasksAndEndAnimation();
			}
		}

		private void HideIncreaseDecreaseMasksAndEndAnimation()
		{
			if (BarIncreaseMaskCanvasGroup)
			{
				BarIncreaseMaskCanvasGroup.alpha = 0f;
			}
			if (BarDecreaseMaskCanvasGroup)
			{
				BarDecreaseMaskCanvasGroup.alpha = 0f;
			}

			AnimationEndValue = float.NaN;
		}

		private void TweenSetterX(float newValue)
		{
			if (BarDecreaseMaskCanvasGroup && BarDecreaseMaskCanvasGroup.alpha > 0f)
			{
				var sizeX = BarMask.sizeDelta.x - AnimationEndValue;
				BarDecreaseMask.anchoredPosition = new Vector2(BarMask.anchoredPosition.x + AnimationEndValue, BarDecreaseMask.anchoredPosition.y);
				BarDecreaseMask.sizeDelta = new Vector2(sizeX, BarDecreaseMask.sizeDelta.y);
			}

			BarMask.sizeDelta = new Vector2(newValue, BarMask.sizeDelta.y);
		}

		private float TweenGetterX()
		{
			return BarMask.sizeDelta.x;
		}

		#region Test

#if UNITY_EDITOR
#pragma warning disable 414

		[NonSerialized, ShowInInspector]
		[BoxGroup("Test")]
		[HorizontalGroup("Test/Hor")]
		[Range(0f, 1f)]
		[EnableIf(nameof(BarMask))]
		[OnValueChanged(nameof(SetValue))]
		private float _InspectorBar;

		[ShowInInspector]
		[Button("Min"), ButtonGroup("Test/Hor/ValueGroup")]
		[EnableIf(nameof(BarMask))]
		private void _SetToMin()
		{
			SetValue(_InspectorBar = 0f);
		}

		[ShowInInspector]
		[Button("Max"), ButtonGroup("Test/Hor/ValueGroup")]
		[EnableIf(nameof(BarMask))]
		private void _SetToMax()
		{
			SetValue(_InspectorBar = 1f);
		}

#pragma warning restore 414
#endif

		#endregion
	}

}
using System;
using System.Collections.Generic;
using DG.Tweening;
using Extenity.DataToolbox;
using Extenity.FlowToolbox;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Extenity.UIToolbox
{

	public struct FeedLine
	{
		public float ExpireTime;
		public FeedLineUI UI;
		//public string Content; No need to keep content data. But keep it commented out for future needs.

		public FeedLine(float expireTime, FeedLineUI ui/*, string content*/)
		{
			ExpireTime = expireTime;
			UI = ui;
			//Content = content;
		}
	}

	public class FeedUI : MonoBehaviour
	{
		#region Initialization

		protected void Awake()
		{
			InitializeLines();
			InitializeLineUIs();
			InitializeLinePositioning();
		}

		#endregion

		#region Deinitialization

		protected void OnDestroy()
		{
			DeinitializeLinePositioning();
		}

		#endregion

		#region Update

		//protected void Update()
		//{
		//}

		#endregion

		#region Lines

		[Header("Lines")]
		public int LineCount = 10;
		public float LineDuration = 5f;
		public bool UnscaledLineDuration = false;

		[NonSerialized]
		public CircularArray<FeedLine> Lines;

		private void InitializeLines()
		{
			Lines = new CircularArray<FeedLine>(LineCount);
		}

		public void Append(string content)
		{
#if UNITY_EDITOR
			if (_AppendLineNumberPrefix)
			{
				content = ++_LastLineNumber + " | " + content;
			}
#endif

			if (Lines.Count == 0)
			{
				this.FastInvoke(TimeToDecay, LineDuration, UnscaledLineDuration);
			}
			else if (Lines.Count == Lines.Capacity)
			{
				RemoveTailingImmediately();
			}

			var lineUI = GetLineUIFromPool();
			lineUI.Set(content);
			Lines.Add(new FeedLine(Loop.Time + LineDuration, lineUI));
			InvalidateLinePositions();
		}

		private void RemoveTailingImmediately()
		{
			if (Lines.Count == 0)
				return;

			var ui = Lines.TailingItem.UI;
			Lines.RemoveTailing();
			AddToPool(ui);
			InvalidateLinePositions();
		}

		private void TimeToDecay()
		{
			var now = Loop.Time;

			while (Lines.Count > 0 && Lines.TailingItem.ExpireTime <= now + 0.01f)
			{
				var ui = Lines.TailingItem.UI;
				Lines.RemoveTailing();
				AddToPool(ui);
				InvalidateLinePositions();
			}

			if (Lines.Count > 0)
			{
				this.FastInvoke(TimeToDecay, Lines.TailingItem.ExpireTime - now, UnscaledLineDuration);
			}
		}

		#endregion

		#region Line UIs

		[Header("Line UI")]
		public FeedLineUI LineTemplate;

		private List<FeedLineUI> PooledLineUIs;
		/// <summary>
		/// Used by the animation system to know if a line should be placed before animation begins.
		/// </summary>
		private List<int> LineUIInstanceIDsThatAreJustBeingCreated;

		private void InitializeLineUIs()
		{
			LineTemplate.gameObject.SetActive(false);

			PooledLineUIs = new List<FeedLineUI>(LineCount);
			for (int i = 0; i < LineCount; i++)
			{
				var lineUI = Instantiate(LineTemplate, LineTemplate.transform.parent);
#if UNITY_EDITOR
				lineUI.gameObject.name = "Line-" + i;
#endif
				PooledLineUIs.Add(lineUI);
			}

			LineUIInstanceIDsThatAreJustBeingCreated = new List<int>(LineCount);
		}

		private FeedLineUI GetLineUIFromPool()
		{
			// By the time the code gets to this method, there must be at least one line should exist in the pool.
			Debug.Assert(PooledLineUIs.Count > 0);

			var index = PooledLineUIs.Count - 1;
			var ui = PooledLineUIs[index];
			PooledLineUIs.RemoveAt(index);
			ui.gameObject.SetActive(true);
			LineUIInstanceIDsThatAreJustBeingCreated.Add(ui.GetInstanceID());
			return ui;
		}

		private void AddToPool(FeedLineUI ui)
		{
			Debug.Assert(!PooledLineUIs.Contains(ui));

			DOTween.Kill(ui, false); // Kill any animations before adding into the pool.
			ui.gameObject.SetActive(false);
			PooledLineUIs.Add(ui);
		}

		#endregion

		#region ID

		/*
		private static int _LastGivenID = 100;

		private static int GenerateID()
		{
			++_LastGivenID;
			if (_LastGivenID > 1000000000)
			{
				_LastGivenID = 100; // Prevent overflow.
			}
			return _LastGivenID;
		}
		*/

		#endregion

		#region Line Positioning

		[Header("Line Positioning")]
		public bool InverseLines;
		public float LinePositionStepY = 20f;

		[Header("Animations")]
		public float LineAppearanceOffsetY = 20f;
		[Range(0f, 3f)]
		[Tooltip("Zero means no animation which also means no overhead of animation system.")]
		public float LineMoveAnimationDuration = 0f;
		public Ease LineMoveAnimationEasing = Ease.OutCubic;

		private bool IsLinePositionsInvalidated;

		private void InitializeLinePositioning()
		{
			Loop.LateUpdateCallbacks.AddListener(OnCustomLateUpdate);
		}

		private void DeinitializeLinePositioning()
		{
			Loop.LateUpdateCallbacks.RemoveListener(OnCustomLateUpdate);
		}

		private void OnCustomLateUpdate()
		{
			if (IsLinePositionsInvalidated)
			{
				if (LineMoveAnimationDuration > 0f)
				{
					RecalculateLinePositionsWithAnimation();
				}
				else
				{
					RecalculateLinePositionsWithoutAnimation();
				}

				IsLinePositionsInvalidated = false;
			}
		}

		private void InvalidateLinePositions()
		{
			IsLinePositionsInvalidated = true;
		}

		private void RecalculateLinePositionsWithoutAnimation()
		{
			var stepY = LinePositionStepY;
			var positionY = InverseLines
				? -stepY * (Lines.Count - 1)
				: 0f;

			foreach (var line in Lines)
			{
				var ui = line.UI;
				ui.RectTransform.anchoredPosition = new Vector2(0f, positionY);
				positionY += stepY;
			}
		}

		private void RecalculateLinePositionsWithAnimation()
		{
			var stepY = LinePositionStepY;
			var positionY = InverseLines
				? -stepY * (Lines.Count - 1)
				: 0f;

			foreach (var line in Lines)
			{
				var ui = line.UI;
				var position = new Vector2(0f, positionY);
				var justBeingCreated = LineUIInstanceIDsThatAreJustBeingCreated.Contains(ui.GetInstanceID());
				if (justBeingCreated)
				{
					ui.RectTransform.anchoredPosition = position + new Vector2(0f, LineAppearanceOffsetY);
				}

				// This didn't do good for some reason, when instantiating many lines rapidly. 
				//ui.RectTransform.DOAnchorPos(position, LineMoveAnimationDuration, false).SetEase(LineMoveAnimationEasing);

				DOTween.To(() => ui.RectTransform.anchoredPosition, x => ui.RectTransform.anchoredPosition = x, position, LineMoveAnimationDuration)
					.SetTarget(ui)
					.SetEase(LineMoveAnimationEasing);

				positionY += stepY;
			}

			LineUIInstanceIDsThatAreJustBeingCreated.Clear();
		}

		#endregion

		#region Debug

#if UNITY_EDITOR

		[Header("Debug")]
		[NonSerialized, ShowInInspector]
		public bool _AppendLineNumberPrefix;
		private int _LastLineNumber;

#endif

		#endregion
	}

}
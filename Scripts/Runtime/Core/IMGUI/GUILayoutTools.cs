﻿using System;
using Extenity.ColoringToolbox;
using UnityEngine;

namespace Extenity.IMGUIToolbox
{

	public static class GUILayoutTools
	{
		#region Controls - Button

		public static bool Button(string text, bool enabledState, params GUILayoutOption[] layoutOptions)
		{
			return Button(text, enabledState ? EnabledState.Unchanged : EnabledState.Disabled, GUI.skin.button, layoutOptions);
		}

		public static bool Button(string text, EnabledState enabledState, params GUILayoutOption[] layoutOptions)
		{
			return Button(text, enabledState, GUI.skin.button, layoutOptions);
		}

		public static bool Button(string text, bool enabledState, GUIStyle guiStyle, params GUILayoutOption[] layoutOptions)
		{
			return Button(text, enabledState ? EnabledState.Unchanged : EnabledState.Disabled, guiStyle, layoutOptions);
		}

		public static bool Button(string text, EnabledState enabledState, GUIStyle guiStyle, params GUILayoutOption[] layoutOptions)
		{
			switch (enabledState)
			{
				case EnabledState.Unchanged:
					return GUILayout.Button(text, guiStyle, layoutOptions);
				case EnabledState.Enabled:
					{
						var enabledWas = GUI.enabled;
						GUI.enabled = true;
						var result = GUILayout.Button(text, guiStyle, layoutOptions);
						GUI.enabled = enabledWas;
						return result;
					}
				case EnabledState.Disabled:
					{
						var enabledWas = GUI.enabled;
						GUI.enabled = false;
						var result = GUILayout.Button(text, guiStyle, layoutOptions);
						GUI.enabled = enabledWas;
						return result;
					}
				default:
					throw new ArgumentOutOfRangeException(nameof(enabledState), enabledState, null);
			}
		}

		#endregion

		#region Controls - Bars

		public static void Bars(float width, float height, float separatorLength, bool drawBottomLabels, ColorScale barColorScale, ColorScale barBackgroundColorScale, int barCount, Func<int, float> barValueGetter)
		{
			var rect = GUILayoutUtility.GetRect(width, width, height, height);
			GUITools.Bars(rect, separatorLength, drawBottomLabels, barColorScale, barBackgroundColorScale, barCount, barValueGetter);
		}

		#endregion

		#region Layout

		public static void BeginHorizontalWithSpace(float spaceBefore)
		{
			if (spaceBefore > 0f)
				GUILayout.Space(spaceBefore);
			GUILayout.BeginHorizontal();
		}

		public static void EndHorizontalWithSpace(float spaceAfter)
		{
			GUILayout.EndHorizontal();
			if (spaceAfter > 0f)
				GUILayout.Space(spaceAfter);
		}

		public static void BeginVerticalWithSpace(float spaceBefore)
		{
			if (spaceBefore > 0f)
				GUILayout.Space(spaceBefore);
			GUILayout.BeginVertical();
		}

		public static void EndVerticalWithSpace(float spaceAfter)
		{
			GUILayout.EndVertical();
			if (spaceAfter > 0f)
				GUILayout.Space(spaceAfter);
		}

		#endregion

		#region Non-Alloc Helpers

		public static readonly GUILayoutOption ExpandWidth = GUILayout.ExpandWidth(true);
		public static readonly GUILayoutOption DontExpandWidth = GUILayout.ExpandWidth(false);
		public static readonly GUILayoutOption ExpandHeight = GUILayout.ExpandHeight(true);
		public static readonly GUILayoutOption DontExpandHeight = GUILayout.ExpandHeight(false);
		public static readonly GUILayoutOption[] ExpandWidthAndHeight = { GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true) };
		public static readonly GUILayoutOption[] DontExpandWidthAndHeight = { GUILayout.ExpandWidth(false), GUILayout.ExpandHeight(false) };

		#endregion
	}

}
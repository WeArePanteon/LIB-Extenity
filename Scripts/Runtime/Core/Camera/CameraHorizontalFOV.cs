﻿using Sirenix.OdinInspector;
using UnityEngine;

namespace Extenity.CameraToolbox
{

	[ExecuteAlways]
	public class CameraHorizontalFOV : MonoBehaviour
	{
		[Required]
		public Camera Camera;

		[OnValueChanged(nameof(Invalidate))]
		public float HorizontalFOV = 45f;

		private float LastAspectRatio = -1f;

		protected void LateUpdate()
		{
			if (!Camera)
				return;

			// Method 1: This is just an estimation and drastically fails in large aspect ratios.
			// Camera.fieldOfView = HorizontalFOV / ((float)Camera.pixelWidth / Camera.pixelHeight);

			// Method 2: This is precise.
			var currentAspectRatio = Camera.aspect;
			if (LastAspectRatio != currentAspectRatio)
			{
				LastAspectRatio = currentAspectRatio;

				var hFOVrad = HorizontalFOV * Mathf.Deg2Rad;
				var camH = Mathf.Tan(hFOVrad * 0.5f) / currentAspectRatio;
				var vFOVrad = Mathf.Atan(camH) * 2f;
				Camera.fieldOfView = vFOVrad * Mathf.Rad2Deg;
			}
		}

		public void Invalidate()
		{
			LastAspectRatio = -1f;
		}
	}

}
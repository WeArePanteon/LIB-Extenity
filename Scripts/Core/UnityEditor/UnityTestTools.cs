﻿using System;
using Extenity.SceneManagementToolbox;
using UnityEngine;

namespace Extenity.UnityTestToolbox
{

	public static class UnityTestTools
	{
		#region Cleanup

		public static void Cleanup()
		{
			SceneManagerTools.GetLoadedScenes().ForEach(scene =>
			{
				foreach (var rootObject in scene.GetRootGameObjects())
				{
					if (!rootObject.GetComponent("UnityEngine.TestTools.TestRunner.PlaymodeTestsController"))
					{
						GameObject.DestroyImmediate(rootObject);
					}
				}
			});
		}

		#endregion

		#region Memory Checker

		private static Int64 DetectedMemoryInMemoryCheck;

		/// <summary>
		/// Note that memory check does not work consistently. So make sure you run the tests multiple times.
		/// </summary>
		public static void BeginMemoryCheck()
		{
			if (DetectedMemoryInMemoryCheck != 0)
			{
				DetectedMemoryInMemoryCheck = 0; // Reset it for further use.
				throw new Exception("Memory check was already started.");
			}

			DetectedMemoryInMemoryCheck = GC.GetTotalMemory(false);
			if (DetectedMemoryInMemoryCheck == 0)
			{
				throw new Exception("Failed to get current memory size.");
			}
		}

		/// <summary>
		/// Note that memory check does not work consistently. So make sure you run the tests multiple times.
		/// </summary>
		public static bool EndMemoryCheck()
		{
			if (DetectedMemoryInMemoryCheck == 0)
			{
				throw new Exception("Memory check was not started.");
			}

			var change = GC.GetTotalMemory(false) - DetectedMemoryInMemoryCheck;
			DetectedMemoryInMemoryCheck = 0;
			if (change != 0)
			{
				Debug.LogWarning($"Detected a memory change of '{change}' bytes.");
			}
			return change != 0;
		}

		#endregion
	}

}
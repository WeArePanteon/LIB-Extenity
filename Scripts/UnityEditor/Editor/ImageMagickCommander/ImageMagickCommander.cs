using System;
using System.Diagnostics;
using System.IO;
using Extenity.DataToolbox;
using Extenity.GameObjectToolbox;
using Extenity.SceneManagementToolbox;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using Debug = UnityEngine.Debug;

namespace Extenity.UnityEditorToolbox.ImageMagick
{

	public static class ImageMagickCommander
	{
		#region Reflection Probe Blurring

		public static void BlurReflectionProbesInActiveScene(ActiveCheck activeCheck)
		{
			SceneManager.GetActiveScene().BlurReflectionProbes(activeCheck);
		}

		public static void BlurReflectionProbesInLoadedScenes(ActiveCheck activeCheck)
		{
			SceneManagerTools.GetLoadedScenes(true).ForEach(scene => scene.BlurReflectionProbes(activeCheck));
		}

		public static void BlurReflectionProbes(this Scene scene, ActiveCheck activeCheck)
		{
			const int sizeX = 96;
			const int sizeY = 16;

			var reflectionProbes = scene.FindObjectsOfType<ReflectionProbe>(activeCheck);
			foreach (var reflectionProbe in reflectionProbes)
			{
				if (!reflectionProbe.bakedTexture)
					continue;

				var path = AssetDatabase.GetAssetPath(reflectionProbe.bakedTexture);
				Debug.LogFormat("Downsizing Reflection Probe baked texture to '{0}x{1}' at path: {2}", sizeX, sizeY, path);

				path = Path.GetFullPath(path).FixDirectorySeparatorChars('\\');
				var directoryPath = Path.GetDirectoryName(path).FixDirectorySeparatorChars('\\');

				ResizeImages(path, sizeX, sizeY, directoryPath + "/%[filename:fname].exr");
			}

			AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate | ImportAssetOptions.ForceSynchronousImport);
		}

		#endregion

		#region Resize Image

		public static void ResizeImages(string inputFilter, int sizeX, int sizeY, string outputFilter)
		{
			var errorReceived = false;

			var process = new Process();
			process.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
			process.StartInfo.RedirectStandardError = true;
			process.StartInfo.RedirectStandardOutput = true;
			process.StartInfo.CreateNoWindow = false;
			process.StartInfo.UseShellExecute = false;
			process.StartInfo.FileName = "magick.exe";
			process.StartInfo.Arguments = $"\"{inputFilter}\" -resize {sizeX}x{sizeY} -set filename:fname \"%t\" \"{outputFilter}\"";

			process.ErrorDataReceived += (sender, eventArgs) =>
			{
				var data = eventArgs.Data.Trim();
				if (!string.IsNullOrEmpty(data))
				{
					errorReceived = true;
					Debug.LogError("ImageMagick Error: " + data);
				}
			};
			process.OutputDataReceived += (sender, eventArgs) =>
			{
				var data = eventArgs.Data.Trim();
				if (!string.IsNullOrEmpty(data))
					Debug.Log("ImageMagick Output: " + data);
			};

			//Debug.Log("launch |     " + process.StartInfo.FileName);
			//Debug.Log("args |     " + process.StartInfo.Arguments);

			process.Start();

			process.BeginErrorReadLine();
			process.BeginOutputReadLine();

			process.WaitForExit();

			if (errorReceived)
			{
				throw new Exception("Failed to convert image. See previous errors.");
			}
		}

		#endregion
	}

}
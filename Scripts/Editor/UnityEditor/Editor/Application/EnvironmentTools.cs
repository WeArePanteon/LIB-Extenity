using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Extenity.FileSystemToolbox;
using Extenity.UnityEditorToolbox.Editor;
using Unity.EditorCoroutines.Editor;
using UnityEditor;
using UnityEngine;

namespace Extenity.ApplicationToolbox.Editor
{

	public static class EnvironmentTools
	{
		#region Configuration

#if UNITY_EDITOR_WIN
		private const string PathKey = "PATH";
#elif UNITY_EDITOR_OSX
		private const string PathKey = "HOME";
#else
		RequiresAttention;
#endif

		private const string Menu = "Tools/Environment Variables/";

		#endregion

		#region Menu - Delete Loose Environment Paths

		[MenuItem(Menu + "Delete Loose Environment Paths/Machine", priority = 1000)]
		public static void DeleteLooseEnvironmentPaths_Machine()
		{
			EditorCoroutineUtility.StartCoroutineOwnerless(DeleteLooseEnvironmentPaths(EnvironmentVariableTarget.Machine, true));
		}

		[MenuItem(Menu + "Delete Loose Environment Paths/User", priority = 1000)]
		public static void DeleteLooseEnvironmentPaths_User()
		{
			EditorCoroutineUtility.StartCoroutineOwnerless(DeleteLooseEnvironmentPaths(EnvironmentVariableTarget.User, true));
		}

		[MenuItem(Menu + "Delete Loose Environment Paths/Process", priority = 1000)]
		public static void DeleteLooseEnvironmentPaths_Process()
		{
			EditorCoroutineUtility.StartCoroutineOwnerless(DeleteLooseEnvironmentPaths(EnvironmentVariableTarget.Process, true));
		}

		[MenuItem(Menu + "Delete Loose Environment Paths/Machine, User", priority = 1000)]
		public static void DeleteLooseEnvironmentPaths_MachineUser()
		{
			EditorCoroutineUtility.StartCoroutineOwnerless(DeleteLooseEnvironmentPaths(new[] { EnvironmentVariableTarget.Machine, EnvironmentVariableTarget.User }, true));
		}

		[MenuItem(Menu + "Delete Loose Environment Paths/Machine, User, Process", priority = 1000)]
		public static void DeleteLooseEnvironmentPaths_MachineUserProcess()
		{
			EditorCoroutineUtility.StartCoroutineOwnerless(DeleteLooseEnvironmentPaths(new[] { EnvironmentVariableTarget.Machine, EnvironmentVariableTarget.User, EnvironmentVariableTarget.Process }, true));
		}

		#endregion

		#region Menu - Log

		[MenuItem(Menu + "Log Environment Paths/Machine", priority = 2000)]
		public static void LogEnvironmentPaths_Machine()
		{
			LogPaths(GetEnvironmentPaths(EnvironmentVariableTarget.Machine), EnvironmentVariableTarget.Machine);
		}

		[MenuItem(Menu + "Log Environment Paths/User", priority = 2000)]
		public static void LogEnvironmentPaths_User()
		{
			LogPaths(GetEnvironmentPaths(EnvironmentVariableTarget.User), EnvironmentVariableTarget.User);
		}

		[MenuItem(Menu + "Log Environment Paths/Process", priority = 2000)]
		public static void LogEnvironmentPaths_Process()
		{
			LogPaths(GetEnvironmentPaths(EnvironmentVariableTarget.Process), EnvironmentVariableTarget.Process);
		}

		#endregion

		#region Fix Android Tool Paths

#if UNITY_EDITOR_WIN

		[InitializeOnEditorLaunchMethod]
		public static void EnsureAndroidToolPathsAreUnderUnityInstallation()
		{
			const bool log = false;

			// Should not even try to run if there is no Android package installed or the Android SDK is not installed with Unity.
			if (!EditorApplicationTools.IsAndroidSDKInstalledWithUnity())
			{
				return;
			}

			EnsurePathsAreUnderUnityInstallation(
				new[]
				{
					"adb.exe",
					"aapt.exe",
					"java.exe",
					"javaw.exe"
				},
				new[]
				{
					("adb.exe", null), // Android Debug Bridge tool, which will direct us into the 'platform-tools' directory.
					("aapt.exe", null), // Android Asset Packaging Tool, which will direct us into the 'build-tools' directory.
					("javaw.exe", "jre/bin") // There are 2 'javaw.exe' exist under Unity Installation. We are interested in the one that is not under 'jre/bin' directory.
				},
				log
			);
		}

#elif UNITY_EDITOR_OSX

		// Looks like Mac does not need any adjustments. Nothing to do here.

#else

		RequiresAttention;

#endif

		public static void EnsurePathsAreUnderUnityInstallation(string[] filesThatShouldNotExistOutsideOfUnityInstallation, (string FileName, string IgnoreContaining)[] filesThatWillBeSearchedInUnityInstallation, bool log)
		{
			var pathsUnderUnityInstallation = new List<string>(filesThatWillBeSearchedInUnityInstallation.Length);
			foreach (var entry in filesThatWillBeSearchedInUnityInstallation)
			{
				var path = FindToolPathInEditorInstallation(entry.FileName, entry.IgnoreContaining, log);
				pathsUnderUnityInstallation.Add(path);
			}

			var currentlyExistingPaths = new List<string>(10);
			foreach (var entry in filesThatShouldNotExistOutsideOfUnityInstallation)
			{
				GetFullPathsForFile(entry, ref currentlyExistingPaths);
			}
			for (int i = 0; i < currentlyExistingPaths.Count; i++)
			{
				currentlyExistingPaths[i] = Path.GetDirectoryName(currentlyExistingPaths[i]).AddDirectorySeparatorToEnd().FixDirectorySeparatorChars();
			}

			var message = new StringBuilder();

			// Find the paths that should be removed from PATH
			foreach (var currentlyExistingPath in currentlyExistingPaths)
			{
				var found = pathsUnderUnityInstallation.Any(pathUnderUnityInstallation => currentlyExistingPath.PathCompare(pathUnderUnityInstallation));
				if (!found)
				{
					message.AppendLine("REMOVE: " + currentlyExistingPath);
				}
			}

			// Find the paths that should be added to PATH
			foreach (var pathUnderUnityInstallation in pathsUnderUnityInstallation)
			{
				var found = currentlyExistingPaths.Any(currentlyExistingPath => pathUnderUnityInstallation.PathCompare(currentlyExistingPath));
				if (!found)
				{
					message.AppendLine("ADD: " + pathUnderUnityInstallation);
				}
			}

			if (message.Length > 0)
			{
				var fullMessage = "Attention required for Android SDK to work correctly. These PATH environment variables should be adjusted manually.\n" + message.ToString();
				Log.Error(fullMessage);
			}
		}

		#endregion

		#region Get File In Environment Path Variable

		public static bool FileExistsOnPath(string fileName)
		{
			List<string> result = null;
			GetFullPathsForFile(fileName, ref result);
			return result.Count > 0;
		}

		/// <summary>
		/// Source: https://stackoverflow.com/questions/3855956/check-if-an-executable-exists-in-the-windows-path
		/// </summary>
		public static void GetFullPathsForFile(string fileName, ref List<string> result)
		{
			if (result == null)
				result = new List<string>(1);

			// First check if file exists in working directory
			if (File.Exists(fileName))
			{
				var fullPath = Path.GetFullPath(fileName);
				result.Add(fullPath);
			}

			var paths = GetEnvironmentPaths(EnvironmentVariableTarget.Process);

			foreach (var path in paths)
			{
				if (Directory.Exists(path))
				{
					var fullPath = Path.Combine(path, fileName);
					if (File.Exists(fullPath))
						result.Add(fullPath);
				}
			}
		}

		#endregion

		#region Get Path

		public static List<string> GetEnvironmentPaths(EnvironmentVariableTarget target)
		{
			var paths = Environment.GetEnvironmentVariable(PathKey, target);
			return paths.Split(new[] { Path.PathSeparator }, StringSplitOptions.RemoveEmptyEntries)
				.ToList();
		}

		public static bool SetEnvironmentPaths(List<string> paths, EnvironmentVariableTarget target)
		{
			var joined = string.Join(Path.PathSeparator.ToString(), paths);
			Log.Info($"Setting '{target}' environment variables to: {joined}");
			try
			{
				Environment.SetEnvironmentVariable(PathKey, joined, target);
				return true;
			}
			catch (Exception exception)
			{
				Log.Error($"Failed to set environment variables of '{target}', probably because the lack of elevated rights. You may do the changes manually or launch the Editor with Administrator rights. Error: " + exception);
				return false;
			}
		}

		#endregion

		#region Find Tool Path In Editor Installation

		public static string FindToolPathInEditorInstallation(string fileName, string ignoreContaining = null, bool log = true)
		{
			ignoreContaining = ignoreContaining.FixDirectorySeparatorChars('/');
			var editorDirectory = EditorApplicationTools.UnityEditorExecutableDirectory;
			List<string> paths;

			if (!string.IsNullOrEmpty(ignoreContaining))
			{
				paths = Directory.GetFiles(editorDirectory, fileName, SearchOption.AllDirectories)
					.Where(path => path.FixDirectorySeparatorChars('/').Contains(ignoreContaining))
					.ToList();
			}
			else
			{
				paths = Directory.GetFiles(editorDirectory, fileName, SearchOption.AllDirectories)
					.ToList();
			}

			if (paths.Count == 0)
			{
				throw new FileNotFoundException($"Could not find '{fileName}' under Unity Editor installation at '{editorDirectory}'.");
			}
			else if (paths.Count > 1)
			{
				throw new Exception($"There are more than one '{fileName}' under Unity Editor installation at '{editorDirectory}':\n{string.Join("\n", paths)}");
			}
			else
			{
				var path = paths[0];
				path = Path.GetDirectoryName(path).AddDirectorySeparatorToEnd().FixDirectorySeparatorChars();
				if (log)
					Log.Info($"Found '{fileName}' at path: {path}");
				return path;
			}
		}

		#endregion

		#region Delete Path

		public static void DeleteAnyExistingEnvironmentPathThatPointsTo(string fileName, EnvironmentVariableTarget target, bool log = true)
		{
			var fullPaths = new List<string>(1);
			GetFullPathsForFile(fileName, ref fullPaths);
			if (fullPaths.Count > 0)
			{
				foreach (var fullPath in fullPaths)
				{
					var directoryPath = Path.GetDirectoryName(fullPath);
					var deletedCount = DeleteEnvironmentPath(target, directoryPath, log);
					if (deletedCount == 0)
					{
						// For some reason, the 'fileName' executable can be reached
						// via Path environment variable, but could not be deleted from
						// Path. This may cause serious side effects. Further inspection
						// is required.
						throw new Exception($"Failed to remove '{fileName}' from environment variable path of '{target}' even though it can be reached via environment path.");
					}
				}
			}
			else
			{
				if (log)
					Log.Info($"File '{fileName}' is not in the path as expected.");
			}
		}

		public static int DeleteEnvironmentPath(EnvironmentVariableTarget target, string deletedPath, bool log = true)
		{
			if (log)
				Log.Info("Removing path: " + deletedPath);

			var split = GetEnvironmentPaths(target);
			var removedEntries = new List<string>();
			for (var i = split.Count - 1; i >= 0; i--)
			{
				var path = split[i];
				if (path.PathCompare(deletedPath))
				{
					if (log)
						Log.Info("Removed path: " + split[i]);
					removedEntries.Add(split[i]);
					split.RemoveAt(i);
					i--;
				}
			}

			if (removedEntries.Count > 0)
			{
				if (!SetEnvironmentPaths(split, target))
				{
					Log.Info($"These entries should be removed if you want to do it manually ({removedEntries.Count}):");
					foreach (var removedEntry in removedEntries)
					{
						Log.Info(removedEntry);
					}
					return 0;
				}
			}

			return removedEntries.Count;
		}

		#endregion

		#region Delete Loose Paths

		public static IEnumerator DeleteLooseEnvironmentPaths(EnvironmentVariableTarget[] targets, bool askUser)
		{
			foreach (var target in targets)
			{
				yield return EditorCoroutineUtility.StartCoroutineOwnerless(DeleteLooseEnvironmentPaths(target, askUser));
			}
		}

		public static IEnumerator DeleteLooseEnvironmentPaths(EnvironmentVariableTarget target, bool askUser)
		{
			var split = GetEnvironmentPaths(target);
			var removedEntries = new List<string>();
			for (var i = split.Count - 1; i >= 0; i--)
			{
				var iCached = i;
				var path = split[i];
				var doesNotExist = false;
				try
				{
					doesNotExist = !Directory.Exists(path) && !File.Exists(path);
				}
				catch (Exception exception)
				{
					Log.Error($"Failed to check existence of path: {path}\n{exception}");
				}
				if (doesNotExist)
				{
					if (askUser)
					{
						var answered = false;

						EditorMessageBox.Show(new Vector2Int(600, 100), "Delete Path?",
							"The path is not pointing to a directory or file. Do you want to delete it?\n\n" + path,
							"Delete", "Skip",
							() =>
							{
								removedEntries.Add(split[iCached]);
								split.RemoveAt(iCached);
								answered = true;
							},
							() =>
							{
								answered = true;
							});

						while (!answered)
						{
							yield return null;
						}
					}
					else
					{
						removedEntries.Add(split[iCached]);
						split.RemoveAt(iCached);
					}
				}
			}

			if (removedEntries.Count > 0)
			{
				if (!SetEnvironmentPaths(split, target))
				{
					Log.Info($"These entries should be removed if you want to do it manually ({removedEntries.Count}):");
					foreach (var removedEntry in removedEntries)
					{
						Log.Info(removedEntry);
					}
				}
			}
		}

		#endregion

		#region Debug

		public static void LogPaths(IEnumerable<string> paths, EnvironmentVariableTarget target)
		{
			Log.Info($"Listing paths in environment variable of '{target}'\n{string.Join("\n", paths)}");
		}

		#endregion
	}

}
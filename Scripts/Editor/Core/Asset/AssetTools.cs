using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Extenity.ApplicationToolbox;
using Object = UnityEngine.Object;
using SelectionMode = UnityEditor.SelectionMode;

namespace Extenity.AssetToolbox.Editor
{

	public static class AssetTools
	{
		#region Prefabs

		public static List<GameObject> FindAllPrefabs()
		{
			var assetFolderPaths = GetAllPrefabAssetPaths();
			var assets = assetFolderPaths.Select(item => AssetDatabase.LoadAssetAtPath(item, typeof(GameObject))).Cast<GameObject>().Where(obj => obj != null);
			var list = assets.ToList();
			return list;
		}

		public static List<GameObject> FindAllPrefabsWhere(Func<GameObject, bool> predicate)
		{
			var assetFolderPaths = GetAllPrefabAssetPaths();
			var assets = assetFolderPaths.Select(item => AssetDatabase.LoadAssetAtPath(item, typeof(GameObject))).Cast<GameObject>().Where(obj => obj != null).Where(predicate);
			var list = assets.ToList();
			return list;
		}

		public static List<GameObject> FindAllPrefabsContainingComponent<TComponent>() where TComponent : Component
		{
			return FindAllPrefabsWhere(gameObject => gameObject.GetComponent<TComponent>() != null);
		}

		#endregion

		#region Get Asset Paths

		public static List<string> GetAllSceneAssetPaths()
		{
			return AssetDatabase.GetAllAssetPaths().Where(item => item.ToLowerInvariant().EndsWith(".unity")).ToList();
		}

		public static List<string> GetAllPrefabAssetPaths()
		{
			return AssetDatabase.GetAllAssetPaths().Where(item => item.ToLowerInvariant().EndsWith(".prefab")).ToList();
		}

		public static List<string> GetAllModelAssetPaths()
		{
			return AssetDatabase.GetAllAssetPaths().Where(item =>
			{
				var lower = item.ToLowerInvariant();
				return lower.EndsWith(".fbx") ||
					   lower.EndsWith(".dae") ||
					   lower.EndsWith(".3ds") ||
					   lower.EndsWith(".dxf") ||
					   lower.EndsWith(".obj") ||
					   lower.EndsWith(".skp");
			}).ToList();
		}

		public static List<string> GetAllAnimationAssetPaths()
		{
			return AssetDatabase.GetAllAssetPaths().Where(item =>
			{
				var lower = item.ToLowerInvariant();
				return lower.EndsWith(".anim") ||
					   lower.EndsWith(".controller") ||
					   lower.EndsWith(".overrideController") ||
					   lower.EndsWith(".mask");
			}).ToList();
		}

		public static List<string> GetAllMaterialAssetPaths()
		{
			return AssetDatabase.GetAllAssetPaths().Where(item => item.ToLowerInvariant().EndsWith(".mat")).ToList();
		}

		public static List<string> GetAllShaderAssetPaths()
		{
			return AssetDatabase.GetAllAssetPaths().Where(item =>
			{
				var lower = item.ToLowerInvariant();
				return lower.EndsWith(".compute") ||
					   lower.EndsWith(".shadervariants") ||
					   lower.EndsWith(".shader");
			}).ToList();
		}

		public static List<string> GetAllTextureAssetPaths()
		{
			return AssetDatabase.GetAllAssetPaths().Where(item =>
			{
				var lower = item.ToLowerInvariant();
				return lower.EndsWith(".cubemap") ||
					   lower.EndsWith(".bmp") ||
					   lower.EndsWith(".exr") ||
					   lower.EndsWith(".gif") ||
					   lower.EndsWith(".hdr") ||
					   lower.EndsWith(".iff") ||
					   lower.EndsWith(".jpg") ||
					   lower.EndsWith(".pict") ||
					   lower.EndsWith(".png") ||
					   lower.EndsWith(".psd") ||
					   lower.EndsWith(".tga") ||
					   lower.EndsWith(".tiff");
			}).ToList();
		}

		public static List<string> GetAllProceduralTextureAssetPaths()
		{
			return AssetDatabase.GetAllAssetPaths().Where(item => item.ToLowerInvariant().EndsWith(".sbsar")).ToList();
		}

		public static List<string> GetAllRenderTextureAssetPaths()
		{
			return AssetDatabase.GetAllAssetPaths().Where(item => item.ToLowerInvariant().EndsWith(".renderTexture")).ToList();
		}

		public static List<string> GetAllLightmapAssetPaths()
		{
			return AssetDatabase.GetAllAssetPaths().Where(item => item.ToLowerInvariant().EndsWith(".giparams")).ToList();
		}

		public static List<string> GetAllFlareAssetPaths()
		{
			return AssetDatabase.GetAllAssetPaths().Where(item => item.ToLowerInvariant().EndsWith(".flare")).ToList();
		}

		public static List<string> GetAllVideoAssetPaths()
		{
			return AssetDatabase.GetAllAssetPaths().Where(item =>
			{
				var lower = item.ToLowerInvariant();
				return lower.EndsWith(".asf") ||
					   lower.EndsWith(".avi") ||
					   lower.EndsWith(".dv") ||
					   lower.EndsWith(".m4v") ||
					   lower.EndsWith(".mov") ||
					   lower.EndsWith(".mp4") ||
					   lower.EndsWith(".mpg") ||
					   lower.EndsWith(".mpeg") ||
					   lower.EndsWith(".ogv") ||
					   lower.EndsWith(".vp8") ||
					   lower.EndsWith(".webm") ||
					   lower.EndsWith(".wmv");
			}).ToList();
		}

		public static List<string> GetAllUIAssetPaths()
		{
			return AssetDatabase.GetAllAssetPaths().Where(item =>
			{
				var lower = item.ToLowerInvariant();
				return lower.EndsWith(".guiskin") ||
					   lower.EndsWith(".fontsettings");
			}).ToList();
		}

		public static List<string> GetAllAudioAssetPaths()
		{
			return AssetDatabase.GetAllAssetPaths().Where(item =>
			{
				var lower = item.ToLowerInvariant();
				return lower.EndsWith(".mixer") ||
					   lower.EndsWith(".mp3") ||
					   lower.EndsWith(".ogg") ||
					   lower.EndsWith(".wav") ||
					   lower.EndsWith(".aiff ") ||
					   lower.EndsWith(".aif") ||
					   lower.EndsWith(".mod") ||
					   lower.EndsWith(".it") ||
					   lower.EndsWith(".s3m") ||
					   lower.EndsWith(".xm");
			}).ToList();
		}

		public static List<string> GetAllPhysicsAssetPaths()
		{
			return AssetDatabase.GetAllAssetPaths().Where(item =>
			{
				var lower = item.ToLowerInvariant();
				return lower.EndsWith(".physicMaterial") ||
					   lower.EndsWith(".physicsMaterial2D");
			}).ToList();
		}

		public static List<string> GetAllScriptAssetPaths()
		{
			return AssetDatabase.GetAllAssetPaths().Where(item =>
			{
				var lower = item.ToLowerInvariant();
				return lower.EndsWith(".cs", StringComparison.OrdinalIgnoreCase);
			}).ToList();
		}

		#endregion

		#region Get Asset Path Of Selection

		public static string GetAssetPathOfActiveGameObject()
		{
			string path = AssetDatabase.GetAssetPath(Selection.activeObject);
			if (string.IsNullOrEmpty(path))
			{
				return "";
			}

			if (Path.GetExtension(path) != "")
			{
				path = path.Replace(Path.GetFileName(path), "");
			}

			return path;
		}

		public static string GetSelectedPathOrAssetRootPath()
		{
			var path = GetSelectedPath();
			if (string.IsNullOrEmpty(path))
			{
				return "Assets";
			}
			return path;
		}

		public static string GetSelectedPath()
		{
			var filteredSelection = Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.Assets);

			foreach (UnityEngine.Object obj in filteredSelection)
			{
				var path = AssetDatabase.GetAssetPath(obj);
				if (string.IsNullOrEmpty(path))
					continue;

				if (Directory.Exists(path))
				{
					return path;
				}
				if (File.Exists(path))
				{
					return Path.GetDirectoryName(path);
				}
			}
			return "";
		}

		#endregion

		#region Get Asset Path Without Root

		public static string GetAssetPathWithoutRoot(Object obj)
		{
			var path = AssetDatabase.GetAssetPath(obj);
			if (path.StartsWith("Assets/"))
				return path.Remove(0, "Assets/".Length);
			return path;
		}

		#endregion

		#region Scene Path

		public static void SplitSceneAndOtherAssetPaths(IEnumerable<string> assetPaths, List<string> sceneAssetPaths, List<string> otherAssetPaths)
		{
			foreach (var assetPath in assetPaths)
			{
				if (assetPath.EndsWith(".unity", StringComparison.OrdinalIgnoreCase))
				{
					sceneAssetPaths.Add(assetPath);
				}
				else
				{
					otherAssetPaths.Add(assetPath);
				}
			}
		}

		#endregion

		#region CreateOrReplaceAsset

		public static T CreateOrReplaceAsset<T>(T asset, string path) where T : Object
		{
			var existingAsset = AssetDatabase.LoadAssetAtPath<T>(path);

			if (!existingAsset)
			{
				AssetDatabase.CreateAsset(asset, path);
				existingAsset = asset;
			}
			else
			{
				EditorUtility.CopySerialized(asset, existingAsset);
			}

			return existingAsset;
		}

		public static void CreateOrReplaceAsset(string sourcePath, string destinationPath)
		{
			var destinationAsset = AssetDatabase.LoadAssetAtPath<Object>(destinationPath);

			if (!destinationAsset)
			{
				AssetDatabase.CopyAsset(sourcePath, destinationPath);
			}
			else
			{
				var sourceAsset = AssetDatabase.LoadAssetAtPath<Object>(sourcePath);
				EditorUtility.CopySerialized(sourceAsset, destinationAsset);
				AssetDatabase.SaveAssets();
			}
		}

		public static void CreateOrReplaceScene(string sourcePath, string destinationPath)
		{
			File.Copy(sourcePath, destinationPath, true);
			AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate | ImportAssetOptions.ForceSynchronousImport);
		}

		#endregion

		[MenuItem("Assets/Copy Asset Path")]
		public static void CopySelectedAssetPaths()
		{
			var stringBuilder = new StringBuilder();

			foreach (Object obj in Selection.objects)
			{
				if (AssetDatabase.Contains(obj))
				{
					stringBuilder.AppendLine(AssetDatabase.GetAssetPath(obj));
				}
				else
				{
					Debug.LogWarning($"{obj} is not a source asset.");
				}
			}

			var paths = stringBuilder.ToString().Trim();
			Clipboard.SetClipboardText(paths, true);
		}

		public static List<string> GetSelectedAssetPaths(bool includeFilesInSubdirectoriesOfSelectedDirectories)
		{
			var selectionObjects = Selection.objects;
			var list = new List<string>(selectionObjects.Length);

			foreach (Object obj in selectionObjects)
			{
				if (AssetDatabase.Contains(obj))
				{
					var path = AssetDatabase.GetAssetPath(obj);
					if (AssetDatabase.IsValidFolder(path))
					{
						if (includeFilesInSubdirectoriesOfSelectedDirectories)
						{
							var assetGuids = AssetDatabase.FindAssets("*", new[] { path });
							for (int i = 0; i < assetGuids.Length; i++)
							{
								var assetPath = AssetDatabase.GUIDToAssetPath(assetGuids[i]);
								list.Add(assetPath);
							}
						}
					}
					else
					{
						list.Add(path);
					}
				}
				else
				{
					Debug.LogWarning($"{obj} is not a source asset.");
				}
			}

			return list;
		}

		public static string GenerateUniqueAssetPathAtSelectedFolder(string fileName)
		{
			try
			{
				// Private implementation of a filenaming function which puts the file at the selected path.
				Type assetDatabase = typeof(AssetDatabase);
				var method = assetDatabase.GetMethod("GetUniquePathNameAtSelectedPath", BindingFlags.NonPublic | BindingFlags.Static);
				return (string)method.Invoke(assetDatabase, new object[] { fileName });
			}
			catch
			{
				// Protection against implementation changes.
				return AssetDatabase.GenerateUniqueAssetPath("Assets/" + fileName);
			}
		}

		public static bool IsFolderAsset(Object obj)
		{
			if (obj == null)
				return false;

			var path = AssetDatabase.GetAssetPath(obj.GetInstanceID());
			if (string.IsNullOrEmpty(path))
				return false;

			return Directory.Exists(path);
		}

		public static T CreateAsset<T>(string assetPath = "", bool pathRelativeToActiveObject = false) where T : ScriptableObject
		{
			// Create scriptable object instance
			T asset = ScriptableObject.CreateInstance<T>();

			// Generate asset file name
			if (string.IsNullOrEmpty(assetPath))
			{
				assetPath = "New" + typeof(T).ToString() + ".asset";
			}

			// Generate full asset path
			string fullPath;
			if (pathRelativeToActiveObject)
			{
				string path = GetAssetPathOfActiveGameObject();
				if (string.IsNullOrEmpty(path))
				{
					path = "Assets";
				}
				fullPath = path + "/" + assetPath;
			}
			else
			{
				fullPath = assetPath;
			}
			fullPath = AssetDatabase.GenerateUniqueAssetPath(fullPath);

			// Create asset
			AssetDatabase.CreateAsset(asset, fullPath);
			AssetDatabase.SaveAssets();

			// Focus on created asset
			EditorUtility.FocusProjectWindow();
			Selection.activeObject = asset;

			return asset;
		}

		public static T SaveAssetToFile<T>(T asset, string assetPath) where T : Object
		{
			if (asset == null)
				throw new NullReferenceException("asset");
			if (string.IsNullOrEmpty(assetPath))
				throw new NullReferenceException("assetPath");

			AssetDatabase.CreateAsset(asset, assetPath);
			AssetDatabase.SaveAssets();
			return AssetDatabase.LoadAssetAtPath<T>(assetPath);
		}
	}

}
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Extenity.ApplicationToolbox;
using Extenity.ConsistencyToolbox;
using Extenity.DataToolbox;
using Extenity.GameObjectToolbox;
using Extenity.GameObjectToolbox.Editor;
using Extenity.SceneManagementToolbox.Editor;
using Extenity.UnityEditorToolbox;
using Extenity.UnityEditorToolbox.Editor;
using Extenity.UnityEditorToolbox.ImageMagick;
using Unity.EditorCoroutines.Editor;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Extenity.BuildMachine.Editor
{

	/// <summary>
	/// </summary>
	/// <remarks>
	/// During scene processes, no changes are allowed that requires a code recompilation.
	/// That complicates things tremendously.
	/// See 112739521.
	/// </remarks>
	public abstract class SceneProcessorBase<TSceneProcessor>
		where TSceneProcessor : SceneProcessorBase<TSceneProcessor>
	{
		#region Configuration

		public abstract SceneDefinition[] Scenes { get; }
		public abstract Dictionary<string, SceneProcessorConfiguration> Configurations { get; }

		#endregion

		#region Process

		public static bool IsProcessorRunning { get; private set; }
		public static int CurrentStep { get; private set; }
		public static string CurrentStepTitle { get; private set; }
		public static Stopwatch ProcessStopwatch { get; private set; }
		public static TimeSpan CurrentStepStartTime { get; private set; }

		protected abstract IEnumerator OnBeforeSceneProcess(SceneDefinition definition, SceneProcessorConfiguration configuration, bool runAsync);
		protected abstract IEnumerator OnAfterSceneProcess(SceneDefinition definition, SceneProcessorConfiguration configuration, bool runAsync);

		public static IEnumerator ProcessScene(Scene scene, string configurationName, bool askUserForUnsavedChanges)
		{
			EnsureNotCompiling("Tried to start scene processing while compiling.");
			if (EditorApplication.isPlayingOrWillChangePlaymode)
			{
				throw new Exception("Tried to start scene processing while in play mode.");
			}
			if (askUserForUnsavedChanges)
			{
				EditorSceneManagerTools.EnforceUserToSaveAllModifiedScenes("First you need to save the scene before processing.");
			}
			var processorInstance = (TSceneProcessor)Activator.CreateInstance(typeof(TSceneProcessor));
			yield return EditorCoroutineUtility.StartCoroutineOwnerless(processorInstance.DoProcessScene(scene, configurationName, true));
		}

		private IEnumerator DoProcessScene(Scene scene, string configurationName, bool runAsync)
		{
			if (IsProcessorRunning)
				throw new Exception("Scene processor was already running.");
			IsProcessorRunning = true;

			var indented = false;

			try
			{
				// Get scene definition
				var scenePath = scene.path;
				var definition = Scenes.FirstOrDefault(item =>
					item.ProcessedScenePath.Equals(scenePath, StringComparison.InvariantCultureIgnoreCase) ||
					item.MainScenePath.Equals(scenePath, StringComparison.InvariantCultureIgnoreCase) ||
					(item.MergedScenePaths != null && item.MergedScenePaths.Any(mergedScenePath => mergedScenePath.Equals(scenePath, StringComparison.InvariantCultureIgnoreCase)))
				);
				if (definition == null)
				{
					Log.Info($"Skipping scene processing for scene '{scene.name}'.");
					yield break;
				}

				// Get process configuration
				if (!Configurations.TryGetValue(configurationName, out var configuration))
				{
					throw new Exception($"Configuration '{configurationName}' does not exist.");
				}
				configuration.CheckConsistencyAndThrow();

				Log.Info($"Processing configuration '{configurationName}' on scene at path: {scenePath}");
				Log.IncreaseIndent();
				indented = true;

				CurrentStep = 0;
				CurrentStepTitle = null;
				ProcessStopwatch = new Stopwatch();
				ProcessStopwatch.Start();
				CurrentStepStartTime = new TimeSpan();

				if (!configuration.DontLoadAndMergeScenes)
				{
					MergeScenesIntoProcessedScene(definition);
				}

				Log.Info("Scene is ready to be processed. Starting the process.");

				// Call initialization process
				yield return EditorCoroutineUtility.StartCoroutineOwnerless(OnBeforeSceneProcess(definition, configuration, runAsync));

				// Call custom processors
				{
					EnsureNotCompiling("Detected script compilation before processing steps.");
					var methods = CollectProcessorMethods(configuration);
					foreach (var method in methods)
					{
						CurrentStepTitle = method.Name;
						EnsureNotCompiling($"Detected script compilation while stepping into '{CurrentStepTitle}'");
						StartStep();
						var enumerator = (IEnumerator)method.Invoke(this, new object[] { definition, configuration, runAsync });
						yield return EditorCoroutineUtility.StartCoroutineOwnerless(enumerator);
						EndStep();
						yield return null; // As a precaution, won't hurt to wait for one frame for all things to settle down.
					}
					EnsureNotCompiling("Detected script compilation after processing steps.");
				}

				// Call finalization process
				DisplayProgressBar("Finalizing Scene Processor", "Finalization");
				yield return EditorCoroutineUtility.StartCoroutineOwnerless(OnAfterSceneProcess(definition, configuration, runAsync));

				EnsureNotCompiling("Detected script compilation before finalization.");
				DisplayProgressBar("Finalizing Scene Processor", "Saving scene");
				// Hack: This is needed to save the scene after lightmap settings change.
				// For some reason, we need to wait one more frame or the scene would get
				// marked as unsaved.
				yield return null;
				AggressivelySaveOpenScenes();

				Log.Info($"{ProcessStopwatch.Elapsed.ToStringHoursMinutesSecondsMilliseconds()} | Scene processor finished.");

				ClearProgressBar();
			}
			//catch (Exception e)
			//{
			//	Log.Exception(e);
			//}
			finally
			{
				IsProcessorRunning = false;
				ProcessStopwatch = null;
				CurrentStepStartTime = new TimeSpan();
				CurrentStep = 0;
				CurrentStepTitle = null;

				if (indented)
				{
					Log.DecreaseIndent();
				}
			}
		}

		#endregion

		#region Merge Scenes

		private void MergeScenesIntoProcessedScene(SceneDefinition definition)
		{
			// Copy main scene to processed scene path
			{
				// Open the main scene, or reopen if already opened. This will head us to a clean start.
				EditorSceneManager.OpenScene(definition.MainScenePath, OpenSceneMode.Single);

				// Copy the main scene to processed scene path and load the processed scene which we will need when merging scenes.
				var activeScene = EditorSceneManager.GetActiveScene();
				var result = EditorSceneManager.SaveScene(activeScene, definition.ProcessedScenePath, false);
				if (!result)
				{
					throw new Exception("Could not copy main scene to processed scene path.");
				}
			}

			// Disable automatic lightmap baking in processed scene. That complicates things.
			// The lighting should be calculated in a scene processing step, after modifications.
			{
				DisableAutomaticLightingForActiveScene();
			}

			// Merge scenes into processed scene
			{
				// Processed scene should already be loaded by now.
				var processingScene = EditorSceneManager.GetSceneByPath(definition.ProcessedScenePath);
				if (!processingScene.IsValid())
				{
					throw new Exception($"Processing scene could not be found at path '{definition.ProcessedScenePath}'.");
				}

				// Merge other scenes into processing scene.
				if (definition.MergedScenePaths != null)
				{
					foreach (var mergedScenePath in definition.MergedScenePaths)
					{
						if (!EditorSceneManagerTools.IsSceneExistsAtPath(mergedScenePath))
						{
							throw new Exception($"Merged scene could not be found at path '{mergedScenePath}'.");
						}

						// Load merging scene additively. It will automatically unload when merging is done, which will leave processed scene as the only loaded scene.
						var mergedScene = EditorSceneManager.OpenScene(mergedScenePath, OpenSceneMode.Additive);
						EditorSceneManager.MergeScenes(mergedScene, processingScene);
					}
				}

				// Save processed scene
				EditorSceneManager.SaveOpenScenes();
			}
		}

		#endregion

		#region Collect Processor Methods

		public static List<MethodInfo> CollectProcessorMethods(SceneProcessorConfiguration configuration)
		{
			configuration.CheckConsistencyAndThrow();
			var includedCategories = configuration.IncludedCategories;
			var excludedCategories = configuration.ExcludedCategories;

			var methods = typeof(TSceneProcessor)
				.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
				.Where(method =>
					{
						if (!method.IsVirtual && method.ReturnType == typeof(IEnumerator))
						{
							var parameters = method.GetParameters();
							if (parameters.Length == 3 &&
								parameters[0].ParameterType == typeof(SceneDefinition) &&
								parameters[1].ParameterType == typeof(SceneProcessorConfiguration) &&
								parameters[2].ParameterType == typeof(bool)
							)
							{
								var attribute = method.GetAttribute<SceneProcessStepAttribute>(true);
								if (attribute != null && attribute.Categories.IsNotNullAndEmpty())
								{
									// See if the attribute contains any one of the included categories
									foreach (var includedCategory in includedCategories)
									{
										if (attribute.Categories.Contains(includedCategory))
										{
											// Make sure the attribute does not contain any one of the excluded categories
											if (excludedCategories.IsNotNullAndEmpty())
											{
												foreach (var excludedCategory in excludedCategories)
												{
													if (attribute.Categories.Contains(excludedCategory))
													{
														return false;
													}
												}
											}
											return true;
										}
									}
								}
							}
						}
						return false;
					}
				)
				.OrderBy(method => method.GetAttribute<SceneProcessStepAttribute>(true).Order)
				.ToList();

			//methods.LogList();

			// Check for duplicate Order values. Note that we already ordered it above.
			if (methods.Count > 1)
			{
				var detected = false;
				var previousMethod = methods[0];
				var previousMethodOrder = previousMethod.GetAttribute<SceneProcessStepAttribute>(true).Order;
				for (int i = 1; i < methods.Count; i++)
				{
					var currentMethod = methods[i];
					var currentMethodOrder = currentMethod.GetAttribute<SceneProcessStepAttribute>(true).Order;

					if (previousMethodOrder == currentMethodOrder)
					{
						detected = true;
						Log.Error($"Methods '{previousMethod.Name}' and '{currentMethod.Name}' have the same order of '{currentMethodOrder}'.");
					}

					previousMethod = currentMethod;
					previousMethodOrder = currentMethodOrder;
				}
				if (detected)
				{
					throw new Exception("Failed to sort processor method list because there were methods with the same order value.");
				}
			}

			return methods;
		}

		#endregion

		#region Start/End Step

		protected static void StartStep()
		{
			var now = ProcessStopwatch.Elapsed;

			CurrentStep++;
			CurrentStepStartTime = now;

			Log.Info($"{now.ToStringHoursMinutesSecondsMilliseconds()} | Scene Processor Step {CurrentStep} - {CurrentStepTitle}");
			DisplayProgressBar("Scene Processor Step " + CurrentStep, CurrentStepTitle);
		}

		private static void EndStep()
		{
			var duration = ProcessStopwatch.Elapsed - CurrentStepStartTime;
			Log.Info($"Step '{CurrentStepTitle}' took {duration.ToStringHoursMinutesSecondsMilliseconds()}.");
			CurrentStepTitle = null;
			CurrentStepStartTime = new TimeSpan();
		}

		#endregion

		#region Lightmap

		protected void DisableAutomaticLightingForActiveScene()
		{
			Lightmapping.giWorkflowMode = Lightmapping.GIWorkflowMode.OnDemand;
		}

		protected void ApplyLightingConfigurationToActiveScene(LightingConfiguration configuration)
		{
			Lightmapping.bakedGI = configuration.BakedGlobalIlluminationEnabled;
			Lightmapping.realtimeGI = configuration.RealtimeGlobalIlluminationEnabled;
			LightmapEditorSettings.textureCompression = configuration.CompressLightmaps;
			LightmapEditorSettings.enableAmbientOcclusion = configuration.AmbientOcclusion;
			LightmapEditorSettingsTools.SetDirectSamples(configuration.DirectSamples);
			LightmapEditorSettingsTools.SetIndirectSamples(configuration.IndirectSamples);
			LightmapEditorSettingsTools.SetBounces(configuration.Bounces);
		}

		#endregion

		#region Progress Bar

		private static string ProgressBarTitle;
		private static string ProgressBarMessage;
		private static double LastProgressBarUpdateTime;
		private const float ProgressBarUpdateInterval = 0.2f;

		protected static void DisplayProgressBar(string title, string message, float progress = 0f)
		{
			ProgressBarTitle = title;
			ProgressBarMessage = message;
			UpdateProgressBar(progress);
		}

		protected static void UpdateProgressBar(float progress)
		{
			var now = PrecisionTiming.PreciseTime;
			if (now > ProgressBarUpdateInterval + LastProgressBarUpdateTime)
			{
				LastProgressBarUpdateTime = now;
				EditorUtility.DisplayProgressBar(ProgressBarTitle, ProgressBarMessage, progress);
			}
		}

		protected static void ClearProgressBar()
		{
			ProgressBarTitle = null;
			ProgressBarMessage = null;
			EditorUtility.ClearProgressBar();
		}

		#endregion

		#region Tools

		/// <summary>
		/// During scene processes, no changes are allowed that requires a code recompilation.
		/// That complicates things tremendously.
		/// See 112739521.
		/// </summary>
		private static void EnsureNotCompiling(string message)
		{
			if (EditorApplication.isCompiling)
			{
				throw new Exception(message);
			}
		}

		protected static void AggressivelySaveOpenScenes()
		{
			EditorSceneManager.MarkAllScenesDirty();
			EditorSceneManager.SaveOpenScenes();
			AssetDatabase.SaveAssets();
		}

		// -------------------------------------------------------------------------

		protected void DeparentAllStaticObjectsContainingComponentInLoadedScenes<T>(bool worldPositionStays, StaticEditorFlags leastExpectedFlags, ActiveCheck activeCheck) where T : Component
		{
			EditorGameObjectTools.SetParentOfAllStaticObjectsContainingComponentInLoadedScenes<T>(null, worldPositionStays, leastExpectedFlags, activeCheck);
		}

		protected void DeparentAllStaticObjectsContainingComponentInActiveScene<T>(bool worldPositionStays, StaticEditorFlags leastExpectedFlags, ActiveCheck activeCheck) where T : Component
		{
			EditorGameObjectTools.SetParentOfAllStaticObjectsContainingComponentInActiveScene<T>(null, worldPositionStays, leastExpectedFlags, activeCheck);
		}

		protected void DeparentAllObjectsContainingComponentInLoadedScenes<T>(bool worldPositionStays, ActiveCheck activeCheck) where T : Component
		{
			GameObjectTools.SetParentOfAllObjectsContainingComponentInLoadedScenes<T>(null, worldPositionStays, activeCheck);
		}

		protected void DeparentAllObjectsContainingComponentInActiveScene<T>(bool worldPositionStays, ActiveCheck activeCheck) where T : Component
		{
			GameObjectTools.SetParentOfAllObjectsContainingComponentInActiveScene<T>(null, worldPositionStays, activeCheck);
		}

		// -------------------------------------------------------------------------

		protected void MakeSureNoStaticObjectsContainingComponentExistInLoadedScenes<T>(StaticEditorFlags leastExpectedFlags, ActiveCheck activeCheck) where T : Component
		{
			EditorGameObjectTools.MakeSureNoStaticObjectsContainingComponentExistInLoadedScenes<T>(leastExpectedFlags, activeCheck);
		}

		protected void MakeSureNoStaticObjectsContainingComponentExistInActiveScene<T>(StaticEditorFlags leastExpectedFlags, ActiveCheck activeCheck) where T : Component
		{
			EditorGameObjectTools.MakeSureNoStaticObjectsContainingComponentExistInActiveScene<T>(leastExpectedFlags, activeCheck);
		}

		// -------------------------------------------------------------------------

		protected static void DestroyAllGameObjectsContainingComponentInLoadedScenes<T>(ActiveCheck activeCheck) where T : Component
		{
			EditorGameObjectTools.DestroyAllGameObjectsContainingComponentInLoadedScenes<T>(activeCheck, true, true);
		}

		protected static void DestroyAllGameObjectsContainingComponentInActiveScene<T>(ActiveCheck activeCheck) where T : Component
		{
			EditorGameObjectTools.DestroyAllGameObjectsContainingComponentInActiveScene<T>(activeCheck, true, true);
		}

		protected static void DestroyAllStaticGameObjectsContainingComponentInLoadedScenes<T>(StaticEditorFlags leastExpectedFlags, ActiveCheck activeCheck) where T : Component
		{
			EditorGameObjectTools.DestroyAllStaticGameObjectsContainingComponentInLoadedScenes<T>(leastExpectedFlags, activeCheck, true, true);
		}

		protected static void DestroyAllStaticGameObjectsContainingComponentInActiveScene<T>(StaticEditorFlags leastExpectedFlags, ActiveCheck activeCheck) where T : Component
		{
			EditorGameObjectTools.DestroyAllStaticGameObjectsContainingComponentInActiveScene<T>(leastExpectedFlags, activeCheck, true, true);
		}

		protected static void DestroyEmptyUnreferencedGameObjectsInLoadedScenes(Type[] excludedTypes = null)
		{
			EditorGameObjectTools.DestroyEmptyUnreferencedGameObjectsInLoadedScenes(excludedTypes, true, true);
		}

		protected static void DestroyEmptyUnreferencedGameObjectsInActiveScene(Type[] excludedTypes = null)
		{
			EditorGameObjectTools.DestroyEmptyUnreferencedGameObjectsInActiveScene(excludedTypes, true, true);
		}

		// -------------------------------------------------------------------------

		protected static void DestroyAllComponentsInLoadedScenes<T>(ActiveCheck activeCheck) where T : Component
		{
			EditorGameObjectTools.DestroyAllComponentsInLoadedScenes<T>(activeCheck, true, true);
		}

		protected static void DestroyAllComponentsInActiveScene<T>(ActiveCheck activeCheck) where T : Component
		{
			EditorGameObjectTools.DestroyAllComponentsInActiveScene<T>(activeCheck, true, true);
		}

		protected static void DestroyAllStaticComponentsInLoadedScenes<T>(StaticEditorFlags leastExpectedFlags, ActiveCheck activeCheck) where T : Component
		{
			EditorGameObjectTools.DestroyAllStaticComponentsInLoadedScenes<T>(leastExpectedFlags, activeCheck, true, true);
		}

		protected static void DestroyAllStaticComponentsInActiveScene<T>(StaticEditorFlags leastExpectedFlags, ActiveCheck activeCheck) where T : Component
		{
			EditorGameObjectTools.DestroyAllStaticComponentsInActiveScene<T>(leastExpectedFlags, activeCheck, true, true);
		}

		protected static void DestroyAllStaticMeshRenderersAndMeshFiltersInLoadedScenes(ActiveCheck activeCheck)
		{
			EditorGameObjectTools.DestroyAllStaticMeshRenderersAndMeshFiltersInLoadedScenes(activeCheck, true, true);
		}

		protected static void DestroyAllStaticMeshRenderersAndMeshFiltersInActiveScene(ActiveCheck activeCheck)
		{
			EditorGameObjectTools.DestroyAllStaticMeshRenderersAndMeshFiltersInActiveScene(activeCheck, true, true);
		}

		// -------------------------------------------------------------------------

		protected void DeleteComponentsOfEditorOnlyToolsInLoadedScenes()
		{
			DestroyAllComponentsInLoadedScenes<SnapToGroundInEditor>(ActiveCheck.IncludingInactive);
			DestroyAllComponentsInLoadedScenes<DontShowEditorHandler>(ActiveCheck.IncludingInactive);
			DestroyAllComponentsInLoadedScenes<Devnote>(ActiveCheck.IncludingInactive);
		}

		// -------------------------------------------------------------------------

		protected static void BlurReflectionProbesInLoadedScenes(ActiveCheck activeCheck)
		{
			ImageMagickCommander.BlurReflectionProbesInLoadedScenes(activeCheck);
		}

		protected static void BlurReflectionProbesInActiveScene(ActiveCheck activeCheck)
		{
			ImageMagickCommander.BlurReflectionProbesInActiveScene(activeCheck);
		}

		#endregion
	}

}
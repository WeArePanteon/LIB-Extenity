using System.Linq;
using Extenity.UnityEditorToolbox.Editor;
using UnityEditor;
using UnityEngine;

namespace Extenity.PainkillaTool.Editor
{

	public class Selecta : ExtenityEditorWindowBase
	{
		#region Configuration

		private static readonly Vector2 MinimumWindowSize = new Vector2(200f, 50f);

		#endregion

		#region Initialization

		[MenuItem("Edit/Selecta", false, 1005)] // Just below Unity's "Snap Settings"
		private static void ShowWindow()
		{
			var window = GetWindow<Selecta>();
			window.Show();
		}

		private void OnEnable()
		{
			SetTitleAndIcon("Selecta", null);
			minSize = MinimumWindowSize;

			//SceneView.onSceneGUIDelegate -= OnSceneGUI;
			//SceneView.onSceneGUIDelegate += OnSceneGUI;
			Selection.selectionChanged -= SelectionChanged;
			Selection.selectionChanged += SelectionChanged;
		}

		#endregion

		#region Deinitialization

		protected void OnDestroy()
		{
			//SceneView.onSceneGUIDelegate -= OnSceneGUI;
			Selection.selectionChanged -= SelectionChanged;
		}

		#endregion

		#region GUI - Window

		private readonly GUILayoutOption[] ActiveButtonOptions = { GUILayout.Width(100f), GUILayout.Height(30f) };
		private readonly GUIContent ActiveButtonContent = new GUIContent("Active", "Toggle whole Selecta tool functionality. Useful for temporarily deactivating the tool.");

		protected override void OnGUIDerived()
		{
			if (SelectionShouldUpdate)
			{
				SelectionShouldUpdate = false;
				Selection.objects = NewSelection;
				//Debug.LogFormat("Applying new selection ({0}): {1}",
				//	NewSelection.Length,
				//	NewSelection.Serialize('|'));
			}

			GUILayout.Space(8f);

			GUILayout.BeginHorizontal();
			IsActive = GUILayout.Toggle(IsActive, ActiveButtonContent, "Button", ActiveButtonOptions);
			GUILayout.EndHorizontal();

			if (GUI.changed)
			{
				Calculate();
				SceneView.RepaintAll();
			}
		}

		#endregion

		#region GUI - Scene

		//private void OnSceneGUI(SceneView sceneview)
		//{
		//	var currentEvent = Event.current;
		//	var currentEventType = currentEvent.type;

		//	if (!IsActive)
		//		return;

		//	// Keep track of mouse events
		//	switch (currentEventType)
		//	{
		//		case EventType.MouseDown:
		//			{
		//				RecordedSelectionOnMouseDown = Selection.objects;
		//				Debug.LogFormat("Selection was ({0}): {1}",
		//					RecordedSelectionOnMouseDown.Length,
		//					RecordedSelectionOnMouseDown.Serialize('|'));

		//				//if (currentEvent.button == 0)
		//				//{
		//				//	var ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
		//				//	RaycastHit hitInfo;
		//				//	if (Physics.Raycast(ray, out hitInfo, float.MaxValue))
		//				//	{
		//				//		Event.current.Use();
		//				//		Debug.Log("Selected: " + hitInfo.transform);
		//				//		//Selection.activeTransform = hitInfo.transform;
		//				//		HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
		//				//	}
		//				//	else
		//				//	{
		//				//		Event.current.Use();
		//				//		Debug.Log("Miss! Selection: " + Selection.activeTransform);
		//				//		//Selection.activeObject = null;
		//				//		HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
		//				//	}
		//				//}
		//			}
		//			break;
		//	}
		//}

		#endregion

		#region Enabled/Disabled

		public bool IsActive = true;

		#endregion

		#region Selection

		//private Object[] RecordedSelectionOnMouseDown;
		private bool SelectionShouldUpdate;
		private Object[] NewSelection;

		private void SelectionChanged()
		{
			Calculate();
		}

		private void Calculate()
		{
			if (!IsActive)
				return;

			var changed = false;
			var currentSelection = Selection.objects;
			//var expectedSelection = currentSelection.Where(
			//	(item) =>
			//	{
			//		var gameObject = item as GameObject;
			//		var shouldDeselect = gameObject == null || gameObject.GetComponent<Collider>() == null;
			//		if (shouldDeselect)
			//			change = true;
			//		return !shouldDeselect;
			//	}
			//).ToList();
			var expectedSelection = currentSelection.Select(
				(item) =>
				{
					var gameObject = item as GameObject;
					if (gameObject == null)
					{
						changed = true;
						return null;
					}
					else
					{
						if (gameObject.GetComponent<Collider>() != null)
						{
							return gameObject;
						}
						else
						{
							changed = true;

							// See if we can find a collider on parent objects
							var parent = gameObject.GetComponentInParent<Collider>();
							if (parent != null)
							{
								return parent.gameObject;
							}
							return null;
						}
					}
				}
			).Where(item => item != null).ToList();

			//Debug.LogFormat("Current selection ({0}): {1}",
			//	currentSelection.Length,
			//	currentSelection.Serialize('|'));

			if (changed)
			{
				//Debug.Log("Should deselect");
				NewSelection = expectedSelection.ToArray();
				SelectionShouldUpdate = true;
				Repaint();

				// Somehow changing the selection won't work inside SelectionChanged event.
				//Selection.activeObject = null;
			}
		}

		#endregion
	}

}
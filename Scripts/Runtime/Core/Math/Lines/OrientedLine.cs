using System;
using UnityEngine;
using System.Collections.Generic;
using Extenity.DataToolbox;
using Extenity.GameObjectToolbox;
using Extenity.UnityEditorToolbox;
using UnityEngine.Events;

namespace Extenity.MathToolbox
{

	public class OrientedLine : MonoBehaviour
	{
		#region Initialization

		public void ClearData()
		{
			ClearPoints();
			Invalidate();
		}

		#endregion

		#region Deinitialization

#if UNITY_EDITOR

		private void OnDisable()
		{
			StopEditing();
		}

#endif

		#endregion

		#region Configuration

		[Header("Configuration")]
		public bool Loop = false;
		public bool KeepDataInLocalCoordinates = true;

		#endregion

		#region Points

		[Header("Data")]
		public List<OrientedPoint> Points;

		public bool IsAnyPointAvailable => Points != null && Points.Count > 0;

		private void ClearPoints()
		{
			Points?.Clear();
		}

		public Vector3 GetPointPosition(int index)
		{
			return Points[index].Position;
		}

		public Vector3 GetPointLocalPosition(int index)
		{
			return KeepDataInLocalCoordinates
				? Points[index].Position
				: transform.InverseTransformPoint(Points[index].Position);
		}

		public Vector3 GetPointWorldPosition(int index)
		{
			return KeepDataInLocalCoordinates
				? transform.TransformPoint(Points[index].Position)
				: Points[index].Position;
		}

		public OrientedPoint GetPoint(int index)
		{
			return Points[index];
		}

		public OrientedPoint GetPointLocal(int index)
		{
			return KeepDataInLocalCoordinates
				? Points[index]
				: transform.InverseTransformOrientedPoint(Points[index]);
		}

		public OrientedPoint GetPointWorld(int index)
		{
			return KeepDataInLocalCoordinates
				? transform.TransformOrientedPoint(Points[index])
				: Points[index];
		}

		public int SortLineStripUsingClosestSequentialPointsMethod(Vector3 initialPointReference)
		{
			return Points.SortLineStripUsingClosestSequentialPointsMethod(initialPointReference);
		}

		#endregion

		#region Segments

		public int SegmentCount => Points.GetLineSegmentCount(Loop);

		#endregion

		#region Length

		public bool IsTotalLengthInvalidated { get; private set; }
		private float _TotalLength = -1f;
		public float TotalLength
		{
			get
			{
				if (IsTotalLengthInvalidated || _TotalLength < 0f)
				{
					_TotalLength = Points?.CalculateLineStripLength(Loop) ?? 0f;
				}
				return _TotalLength;
			}
		}

		public bool IsAverageSegmentLengthInvalidated { get; private set; }
		private float _AverageSegmentLength = -1f;
		public float AverageSegmentLength
		{
			get
			{
				if (IsAverageSegmentLengthInvalidated || _AverageSegmentLength < 0f)
				{
					_AverageSegmentLength = CalculateAverageSegmentLength();
				}
				return _AverageSegmentLength;
			}
		}

		private float CalculateAverageSegmentLength()
		{
			if (Points == null || Points.Count < 2)
				return 0f;

			return TotalLength / SegmentCount;
		}

		#endregion

		#region Calculations

		public Vector3 GetPointPositionAtDistanceFromStart(float distanceFromStart, Space space)
		{
			var position = Points.GetPointPositionAtDistanceFromStart(Loop, distanceFromStart);
			return TransformFromDataSpace(position, space);
		}

		public Vector3 GetPointPositionAtDistanceFromStart(float distanceFromStart, ref Vector3 part, Space space)
		{
			var position = Points.GetPointPositionAtDistanceFromStart(Loop, distanceFromStart, ref part);
			return TransformFromDataSpace(position, space);
		}

		public Vector3 ClosestPointPositionOnLine(Vector3 point, Space space)
		{
			var position = Points.ClosestPointPositionOnLineStrip(point, Loop);
			return TransformFromDataSpace(position, space);
		}

		public Vector3 ClosestPointPositionOnLine(Vector3 point, ref Vector3 part, Space space)
		{
			var position = Points.ClosestPointPositionOnLineStrip(point, Loop, ref part);
			return TransformFromDataSpace(position, space);
		}

		public float DistanceFromStartOfClosestPointOnLine(Vector3 point)
		{
			return Points.DistanceFromStartOfClosestPointOnLineStrip(point, Loop);
		}

		public Vector3 GetPointPositionAheadOfClosestPoint(Vector3 point, float resultingPointDistanceToClosestPoint, Space space)
		{
			var position = Points.GetPointPositionAheadOfClosestPoint(point, resultingPointDistanceToClosestPoint, Loop, TotalLength);
			return TransformFromDataSpace(position, space);
		}

		#endregion

		#region Operations

		public void MirrorX()
		{
			if (Points == null)
				return;
			for (var i = 0; i < Points.Count; i++)
			{
				var point = Points[i];
				point.Position.x = -point.Position.x;
				Points[i] = point;
			}
			Invalidate();
		}

		public void MirrorY()
		{
			if (Points == null)
				return;
			for (var i = 0; i < Points.Count; i++)
			{
				var point = Points[i];
				point.Position.y = -point.Position.y;
				Points[i] = point;
			}
			Invalidate();
		}

		public void MirrorZ()
		{
			if (Points == null)
				return;
			for (var i = 0; i < Points.Count; i++)
			{
				var point = Points[i];
				point.Position.z = -point.Position.z;
				Points[i] = point;
			}
			Invalidate();
		}

		public void NormalizeAllOrientations()
		{
			if (Points == null)
				return;

			for (var i = 0; i < Points.Count; i++)
			{
				Points[i] = Points[i].WithOrientation(Points[i].Orientation.normalized);
			}

			Invalidate();
		}

		#endregion

		#region Invalidate

		public readonly UnityEvent OnInvalidated = new UnityEvent();

		public void Invalidate()
		{
			IsTotalLengthInvalidated = true;
			IsAverageSegmentLengthInvalidated = true;

			OnInvalidated.Invoke();
		}

		#endregion

		#region Space Transformation

		public Vector3 TransformFromDataSpace(Vector3 pointInDataSpace, Space targetSpace)
		{
			switch (targetSpace)
			{
				case Space.Unspecified: return pointInDataSpace;
				case Space.World:
					return KeepDataInLocalCoordinates
						? transform.TransformPoint(pointInDataSpace)
						: pointInDataSpace;
				case Space.Local:
					return KeepDataInLocalCoordinates
						? pointInDataSpace
						: transform.InverseTransformPoint(pointInDataSpace);
				default:
					throw new ArgumentOutOfRangeException(nameof(targetSpace), targetSpace, null);
			}
		}

		public OrientedPoint TransformFromDataSpace(OrientedPoint pointInDataSpace, Space targetSpace)
		{
			switch (targetSpace)
			{
				case Space.Unspecified: return pointInDataSpace;
				case Space.World:
					return KeepDataInLocalCoordinates
						? transform.TransformOrientedPoint(pointInDataSpace)
						: pointInDataSpace;
				case Space.Local:
					return KeepDataInLocalCoordinates
						? pointInDataSpace
						: transform.InverseTransformOrientedPoint(pointInDataSpace);
				default:
					throw new ArgumentOutOfRangeException(nameof(targetSpace), targetSpace, null);
			}
		}

		#endregion

		#region Debug and Gizmos

#if UNITY_EDITOR

		[Serializable]
		public class DebugConfigurationData
		{
			public bool DrawUnselected = true;
			public Color UnselectedColor = new Color(0.4f, 0.4f, 0.42f);
			public Color PointColor = new Color(0.0f, 0.0f, 1.0f);
			public Color LineColor = new Color(0.0f, 0.0f, 1.0f);
			public float PointSize = 0.04f;
			public float FirstPointSizeFactor = 1.1f;
		}

		[Header("Debug")]
		public DebugConfigurationData DEBUG;

		private void OnDrawGizmos()
		{
			if (DEBUG.DrawUnselected)
			{
				GizmosTools.DrawPathLines(GetPointWorldPosition, Points?.Count ?? 0, Loop, DEBUG.UnselectedColor);
			}
		}

		private void OnDrawGizmosSelected()
		{
			GizmosTools.DrawPath(GetPointWorldPosition, Points?.Count ?? 0, Loop, true, DEBUG.PointColor, true, DEBUG.LineColor, AverageSegmentLength * DEBUG.PointSize, DEBUG.FirstPointSizeFactor);

			if (Points?.Count > 0)
			{
				Gizmos.color = DEBUG.LineColor;
				for (var i = 0; i < Points.Count; i++)
				{
					var point = GetPointWorld(i);
					Gizmos.DrawLine(point.Position, point.Position + point.Orientation * AverageSegmentLength * DEBUG.PointSize);
				}
			}
		}

#endif

		#endregion

		#region Edit Mode

#if UNITY_EDITOR

		public bool IsEditing { get; private set; }

		public void StartEditing()
		{
			Log.Info($"Starting to edit '{gameObject.FullName()}'");

			IsEditing = true;
			gameObject.GetSingleOrAddComponent<DontShowEditorHandler>();

			UnityEditor.Selection.selectionChanged -= CheckIfNeedToStopEditing;
			UnityEditor.Selection.selectionChanged += CheckIfNeedToStopEditing;
		}

		public void StopEditing()
		{
			UnityEditor.Selection.selectionChanged -= CheckIfNeedToStopEditing;

			if (IsEditing)
			{
				Log.Info($"Finished editing '{gameObject.FullName()}'");
				IsEditing = false;
			}

			var dontShowEditorHandle = gameObject.GetComponent<DontShowEditorHandler>();
			if (dontShowEditorHandle)
			{
				UnityEditor.EditorApplication.delayCall += () =>
				 {
					 if (dontShowEditorHandle)
					 {
						 DestroyImmediate(dontShowEditorHandle);
					 }
				 };
			}
		}

		private void CheckIfNeedToStopEditing()
		{
			if (!this)
			{
				UnityEditor.Selection.selectionChanged -= CheckIfNeedToStopEditing;
				return;
			}

			if (UnityEditor.Selection.activeGameObject != gameObject)
			{
				StopEditing();
			}
		}

#endif

		#endregion

		#region Editor

#if UNITY_EDITOR

		protected void OnValidate()
		{
			// TODO: Not cool to always invalidate everything. But it's a quick and robust solution for now.
			Invalidate();
		}

#endif

		#endregion
	}

}
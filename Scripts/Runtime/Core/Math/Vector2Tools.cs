using System;
using System.Collections.Generic;
using UnityEngine;

namespace Extenity.MathToolbox
{

	public static class Vector2Tools
	{
		public static readonly Vector2 Zero = Vector2.zero;
		public static readonly Vector2 One = Vector2.one;
		public static readonly Vector2 Up = Vector2.up;
		public static readonly Vector2 Down = Vector2.down;
		public static readonly Vector2 Left = Vector2.left;
		public static readonly Vector2 Right = Vector2.right;
		public static readonly Vector2 PositiveInfinity = Vector2.positiveInfinity;
		public static readonly Vector2 NegativeInfinity = Vector2.negativeInfinity;
		public static readonly Vector2 NaN = new Vector2(float.NaN, float.NaN);

		#region Basic Checks

		public static bool IsZero(this Vector2 value)
		{
			return value.IsAllZero();
		}

		public static bool IsUnit(this Vector2 value)
		{
			return value.magnitude.IsAlmostEqual(1f);
		}

		public static bool IsAllEqual(this Vector2 value, float val)
		{
			return value.x == val && value.y == val;
		}

		public static bool IsAllBetween(this Vector2 value, float minVal, float maxVal)
		{
			return
				value.x <= maxVal && value.x >= minVal &&
				value.y <= maxVal && value.y >= minVal;
		}

		public static bool IsAlmostEqualVector2(this Vector2 value1, Vector2 value2, float precision = MathTools.ZeroTolerance)
		{
			value1 = value1 - value2;
			return
				value1.x <= precision && value1.x >= -precision &&
				value1.y <= precision && value1.y >= -precision;
		}

		public static bool IsAllZero(this Vector2 value)
		{
			return value.x.IsZero() && value.y.IsZero();
		}

		public static bool IsAllInfinity(this Vector2 value)
		{
			return float.IsInfinity(value.x) && float.IsInfinity(value.y);
		}

		public static bool IsAllNaN(this Vector2 value)
		{
			return float.IsNaN(value.x) && float.IsNaN(value.y);
		}

		public static bool IsAnyEqual(this Vector2 value, float val)
		{
			return value.x == val || value.y == val;
		}

		public static bool IsAnyAlmostEqual(this Vector2 value, float val)
		{
			return value.x.IsAlmostEqual(val) || value.y.IsAlmostEqual(val);
		}

		public static bool IsAnyZero(this Vector2 value)
		{
			return value.x.IsZero() || value.y.IsZero();
		}

		public static bool IsAnyInfinity(this Vector2 value)
		{
			return float.IsInfinity(value.x) || float.IsInfinity(value.y);
		}

		public static bool IsAnyNaN(this Vector2 value)
		{
			return float.IsNaN(value.x) || float.IsNaN(value.y);
		}

		#endregion

		#region Vector2 - Vector3 Conversions

		public static Vector3 ToVector3XY(this Vector2 vector) { return new Vector3(vector.x, vector.y, 0f); }
		public static Vector3 ToVector3XY(this Vector2 vector, float z) { return new Vector3(vector.x, vector.y, z); }
		public static Vector3 ToVector3XZ(this Vector2 vector) { return new Vector3(vector.x, 0f, vector.y); }
		public static Vector3 ToVector3XZ(this Vector2 vector, float y) { return new Vector3(vector.x, y, vector.y); }
		public static Vector3 ToVector3YZ(this Vector2 vector) { return new Vector3(0f, vector.x, vector.y); }
		public static Vector3 ToVector3YZ(this Vector2 vector, float x) { return new Vector3(x, vector.x, vector.y); }

		public static Vector2Int ToVector2IntRounded(this Vector2 vector) { return new Vector2Int(Mathf.RoundToInt(vector.x), Mathf.RoundToInt(vector.y)); }
		public static Vector2Int ToVector2IntFloored(this Vector2 vector) { return new Vector2Int(Mathf.FloorToInt(vector.x), Mathf.FloorToInt(vector.y)); }
		public static Vector2Int ToVector2IntCeiled(this Vector2 vector) { return new Vector2Int(Mathf.CeilToInt(vector.x), Mathf.CeilToInt(vector.y)); }

		#endregion

		#region Vector2 - Vector4 Conversions

		public static Vector4 ToVector4XY(this Vector2 vector) { return new Vector4(vector.x, vector.y, 0f, 0f); }
		public static Vector4 ToVector4YZ(this Vector2 vector) { return new Vector4(0f, vector.x, vector.y, 0f); }
		public static Vector4 ToVector4XZ(this Vector2 vector) { return new Vector4(vector.x, 0f, vector.y, 0f); }
		public static Vector4 ToVector4ZW(this Vector2 vector) { return new Vector4(0f, 0f, vector.x, vector.y); }
		public static Vector2 ToVector2XY(this Vector4 vector) { return new Vector2(vector.x, vector.y); }
		public static Vector2 ToVector2YZ(this Vector4 vector) { return new Vector2(vector.y, vector.z); }
		public static Vector2 ToVector2XZ(this Vector4 vector) { return new Vector2(vector.x, vector.z); }
		public static Vector2 ToVector2ZW(this Vector4 vector) { return new Vector2(vector.z, vector.w); }

		#endregion

		#region Mul / Div

		public static Vector2 Mul(this Vector2 va, Vector2 vb)
		{
			return new Vector2(va.x * vb.x, va.y * vb.y);
		}

		public static Vector2 Mul(this Vector2 va, Vector2Int vb)
		{
			return new Vector2(va.x * vb.x, va.y * vb.y);
		}

		public static Vector2 Div(this Vector2 va, Vector2 vb)
		{
			return new Vector2(va.x / vb.x, va.y / vb.y);
		}

		public static Vector2 Div(this Vector2 va, Vector2Int vb)
		{
			return new Vector2(va.x / vb.x, va.y / vb.y);
		}

		#endregion

		#region Four Basic Math Operations on Vector Arrays

		public static void Plus(this Vector2[] array, Vector2 value)
		{
			for (int i = 0; i < array.Length; i++) array[i] += value;
		}

		public static void Plus(this Vector2[] array, Vector2Int value)
		{
			for (int i = 0; i < array.Length; i++)
			{
				array[i].x += value.x;
				array[i].y += value.y;
			}
		}

		public static void Minus(this Vector2[] array, Vector2 value)
		{
			for (int i = 0; i < array.Length; i++) array[i] -= value;
		}

		public static void Minus(this Vector2[] array, Vector2Int value)
		{
			for (int i = 0; i < array.Length; i++)
			{
				array[i].x -= value.x;
				array[i].y -= value.y;
			}
		}

		public static void Mul(this Vector2[] array, Vector2 value)
		{
			for (int i = 0; i < array.Length; i++) array[i] = Mul(array[i], value);
		}

		public static void Mul(this Vector2[] array, Vector2Int value)
		{
			for (int i = 0; i < array.Length; i++) array[i] = Mul(array[i], value);
		}

		public static void Div(this Vector2[] array, Vector2 value)
		{
			for (int i = 0; i < array.Length; i++) array[i] = Div(array[i], value);
		}

		public static void Div(this Vector2[] array, Vector2Int value)
		{
			for (int i = 0; i < array.Length; i++) array[i] = Div(array[i], value);
		}

		#endregion

		#region Mid

		public static Vector2 Mid(this Vector2 va, Vector2 vb)
		{
			vb.x = (va.x + vb.x) * 0.5f;
			vb.y = (va.y + vb.y) * 0.5f;
			return vb;
		}

		#endregion

		#region Clamp Components

		public static Vector2 ClampComponents(this Vector2 value, float min, float max)
		{
			return new Vector2(
				Mathf.Clamp(value.x, min, max),
				Mathf.Clamp(value.y, min, max));
		}

		#endregion

		#region Raise To Minimum

		public static Vector2 RaiseToMinimum(this Vector2 value, float min)
		{
			if (value.x > 0f && value.x < min) value.x = min;
			else if (value.x < 0f && value.x > -min) value.x = -min;
			if (value.y > 0f && value.y < min) value.y = min;
			else if (value.y < 0f && value.y > -min) value.y = -min;
			return value;
		}

		#endregion

		#region Clamp Length / SqrLength

		public static Vector2 ClampLength01(this Vector2 value)
		{
			if (value.x * value.x + value.y * value.y > 1f)
				return value.normalized;
			return value;
		}

		public static Vector2 ClampLengthMax(this Vector2 value, float max)
		{
			if (value.magnitude > max)
				return value.normalized * max;
			return value;
		}

		public static Vector2 ClampLengthMin(this Vector2 value, float min)
		{
			if (value.magnitude < min)
				return value.normalized * min;
			return value;
		}

		public static Vector2 ClampSqrLengthMax(this Vector2 value, float sqrMax)
		{
			if (value.sqrMagnitude > sqrMax)
				return value.normalized * sqrMax;
			return value;
		}

		public static Vector2 ClampSqrLengthMin(this Vector2 value, float sqrMin)
		{
			if (value.sqrMagnitude < sqrMin)
				return value.normalized * sqrMin;
			return value;
		}

		#endregion

		#region Abs / Sign

		public static Vector2 Abs(this Vector2 value)
		{
			return new Vector2(
				value.x < 0f ? -value.x : value.x,
				value.y < 0f ? -value.y : value.y);
		}

		public static Vector2 Sign(this Vector2 value)
		{
			return new Vector2(
				value.x > 0f ? 1f : (value.x < 0f ? -1f : 0f),
				value.y > 0f ? 1f : (value.y < 0f ? -1f : 0f));
		}

		public static Vector2Int SignInt(this Vector2 value)
		{
			return new Vector2Int(
				value.x > 0 ? 1 : (value.x < 0 ? -1 : 0),
				value.y > 0 ? 1 : (value.y < 0 ? -1 : 0));
		}

		#endregion

		#region Min / Max Component

		public static float MinComponent(this Vector2 value)
		{
			return value.x < value.y ? value.x : value.y;
		}

		public static float MaxComponent(this Vector2 value)
		{
			return value.x > value.y ? value.x : value.y;
		}

		public static float MultiplyComponents(this Vector2 value)
		{
			return value.x * value.y;
		}

		#endregion

		#region Distance and Difference

		public static float SqrDistanceTo(this Vector2 a, Vector2 b)
		{
			var dx = b.x - a.x;
			var dy = b.y - a.y;
			return dx * dx + dy * dy;
		}

		public static float DistanceTo(this Vector2 a, Vector2 b)
		{
			var dx = b.x - a.x;
			var dy = b.y - a.y;
			return Mathf.Sqrt(dx * dx + dy * dy);
		}

		#endregion

		#region Inside Bounds

		public static bool IsInsideBounds(this Vector2 a, Vector2 b, float maxDistance)
		{
			var dx = b.x - a.x;
			var dy = b.y - a.y;
			return
				dx > -maxDistance && dx < maxDistance &&
				dy > -maxDistance && dy < maxDistance;
		}

		#endregion

		#region Rotation

		public static Vector2 Rotate(this Vector2 vector, float angleInRadians)
		{
			float cosa = Mathf.Cos(angleInRadians);
			float sina = Mathf.Sin(angleInRadians);
			return new Vector2(cosa * vector.x - sina * vector.y, sina * vector.x + cosa * vector.y);
		}

		#endregion

		#region Angles

		public static float AngleBetweenXAxis_NegPIToPI(this Vector2 vector)
		{
			return Mathf.Atan2(vector.y, vector.x);
		}

		public static float AngleBetweenXAxis_ZeroToTwoPI(this Vector2 vector)
		{
			float angle = Mathf.Atan2(vector.y, vector.x);
			if (angle < 0f)
				return angle + MathTools.TwoPI;
			return angle;
		}

		public static float AngleBetween(this Vector2 vector1, Vector2 vector2)
		{
			return Mathf.Acos(Vector2.Dot(vector1.normalized, vector2.normalized));
		}

		public static float AngleBetween_NegPIToPI(this Vector2 vector1, Vector2 vector2)
		{
			float angle = Mathf.Atan2(vector2.y, vector2.x) - Mathf.Atan2(vector1.y, vector1.x);

			if (angle < 0)
				angle += MathTools.TwoPI;

			if (angle > MathTools.PI)
				angle -= MathTools.TwoPI;

			return angle;
		}

		#endregion

		#region Perpendicular / Reflection

		public static Vector2 Perpendicular(this Vector2 vector)
		{
			return new Vector2(-vector.y, vector.x);
		}

		public static Vector2 Reflect(this Vector2 vector, Vector2 normal)
		{
			return vector - (2 * Vector2.Dot(vector, normal) * normal);
		}

		#endregion

		#region Swap

		public static Vector2 Swap(this Vector2 vector)
		{
			return new Vector2(vector.y, vector.x);
		}

		public static void SwapToMakeLesserAndGreater(ref Vector2 shouldBeLesser, ref Vector2 shouldBeGreater)
		{
			float temp;

			if (shouldBeLesser.x > shouldBeGreater.x)
			{
				temp = shouldBeLesser.x;
				shouldBeLesser.x = shouldBeGreater.x;
				shouldBeGreater.x = temp;
			}

			if (shouldBeLesser.y > shouldBeGreater.y)
			{
				temp = shouldBeLesser.y;
				shouldBeLesser.y = shouldBeGreater.y;
				shouldBeGreater.y = temp;
			}
		}

		#endregion

		#region Manipulate Components

		public static void ChangeZerosTo(ref Vector2 value, float changeTo)
		{
			if (value.x == 0f) value.x = changeTo;
			if (value.y == 0f) value.y = changeTo;
		}

		public static Vector2 MakeZeroIfNaN(this Vector2 val)
		{
			if (float.IsNaN(val.x)) val.x = 0f;
			if (float.IsNaN(val.y)) val.y = 0f;
			return val;
		}

		public static Vector2 ScaleX(this Vector2 vector, float scale)
		{
			vector.x *= scale;
			return vector;
		}
		public static Vector2 ScaleY(this Vector2 vector, float scale)
		{
			vector.y *= scale;
			return vector;
		}

		#endregion

		#region Flat Check

		public static bool IsFlatX(this List<Vector2> points)
		{
			if (points == null || points.Count == 0)
				throw new Exception("List contains no points.");

			var value = points[0].x;
			for (int i = 1; i < points.Count; i++)
			{
				if (!value.IsAlmostEqual(points[i].x))
					return false;
			}

			return true;
		}

		public static bool IsFlatY(this List<Vector2> points)
		{
			if (points == null || points.Count == 0)
				throw new Exception("List contains no points.");

			var value = points[0].y;
			for (int i = 1; i < points.Count; i++)
			{
				if (!value.IsAlmostEqual(points[i].y))
					return false;
			}

			return true;
		}

		#endregion
	}

}
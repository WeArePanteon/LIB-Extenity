using System;
using UnityEngine;

namespace Extenity.MathToolbox
{

	[Serializable]
	public class SmoothQuaternion
	{
		#region Configuration

		public float SmoothingFactor = 0.02f;

		#endregion

		#region Current / Target

		[NonSerialized]
		public Quaternion Current;
		[NonSerialized]
		public Quaternion Target;

		#endregion

		#region Calculate

		public void Calculate()
		{
			Current = Quaternion.Slerp(Current, Target, SmoothingFactor);
		}

		public void Calculate(Quaternion target)
		{
			Target = target;
			Calculate();
		}

		#endregion

		#region Helpers

		public static Quaternion operator *(SmoothQuaternion leftValue, SmoothQuaternion rightValue)
		{
			return leftValue.Current * rightValue.Current;
		}

		public static implicit operator Quaternion(SmoothQuaternion me)
		{
			return me.Current;
		}

		public Vector3 Forward
		{
			get { return Current * Vector3.forward; }
		}

		public Vector3 TargetForward
		{
			get { return Target * Vector3.forward; }
		}

		public Vector3 Up
		{
			get { return Current * Vector3.up; }
		}

		public Vector3 TargetUp
		{
			get { return Target * Vector3.up; }
		}

		public Vector3 Right
		{
			get { return Current * Vector3.right; }
		}

		public Vector3 TargetRight
		{
			get { return Target * Vector3.right; }
		}

		#endregion
	}

}
﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace Extenity.UnityEditorToolbox.GraphPlotting
{

	public enum SampleTime
	{
		FixedUpdate = 0,
		Update = 1,
		// TODO:
		//LateUpdate = 2,

		/// <summary>
		/// Need to manually call Sample.
		/// </summary>
		Custom = 7,
	}

	public enum ValueAxisSizing
	{
		Fixed,
		Expansive,
		Adaptive
	};

	[Serializable]
	public class ValueAxisRangeConfiguration
	{
		public ValueAxisSizing Sizing;
		public float Min;
		public float Max;

		public float Span => Max - Min;

		public ValueAxisRangeConfiguration(ValueAxisSizing sizing, float min, float max)
		{
			Sizing = sizing;
			Min = min;
			Max = max;
		}

		public void CopyFrom(ValueAxisRangeConfiguration other)
		{
			Sizing = other.Sizing;
			Min = other.Min;
			Max = other.Max;
		}

		public void Expand(float value)
		{
			if (Min > value)
				Min = value;
			if (Max < value)
				Max = value;
		}
	}

	public class Monitor
	{
		public string Name;
		public GameObject GameObject = null;

		public readonly ValueAxisRangeConfiguration Range = new ValueAxisRangeConfiguration(ValueAxisSizing.Adaptive, float.PositiveInfinity, float.NegativeInfinity);

		#region Initialization

		public Monitor(string name, GameObject gameObject = null)
		{
			Name = name;
			GameObject = gameObject;

			GraphPlotters.Register(this);
		}

		#endregion

		#region Deinitialization

		public void Close()
		{
			GraphPlotters.Deregister(this);
		}

		#endregion

		#region Channels

		public readonly List<Channel> Channels = new List<Channel>();

		internal void RegisterChannel(Channel channel)
		{
			Channels.Add(channel);
		}

		internal void DeregisterChannel(Channel channel)
		{
			Channels.Remove(channel);
		}

		#endregion

		#region Tags

		public readonly List<TagEntry> Tags = new List<TagEntry>();

		private readonly TagEntry _LookupEvent = new TagEntry();
		private readonly TagEntryTimeComparer _EventComparer = new TagEntryTimeComparer();

		public void Add(TagEntry entry)
		{
			var index = Tags.BinarySearch(entry, _EventComparer);
			if (index < 0)
			{
				index = ~index;
			}

			Tags.Insert(index, entry);
		}

		public void GetTagEntries(float minTime, float maxTime, List<TagEntry> result)
		{
			if (Tags.Count == 0)
				return;

			// Find starting index
			_LookupEvent.Time = minTime;
			var index = Tags.BinarySearch(_LookupEvent, _EventComparer);
			if (index < 0)
			{
				index = ~index;
			}

			// Add all entries until maxTime
			for (var i = index; i < Tags.Count; i++)
			{
				var entry = Tags[i];

				if (entry.Time <= maxTime)
				{
					result.Add(entry);
				}
				else
				{
					break;
				}
			}
		}

		#endregion

		#region Timing

		public float LatestTime = 0f;

		public void SetTimeCursor(float time)
		{
			LatestTime = time;
		}

		#endregion

		internal void InformNewEntry(float value, float time)
		{
			SetTimeCursor(time);

			switch (Range.Sizing)
			{
				case ValueAxisSizing.Expansive:
					Range.Expand(value);
					break;
				case ValueAxisSizing.Fixed:
				case ValueAxisSizing.Adaptive:
					// Nothing to do.
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		public void GetMinMaxTime(out float minTime, out float maxTime)
		{
			minTime = float.PositiveInfinity;
			maxTime = float.NegativeInfinity;

			for (var i = 0; i < Tags.Count; i++)
			{
				var entry = Tags[i];
				if (minTime > entry.Time)
					minTime = entry.Time;
				if (maxTime < entry.Time)
					maxTime = entry.Time;
			}

			for (var i = 0; i < Channels.Count; i++)
			{
				float entryMin;
				float entryMax;
				Channels[i].GetMinMaxTime(out entryMin, out entryMax);

				if (minTime > entryMin)
					minTime = entryMin;
				if (maxTime < entryMax)
					maxTime = entryMax;
			}
		}

		public void CalculateValueAxisRangeInTimeWindow(float timeStart, float timeEnd)
		{
			var min = float.PositiveInfinity;
			var max = float.NegativeInfinity;

			for (var i = 0; i < Channels.Count; i++)
			{
				float channelMin, channelMax;
				Channels[i].GetValueRangeInTimeWindow(timeStart, timeEnd, out channelMin, out channelMax);
				if (min > channelMin)
					min = channelMin;
				if (max < channelMax)
					max = channelMax;
			}
		}

		public void SetRangeConfiguration(ValueAxisRangeConfiguration range)
		{
			Range.CopyFrom(range);
		}
	}

}
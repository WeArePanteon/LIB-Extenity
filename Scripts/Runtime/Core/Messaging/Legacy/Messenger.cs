﻿#if UseLegacyMessenger

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Extenity.DataToolbox;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Extenity.MessagingToolbox
{

	public static class Messenger
	{
		#region Events and Registration

		private static readonly Dictionary<string, ExtenityEvent> EventsByEventNames = new Dictionary<string, ExtenityEvent>();

		public static void RegisterEvent(string eventName, Action callback)
		{
			if (string.IsNullOrEmpty(eventName) ||
			    callback == null ||
			    callback.IsUnityObjectTargetedAndDestroyed()
			)
			{
				return; // Ignore
			}

			if (IsRegistrationLoggingEnabled)
				LogRegistration($"Registering callback '<b>{callback.Method.Name}</b> in {callback.Target}' for event '<b>{eventName}</b>'", callback.Target as Object);

			if (!EventsByEventNames.TryGetValue(eventName, out var events))
			{
				events = new ExtenityEvent();
				EventsByEventNames[eventName] = events;
			}
			events.AddListener(callback);
		}

		public static void DeregisterEvent(string eventName, Action callback)
		{
			if (string.IsNullOrEmpty(eventName) ||
			    callback == null)
				// (callback.Target as UnityEngine.Object) == null) No! We should remove the callback even though the object is no longer available.
			{
				return; // Ignore
			}

			if (IsRegistrationLoggingEnabled)
				LogRegistration($"Deregistering callback '<b>{callback.Method.Name}</b> in {callback.Target}' from event '<b>{eventName}</b>'", callback.Target as Object);

			if (EventsByEventNames.TryGetValue(eventName, out var events))
			{
				events.RemoveListener(callback);
			}
		}

		public static void DeregisterAllEvents(object callbackTarget)
		{
			if (callbackTarget == null)
				// (callback.Target as UnityEngine.Object) == null) No! We should remove the callback even though the object is no longer available.
			{
				return; // Ignore
			}

			if (IsRegistrationLoggingEnabled)
				LogRegistration($"Deregistering all callbacks of '{callbackTarget}'", callbackTarget as Object);

			foreach (var item in EventsByEventNames)
			{
				item.Value.RemoveAllListenersThatTargets(callbackTarget);
			}
		}

		#endregion

		#region Event Names

		private static string[] _EventNames;
		public static string[] EventNames
		{
			get
			{
				if (_EventNames == null || _EventNames.Length == 0)
				{
					Debug.LogException(new Exception("No event names were registered yet. It might probably be some system initialization order issue."));
				}
				return _EventNames;
			}
		}

		public static void AddConstStringFieldsAsEventNames(Type type)
		{
			var fields = type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);

			var result = _EventNames == null
				? new List<string>(fields.Length)
				: _EventNames.ToList();

			for (var i = 0; i < fields.Length; i++)
			{
				var field = fields[i];
				if (field.IsLiteral && !field.IsInitOnly)
				{
					var value = field.GetValue(null) as string;
					if (!string.IsNullOrWhiteSpace(value))
					{
						if (!result.Contains(value))
						{
							result.Add(value);
						}
					}
				}
			}

			_EventNames = result.ToArray();
		}

		#endregion

		#region Emit

		public static void EmitEvent(string eventName)
		{
			if (IsEmitLoggingEnabled && !EmitLogFilter.Contains(eventName))
				LogEmit($"Emitting '<b>{eventName}</b>'");

			if (EventsByEventNames.TryGetValue(eventName, out var events))
			{
				events.InvokeSafe();
			}
		}

		#endregion

		#region Log

		public static bool IsEmitLoggingEnabled = false;
		public static bool IsRegistrationLoggingEnabled = false;
		public static HashSet<string> EmitLogFilter = new HashSet<string>();

		private const string LogPrefix = "<b>[Messenger]</b> ";

		private static void LogRegistration(string message, Object context)
		{
			Debug.Log(LogPrefix + message, context);
		}

		private static void LogEmit(string message)
		{
			Debug.Log(LogPrefix + message);
		}

		#endregion
	}

}

#endif
//#define EnablePlayerPrefLogging

using System;
using System.Diagnostics;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;
using Debug = UnityEngine.Debug;

namespace Extenity.DataToolbox
{

	public abstract class PlayerPref<T>
	{
		#region Initialization

		public PlayerPref([NotNull]string prefsKey, PathHashPostfix appendPathHashToKey, T defaultValue, Func<PlayerPref<T>, T> defaultValueOverride, float saveDelay)
		{
			PrefsKey = prefsKey;
			_AppendPathHashToKey = appendPathHashToKey;
			_Value = defaultValue;
			_DefaultValueOverride = defaultValueOverride;
			SaveDelay = saveDelay;
		}

		#endregion

		#region Key

		public readonly string PrefsKey;
		private readonly PathHashPostfix _AppendPathHashToKey;

		private string _ProcessedPrefsKey;
		public string ProcessedPrefsKey
		{
			get
			{
				if (string.IsNullOrEmpty(_ProcessedPrefsKey))
				{
					_ProcessedPrefsKey = PlayerPrefsTools.GenerateKey(PrefsKey, _AppendPathHashToKey);
				}
				return _ProcessedPrefsKey;
			}
		}

		#endregion

		#region Default Value

		private readonly Func<PlayerPref<T>, T> _DefaultValueOverride;

		#endregion

		#region Value

		private bool _IsInitialized;
		protected T _Value;
		public T Value
		{
			get
			{
				if (!_IsInitialized)
				{
					_IsInitialized = true;
					if (!PlayerPrefs.HasKey(ProcessedPrefsKey))
					{
						if (_DefaultValueOverride != null)
						{
							_Value = _DefaultValueOverride(this);
							Log($"Initialized value from override as '{_Value}'");
						}
						else
						{
							// Default value was already assigned to _Value at construction time. Nothing to do here.
							Log($"Initialized value as default '{_Value}'");
						}
					}
					else
					{
						_Value = InternalGetValue();
						Log($"Initialized value as '{_Value}'");
					}
				}
				Log($"Got the value '{_Value}'");
				return _Value;
			}
			set
			{
				if (_IsInitialized)
				{
					var oldValue = Value; // This must be called before setting _IsInitialized to true;
					Log($"Setting value to '{value}' which previously was '{oldValue}'");
					if (IsSame(oldValue, value))
						return;
				}
				else
				{
					Log($"Setting value to '{value}' <b>as initialization</b>");
					_IsInitialized = true;
				}

				_Value = value;
				InternalSetValue(value);

				if (SaveDelay > 0f)
				{
					Log($"Saving deferred for '{SaveDelay}' seconds");
					DeferredSave(SaveDelay);
				}
				else
				{
					Save();
				}

				if (_DontEmitNextValueChangedEvent)
				{
					Log("Skipping value change event");
					_DontEmitNextValueChangedEvent = false;
				}
				else
					OnValueChanged.Invoke(value);
			}
		}

		#endregion

		#region Value Changed Event

		public class ValueChangedEvent : UnityEvent<T> { }
		public readonly ValueChangedEvent OnValueChanged = new ValueChangedEvent();

		private bool _DontEmitNextValueChangedEvent;

		public void AddOnValueChangedListenerAndInvoke(UnityAction<T> listener)
		{
			if (listener == null)
				throw new ArgumentNullException();

			OnValueChanged.AddListener(listener);
			listener.Invoke(Value);
		}

		public void InvokeValueChanged()
		{
			Log("Invoking value change event");
			OnValueChanged.Invoke(Value);
		}

		public void SuppressNextValueChangedEvent()
		{
			Log("Suppressing next value change event");
			_DontEmitNextValueChangedEvent = true;
		}

		#endregion

		#region Saving, Loading and Comparing Values

		protected abstract T InternalGetValue();
		protected abstract void InternalSetValue(T value);
		protected abstract bool IsSame(T oldValue, T newValue);

		#endregion

		#region Deferred Saving

		public float SaveDelay = 0f;

		public void Save()
		{
			PlayerPrefs.Save();
		}

		public void DeferredSave(float saveDelay)
		{
			PlayerPrefsTools.DeferredSave(saveDelay);
		}

		#endregion

		#region Log - Collectible

		[Conditional("EnablePlayerPrefLogging")]
		private void Log(string message)
		{
			Debug.Log($"<b><i>Pref-{PrefsKey} | </i></b>" + message);
		}

		//[Conditional("EnablePlayerPrefLogging")] Do not uncomment this. Always show errors.
		private void LogError(string message)
		{
			Debug.LogError($"<b><i>Pref-{PrefsKey} | </i></b>" + message);
		}

		#endregion
	}

}
using System;
using UnityEngine;

namespace Extenity.DataToolbox
{

	public class StringPlayerPref : PlayerPref<string>
	{
		public StringPlayerPref(string prefsKey, PathHashPostfix appendPathHashToKey, string defaultValue, float saveDelay = 0f)
			: base(prefsKey, appendPathHashToKey, defaultValue, null, saveDelay)
		{
		}

		public StringPlayerPref(string prefsKey, PathHashPostfix appendPathHashToKey, Func<PlayerPref<string>, string> defaultValueOverride, float saveDelay = 0f)
			: base(prefsKey, appendPathHashToKey, default(string), defaultValueOverride, saveDelay)
		{
		}

		protected override string InternalGetValue()
		{
			return PlayerPrefs.GetString(ProcessedPrefsKey, _Value);
		}

		protected override void InternalSetValue(string value)
		{
			PlayerPrefs.SetString(ProcessedPrefsKey, value);
		}

		protected override bool IsSame(string oldValue, string newValue)
		{
			return oldValue.EqualsOrBothEmpty(newValue);
		}
	}

}
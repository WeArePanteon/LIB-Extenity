﻿using System;
using System.Collections.Generic;
using Extenity.ConsistencyToolbox;
using UnityEngine;

namespace Extenity.DLLBuilder
{

	[Serializable]
	public class DistributerConfiguration : IConsistencyChecker
	{
		[Serializable]
		public class DistributionTarget
		{
			public bool Enabled = true;
			[Tooltip("Allows environment variables.")]
			public string SourceDirectoryPath;
			[Tooltip("Allows environment variables.")]
			public string TargetDirectoryPath;
			//public string[] ExcludedKeywords;
		}

		[Header("Meta")]
		public string ConfigurationName;
		public bool Enabled = true;

		[Header("Targets")]
		public DistributionTarget[] Targets;

		public void CheckConsistency(ref List<ConsistencyError> errors)
		{
			// We won't be doing this anymore. Instead, we won't be calling consistency checks on disabled configurations.
			//if (!Enabled)
			//	return;

			if (string.IsNullOrEmpty(ConfigurationName))
			{
				errors.Add(new ConsistencyError(this, "Configuration Name must be specified."));
			}
			if (Targets == null || Targets.Length == 0)
			{
				errors.Add(new ConsistencyError(this, "There must be at least one entry in Targets."));
			}
			else
			{
				for (var i = 0; i < Targets.Length; i++)
				{
					var target = Targets[i];
					if (target.Enabled)
					{
						if (string.IsNullOrEmpty(target.SourceDirectoryPath))
						{
							errors.Add(new ConsistencyError(this, $"Source Directory Path in Targets (at index '{i}') must be specified."));
						}
						DLLBuilderConfiguration.CheckEnvironmentVariableConsistency(target.SourceDirectoryPath, ref errors);

						if (string.IsNullOrEmpty(target.TargetDirectoryPath))
						{
							errors.Add(new ConsistencyError(this, $"Target Directory Path in Targets (at index '{i}') must be specified."));
						}
						DLLBuilderConfiguration.CheckEnvironmentVariableConsistency(target.TargetDirectoryPath, ref errors);
					}
				}
			}
		}
	}

}
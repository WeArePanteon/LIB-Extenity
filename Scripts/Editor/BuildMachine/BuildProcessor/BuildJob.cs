using System;
using System.IO;
using System.Text;
using Extenity.DataToolbox;
using Newtonsoft.Json;
using UnityEngine;

namespace Extenity.BuildMachine.Editor
{

	/// <summary>
	/// A build job is created when user requests a build. It keeps the build options that are
	/// specified by the user and also keeps track of whole build progress.
	///
	/// Build Job can be serialized so the data can survive assembly reloads, recompilations and
	/// editor restarts.
	/// 
	/// A Build Job can handle multiple platform builds in one go. Better yet, multiple builds
	/// on the same platform is also supported, if needed. Failing in one platform, fails the whole
	/// build run. This feature is made possible by specifying multiple <see cref="Builder"/>
	/// configurations.
	///
	/// A Build Job can work in phases (See <see cref="BuildPhaseInfo"/>). A Build Phase contains info
	/// about which Build Steps are run (See  <see cref="BuildStepAttribute"/>).
	///
	/// Understanding the flow of the whole build run is essential. For each Build Phase, all
	/// Builders are run with the Build Steps defined in this Build Phase. This allows designing
	/// multi-platform builds that first outputs the binaries and then deploys them. Designing
	/// such builds allows failing the whole multi-platform builds if a single platform fails.
	/// </summary>
	[Serializable]
	public class BuildJob
	{
		#region Initialization

		public static BuildJob Create(BuildPhaseInfo[] buildPhases, params Builder[] builders)
		{
			if (builders.IsNullOrEmpty())
				throw new ArgumentNullException(nameof(builders));

			return new BuildJob
			{
				BuildPhases = buildPhases,
				Builders = builders,
			};
		}

		#endregion

		#region Options

		public BuildPhaseInfo[] BuildPhases;

		#endregion

		#region Builders

		[SerializeField]
		public Builder[] Builders;

		#endregion

		#region State

		public int CurrentPhase = -1;
		public int CurrentBuilder = -1;
		public string CurrentStep = "";

		#endregion

		#region Start

		public void Start()
		{
			BuildJobRunner.Start(this);
		}

		#endregion

		#region Serialization

		public string SerializeToJson()
		{
			var config = new JsonSerializerSettings
			{
				Formatting = Formatting.Indented,
				TypeNameHandling = TypeNameHandling.Auto,
			};
			var stringBuilder = new StringBuilder();
			using (var stringWriter = new StringWriter(stringBuilder))
			{
				using (var jsonTextWriter = new JsonTextWriter(stringWriter)
				{
					Formatting = Formatting.Indented,
					Indentation = 1,
					IndentChar = '\t',
				})
				{
					(JsonSerializer.CreateDefault(config)).Serialize(jsonTextWriter, this);
				}
			}
			return stringBuilder.ToString();
			//return JsonConvert.SerializeObject(this, config); Unfortunately there is no way to specify IndentChar when using this single-liner.

			//return JsonUtility.ToJson(this, true); Unfortunately Unity's Json implementation does not support inheritance.
		}

		public static BuildJob DeserializeFromJson(string json)
		{
			return JsonConvert.DeserializeObject<BuildJob>(json);

			//return JsonUtility.FromJson<BuildJob>(json); Unfortunately Unity's Json implementation does not support inheritance.
		}

		#endregion
	}

}
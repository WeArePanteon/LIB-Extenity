using UnityEngine;

namespace Extenity.PainkillaTool.Editor
{

	public static class SnappaIcons
	{
		#region Embedded Texture - ArrowStraight

		private static Texture2D _Texture_ArrowStraight;
		public static Texture2D Texture_ArrowStraight
		{
			get
			{
				if (_Texture_ArrowStraight == null)
				{
#if OverrideTextures
					_Texture_ArrowStraight = LoadTexture("ArrowStraight");
#else
					_Texture_ArrowStraight = new Texture2D(1, 1, TextureFormat.ARGB32, false, false);
					_Texture_ArrowStraight.LoadImage(_TextureData_ArrowStraight, true);
#endif
					Texture.DontDestroyOnLoad(_Texture_ArrowStraight);
					_Texture_ArrowStraight.hideFlags = HideFlags.HideAndDontSave;
				}
				return _Texture_ArrowStraight;
			}
		}

		private static readonly byte[] _TextureData_ArrowStraight =
		{
			137,80,78,71,13,10,26,10,0,0,0,13,73,72,68,82,0,0,0,118,0,0,0,128,8,4,0,0,0,116,101,85,158,0,0,11,220,73,68,65,84,120,218,229,91,203,142,93,87,17,93,
			85,253,116,12,73,48,208,61,32,32,66,236,240,7,48,77,79,8,19,62,1,49,96,132,252,3,201,16,38,246,144,73,104,38,72,32,139,31,96,68,16,194,67,200,31,68,81,35,164,208,8,
			117,39,116,26,9,199,142,31,187,24,220,115,206,173,215,190,143,238,179,175,251,38,87,114,119,223,247,89,167,86,173,90,85,117,12,60,135,219,249,253,179,63,226,139,113,59,191,127,42,39,242,
			133,128,59,129,122,42,167,159,127,184,231,247,79,228,84,78,100,242,115,213,112,105,181,80,63,123,131,32,234,139,55,223,189,241,195,207,37,216,243,251,143,223,0,100,248,74,1,65,176,181,66,
			184,188,74,168,210,129,236,127,10,128,39,111,174,142,204,180,58,2,79,191,78,220,95,171,34,51,175,42,170,4,66,159,175,61,141,123,74,175,42,186,188,106,2,203,0,85,223,86,3,151,87,
			5,85,66,254,208,240,40,129,240,116,5,112,121,21,185,74,29,93,53,149,37,72,70,251,232,210,42,160,202,16,69,93,101,197,28,198,228,53,109,165,138,219,18,216,138,17,169,42,59,61,211,
			253,227,2,105,76,102,106,25,85,27,83,24,250,138,138,169,62,148,150,54,131,218,66,245,95,32,3,133,251,104,66,253,213,223,90,193,229,118,80,53,80,235,157,68,229,174,143,44,128,102,100,
			230,86,178,36,6,98,31,67,26,96,81,168,182,210,61,79,205,224,114,27,168,94,125,209,9,208,52,182,209,58,146,49,28,45,224,114,11,168,211,232,137,83,227,105,181,245,162,65,74,186,38,
			167,106,252,186,203,227,231,42,41,2,107,128,26,148,142,114,180,24,253,189,177,163,75,99,67,21,71,79,65,246,24,156,161,128,43,63,109,148,153,90,228,170,175,173,48,112,53,85,41,121,206,
			190,99,76,87,69,99,65,125,252,134,53,133,211,168,102,16,196,197,149,194,4,67,71,124,172,232,210,120,4,134,171,170,185,75,18,71,100,123,26,180,74,211,232,209,29,65,160,254,251,151,222,
			45,201,80,93,167,7,78,213,51,219,247,64,50,116,186,18,178,182,151,175,39,111,158,253,225,10,128,125,240,215,141,131,77,5,198,91,68,49,170,236,157,20,76,214,146,249,132,62,214,147,251,
			79,126,116,114,231,57,211,248,193,223,228,251,0,240,24,79,29,241,196,168,43,37,205,187,84,149,152,156,147,30,72,125,119,239,237,231,22,217,7,239,77,160,2,219,216,80,100,20,87,83,201,
			181,116,125,238,82,18,83,164,3,156,238,147,222,58,189,243,156,34,251,224,61,249,158,190,255,25,158,6,37,214,189,13,76,227,78,6,136,134,74,174,173,119,146,119,119,255,237,149,71,246,211,
			119,45,84,96,7,155,67,212,98,137,137,185,44,202,103,137,138,177,159,83,57,201,187,68,116,47,8,246,209,29,254,65,36,197,78,71,102,82,197,132,20,73,235,101,199,103,114,246,236,64,245,
			183,46,42,85,124,49,168,120,11,216,64,6,119,211,53,109,82,25,160,82,229,183,167,175,36,127,95,52,186,116,81,168,147,219,179,160,178,192,99,60,49,243,7,10,214,209,19,154,28,27,196,
			148,178,172,28,225,66,202,76,151,129,90,135,251,52,17,37,4,138,75,66,94,13,94,102,206,170,150,151,42,190,28,212,156,204,219,157,84,89,105,145,164,171,209,89,45,182,204,184,87,72,164,
			253,210,100,166,203,65,173,71,183,47,68,153,87,134,113,202,150,234,146,30,152,164,239,1,176,20,153,233,242,80,23,33,51,146,25,163,239,118,236,73,241,205,67,94,183,9,178,4,153,105,12,
			168,243,163,43,201,23,139,27,162,198,193,42,185,150,175,98,60,22,134,75,227,64,173,195,125,230,156,49,85,170,170,36,237,95,140,51,133,103,151,145,42,26,11,234,236,66,148,13,93,200,149,
			24,107,36,81,161,49,194,107,22,111,17,120,60,168,117,101,142,86,95,219,127,189,241,169,79,164,102,13,221,177,160,50,243,120,80,107,112,119,176,165,252,143,152,233,35,220,10,211,102,170,63,
			73,18,70,239,138,156,11,192,165,49,161,206,83,230,44,243,40,153,58,81,200,115,164,207,135,98,54,39,119,105,108,168,179,114,151,146,108,139,100,149,176,197,165,212,104,74,24,213,206,171,187,
			60,62,212,122,238,10,200,76,155,242,233,133,22,43,189,81,176,13,1,76,103,213,55,140,50,179,35,162,241,161,206,171,187,72,103,143,185,147,138,163,128,188,222,154,52,168,146,153,218,64,5,
			4,165,74,102,164,52,149,180,113,64,120,62,75,6,55,191,170,144,153,218,64,93,220,102,80,48,20,53,251,8,39,75,222,61,155,206,42,133,203,237,160,214,10,209,134,154,26,235,75,191,236,
			240,180,223,251,81,210,78,144,137,172,132,194,84,155,102,80,59,168,243,91,132,172,17,160,196,94,82,210,66,96,38,149,243,232,114,91,168,121,116,55,241,112,230,230,93,220,38,161,31,207,73,
			98,40,37,124,14,85,163,203,109,161,78,190,194,31,248,239,241,59,60,72,69,45,159,83,233,57,35,133,234,76,149,161,64,132,75,173,161,122,50,11,238,225,125,48,94,198,79,112,221,196,169,
			214,251,120,143,149,93,217,154,189,182,139,178,42,68,188,10,168,83,50,247,80,129,243,46,186,122,19,47,206,41,81,210,209,34,20,46,221,82,216,249,51,65,204,36,146,86,1,181,143,110,193,
			61,124,160,30,185,129,31,227,250,204,157,143,132,6,79,144,45,172,163,220,153,87,119,82,197,171,130,10,60,198,111,241,1,74,119,175,0,56,195,189,46,186,62,51,69,93,228,231,213,151,156,
			48,249,190,87,66,114,244,185,75,171,130,250,8,239,224,163,14,38,119,63,11,184,139,110,205,228,99,206,166,62,55,151,233,142,225,238,222,219,180,58,168,39,137,131,233,225,190,80,129,53,111,
			100,142,224,145,145,206,51,9,128,220,101,188,212,30,234,67,28,226,35,5,181,152,202,119,134,123,248,52,41,16,218,29,233,235,148,197,208,158,130,215,142,61,81,119,122,94,98,220,198,97,235,
			168,254,26,39,73,113,159,66,254,88,193,165,48,60,207,54,123,100,114,150,66,189,78,54,71,135,184,205,187,210,22,238,67,188,163,160,150,138,237,232,165,74,84,161,209,69,68,220,242,211,238,
			25,196,153,14,40,54,116,39,235,80,110,239,9,3,45,225,62,196,175,6,89,210,101,253,70,39,83,147,103,38,202,220,187,42,114,45,187,184,221,61,65,111,119,53,145,189,74,15,234,124,136,
			219,251,50,173,179,132,119,240,179,118,80,181,32,1,175,226,69,156,225,95,33,206,83,169,202,174,207,64,16,39,74,198,170,137,146,119,80,85,58,140,15,215,67,157,0,221,192,171,184,14,6,
			240,31,124,168,226,107,149,57,251,223,4,228,246,123,168,142,225,140,219,30,160,90,111,60,42,220,71,56,116,178,4,20,108,225,38,118,135,76,253,4,31,162,12,167,65,195,141,151,137,69,217,
			241,99,115,201,174,206,56,164,219,123,146,246,179,227,193,213,185,202,195,207,45,220,194,142,41,60,231,248,71,120,111,111,51,80,217,253,100,155,163,176,221,155,220,51,80,93,139,55,150,84,61,
			10,185,202,40,216,237,160,178,122,236,69,188,26,52,250,99,165,204,241,170,140,104,254,169,54,123,118,80,179,73,197,165,163,155,203,210,46,94,195,54,74,7,114,122,166,11,254,135,191,155,220,
			213,45,66,189,207,169,173,197,6,70,28,202,144,171,213,25,212,101,163,91,78,50,168,215,241,250,112,105,73,233,190,150,187,162,243,37,220,52,151,157,0,5,31,227,30,158,126,4,71,91,49,
			123,31,127,81,160,41,73,9,212,116,224,118,41,184,71,127,186,115,226,136,201,120,25,175,97,99,200,92,54,14,138,1,124,25,55,157,42,51,206,240,231,95,210,145,39,44,92,189,245,4,239,
			94,163,20,120,238,70,224,194,112,143,112,240,254,39,254,99,191,134,111,129,58,104,189,92,245,95,222,195,190,166,224,246,191,255,121,46,7,56,18,179,63,32,55,89,36,211,16,246,4,166,20,
			106,117,253,113,33,184,71,56,216,61,6,177,33,228,215,241,13,80,247,53,60,144,184,12,222,169,7,126,13,183,176,161,24,81,0,236,31,203,1,142,200,216,198,236,63,166,154,202,122,40,78,
			150,22,216,245,44,13,119,2,213,57,224,87,240,138,139,24,155,210,195,93,116,25,140,23,84,94,247,183,253,99,28,224,104,58,184,33,99,30,40,20,166,122,84,103,130,93,18,238,0,245,25,
			77,63,244,155,216,55,217,91,212,63,29,89,238,254,222,86,209,229,14,194,254,177,28,208,145,31,174,198,245,167,64,64,135,168,70,117,14,216,37,224,170,168,110,80,79,213,111,227,134,130,198,
			166,194,234,175,158,64,158,220,219,197,119,177,19,162,59,129,43,70,145,147,213,216,12,2,47,0,118,65,184,10,42,240,172,35,229,119,240,149,160,188,165,187,95,6,240,172,186,219,201,239,137,
			203,178,153,104,115,23,174,207,233,50,186,162,192,75,128,93,0,174,129,10,108,80,193,6,110,226,122,23,213,162,132,41,118,180,37,28,4,99,11,175,117,23,38,232,232,210,129,28,101,61,109,
			119,155,67,224,5,193,206,129,235,160,2,207,104,7,175,227,154,42,45,125,12,117,28,97,226,61,37,246,228,181,91,120,29,59,206,220,237,29,227,128,142,236,133,127,67,155,31,140,225,133,193,
			206,128,27,160,2,215,206,110,97,123,160,102,49,13,58,134,236,228,80,107,109,212,183,113,19,219,167,81,153,229,72,204,117,204,210,245,171,139,64,93,248,66,205,20,110,2,21,248,234,191,183,
			28,136,50,196,172,36,229,167,168,19,161,53,123,27,123,225,179,251,232,154,222,117,102,177,185,16,216,4,110,10,21,38,67,89,129,211,211,98,255,172,150,46,158,57,175,218,239,224,14,166,98,
			225,168,46,5,214,193,173,66,45,170,21,47,38,43,139,131,80,204,4,163,56,63,93,187,237,117,202,140,25,30,120,4,176,10,110,21,170,205,65,223,251,76,221,18,155,202,91,66,9,42,161,
			233,179,117,87,142,38,178,180,12,84,4,127,54,23,238,163,219,56,193,111,106,80,173,198,90,90,23,149,175,108,218,250,233,50,68,179,160,84,191,97,255,248,244,0,63,149,95,236,201,114,71,
			191,36,88,96,87,240,243,153,253,236,16,179,98,50,18,174,101,247,247,237,44,106,30,233,246,142,103,31,197,8,52,94,184,129,55,7,108,205,133,118,82,58,87,217,153,200,22,55,30,255,3,
			89,253,229,13,67,78,243,162,172,69,169,120,173,49,110,155,45,162,202,198,70,212,243,218,18,214,210,154,215,1,172,61,108,155,173,72,100,200,190,122,74,242,210,224,184,54,219,228,69,9,25,
			82,28,97,75,242,46,78,222,119,165,115,86,199,176,132,71,116,203,231,41,92,66,164,215,4,172,142,209,212,78,120,97,98,101,40,188,96,173,141,64,193,57,36,109,26,224,78,66,20,163,210,
			40,99,155,228,108,49,212,100,87,115,251,166,160,4,151,149,113,227,138,211,56,122,90,86,214,130,141,60,177,91,72,219,172,229,245,136,108,173,240,216,213,7,135,190,71,251,228,210,68,73,26,
			69,214,207,42,108,19,8,215,243,182,146,164,21,152,10,159,113,28,140,70,108,23,138,1,205,141,142,10,45,98,171,205,1,171,201,147,174,176,57,164,18,186,162,43,221,8,216,198,142,211,54,
			65,147,220,118,59,60,216,69,94,23,26,199,209,90,49,70,82,55,235,37,41,78,107,98,42,102,217,130,146,204,22,61,72,110,152,97,13,212,216,126,184,38,46,7,210,218,33,29,87,233,127,
			165,115,182,152,25,5,27,213,205,252,51,15,175,109,215,186,55,157,130,196,5,180,157,52,233,231,230,113,228,202,130,101,53,255,231,208,238,233,194,82,156,245,96,192,121,233,53,200,217,226,96,
			20,53,49,182,83,167,204,74,90,87,117,197,213,216,79,126,121,168,167,37,233,142,138,219,207,214,102,25,87,54,103,245,220,95,171,107,255,120,49,80,139,251,141,176,245,185,194,145,245,235,200,
			226,234,110,77,111,185,242,186,43,110,42,34,33,139,51,11,37,201,220,18,154,251,53,234,103,227,36,162,152,232,21,179,26,41,141,231,79,205,212,152,147,101,70,49,82,196,110,62,161,51,181,
			149,127,106,22,217,146,204,164,52,169,125,159,107,231,22,104,52,63,110,50,169,40,110,42,81,204,56,149,221,5,67,112,142,185,93,100,185,101,206,102,179,138,18,198,166,150,228,118,52,183,22,
			93,79,204,88,157,207,37,93,126,148,70,243,137,230,93,79,92,88,66,77,254,217,93,140,80,204,171,184,153,30,55,171,179,118,173,81,18,138,114,232,126,75,218,224,175,65,35,224,243,46,90,
			252,18,12,133,239,123,214,162,197,243,166,130,171,173,160,207,239,226,254,187,196,90,228,236,52,251,236,124,169,36,77,93,9,131,185,86,89,219,72,141,139,43,50,197,76,37,88,185,165,210,116,
			196,182,146,58,171,219,116,54,26,237,45,164,237,109,225,114,119,13,28,148,31,172,216,235,141,179,13,94,237,146,132,53,152,84,248,221,142,173,158,211,230,61,203,212,252,254,149,109,4,226,133,
			63,126,160,22,55,181,217,36,106,252,219,255,1,225,162,23,112,55,196,233,245,0,0,0,0,73,69,78,68,174,66,96,130,
		};

		#endregion

		#region Embedded Texture - ArrowBent

		private static Texture2D _Texture_ArrowBent;
		public static Texture2D Texture_ArrowBent
		{
			get
			{
				if (_Texture_ArrowBent == null)
				{
#if OverrideTextures
					_Texture_ArrowBent = LoadTexture("ArrowBent");
#else
					_Texture_ArrowBent = new Texture2D(1, 1, TextureFormat.ARGB32, false, false);
					_Texture_ArrowBent.LoadImage(_TextureData_ArrowBent, true);
#endif
					Texture.DontDestroyOnLoad(_Texture_ArrowBent);
					_Texture_ArrowBent.hideFlags = HideFlags.HideAndDontSave;
				}
				return _Texture_ArrowBent;
			}
		}

		private static readonly byte[] _TextureData_ArrowBent =
		{
			137,80,78,71,13,10,26,10,0,0,0,13,73,72,68,82,0,0,0,128,0,0,0,110,8,4,0,0,0,69,121,211,178,0,0,13,88,73,68,65,84,120,218,229,93,91,108,92,87,21,93,
			103,99,199,198,113,192,182,84,187,226,163,41,207,134,38,2,4,18,36,56,41,216,4,68,249,162,162,8,161,22,37,3,8,1,82,219,160,146,132,138,8,42,181,18,161,65,180,8,181,124,240,
			113,49,95,20,33,170,162,66,250,147,68,162,161,80,9,41,72,81,157,144,54,137,90,149,54,51,173,90,247,145,104,252,58,135,143,251,218,123,159,115,39,51,158,59,241,184,51,209,140,61,119,
			238,237,245,90,103,63,214,222,251,184,6,186,238,81,191,26,189,252,168,111,169,215,234,63,235,109,248,174,238,234,7,123,27,126,111,82,32,224,247,30,5,30,252,222,162,160,190,57,0,191,119,
			40,168,111,174,87,131,240,123,131,130,134,240,223,254,20,92,22,190,171,187,250,207,123,27,126,71,41,48,171,10,255,122,28,195,120,147,39,223,55,184,255,109,70,64,75,240,59,70,129,89,51,
			240,59,68,1,173,26,252,163,45,195,7,246,149,31,11,204,170,193,159,88,225,197,37,91,193,42,16,80,255,48,142,173,24,62,0,28,26,220,183,134,9,104,27,126,201,20,152,53,8,191,84,
			10,204,154,132,95,34,5,102,141,194,47,141,2,186,130,240,143,150,10,31,216,91,191,111,205,88,64,125,19,142,161,19,221,222,182,173,192,172,105,248,0,240,139,193,189,221,239,2,247,162,115,
			189,254,31,214,15,117,191,5,12,227,48,182,119,240,6,109,88,193,149,138,1,157,166,224,129,193,31,116,121,26,236,44,5,203,192,83,235,183,118,117,26,28,124,11,55,226,120,167,224,59,184,
			79,93,252,103,247,43,193,142,88,193,50,92,10,230,95,235,183,117,117,63,160,19,86,144,195,7,220,214,139,79,118,121,67,164,108,10,56,124,0,112,219,222,56,214,253,253,128,210,28,65,195,
			7,22,177,136,117,79,140,220,208,213,4,148,69,65,24,62,128,150,40,88,165,166,104,251,20,132,225,187,4,84,243,20,172,94,87,184,45,10,66,240,23,50,64,14,192,64,147,20,208,106,17,
			208,78,56,12,195,55,25,124,3,96,126,199,220,223,187,154,0,96,207,210,95,191,101,255,93,30,124,151,172,125,250,58,191,99,238,137,46,114,129,136,104,227,197,143,191,250,85,123,237,226,216,
			242,187,150,215,47,14,218,62,96,8,183,99,164,36,223,55,12,124,106,9,235,142,143,236,88,85,2,34,162,143,97,218,110,197,117,248,32,13,0,47,224,101,88,16,0,11,130,5,181,72,65,
			163,213,71,246,53,255,254,242,20,116,140,128,104,19,62,71,211,246,179,24,75,61,205,2,112,120,1,175,170,51,155,167,160,216,247,29,76,242,204,195,96,74,195,64,67,10,58,64,64,116,13,
			221,130,91,113,125,186,198,150,133,27,11,224,57,204,137,243,45,134,112,7,70,87,108,252,41,4,151,0,54,236,93,252,104,100,5,165,18,16,189,155,110,182,183,210,103,172,73,97,199,95,243,
			215,248,187,115,120,61,115,131,102,173,160,72,246,112,2,12,123,229,14,209,200,17,74,35,32,122,15,246,226,59,24,202,87,58,77,49,150,173,127,76,194,50,206,226,98,242,89,78,193,109,13,
			172,160,81,232,115,12,136,19,176,57,57,69,142,80,10,1,51,27,237,126,124,19,3,249,90,35,131,158,187,0,50,155,32,88,156,195,155,77,199,2,31,254,18,22,152,167,67,193,230,224,114,
			87,8,91,129,105,31,60,126,130,111,216,254,16,104,29,3,184,59,44,225,44,46,101,215,196,87,15,98,79,128,130,98,223,119,42,245,57,241,244,129,134,40,104,139,128,104,29,221,105,15,96,
			136,50,227,206,77,30,42,244,229,116,164,79,139,103,80,103,164,133,173,160,145,230,15,7,61,199,146,160,83,57,194,119,132,54,8,152,153,182,15,98,147,22,148,182,225,250,75,75,176,56,131,
			121,69,193,160,200,8,197,137,15,44,229,249,97,79,147,145,63,181,21,172,144,128,232,106,252,146,190,110,69,124,207,97,72,169,131,0,33,233,215,69,156,85,20,88,12,103,86,208,88,245,113,
			2,228,247,70,36,65,237,24,210,10,86,68,64,244,69,252,30,87,65,121,59,143,250,36,188,219,138,16,40,201,88,192,179,88,100,113,32,86,135,183,97,180,65,189,239,84,120,243,245,159,83,
			20,73,119,225,86,208,50,1,51,125,184,215,238,203,175,35,97,248,220,199,161,210,159,159,29,226,207,98,10,56,1,192,16,246,162,191,160,224,245,13,63,148,15,184,60,146,89,66,38,197,22,
			9,152,185,198,254,1,219,160,214,213,47,49,173,8,128,228,69,7,8,146,22,112,6,203,42,138,140,97,15,250,10,85,31,132,220,245,133,143,150,197,58,88,58,12,252,99,100,123,203,229,112,
			244,121,251,31,108,147,177,158,146,127,8,154,185,204,3,200,206,181,217,90,19,8,235,240,126,229,52,132,57,60,192,72,89,194,66,6,207,193,21,172,184,129,17,38,15,37,146,28,115,155,249,
			201,185,227,45,18,16,125,13,143,97,52,15,105,196,192,217,134,237,5,43,52,31,4,65,22,22,192,122,188,79,125,10,204,225,254,132,130,37,204,39,208,144,192,68,129,254,115,202,1,140,16,
			200,241,181,14,14,6,38,161,160,105,23,152,249,190,253,53,145,245,148,93,120,165,161,52,32,121,240,180,84,6,222,194,121,88,21,47,198,112,7,92,182,250,69,70,14,145,239,253,168,0,79,
			39,230,197,114,147,22,16,221,141,7,17,132,159,2,161,100,45,57,88,202,108,197,6,219,80,54,185,38,182,166,97,140,101,255,173,244,140,75,184,128,249,228,135,205,193,25,102,242,60,27,112,
			245,111,178,99,72,86,220,40,155,49,112,176,243,77,17,16,253,10,63,181,94,145,147,3,207,99,58,183,2,43,164,47,196,39,196,232,73,31,175,227,21,69,212,32,118,43,93,40,189,217,177,
			184,192,3,161,19,32,83,235,224,54,17,31,233,63,50,182,179,9,2,102,238,166,219,117,192,35,1,218,6,155,140,252,156,60,27,144,170,2,211,107,47,226,121,221,54,197,110,140,177,86,39,
			216,26,26,113,92,139,219,220,30,28,139,252,250,120,255,145,177,157,77,196,128,232,123,120,168,40,209,249,229,110,56,11,232,90,192,23,75,151,240,12,150,4,57,49,124,158,244,184,188,129,200,
			2,166,64,14,235,32,201,137,236,59,50,182,179,9,29,16,221,76,15,91,10,173,36,60,13,104,131,186,16,193,202,64,214,6,11,56,147,104,188,244,138,119,162,194,224,59,5,87,107,128,162,
			156,15,213,47,202,207,233,79,224,95,134,128,104,10,135,105,64,42,57,91,8,223,143,238,190,64,182,98,221,227,167,195,153,164,42,76,143,15,98,55,70,69,164,15,23,184,46,88,218,134,206,
			147,101,82,95,6,191,33,1,209,70,156,160,81,169,221,116,62,15,43,125,185,226,36,224,107,141,184,140,179,120,83,232,135,1,84,48,170,86,50,100,5,80,173,48,45,128,141,215,41,140,207,
			233,103,240,33,212,166,12,125,253,248,35,70,185,249,134,202,25,89,243,113,112,86,92,149,203,30,29,73,158,195,69,65,239,80,182,250,190,204,49,170,217,169,215,219,40,237,239,178,228,153,31,
			239,63,50,186,179,185,201,208,33,124,146,167,43,153,218,72,152,63,37,128,57,248,20,172,21,159,105,7,121,17,175,139,163,49,124,169,237,52,124,39,94,211,180,104,130,198,143,140,8,36,171,
			47,225,23,186,64,116,19,254,140,66,205,134,64,207,135,148,151,135,223,201,56,242,18,46,136,64,57,132,221,24,243,26,155,178,250,11,133,58,30,238,164,219,72,155,233,19,198,223,128,128,232,
			90,156,160,17,27,8,104,178,179,23,162,65,146,33,143,75,34,95,203,50,127,158,248,70,3,41,205,40,179,215,109,46,163,186,195,225,234,207,4,225,23,196,0,122,200,142,192,43,104,41,144,
			241,155,9,133,80,29,159,248,248,27,74,248,12,38,171,15,229,223,41,196,48,96,231,133,71,167,162,67,74,76,24,126,48,6,68,55,217,27,253,238,110,216,240,145,85,116,164,60,30,129,6,
			88,110,3,151,112,158,93,159,38,62,120,63,188,84,238,188,170,75,245,125,94,2,107,135,200,43,6,167,34,127,67,23,136,134,48,139,141,126,18,43,110,125,104,61,8,209,254,164,128,240,149,
			13,16,120,240,181,241,135,114,128,83,0,100,155,92,58,68,49,252,144,5,252,152,54,34,0,159,132,150,71,65,47,32,95,83,253,105,190,254,75,56,135,101,118,60,21,189,188,142,115,108,149,
			121,35,132,127,103,178,98,199,175,4,249,177,70,240,61,11,152,249,144,61,137,117,126,168,163,2,79,39,213,10,105,84,5,164,243,225,103,113,73,12,67,118,103,178,7,65,249,2,47,200,153,
			128,26,240,207,140,191,91,231,37,190,134,22,96,239,241,225,243,234,157,20,84,105,220,161,74,0,42,78,156,199,37,62,8,177,185,241,27,47,140,133,6,94,78,212,249,96,109,46,167,36,79,
			92,240,54,134,175,44,32,186,14,179,68,33,57,139,192,136,163,168,197,233,87,4,121,72,124,165,246,218,28,206,226,156,57,189,124,114,249,191,223,174,143,60,134,73,157,229,195,242,39,212,219,
			245,39,1,210,114,250,26,26,127,40,13,222,21,255,180,20,240,92,235,201,89,25,35,66,99,240,188,27,104,45,61,133,71,241,232,222,211,249,205,106,27,220,97,76,74,223,245,3,161,83,102,
			175,149,65,122,189,47,150,250,155,128,47,238,25,189,23,103,208,39,203,24,191,149,93,20,239,17,136,255,201,59,75,51,246,238,138,234,119,212,54,224,176,155,12,109,110,241,203,94,35,98,124,
			163,50,55,92,241,53,107,1,251,211,119,124,253,56,120,202,94,33,142,248,109,145,188,87,68,127,195,143,118,157,212,183,173,13,167,171,143,224,32,203,41,111,135,183,238,80,81,95,90,65,179,
			240,153,5,204,140,227,121,59,128,160,140,181,13,234,1,242,26,222,44,251,159,176,119,86,2,155,151,171,195,120,220,76,134,6,154,225,73,79,40,62,248,27,34,154,203,251,197,22,112,75,12,
			95,250,53,121,67,109,13,94,186,139,144,190,191,179,223,173,204,23,193,119,222,88,67,182,56,253,86,136,243,122,191,206,171,0,91,133,207,8,176,187,194,202,223,178,208,167,171,57,94,8,91,
			70,24,128,101,236,173,220,31,186,97,117,216,60,142,73,189,106,254,72,147,195,213,213,65,120,42,16,127,237,123,116,236,203,173,76,187,18,36,209,71,240,81,10,26,181,126,151,103,117,155,181,
			64,172,82,136,52,103,191,20,134,95,27,198,227,152,132,234,239,67,52,46,100,19,195,9,141,96,10,154,224,25,133,7,91,131,159,89,0,237,178,74,242,66,181,60,252,116,23,218,5,2,0,
			120,213,78,86,78,135,225,187,204,247,121,200,42,146,177,185,143,59,209,241,55,158,40,74,172,228,224,196,93,173,78,187,13,0,68,239,160,255,217,9,4,246,244,249,245,125,184,247,199,50,198,
			146,253,66,229,88,216,248,211,253,225,126,19,194,223,221,99,2,77,110,191,9,46,40,59,56,222,50,252,116,177,111,176,19,36,134,89,82,238,112,155,32,65,132,95,9,226,246,34,248,230,176,
			217,174,155,156,142,25,113,104,224,229,148,171,184,128,106,76,62,91,17,252,228,167,166,105,222,241,131,106,126,147,80,129,54,40,117,179,16,249,208,174,223,20,173,190,219,238,148,162,247,135,85,
			82,1,248,131,175,176,134,119,43,50,126,134,194,78,17,43,98,101,97,163,91,30,105,172,231,237,144,44,110,28,183,119,20,27,63,15,119,198,131,233,88,60,8,249,185,140,14,106,8,190,98,
			248,128,1,162,33,154,67,191,47,127,201,219,241,85,212,39,72,84,130,179,159,168,156,8,6,191,63,225,43,240,166,55,240,166,56,254,94,63,19,28,142,201,35,104,3,126,252,243,111,183,253,
			254,196,198,239,236,89,111,55,32,201,137,224,195,97,248,128,59,128,11,78,13,44,100,52,112,158,186,135,250,5,8,167,100,114,150,52,219,130,15,16,64,83,36,76,59,175,253,253,112,167,35,
			127,110,31,118,17,7,138,110,50,113,26,83,230,130,20,185,154,10,94,234,134,244,161,9,156,99,86,24,249,181,5,124,218,250,201,44,48,245,149,195,17,221,75,161,223,86,206,22,223,102,252,
			180,155,198,5,153,183,165,242,231,141,79,191,248,145,178,55,151,61,237,194,7,8,176,155,8,240,134,214,80,5,46,223,21,228,143,68,104,193,222,211,248,70,19,167,204,52,170,206,155,220,57,
			81,201,233,206,62,223,17,144,239,10,72,174,109,211,248,19,12,209,40,198,33,100,173,13,192,214,186,32,31,146,37,153,224,72,229,194,229,110,53,126,202,76,153,106,222,178,118,106,192,205,1,
			27,79,19,234,146,7,165,192,7,8,215,17,32,118,126,228,175,86,8,96,158,12,73,117,133,240,72,51,55,27,63,133,41,84,243,164,103,32,183,179,228,73,142,143,55,164,241,39,214,80,130,
			241,39,4,208,38,94,214,192,171,249,229,100,24,94,131,140,96,1,139,191,52,119,187,241,83,102,10,85,61,199,51,144,191,240,6,53,211,213,26,192,149,180,250,49,154,77,228,137,94,127,243,
			19,119,142,52,1,50,18,158,172,84,155,189,97,108,5,188,8,210,33,209,120,99,15,41,133,77,137,240,1,194,7,164,176,149,250,206,178,138,63,212,34,79,72,120,164,149,91,78,156,66,18,
			14,157,26,118,242,124,96,196,120,44,159,13,154,210,140,63,65,148,142,65,173,88,253,226,92,0,111,59,28,128,167,90,187,233,196,44,166,77,213,239,247,106,207,15,148,189,37,195,7,136,134,
			253,238,190,85,253,29,190,45,150,66,125,131,23,91,189,237,196,44,166,77,77,15,65,165,224,245,55,186,187,82,141,63,93,206,13,186,221,193,203,94,93,29,202,105,64,150,27,94,106,253,198,
			227,179,110,202,212,160,58,65,198,107,121,179,201,79,7,224,3,100,135,165,8,150,208,73,200,99,29,1,18,27,120,173,82,95,201,173,39,102,221,20,106,161,230,151,83,59,192,203,142,252,210,
			5,54,240,196,166,115,191,238,1,72,141,144,156,249,226,74,111,62,49,139,105,212,76,112,234,43,119,117,154,14,193,7,250,236,91,180,192,7,97,164,38,59,214,219,223,197,59,193,4,192,206,
			174,252,246,19,79,215,166,221,81,51,238,212,134,38,213,10,45,61,244,21,12,71,87,231,81,221,108,142,186,113,217,243,19,61,224,142,173,126,151,16,0,84,55,227,168,25,247,247,7,160,195,
			171,223,53,4,0,181,205,56,230,174,50,254,175,194,117,24,254,170,254,47,52,68,82,124,26,211,120,153,43,2,87,106,201,179,38,30,181,45,213,90,213,85,93,205,213,92,205,85,93,173,247,
			254,218,80,109,75,173,86,117,49,9,213,222,252,99,75,213,45,213,90,173,119,225,167,142,80,235,93,248,0,80,189,162,127,108,237,255,44,33,19,125,168,217,68,199,0,0,0,0,73,69,78,
			68,174,66,96,130,
		};

		#endregion
	}

}
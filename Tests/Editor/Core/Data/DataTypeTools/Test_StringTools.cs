using System;
using System.Collections.Generic;
using Extenity.DataToolbox;
using Extenity.MathToolbox;
using Extenity.UnityTestToolbox;
using NUnit.Framework;
using Random = UnityEngine.Random;

namespace ExtenityTests.DataToolbox
{

	public class Test_StringTools : AssertionHelper
	{
		#region Equals

		[Test]
		public static void IsAllZeros()
		{
			Assert.IsFalse(((string)null).IsAllZeros());
			Assert.IsFalse("".IsAllZeros());
			Assert.IsFalse(string.Empty.IsAllZeros());
			Assert.IsFalse(" ".IsAllZeros());
			Assert.IsFalse("\t".IsAllZeros());
			Assert.IsFalse(" 0".IsAllZeros());
			Assert.IsFalse(" 00".IsAllZeros());
			Assert.IsFalse("a00".IsAllZeros());
			Assert.IsFalse("0 ".IsAllZeros());
			Assert.IsFalse("00 ".IsAllZeros());
			Assert.IsFalse("00a".IsAllZeros());
			Assert.IsFalse("00a00".IsAllZeros());
			Assert.IsTrue("0".IsAllZeros());
			Assert.IsTrue("00".IsAllZeros());
			Assert.IsTrue("000".IsAllZeros());
			Assert.IsTrue("0000".IsAllZeros());
			Assert.IsTrue("00000".IsAllZeros());
			Assert.IsTrue("000000".IsAllZeros());
			Assert.IsTrue("0000000000000000000000000".IsAllZeros());
			Assert.IsTrue("0000000000000000000000000000000000000000000000000000000".IsAllZeros());
		}

		#endregion

		#region String Operations

		[Test]
		public static void IsAlphaNumericAscii()
		{
			Assert.False("".IsAlphaNumericAscii(false, false));
			Assert.False(" ".IsAlphaNumericAscii(false, false));
			Assert.False("-".IsAlphaNumericAscii(false, false));
			Assert.False(" a".IsAlphaNumericAscii(false, false));
			Assert.False(" 1".IsAlphaNumericAscii(false, false));
			Assert.False(" a1".IsAlphaNumericAscii(false, false));
			Assert.False("-a".IsAlphaNumericAscii(false, false));
			Assert.False("-1".IsAlphaNumericAscii(false, false));
			Assert.False("-a1".IsAlphaNumericAscii(false, false));
			Assert.False("a-".IsAlphaNumericAscii(false, false));
			Assert.False("1-".IsAlphaNumericAscii(false, false));
			Assert.False("a1-".IsAlphaNumericAscii(false, false));
			Assert.False("a-".IsAlphaNumericAscii(false, false));
			Assert.False("1-".IsAlphaNumericAscii(false, false));
			Assert.False("a1-".IsAlphaNumericAscii(false, false));
			Assert.False("a a".IsAlphaNumericAscii(false, false));

			Assert.True("a".IsAlphaNumericAscii(false, false));
			Assert.True("1".IsAlphaNumericAscii(false, false));
			Assert.True("a1".IsAlphaNumericAscii(false, false));
			Assert.True("a1bjgfy8723bsdk71".IsAlphaNumericAscii(false, false));

			// Allow spaces
			Assert.True("a".IsAlphaNumericAscii(true, false));
			Assert.True("1".IsAlphaNumericAscii(true, false));
			Assert.True("a1".IsAlphaNumericAscii(true, false));
			Assert.True(" a".IsAlphaNumericAscii(true, false));
			Assert.True(" 1".IsAlphaNumericAscii(true, false));
			Assert.True(" a1".IsAlphaNumericAscii(true, false));
			Assert.True("a ".IsAlphaNumericAscii(true, false));
			Assert.True("1 ".IsAlphaNumericAscii(true, false));
			Assert.True("a1 ".IsAlphaNumericAscii(true, false));
			Assert.True("a1bjgfy8723bsdk71".IsAlphaNumericAscii(true, false));
			Assert.True(" a1bjgfy8723bsdk71".IsAlphaNumericAscii(true, false));
			Assert.True("a1bjgfy8723bsdk71 ".IsAlphaNumericAscii(true, false));
			Assert.True(" a1bjgfy8723bsdk71 ".IsAlphaNumericAscii(true, false));
			Assert.False("\ta".IsAlphaNumericAscii(true, false));
			Assert.False("\t1".IsAlphaNumericAscii(true, false));
			Assert.False("\ta1".IsAlphaNumericAscii(true, false));
			Assert.False("a\t".IsAlphaNumericAscii(true, false));
			Assert.False("1\t".IsAlphaNumericAscii(true, false));
			Assert.False("a1\t".IsAlphaNumericAscii(true, false));
			Assert.False("\ta1bjgfy8723bsdk71".IsAlphaNumericAscii(true, false));
			Assert.False("a1bjgfy8723bsdk71\t".IsAlphaNumericAscii(true, false));
			Assert.False("\ta1bjgfy8723bsdk71\t".IsAlphaNumericAscii(true, false));

			// Ensure starts with alpha
			Assert.True("a".IsAlphaNumericAscii(true, true));
			Assert.False("1".IsAlphaNumericAscii(true, true));
			Assert.True("a1".IsAlphaNumericAscii(true, true));
			Assert.False(" a".IsAlphaNumericAscii(true, true));
			Assert.False(" 1".IsAlphaNumericAscii(true, true));
			Assert.False(" a1".IsAlphaNumericAscii(true, true));
			Assert.False("1a1bjgfy8723bsdk7".IsAlphaNumericAscii(true, true));
			Assert.True("a1bjgfy8723bsdk71".IsAlphaNumericAscii(true, true));
		}


		[Test]
		public static void ReplaceBetween()
		{
			var list = new List<KeyValue<string, string>>
			{
				new KeyValue<string, string>("OXFORD", "NOT BROGUES"),
				new KeyValue<string, string>("INNER", "Start <tag>OXFORD</tag> End"),
			};
			TestValue_ReplaceBetween(
				"Some text <tag>OXFORD</tag> and some more.", "<tag>", "</tag>", list, true,
				"Some text <tag>NOT BROGUES</tag> and some more.");
			TestValue_ReplaceBetween(
				"Some text <tag>OXFORD</tag> and some more.", "<tag>", "</tag>", list, false,
				"Some text NOT BROGUES and some more.");
			TestValue_ReplaceBetween(
				"Some text <tag>NON-EXISTING-KEY</tag> and some more.", "<tag>", "</tag>", list, true,
				"Some text <tag>NON-EXISTING-KEY</tag> and some more.");
			TestValue_ReplaceBetween(
				"Some text <tag>NON-EXISTING-KEY</tag> and some more.", "<tag>", "</tag>", list, false,
				"Some text <tag>NON-EXISTING-KEY</tag> and some more.");

			// Multiple
			TestValue_ReplaceBetween(
				"Some text <tag>OXFORD</tag> with <tag>OXFORD</tag> and some more.", "<tag>", "</tag>", list, true,
				"Some text <tag>NOT BROGUES</tag> with <tag>OXFORD</tag> and some more.");

			// Inner
			TestValue_ReplaceBetween(
				"Some text <tag>INNER</tag> and some more.", "<tag>", "</tag>", list, true,
				"Some text <tag>Start <tag>OXFORD</tag> End</tag> and some more.");
		}

		[Test]
		public static void ReplaceBetweenAll()
		{
			var list = new List<KeyValue<string, string>>
			{
				new KeyValue<string, string>("OXFORD", "NOT BROGUES"),
				new KeyValue<string, string>("INNER", "Start <tag>OXFORD</tag> End"),
			};
			TestValue_ReplaceBetweenAll(
				"Some text <tag>OXFORD</tag> and some more.", "<tag>", "</tag>", list, true, true,
				"Some text <tag>NOT BROGUES</tag> and some more.");
			TestValue_ReplaceBetweenAll(
				"Some text <tag>OXFORD</tag> and some more.", "<tag>", "</tag>", list, false, true,
				"Some text NOT BROGUES and some more.");
			TestValue_ReplaceBetweenAll(
				"Some text <tag>NON-EXISTING-KEY</tag> and some more.", "<tag>", "</tag>", list, true, true,
				"Some text <tag>NON-EXISTING-KEY</tag> and some more.");
			TestValue_ReplaceBetweenAll(
				"Some text <tag>NON-EXISTING-KEY</tag> and some more.", "<tag>", "</tag>", list, false, true,
				"Some text <tag>NON-EXISTING-KEY</tag> and some more.");

			// Multiple
			TestValue_ReplaceBetweenAll(
				"Some text <tag>OXFORD</tag> with <tag>OXFORD</tag> and some more.", "<tag>", "</tag>", list, true, true,
				"Some text <tag>NOT BROGUES</tag> with <tag>NOT BROGUES</tag> and some more.");

			// Inner (single)
			TestValue_ReplaceBetweenAll(
				"Some text <tag>INNER</tag> and some more.", "<tag>", "</tag>", list, true, true,
				"Some text <tag>Start <tag>OXFORD</tag> End</tag> and some more.");
			TestValue_ReplaceBetweenAll(
				"Some text <tag>INNER</tag> and some more.", "<tag>", "</tag>", list, false, true,
				"Some text Start <tag>OXFORD</tag> End and some more.");
			TestValue_ReplaceBetweenAll(
				"Some text <tag>INNER</tag> and some more.", "<tag>", "</tag>", list, true, false,
				"Some text <tag>Start <tag>NOT BROGUES</tag> End</tag> and some more.");
			TestValue_ReplaceBetweenAll(
				"Some text <tag>INNER</tag> and some more.", "<tag>", "</tag>", list, false, false,
				"Some text Start NOT BROGUES End and some more.");

			// Inner (multiple)
			TestValue_ReplaceBetweenAll(
				"Some text <tag>INNER</tag> with <tag>INNER</tag> and some more.", "<tag>", "</tag>", list, true, true,
				"Some text <tag>Start <tag>OXFORD</tag> End</tag> with <tag>Start <tag>OXFORD</tag> End</tag> and some more.");
			TestValue_ReplaceBetweenAll(
				"Some text <tag>INNER</tag> with <tag>INNER</tag> and some more.", "<tag>", "</tag>", list, false, true,
				"Some text Start <tag>OXFORD</tag> End with Start <tag>OXFORD</tag> End and some more.");
			TestValue_ReplaceBetweenAll(
				"Some text <tag>INNER</tag> with <tag>INNER</tag> and some more.", "<tag>", "</tag>", list, true, false,
				"Some text <tag>Start <tag>NOT BROGUES</tag> End</tag> with <tag>Start <tag>NOT BROGUES</tag> End</tag> and some more.");
			TestValue_ReplaceBetweenAll(
				"Some text <tag>INNER</tag> with <tag>INNER</tag> and some more.", "<tag>", "</tag>", list, false, false,
				"Some text Start NOT BROGUES End with Start NOT BROGUES End and some more.");
		}

		private static void TestValue_ReplaceBetween(string text, string startTag, string endTag, List<KeyValue<string, string>> list, bool keepTags, string expectedResult)
		{
			var result = text.ReplaceBetween(startTag, endTag,
				key =>
				{
					for (int i = 0; i < list.Count; i++)
					{
						if (list[i].Key == key)
							return list[i].Value;
					}
					return null;
				},
				keepTags
			);
			Assert.AreEqual(expectedResult, result);
		}

		private static void TestValue_ReplaceBetweenAll(string text, string startTag, string endTag, List<KeyValue<string, string>> list, bool keepTags, bool skipTagsInReplacedText, string expectedResult)
		{
			text.ReplaceBetweenAll(startTag, endTag,
				key =>
				{
					for (int i = 0; i < list.Count; i++)
					{
						if (list[i].Key == key)
							return list[i].Value;
					}
					return null;
				},
				keepTags,
				skipTagsInReplacedText,
				out var result
			);
			Assert.AreEqual(expectedResult, result);
		}

		#endregion

		#region String Operations - Replace Selective

		[Test]
		public static void ReplaceSelective()
		{
			Test_ReplaceBetween(". . .", ". . .", ImmutableStack<bool>.CreateInverse(false, false, false));
			Test_ReplaceBetween(". . .", "X . .", ImmutableStack<bool>.CreateInverse(true, false, false));
			Test_ReplaceBetween(". . .", ". X .", ImmutableStack<bool>.CreateInverse(false, true, false));
			Test_ReplaceBetween(". . .", ". . X", ImmutableStack<bool>.CreateInverse(false, false, true));
			Test_ReplaceBetween(". . .", "X . X", ImmutableStack<bool>.CreateInverse(true, false, true));
			Test_ReplaceBetween(". . .", "X X X", ImmutableStack<bool>.CreateInverse(true, true, true));

			Test_ReplaceBetween(". . .", "X . .", ImmutableStack<bool>.CreateInverse(true));
		}

		private static void Test_ReplaceBetween(string input, string expected, ImmutableStack<bool> selection)
		{
			var result = input.ReplaceSelective(".", "X", selection);
			Assert.AreEqual(expected, result);
		}

		#endregion

		#region Number At The End

		[Test]
		public static void RemoveEndingNumberedParentheses()
		{
			Assert.AreEqual("Asd", "Asd".RemoveEndingNumberedParentheses('(', ')', true));
			Assert.AreEqual("Asd", "Asd".RemoveEndingNumberedParentheses('(', ')', false));
			Assert.AreEqual("Asd ", "Asd ".RemoveEndingNumberedParentheses('(', ')', true));
			Assert.AreEqual("Asd ", "Asd ".RemoveEndingNumberedParentheses('(', ')', false));
			Assert.AreEqual("Asd", "Asd ()".RemoveEndingNumberedParentheses('(', ')', true));
			Assert.AreEqual("Asd", "Asd (3)".RemoveEndingNumberedParentheses('(', ')', true));
			Assert.AreEqual("Asd", "Asd (12)".RemoveEndingNumberedParentheses('(', ')', true));
			Assert.AreEqual("Asd()", "Asd()".RemoveEndingNumberedParentheses('(', ')', true));
			Assert.AreEqual("Asd(3)", "Asd(3)".RemoveEndingNumberedParentheses('(', ')', true));
			Assert.AreEqual("Asd(12)", "Asd(12)".RemoveEndingNumberedParentheses('(', ')', true));
			Assert.AreEqual("Asd ", "Asd ()".RemoveEndingNumberedParentheses('(', ')', false));
			Assert.AreEqual("Asd ", "Asd (3)".RemoveEndingNumberedParentheses('(', ')', false));
			Assert.AreEqual("Asd ", "Asd (12)".RemoveEndingNumberedParentheses('(', ')', false));
			Assert.AreEqual("Asd", "Asd()".RemoveEndingNumberedParentheses('(', ')', false));
			Assert.AreEqual("Asd", "Asd(3)".RemoveEndingNumberedParentheses('(', ')', false));
			Assert.AreEqual("Asd", "Asd(12)".RemoveEndingNumberedParentheses('(', ')', false));
		}

		[Test]
		public static void IsEndingWithNumberedParentheses()
		{
			Assert.AreEqual(false, "Asd".IsEndingWithNumberedParentheses('(', ')', true));
			Assert.AreEqual(false, "Asd".IsEndingWithNumberedParentheses('(', ')', false));
			Assert.AreEqual(false, "Asd ".IsEndingWithNumberedParentheses('(', ')', true));
			Assert.AreEqual(false, "Asd ".IsEndingWithNumberedParentheses('(', ')', false));
			Assert.AreEqual(true, "Asd ()".IsEndingWithNumberedParentheses('(', ')', true));
			Assert.AreEqual(true, "Asd (3)".IsEndingWithNumberedParentheses('(', ')', true));
			Assert.AreEqual(true, "Asd (12)".IsEndingWithNumberedParentheses('(', ')', true));
			Assert.AreEqual(false, "Asd()".IsEndingWithNumberedParentheses('(', ')', true));
			Assert.AreEqual(false, "Asd(3)".IsEndingWithNumberedParentheses('(', ')', true));
			Assert.AreEqual(false, "Asd(12)".IsEndingWithNumberedParentheses('(', ')', true));
			Assert.AreEqual(true, "Asd ()".IsEndingWithNumberedParentheses('(', ')', false));
			Assert.AreEqual(true, "Asd (3)".IsEndingWithNumberedParentheses('(', ')', false));
			Assert.AreEqual(true, "Asd (12)".IsEndingWithNumberedParentheses('(', ')', false));
			Assert.AreEqual(true, "Asd()".IsEndingWithNumberedParentheses('(', ')', false));
			Assert.AreEqual(true, "Asd(3)".IsEndingWithNumberedParentheses('(', ')', false));
			Assert.AreEqual(true, "Asd(12)".IsEndingWithNumberedParentheses('(', ')', false));
		}

		#endregion

		#region Normalize Line Endings

		[Test]
		public void IsLineEndingNormalizationNeededCRLF()
		{
			TestIsLineEndingNormalizationNeededCRLF(null, false);
			TestIsLineEndingNormalizationNeededCRLF("", false);
			TestIsLineEndingNormalizationNeededCRLF(" ", false);
			TestIsLineEndingNormalizationNeededCRLF("\t", false);

			TestIsLineEndingNormalizationNeededCRLF("\r\n", false);
			TestIsLineEndingNormalizationNeededCRLF(" \r\n", false);
			TestIsLineEndingNormalizationNeededCRLF("\r\n ", false);
			TestIsLineEndingNormalizationNeededCRLF(" \r\n ", false);

			TestIsLineEndingNormalizationNeededCRLF("\r", true);
			TestIsLineEndingNormalizationNeededCRLF(" \r", true);
			TestIsLineEndingNormalizationNeededCRLF("\r ", true);
			TestIsLineEndingNormalizationNeededCRLF(" \r ", true);

			TestIsLineEndingNormalizationNeededCRLF("\n", true);
			TestIsLineEndingNormalizationNeededCRLF(" \n", true);
			TestIsLineEndingNormalizationNeededCRLF("\n ", true);
			TestIsLineEndingNormalizationNeededCRLF(" \n ", true);

			TestIsLineEndingNormalizationNeededCRLF("\r\r\n", true);
			TestIsLineEndingNormalizationNeededCRLF(" \r\r\n", true);
			TestIsLineEndingNormalizationNeededCRLF("\r\r\n ", true);
			TestIsLineEndingNormalizationNeededCRLF(" \r\r\n ", true);

			TestIsLineEndingNormalizationNeededCRLF("\n\r\n", true);
			TestIsLineEndingNormalizationNeededCRLF(" \n\r\n", true);
			TestIsLineEndingNormalizationNeededCRLF("\n\r\n ", true);
			TestIsLineEndingNormalizationNeededCRLF(" \n\r\n ", true);

			TestIsLineEndingNormalizationNeededCRLF("\r\n\r", true);
			TestIsLineEndingNormalizationNeededCRLF(" \r\n\r", true);
			TestIsLineEndingNormalizationNeededCRLF("\r\n\r ", true);
			TestIsLineEndingNormalizationNeededCRLF(" \r\n\r ", true);

			TestIsLineEndingNormalizationNeededCRLF("\r\n\n", true);
			TestIsLineEndingNormalizationNeededCRLF(" \r\n\n", true);
			TestIsLineEndingNormalizationNeededCRLF("\r\n\n ", true);
			TestIsLineEndingNormalizationNeededCRLF(" \r\n\n ", true);
		}

		[Test]
		public void NormalizeLineEndingsCRLF()
		{
			TestNormalizeLineEndingsCRLF(null, null);
			TestNormalizeLineEndingsCRLF("", "");
			TestNormalizeLineEndingsCRLF(" ", " ");
			TestNormalizeLineEndingsCRLF("\t", "\t");

			TestNormalizeLineEndingsCRLF("\r\n", "\r\n");
			TestNormalizeLineEndingsCRLF(" \r\n", " \r\n");
			TestNormalizeLineEndingsCRLF("\r\n ", "\r\n ");
			TestNormalizeLineEndingsCRLF(" \r\n ", " \r\n ");

			TestNormalizeLineEndingsCRLF("\r", "\r\n");
			TestNormalizeLineEndingsCRLF(" \r", " \r\n");
			TestNormalizeLineEndingsCRLF("\r ", "\r\n ");
			TestNormalizeLineEndingsCRLF(" \r ", " \r\n ");

			TestNormalizeLineEndingsCRLF("\n", "\r\n");
			TestNormalizeLineEndingsCRLF(" \n", " \r\n");
			TestNormalizeLineEndingsCRLF("\n ", "\r\n ");
			TestNormalizeLineEndingsCRLF(" \n ", " \r\n ");

			TestNormalizeLineEndingsCRLF("\r\r\n", "\r\n\r\n");
			TestNormalizeLineEndingsCRLF(" \r\r\n", " \r\n\r\n");
			TestNormalizeLineEndingsCRLF("\r\r\n ", "\r\n\r\n ");
			TestNormalizeLineEndingsCRLF(" \r\r\n ", " \r\n\r\n ");

			TestNormalizeLineEndingsCRLF("\n\r\n", "\r\n\r\n");
			TestNormalizeLineEndingsCRLF(" \n\r\n", " \r\n\r\n");
			TestNormalizeLineEndingsCRLF("\n\r\n ", "\r\n\r\n ");
			TestNormalizeLineEndingsCRLF(" \n\r\n ", " \r\n\r\n ");

			TestNormalizeLineEndingsCRLF("\r\n\r", "\r\n\r\n");
			TestNormalizeLineEndingsCRLF(" \r\n\r", " \r\n\r\n");
			TestNormalizeLineEndingsCRLF("\r\n\r ", "\r\n\r\n ");
			TestNormalizeLineEndingsCRLF(" \r\n\r ", " \r\n\r\n ");

			TestNormalizeLineEndingsCRLF("\r\n\n", "\r\n\r\n");
			TestNormalizeLineEndingsCRLF(" \r\n\n", " \r\n\r\n");
			TestNormalizeLineEndingsCRLF("\r\n\n ", "\r\n\r\n ");
			TestNormalizeLineEndingsCRLF(" \r\n\n ", " \r\n\r\n ");
		}

		private static void TestIsLineEndingNormalizationNeededCRLF(string input, bool expected)
		{
			var result = input.IsLineEndingNormalizationNeededCRLF();
			Assert.AreEqual(expected, result);
		}

		private void TestNormalizeLineEndingsCRLF(string input, string expected)
		{
			var result = input.NormalizeLineEndingsCRLF();
			Assert.AreEqual(expected, result);
		}

		#endregion

		#region Conversions - Int ToStringAsCharArray

		[Test]
		[Repeat(10)]
		public static void ToStringAsCharArray_Int32()
		{
			TestValue_ToStringAsCharArray_Int32(0);
			TestValue_ToStringAsCharArray_Int32(1);
			TestValue_ToStringAsCharArray_Int32(-1);
			for (Int32 value = -10000; value < 10000; value += Random.Range(1, 500))
			{
				TestValue_ToStringAsCharArray_Int32(value);
			}
			TestValue_ToStringAsCharArray_Int32(-99999);
			TestValue_ToStringAsCharArray_Int32(-999999);
			TestValue_ToStringAsCharArray_Int32(-9999999);
			TestValue_ToStringAsCharArray_Int32(-99999999);
			TestValue_ToStringAsCharArray_Int32(-999999999);
			TestValue_ToStringAsCharArray_Int32(-10000);
			TestValue_ToStringAsCharArray_Int32(-100000);
			TestValue_ToStringAsCharArray_Int32(-1000000);
			TestValue_ToStringAsCharArray_Int32(-10000000);
			TestValue_ToStringAsCharArray_Int32(-100000000);
			TestValue_ToStringAsCharArray_Int32(-20000);
			TestValue_ToStringAsCharArray_Int32(-200000);
			TestValue_ToStringAsCharArray_Int32(-2000000);
			TestValue_ToStringAsCharArray_Int32(-20000000);
			TestValue_ToStringAsCharArray_Int32(-200000000);
			TestValue_ToStringAsCharArray_Int32(-2000000000);
			TestValue_ToStringAsCharArray_Int32(10000);
			TestValue_ToStringAsCharArray_Int32(100000);
			TestValue_ToStringAsCharArray_Int32(1000000);
			TestValue_ToStringAsCharArray_Int32(10000000);
			TestValue_ToStringAsCharArray_Int32(100000000);
			TestValue_ToStringAsCharArray_Int32(1000000000);
			TestValue_ToStringAsCharArray_Int32(20000);
			TestValue_ToStringAsCharArray_Int32(200000);
			TestValue_ToStringAsCharArray_Int32(2000000);
			TestValue_ToStringAsCharArray_Int32(20000000);
			TestValue_ToStringAsCharArray_Int32(200000000);
			TestValue_ToStringAsCharArray_Int32(2000000000);
			TestValue_ToStringAsCharArray_Int32(99999);
			TestValue_ToStringAsCharArray_Int32(999999);
			TestValue_ToStringAsCharArray_Int32(9999999);
			TestValue_ToStringAsCharArray_Int32(99999999);
			TestValue_ToStringAsCharArray_Int32(999999999);
			TestValue_ToStringAsCharArray_Int32(123456789);
			TestValue_ToStringAsCharArray_Int32(987654321);
			TestValue_ToStringAsCharArray_Int32(Int32.MinValue);
			TestValue_ToStringAsCharArray_Int32(Int32.MinValue + 1);
			TestValue_ToStringAsCharArray_Int32(Int32.MinValue + 2);
			TestValue_ToStringAsCharArray_Int32(Int32.MaxValue);
			TestValue_ToStringAsCharArray_Int32(Int32.MaxValue - 1);
			TestValue_ToStringAsCharArray_Int32(Int32.MaxValue - 2);
		}

		[Test]
		[Repeat(10)]
		public static void ToStringAsCharArray_Int64()
		{
			TestValue_ToStringAsCharArray_Int64(0);
			TestValue_ToStringAsCharArray_Int64(1);
			TestValue_ToStringAsCharArray_Int64(-1);
			for (Int64 value = -10000; value < 10000; value += Random.Range(1, 500))
			{
				TestValue_ToStringAsCharArray_Int64(value);
			}
			TestValue_ToStringAsCharArray_Int64(-99999);
			TestValue_ToStringAsCharArray_Int64(-999999);
			TestValue_ToStringAsCharArray_Int64(-9999999);
			TestValue_ToStringAsCharArray_Int64(-99999999);
			TestValue_ToStringAsCharArray_Int64(-999999999);
			TestValue_ToStringAsCharArray_Int64(-9999999999);
			TestValue_ToStringAsCharArray_Int64(-99999999999);
			TestValue_ToStringAsCharArray_Int64(-999999999999);
			TestValue_ToStringAsCharArray_Int64(-9999999999999);
			TestValue_ToStringAsCharArray_Int64(-99999999999999);
			TestValue_ToStringAsCharArray_Int64(-999999999999999);
			TestValue_ToStringAsCharArray_Int64(-9999999999999999);
			TestValue_ToStringAsCharArray_Int64(-99999999999999999);
			TestValue_ToStringAsCharArray_Int64(-999999999999999999);
			TestValue_ToStringAsCharArray_Int64(-10000);
			TestValue_ToStringAsCharArray_Int64(-100000);
			TestValue_ToStringAsCharArray_Int64(-1000000);
			TestValue_ToStringAsCharArray_Int64(-10000000);
			TestValue_ToStringAsCharArray_Int64(-100000000);
			TestValue_ToStringAsCharArray_Int64(-1000000000);
			TestValue_ToStringAsCharArray_Int64(-10000000000);
			TestValue_ToStringAsCharArray_Int64(-100000000000);
			TestValue_ToStringAsCharArray_Int64(-1000000000000);
			TestValue_ToStringAsCharArray_Int64(-10000000000000);
			TestValue_ToStringAsCharArray_Int64(-100000000000000);
			TestValue_ToStringAsCharArray_Int64(-1000000000000000);
			TestValue_ToStringAsCharArray_Int64(-10000000000000000);
			TestValue_ToStringAsCharArray_Int64(-100000000000000000);
			TestValue_ToStringAsCharArray_Int64(-1000000000000000000);
			TestValue_ToStringAsCharArray_Int64(-20000);
			TestValue_ToStringAsCharArray_Int64(-200000);
			TestValue_ToStringAsCharArray_Int64(-2000000);
			TestValue_ToStringAsCharArray_Int64(-200000000);
			TestValue_ToStringAsCharArray_Int64(-2000000000);
			TestValue_ToStringAsCharArray_Int64(-20000000000);
			TestValue_ToStringAsCharArray_Int64(-200000000000);
			TestValue_ToStringAsCharArray_Int64(-2000000000000);
			TestValue_ToStringAsCharArray_Int64(-20000000000000);
			TestValue_ToStringAsCharArray_Int64(-200000000000000);
			TestValue_ToStringAsCharArray_Int64(-2000000000000000);
			TestValue_ToStringAsCharArray_Int64(-20000000000000000);
			TestValue_ToStringAsCharArray_Int64(-200000000000000000);
			TestValue_ToStringAsCharArray_Int64(-2000000000000000000);
			TestValue_ToStringAsCharArray_Int64(10000);
			TestValue_ToStringAsCharArray_Int64(100000);
			TestValue_ToStringAsCharArray_Int64(1000000);
			TestValue_ToStringAsCharArray_Int64(10000000);
			TestValue_ToStringAsCharArray_Int64(100000000);
			TestValue_ToStringAsCharArray_Int64(1000000000);
			TestValue_ToStringAsCharArray_Int64(10000000000);
			TestValue_ToStringAsCharArray_Int64(100000000000);
			TestValue_ToStringAsCharArray_Int64(1000000000000);
			TestValue_ToStringAsCharArray_Int64(10000000000000);
			TestValue_ToStringAsCharArray_Int64(100000000000000);
			TestValue_ToStringAsCharArray_Int64(1000000000000000);
			TestValue_ToStringAsCharArray_Int64(10000000000000000);
			TestValue_ToStringAsCharArray_Int64(100000000000000000);
			TestValue_ToStringAsCharArray_Int64(1000000000000000000);
			TestValue_ToStringAsCharArray_Int64(20000);
			TestValue_ToStringAsCharArray_Int64(200000);
			TestValue_ToStringAsCharArray_Int64(2000000);
			TestValue_ToStringAsCharArray_Int64(200000000);
			TestValue_ToStringAsCharArray_Int64(2000000000);
			TestValue_ToStringAsCharArray_Int64(20000000000);
			TestValue_ToStringAsCharArray_Int64(200000000000);
			TestValue_ToStringAsCharArray_Int64(2000000000000);
			TestValue_ToStringAsCharArray_Int64(20000000000000);
			TestValue_ToStringAsCharArray_Int64(200000000000000);
			TestValue_ToStringAsCharArray_Int64(2000000000000000);
			TestValue_ToStringAsCharArray_Int64(20000000000000000);
			TestValue_ToStringAsCharArray_Int64(200000000000000000);
			TestValue_ToStringAsCharArray_Int64(2000000000000000000);
			TestValue_ToStringAsCharArray_Int64(99999);
			TestValue_ToStringAsCharArray_Int64(999999);
			TestValue_ToStringAsCharArray_Int64(9999999);
			TestValue_ToStringAsCharArray_Int64(99999999);
			TestValue_ToStringAsCharArray_Int64(999999999);
			TestValue_ToStringAsCharArray_Int64(9999999999);
			TestValue_ToStringAsCharArray_Int64(99999999999);
			TestValue_ToStringAsCharArray_Int64(999999999999);
			TestValue_ToStringAsCharArray_Int64(9999999999999);
			TestValue_ToStringAsCharArray_Int64(99999999999999);
			TestValue_ToStringAsCharArray_Int64(999999999999999);
			TestValue_ToStringAsCharArray_Int64(9999999999999999);
			TestValue_ToStringAsCharArray_Int64(99999999999999999);
			TestValue_ToStringAsCharArray_Int64(999999999999999999);
			TestValue_ToStringAsCharArray_Int64(123456789);
			TestValue_ToStringAsCharArray_Int64(987654321);
			TestValue_ToStringAsCharArray_Int64(Int32.MinValue);
			TestValue_ToStringAsCharArray_Int64(Int32.MinValue + 1);
			TestValue_ToStringAsCharArray_Int64(Int32.MinValue + 2);
			TestValue_ToStringAsCharArray_Int64(Int32.MaxValue);
			TestValue_ToStringAsCharArray_Int64(Int32.MaxValue - 1);
			TestValue_ToStringAsCharArray_Int64(Int32.MaxValue - 2);
			TestValue_ToStringAsCharArray_Int64(Int64.MinValue);
			TestValue_ToStringAsCharArray_Int64(Int64.MinValue + 1);
			TestValue_ToStringAsCharArray_Int64(Int64.MinValue + 2);
			TestValue_ToStringAsCharArray_Int64(Int64.MaxValue);
			TestValue_ToStringAsCharArray_Int64(Int64.MaxValue - 1);
			TestValue_ToStringAsCharArray_Int64(Int64.MaxValue - 2);
		}

		[Test]
		[Repeat(10)]
		public static void ToStringAsCharArray_WithThousandsSeparator_Int32()
		{
			TestValue_ToStringAsCharArray_WithThousandsSeparator_Int32(0);
			TestValue_ToStringAsCharArray_WithThousandsSeparator_Int32(1);
			TestValue_ToStringAsCharArray_WithThousandsSeparator_Int32(-1);
			for (Int32 value = -10000; value < 10000; value += Random.Range(1, 500))
			{
				TestValue_ToStringAsCharArray_WithThousandsSeparator_Int32(value);
			}
			TestValue_ToStringAsCharArray_WithThousandsSeparator_Int32(-99999);
			TestValue_ToStringAsCharArray_WithThousandsSeparator_Int32(-999999);
			TestValue_ToStringAsCharArray_WithThousandsSeparator_Int32(-9999999);
			TestValue_ToStringAsCharArray_WithThousandsSeparator_Int32(-99999999);
			TestValue_ToStringAsCharArray_WithThousandsSeparator_Int32(-999999999);
			TestValue_ToStringAsCharArray_WithThousandsSeparator_Int32(-10000);
			TestValue_ToStringAsCharArray_WithThousandsSeparator_Int32(-100000);
			TestValue_ToStringAsCharArray_WithThousandsSeparator_Int32(-1000000);
			TestValue_ToStringAsCharArray_WithThousandsSeparator_Int32(-10000000);
			TestValue_ToStringAsCharArray_WithThousandsSeparator_Int32(-100000000);
			TestValue_ToStringAsCharArray_WithThousandsSeparator_Int32(-20000);
			TestValue_ToStringAsCharArray_WithThousandsSeparator_Int32(-200000);
			TestValue_ToStringAsCharArray_WithThousandsSeparator_Int32(-2000000);
			TestValue_ToStringAsCharArray_WithThousandsSeparator_Int32(-20000000);
			TestValue_ToStringAsCharArray_WithThousandsSeparator_Int32(-200000000);
			TestValue_ToStringAsCharArray_WithThousandsSeparator_Int32(-2000000000);
			TestValue_ToStringAsCharArray_WithThousandsSeparator_Int32(10000);
			TestValue_ToStringAsCharArray_WithThousandsSeparator_Int32(100000);
			TestValue_ToStringAsCharArray_WithThousandsSeparator_Int32(1000000);
			TestValue_ToStringAsCharArray_WithThousandsSeparator_Int32(10000000);
			TestValue_ToStringAsCharArray_WithThousandsSeparator_Int32(100000000);
			TestValue_ToStringAsCharArray_WithThousandsSeparator_Int32(1000000000);
			TestValue_ToStringAsCharArray_WithThousandsSeparator_Int32(20000);
			TestValue_ToStringAsCharArray_WithThousandsSeparator_Int32(200000);
			TestValue_ToStringAsCharArray_WithThousandsSeparator_Int32(2000000);
			TestValue_ToStringAsCharArray_WithThousandsSeparator_Int32(20000000);
			TestValue_ToStringAsCharArray_WithThousandsSeparator_Int32(200000000);
			TestValue_ToStringAsCharArray_WithThousandsSeparator_Int32(2000000000);
			TestValue_ToStringAsCharArray_WithThousandsSeparator_Int32(99999);
			TestValue_ToStringAsCharArray_WithThousandsSeparator_Int32(999999);
			TestValue_ToStringAsCharArray_WithThousandsSeparator_Int32(9999999);
			TestValue_ToStringAsCharArray_WithThousandsSeparator_Int32(99999999);
			TestValue_ToStringAsCharArray_WithThousandsSeparator_Int32(999999999);
			TestValue_ToStringAsCharArray_WithThousandsSeparator_Int32(123456789);
			TestValue_ToStringAsCharArray_WithThousandsSeparator_Int32(987654321);
			TestValue_ToStringAsCharArray_WithThousandsSeparator_Int32(Int32.MinValue);
			TestValue_ToStringAsCharArray_WithThousandsSeparator_Int32(Int32.MinValue + 1);
			TestValue_ToStringAsCharArray_WithThousandsSeparator_Int32(Int32.MinValue + 2);
			TestValue_ToStringAsCharArray_WithThousandsSeparator_Int32(Int32.MaxValue);
			TestValue_ToStringAsCharArray_WithThousandsSeparator_Int32(Int32.MaxValue - 1);
			TestValue_ToStringAsCharArray_WithThousandsSeparator_Int32(Int32.MaxValue - 2);
		}

		[Test]
		[Repeat(10)]
		public static void ToStringAsCharArray_WithThousandsSeparator_Int64()
		{
			TestValue_ToStringAsCharArray_WithThousandsSeparator_Int64(0);
			TestValue_ToStringAsCharArray_WithThousandsSeparator_Int64(1);
			TestValue_ToStringAsCharArray_WithThousandsSeparator_Int64(-1);
			for (Int64 value = -10000; value < 10000; value += Random.Range(1, 500))
			{
				TestValue_ToStringAsCharArray_WithThousandsSeparator_Int64(value);
			}
			TestValue_ToStringAsCharArray_WithThousandsSeparator_Int64(-99999);
			TestValue_ToStringAsCharArray_WithThousandsSeparator_Int64(-999999);
			TestValue_ToStringAsCharArray_WithThousandsSeparator_Int64(-9999999);
			TestValue_ToStringAsCharArray_WithThousandsSeparator_Int64(-99999999);
			TestValue_ToStringAsCharArray_WithThousandsSeparator_Int64(-999999999);
			TestValue_ToStringAsCharArray_WithThousandsSeparator_Int64(-9999999999);
			TestValue_ToStringAsCharArray_WithThousandsSeparator_Int64(-99999999999);
			TestValue_ToStringAsCharArray_WithThousandsSeparator_Int64(-999999999999);
			TestValue_ToStringAsCharArray_WithThousandsSeparator_Int64(-9999999999999);
			TestValue_ToStringAsCharArray_WithThousandsSeparator_Int64(-99999999999999);
			TestValue_ToStringAsCharArray_WithThousandsSeparator_Int64(-999999999999999);
			TestValue_ToStringAsCharArray_WithThousandsSeparator_Int64(-9999999999999999);
			TestValue_ToStringAsCharArray_WithThousandsSeparator_Int64(-99999999999999999);
			TestValue_ToStringAsCharArray_WithThousandsSeparator_Int64(-999999999999999999);
			TestValue_ToStringAsCharArray_WithThousandsSeparator_Int64(-10000);
			TestValue_ToStringAsCharArray_WithThousandsSeparator_Int64(-100000);
			TestValue_ToStringAsCharArray_WithThousandsSeparator_Int64(-1000000);
			TestValue_ToStringAsCharArray_WithThousandsSeparator_Int64(-10000000);
			TestValue_ToStringAsCharArray_WithThousandsSeparator_Int64(-100000000);
			TestValue_ToStringAsCharArray_WithThousandsSeparator_Int64(-1000000000);
			TestValue_ToStringAsCharArray_WithThousandsSeparator_Int64(-10000000000);
			TestValue_ToStringAsCharArray_WithThousandsSeparator_Int64(-100000000000);
			TestValue_ToStringAsCharArray_WithThousandsSeparator_Int64(-1000000000000);
			TestValue_ToStringAsCharArray_WithThousandsSeparator_Int64(-10000000000000);
			TestValue_ToStringAsCharArray_WithThousandsSeparator_Int64(-100000000000000);
			TestValue_ToStringAsCharArray_WithThousandsSeparator_Int64(-1000000000000000);
			TestValue_ToStringAsCharArray_WithThousandsSeparator_Int64(-10000000000000000);
			TestValue_ToStringAsCharArray_WithThousandsSeparator_Int64(-100000000000000000);
			TestValue_ToStringAsCharArray_WithThousandsSeparator_Int64(-1000000000000000000);
			TestValue_ToStringAsCharArray_WithThousandsSeparator_Int64(-20000);
			TestValue_ToStringAsCharArray_WithThousandsSeparator_Int64(-200000);
			TestValue_ToStringAsCharArray_WithThousandsSeparator_Int64(-2000000);
			TestValue_ToStringAsCharArray_WithThousandsSeparator_Int64(-200000000);
			TestValue_ToStringAsCharArray_WithThousandsSeparator_Int64(-2000000000);
			TestValue_ToStringAsCharArray_WithThousandsSeparator_Int64(-20000000000);
			TestValue_ToStringAsCharArray_WithThousandsSeparator_Int64(-200000000000);
			TestValue_ToStringAsCharArray_WithThousandsSeparator_Int64(-2000000000000);
			TestValue_ToStringAsCharArray_WithThousandsSeparator_Int64(-20000000000000);
			TestValue_ToStringAsCharArray_WithThousandsSeparator_Int64(-200000000000000);
			TestValue_ToStringAsCharArray_WithThousandsSeparator_Int64(-2000000000000000);
			TestValue_ToStringAsCharArray_WithThousandsSeparator_Int64(-20000000000000000);
			TestValue_ToStringAsCharArray_WithThousandsSeparator_Int64(-200000000000000000);
			TestValue_ToStringAsCharArray_WithThousandsSeparator_Int64(-2000000000000000000);
			TestValue_ToStringAsCharArray_WithThousandsSeparator_Int64(10000);
			TestValue_ToStringAsCharArray_WithThousandsSeparator_Int64(100000);
			TestValue_ToStringAsCharArray_WithThousandsSeparator_Int64(1000000);
			TestValue_ToStringAsCharArray_WithThousandsSeparator_Int64(10000000);
			TestValue_ToStringAsCharArray_WithThousandsSeparator_Int64(100000000);
			TestValue_ToStringAsCharArray_WithThousandsSeparator_Int64(1000000000);
			TestValue_ToStringAsCharArray_WithThousandsSeparator_Int64(10000000000);
			TestValue_ToStringAsCharArray_WithThousandsSeparator_Int64(100000000000);
			TestValue_ToStringAsCharArray_WithThousandsSeparator_Int64(1000000000000);
			TestValue_ToStringAsCharArray_WithThousandsSeparator_Int64(10000000000000);
			TestValue_ToStringAsCharArray_WithThousandsSeparator_Int64(100000000000000);
			TestValue_ToStringAsCharArray_WithThousandsSeparator_Int64(1000000000000000);
			TestValue_ToStringAsCharArray_WithThousandsSeparator_Int64(10000000000000000);
			TestValue_ToStringAsCharArray_WithThousandsSeparator_Int64(100000000000000000);
			TestValue_ToStringAsCharArray_WithThousandsSeparator_Int64(1000000000000000000);
			TestValue_ToStringAsCharArray_WithThousandsSeparator_Int64(20000);
			TestValue_ToStringAsCharArray_WithThousandsSeparator_Int64(200000);
			TestValue_ToStringAsCharArray_WithThousandsSeparator_Int64(2000000);
			TestValue_ToStringAsCharArray_WithThousandsSeparator_Int64(200000000);
			TestValue_ToStringAsCharArray_WithThousandsSeparator_Int64(2000000000);
			TestValue_ToStringAsCharArray_WithThousandsSeparator_Int64(20000000000);
			TestValue_ToStringAsCharArray_WithThousandsSeparator_Int64(200000000000);
			TestValue_ToStringAsCharArray_WithThousandsSeparator_Int64(2000000000000);
			TestValue_ToStringAsCharArray_WithThousandsSeparator_Int64(20000000000000);
			TestValue_ToStringAsCharArray_WithThousandsSeparator_Int64(200000000000000);
			TestValue_ToStringAsCharArray_WithThousandsSeparator_Int64(2000000000000000);
			TestValue_ToStringAsCharArray_WithThousandsSeparator_Int64(20000000000000000);
			TestValue_ToStringAsCharArray_WithThousandsSeparator_Int64(200000000000000000);
			TestValue_ToStringAsCharArray_WithThousandsSeparator_Int64(2000000000000000000);
			TestValue_ToStringAsCharArray_WithThousandsSeparator_Int64(99999);
			TestValue_ToStringAsCharArray_WithThousandsSeparator_Int64(999999);
			TestValue_ToStringAsCharArray_WithThousandsSeparator_Int64(9999999);
			TestValue_ToStringAsCharArray_WithThousandsSeparator_Int64(99999999);
			TestValue_ToStringAsCharArray_WithThousandsSeparator_Int64(999999999);
			TestValue_ToStringAsCharArray_WithThousandsSeparator_Int64(9999999999);
			TestValue_ToStringAsCharArray_WithThousandsSeparator_Int64(99999999999);
			TestValue_ToStringAsCharArray_WithThousandsSeparator_Int64(999999999999);
			TestValue_ToStringAsCharArray_WithThousandsSeparator_Int64(9999999999999);
			TestValue_ToStringAsCharArray_WithThousandsSeparator_Int64(99999999999999);
			TestValue_ToStringAsCharArray_WithThousandsSeparator_Int64(999999999999999);
			TestValue_ToStringAsCharArray_WithThousandsSeparator_Int64(9999999999999999);
			TestValue_ToStringAsCharArray_WithThousandsSeparator_Int64(99999999999999999);
			TestValue_ToStringAsCharArray_WithThousandsSeparator_Int64(999999999999999999);
			TestValue_ToStringAsCharArray_WithThousandsSeparator_Int64(123456789);
			TestValue_ToStringAsCharArray_WithThousandsSeparator_Int64(987654321);
			TestValue_ToStringAsCharArray_WithThousandsSeparator_Int64(Int32.MinValue);
			TestValue_ToStringAsCharArray_WithThousandsSeparator_Int64(Int32.MinValue + 1);
			TestValue_ToStringAsCharArray_WithThousandsSeparator_Int64(Int32.MinValue + 2);
			TestValue_ToStringAsCharArray_WithThousandsSeparator_Int64(Int32.MaxValue);
			TestValue_ToStringAsCharArray_WithThousandsSeparator_Int64(Int32.MaxValue - 1);
			TestValue_ToStringAsCharArray_WithThousandsSeparator_Int64(Int32.MaxValue - 2);
			TestValue_ToStringAsCharArray_WithThousandsSeparator_Int64(Int64.MinValue);
			TestValue_ToStringAsCharArray_WithThousandsSeparator_Int64(Int64.MinValue + 1);
			TestValue_ToStringAsCharArray_WithThousandsSeparator_Int64(Int64.MinValue + 2);
			TestValue_ToStringAsCharArray_WithThousandsSeparator_Int64(Int64.MaxValue);
			TestValue_ToStringAsCharArray_WithThousandsSeparator_Int64(Int64.MaxValue - 1);
			TestValue_ToStringAsCharArray_WithThousandsSeparator_Int64(Int64.MaxValue - 2);
		}

		private static void TestValue_ToStringAsCharArray_Int32(Int32 value)
		{
			var chars = new char[10 + 1];
			UnityTestTools.BeginMemoryCheck();
			value.ToStringAsCharArray(chars, out var startIndex, out var length);
			if (UnityTestTools.EndMemoryCheck())
				Assert.Fail("Memory allocated while converting value '" + value + "' to string resulting '" + chars.ConvertToString(0, length) + "'.");
			var original = value.ToString();
			Assert.AreEqual(original, chars.ConvertToString(startIndex, length));
			Assert.AreEqual(original.Length, length);
		}

		private static void TestValue_ToStringAsCharArray_Int64(Int64 value)
		{
			var chars = new char[19 + 1];
			UnityTestTools.BeginMemoryCheck();
			value.ToStringAsCharArray(chars, out var startIndex, out var length);
			if (UnityTestTools.EndMemoryCheck())
				Assert.Fail("Memory allocated while converting value '" + value + "' to string resulting '" + chars.ConvertToString(0, length) + "'.");
			var original = value.ToString();
			Assert.AreEqual(original, chars.ConvertToString(startIndex, length));
			Assert.AreEqual(original.Length, length);
		}

		private static void TestValue_ToStringAsCharArray_WithThousandsSeparator_Int32(Int32 value)
		{
			var chars = new char[10 + 1 + 3];
			UnityTestTools.BeginMemoryCheck();
			value.ToStringAsCharArray(chars, ',', out var startIndex, out var length);
			if (UnityTestTools.EndMemoryCheck())
				Assert.Fail("Memory allocated while converting value '" + value + "' to string resulting '" + chars.ConvertToString(0, length) + "'.");
			var original = value.ToString("N0");
			Assert.AreEqual(original, chars.ConvertToString(startIndex, length));
			Assert.AreEqual(original.Length, length);
		}

		private static void TestValue_ToStringAsCharArray_WithThousandsSeparator_Int64(Int64 value)
		{
			var chars = new char[19 + 1 + 6];
			UnityTestTools.BeginMemoryCheck();
			value.ToStringAsCharArray(chars, ',', out var startIndex, out var length);
			if (UnityTestTools.EndMemoryCheck())
				Assert.Fail("Memory allocated while converting value '" + value + "' to string resulting '" + chars.ConvertToString(0, length) + "'.");
			var original = value.ToString("N0");
			Assert.AreEqual(original, chars.ConvertToString(startIndex, length));
			Assert.AreEqual(original.Length, length);
		}

		#endregion

		#region Conversions - Formatted Value ToStringAsCharArray

		[Test]
		[Repeat(10)]
		public static void ToStringAsCharArray_FormattedInt()
		{
			WarmUpFastNumberFormatter();

			TestValue_ToStringAsCharArray_FormattedInt(0);
			TestValue_ToStringAsCharArray_FormattedInt(1);
			TestValue_ToStringAsCharArray_FormattedInt(-1);
			for (Int64 value = -10000; value < 10000; value += Random.Range(1, 500))
			{
				TestValue_ToStringAsCharArray_FormattedInt(value);
			}
			TestValue_ToStringAsCharArray_FormattedInt(-99999);
			TestValue_ToStringAsCharArray_FormattedInt(-999999);
			TestValue_ToStringAsCharArray_FormattedInt(-9999999);
			TestValue_ToStringAsCharArray_FormattedInt(-99999999);
			TestValue_ToStringAsCharArray_FormattedInt(-999999999);
			TestValue_ToStringAsCharArray_FormattedInt(-9999999999);
			TestValue_ToStringAsCharArray_FormattedInt(-99999999999);
			TestValue_ToStringAsCharArray_FormattedInt(-999999999999);
			TestValue_ToStringAsCharArray_FormattedInt(-9999999999999);
			TestValue_ToStringAsCharArray_FormattedInt(-99999999999999);
			TestValue_ToStringAsCharArray_FormattedInt(-999999999999999);
			TestValue_ToStringAsCharArray_FormattedInt(-9999999999999999);
			TestValue_ToStringAsCharArray_FormattedInt(-99999999999999999);
			TestValue_ToStringAsCharArray_FormattedInt(-999999999999999999);
			TestValue_ToStringAsCharArray_FormattedInt(-10000);
			TestValue_ToStringAsCharArray_FormattedInt(-100000);
			TestValue_ToStringAsCharArray_FormattedInt(-1000000);
			TestValue_ToStringAsCharArray_FormattedInt(-10000000);
			TestValue_ToStringAsCharArray_FormattedInt(-100000000);
			TestValue_ToStringAsCharArray_FormattedInt(-1000000000);
			TestValue_ToStringAsCharArray_FormattedInt(-10000000000);
			TestValue_ToStringAsCharArray_FormattedInt(-100000000000);
			TestValue_ToStringAsCharArray_FormattedInt(-1000000000000);
			TestValue_ToStringAsCharArray_FormattedInt(-10000000000000);
			TestValue_ToStringAsCharArray_FormattedInt(-100000000000000);
			TestValue_ToStringAsCharArray_FormattedInt(-1000000000000000);
			TestValue_ToStringAsCharArray_FormattedInt(-10000000000000000);
			TestValue_ToStringAsCharArray_FormattedInt(-100000000000000000);
			TestValue_ToStringAsCharArray_FormattedInt(-1000000000000000000);
			TestValue_ToStringAsCharArray_FormattedInt(-20000);
			TestValue_ToStringAsCharArray_FormattedInt(-200000);
			TestValue_ToStringAsCharArray_FormattedInt(-2000000);
			TestValue_ToStringAsCharArray_FormattedInt(-200000000);
			TestValue_ToStringAsCharArray_FormattedInt(-2000000000);
			TestValue_ToStringAsCharArray_FormattedInt(-20000000000);
			TestValue_ToStringAsCharArray_FormattedInt(-200000000000);
			TestValue_ToStringAsCharArray_FormattedInt(-2000000000000);
			TestValue_ToStringAsCharArray_FormattedInt(-20000000000000);
			TestValue_ToStringAsCharArray_FormattedInt(-200000000000000);
			TestValue_ToStringAsCharArray_FormattedInt(-2000000000000000);
			TestValue_ToStringAsCharArray_FormattedInt(-20000000000000000);
			TestValue_ToStringAsCharArray_FormattedInt(-200000000000000000);
			TestValue_ToStringAsCharArray_FormattedInt(-2000000000000000000);
			TestValue_ToStringAsCharArray_FormattedInt(10000);
			TestValue_ToStringAsCharArray_FormattedInt(100000);
			TestValue_ToStringAsCharArray_FormattedInt(1000000);
			TestValue_ToStringAsCharArray_FormattedInt(10000000);
			TestValue_ToStringAsCharArray_FormattedInt(100000000);
			TestValue_ToStringAsCharArray_FormattedInt(1000000000);
			TestValue_ToStringAsCharArray_FormattedInt(10000000000);
			TestValue_ToStringAsCharArray_FormattedInt(100000000000);
			TestValue_ToStringAsCharArray_FormattedInt(1000000000000);
			TestValue_ToStringAsCharArray_FormattedInt(10000000000000);
			TestValue_ToStringAsCharArray_FormattedInt(100000000000000);
			TestValue_ToStringAsCharArray_FormattedInt(1000000000000000);
			TestValue_ToStringAsCharArray_FormattedInt(10000000000000000);
			TestValue_ToStringAsCharArray_FormattedInt(100000000000000000);
			TestValue_ToStringAsCharArray_FormattedInt(1000000000000000000);
			TestValue_ToStringAsCharArray_FormattedInt(20000);
			TestValue_ToStringAsCharArray_FormattedInt(200000);
			TestValue_ToStringAsCharArray_FormattedInt(2000000);
			TestValue_ToStringAsCharArray_FormattedInt(200000000);
			TestValue_ToStringAsCharArray_FormattedInt(2000000000);
			TestValue_ToStringAsCharArray_FormattedInt(20000000000);
			TestValue_ToStringAsCharArray_FormattedInt(200000000000);
			TestValue_ToStringAsCharArray_FormattedInt(2000000000000);
			TestValue_ToStringAsCharArray_FormattedInt(20000000000000);
			TestValue_ToStringAsCharArray_FormattedInt(200000000000000);
			TestValue_ToStringAsCharArray_FormattedInt(2000000000000000);
			TestValue_ToStringAsCharArray_FormattedInt(20000000000000000);
			TestValue_ToStringAsCharArray_FormattedInt(200000000000000000);
			TestValue_ToStringAsCharArray_FormattedInt(2000000000000000000);
			TestValue_ToStringAsCharArray_FormattedInt(99999);
			TestValue_ToStringAsCharArray_FormattedInt(999999);
			TestValue_ToStringAsCharArray_FormattedInt(9999999);
			TestValue_ToStringAsCharArray_FormattedInt(99999999);
			TestValue_ToStringAsCharArray_FormattedInt(999999999);
			TestValue_ToStringAsCharArray_FormattedInt(9999999999);
			TestValue_ToStringAsCharArray_FormattedInt(99999999999);
			TestValue_ToStringAsCharArray_FormattedInt(999999999999);
			TestValue_ToStringAsCharArray_FormattedInt(9999999999999);
			TestValue_ToStringAsCharArray_FormattedInt(99999999999999);
			TestValue_ToStringAsCharArray_FormattedInt(999999999999999);
			TestValue_ToStringAsCharArray_FormattedInt(9999999999999999);
			TestValue_ToStringAsCharArray_FormattedInt(99999999999999999);
			TestValue_ToStringAsCharArray_FormattedInt(999999999999999999);
			TestValue_ToStringAsCharArray_FormattedInt(123456789);
			TestValue_ToStringAsCharArray_FormattedInt(987654321);
			TestValue_ToStringAsCharArray_FormattedInt(Int32.MinValue);
			TestValue_ToStringAsCharArray_FormattedInt(Int32.MinValue + 1);
			TestValue_ToStringAsCharArray_FormattedInt(Int32.MinValue + 2);
			TestValue_ToStringAsCharArray_FormattedInt(Int32.MaxValue);
			TestValue_ToStringAsCharArray_FormattedInt(Int32.MaxValue - 1);
			TestValue_ToStringAsCharArray_FormattedInt(Int32.MaxValue - 2);
			TestValue_ToStringAsCharArray_FormattedInt(Int64.MinValue);
			TestValue_ToStringAsCharArray_FormattedInt(Int64.MinValue + 1);
			TestValue_ToStringAsCharArray_FormattedInt(Int64.MinValue + 2);
			TestValue_ToStringAsCharArray_FormattedInt(Int64.MaxValue);
			TestValue_ToStringAsCharArray_FormattedInt(Int64.MaxValue - 1);
			TestValue_ToStringAsCharArray_FormattedInt(Int64.MaxValue - 2);
		}

		[Test]
		[Repeat(10)]
		public static void ToStringAsCharArray_FormattedDouble()
		{
			WarmUpFastNumberFormatter();

			TestValue_ToStringAsCharArray_FormattedDouble(0);
			TestValue_ToStringAsCharArray_FormattedDouble(1);
			TestValue_ToStringAsCharArray_FormattedDouble(-1);
			for (double value = -10000d; value < 10000d; value += Random.Range(0.1f, 500.0f))
			{
				TestValue_ToStringAsCharArray_FormattedDouble(value);
			}
			TestValue_ToStringAsCharArray_FormattedDouble(-99999);
			TestValue_ToStringAsCharArray_FormattedDouble(-999999);
			TestValue_ToStringAsCharArray_FormattedDouble(-9999999);
			TestValue_ToStringAsCharArray_FormattedDouble(-99999999);
			TestValue_ToStringAsCharArray_FormattedDouble(-999999999);
			TestValue_ToStringAsCharArray_FormattedDouble(-9999999999);
			TestValue_ToStringAsCharArray_FormattedDouble(-99999999999);
			TestValue_ToStringAsCharArray_FormattedDouble(-999999999999);
			TestValue_ToStringAsCharArray_FormattedDouble(-9999999999999);
			TestValue_ToStringAsCharArray_FormattedDouble(-99999999999999);
			TestValue_ToStringAsCharArray_FormattedDouble(-999999999999999);
			TestValue_ToStringAsCharArray_FormattedDouble(-9999999999999999);
			TestValue_ToStringAsCharArray_FormattedDouble(-99999999999999999);
			TestValue_ToStringAsCharArray_FormattedDouble(-999999999999999999);
			TestValue_ToStringAsCharArray_FormattedDouble(-10000);
			TestValue_ToStringAsCharArray_FormattedDouble(-100000);
			TestValue_ToStringAsCharArray_FormattedDouble(-1000000);
			TestValue_ToStringAsCharArray_FormattedDouble(-10000000);
			TestValue_ToStringAsCharArray_FormattedDouble(-100000000);
			TestValue_ToStringAsCharArray_FormattedDouble(-1000000000);
			TestValue_ToStringAsCharArray_FormattedDouble(-10000000000);
			TestValue_ToStringAsCharArray_FormattedDouble(-100000000000);
			TestValue_ToStringAsCharArray_FormattedDouble(-1000000000000);
			TestValue_ToStringAsCharArray_FormattedDouble(-10000000000000);
			TestValue_ToStringAsCharArray_FormattedDouble(-100000000000000);
			TestValue_ToStringAsCharArray_FormattedDouble(-1000000000000000);
			TestValue_ToStringAsCharArray_FormattedDouble(-10000000000000000);
			TestValue_ToStringAsCharArray_FormattedDouble(-100000000000000000);
			TestValue_ToStringAsCharArray_FormattedDouble(-1000000000000000000);
			TestValue_ToStringAsCharArray_FormattedDouble(-20000);
			TestValue_ToStringAsCharArray_FormattedDouble(-200000);
			TestValue_ToStringAsCharArray_FormattedDouble(-2000000);
			TestValue_ToStringAsCharArray_FormattedDouble(-200000000);
			TestValue_ToStringAsCharArray_FormattedDouble(-2000000000);
			TestValue_ToStringAsCharArray_FormattedDouble(-20000000000);
			TestValue_ToStringAsCharArray_FormattedDouble(-200000000000);
			TestValue_ToStringAsCharArray_FormattedDouble(-2000000000000);
			TestValue_ToStringAsCharArray_FormattedDouble(-20000000000000);
			TestValue_ToStringAsCharArray_FormattedDouble(-200000000000000);
			TestValue_ToStringAsCharArray_FormattedDouble(-2000000000000000);
			TestValue_ToStringAsCharArray_FormattedDouble(-20000000000000000);
			TestValue_ToStringAsCharArray_FormattedDouble(-200000000000000000);
			TestValue_ToStringAsCharArray_FormattedDouble(-2000000000000000000);
			TestValue_ToStringAsCharArray_FormattedDouble(10000);
			TestValue_ToStringAsCharArray_FormattedDouble(100000);
			TestValue_ToStringAsCharArray_FormattedDouble(1000000);
			TestValue_ToStringAsCharArray_FormattedDouble(10000000);
			TestValue_ToStringAsCharArray_FormattedDouble(100000000);
			TestValue_ToStringAsCharArray_FormattedDouble(1000000000);
			TestValue_ToStringAsCharArray_FormattedDouble(10000000000);
			TestValue_ToStringAsCharArray_FormattedDouble(100000000000);
			TestValue_ToStringAsCharArray_FormattedDouble(1000000000000);
			TestValue_ToStringAsCharArray_FormattedDouble(10000000000000);
			TestValue_ToStringAsCharArray_FormattedDouble(100000000000000);
			TestValue_ToStringAsCharArray_FormattedDouble(1000000000000000);
			TestValue_ToStringAsCharArray_FormattedDouble(10000000000000000);
			TestValue_ToStringAsCharArray_FormattedDouble(100000000000000000);
			TestValue_ToStringAsCharArray_FormattedDouble(1000000000000000000);
			TestValue_ToStringAsCharArray_FormattedDouble(20000);
			TestValue_ToStringAsCharArray_FormattedDouble(200000);
			TestValue_ToStringAsCharArray_FormattedDouble(2000000);
			TestValue_ToStringAsCharArray_FormattedDouble(200000000);
			TestValue_ToStringAsCharArray_FormattedDouble(2000000000);
			TestValue_ToStringAsCharArray_FormattedDouble(20000000000);
			TestValue_ToStringAsCharArray_FormattedDouble(200000000000);
			TestValue_ToStringAsCharArray_FormattedDouble(2000000000000);
			TestValue_ToStringAsCharArray_FormattedDouble(20000000000000);
			TestValue_ToStringAsCharArray_FormattedDouble(200000000000000);
			TestValue_ToStringAsCharArray_FormattedDouble(2000000000000000);
			TestValue_ToStringAsCharArray_FormattedDouble(20000000000000000);
			TestValue_ToStringAsCharArray_FormattedDouble(200000000000000000);
			TestValue_ToStringAsCharArray_FormattedDouble(2000000000000000000);
			TestValue_ToStringAsCharArray_FormattedDouble(99999);
			TestValue_ToStringAsCharArray_FormattedDouble(999999);
			TestValue_ToStringAsCharArray_FormattedDouble(9999999);
			TestValue_ToStringAsCharArray_FormattedDouble(99999999);
			TestValue_ToStringAsCharArray_FormattedDouble(999999999);
			TestValue_ToStringAsCharArray_FormattedDouble(9999999999);
			TestValue_ToStringAsCharArray_FormattedDouble(99999999999);
			TestValue_ToStringAsCharArray_FormattedDouble(999999999999);
			TestValue_ToStringAsCharArray_FormattedDouble(9999999999999);
			TestValue_ToStringAsCharArray_FormattedDouble(99999999999999);
			TestValue_ToStringAsCharArray_FormattedDouble(999999999999999);
			TestValue_ToStringAsCharArray_FormattedDouble(9999999999999999);
			TestValue_ToStringAsCharArray_FormattedDouble(99999999999999999);
			TestValue_ToStringAsCharArray_FormattedDouble(999999999999999999);
			TestValue_ToStringAsCharArray_FormattedDouble(123456789);
			TestValue_ToStringAsCharArray_FormattedDouble(987654321);
			TestValue_ToStringAsCharArray_FormattedDouble(float.MinValue);
			TestValue_ToStringAsCharArray_FormattedDouble(float.MinValue + 1);
			TestValue_ToStringAsCharArray_FormattedDouble(float.MinValue + 2);
			TestValue_ToStringAsCharArray_FormattedDouble(float.MaxValue);
			TestValue_ToStringAsCharArray_FormattedDouble(float.MaxValue - 1);
			TestValue_ToStringAsCharArray_FormattedDouble(float.MaxValue - 2);
			TestValue_ToStringAsCharArray_FormattedDouble(double.MinValue);
			TestValue_ToStringAsCharArray_FormattedDouble(double.MinValue + 1);
			TestValue_ToStringAsCharArray_FormattedDouble(double.MinValue + 2);
			TestValue_ToStringAsCharArray_FormattedDouble(double.MaxValue);
			TestValue_ToStringAsCharArray_FormattedDouble(double.MaxValue - 1);
			TestValue_ToStringAsCharArray_FormattedDouble(double.MaxValue - 2);
		}

		private static void TestValue_ToStringAsCharArray_FormattedInt(Int64 value)
		{
			TestValue_ToStringAsCharArray_FormattedInt("C", value);
			TestValue_ToStringAsCharArray_FormattedInt("D", value);
			TestValue_ToStringAsCharArray_FormattedInt("e", value);
			TestValue_ToStringAsCharArray_FormattedInt("E", value);
			TestValue_ToStringAsCharArray_FormattedInt("F", value);
			TestValue_ToStringAsCharArray_FormattedInt("G", value);
			TestValue_ToStringAsCharArray_FormattedInt("N", value);
			TestValue_ToStringAsCharArray_FormattedInt("P", value);
			TestValue_ToStringAsCharArray_FormattedInt("X", value);

			for (int i = 0; i <= 10; i++)
			{
				TestValue_ToStringAsCharArray_FormattedInt("C" + i, value);
				TestValue_ToStringAsCharArray_FormattedInt("D" + i, value);
				TestValue_ToStringAsCharArray_FormattedInt("e" + i, value);
				TestValue_ToStringAsCharArray_FormattedInt("E" + i, value);
				TestValue_ToStringAsCharArray_FormattedInt("F" + i, value);
				TestValue_ToStringAsCharArray_FormattedInt("G" + i, value);
				TestValue_ToStringAsCharArray_FormattedInt("N" + i, value);
				TestValue_ToStringAsCharArray_FormattedInt("P" + i, value);
				TestValue_ToStringAsCharArray_FormattedInt("X" + i, value);
			}
		}

		private static void TestValue_ToStringAsCharArray_FormattedDouble(double value)
		{
			TestValue_ToStringAsCharArray_FormattedDouble("C", value);
			TestValue_ToStringAsCharArray_FormattedDouble("e", value);
			TestValue_ToStringAsCharArray_FormattedDouble("E", value);
			TestValue_ToStringAsCharArray_FormattedDouble("F", value);
			TestValue_ToStringAsCharArray_FormattedDouble("G", value);
			TestValue_ToStringAsCharArray_FormattedDouble("N", value);
			TestValue_ToStringAsCharArray_FormattedDouble("P", value);
			// This one allocates memory. We probably won't be interested in using it ever, so leave it as it is.
			//TestValue_ToStringAsCharArray_FormattedDouble("R", value);

			for (int i = 0; i <= 10; i++)
			{
				TestValue_ToStringAsCharArray_FormattedDouble("C" + i, value);
				TestValue_ToStringAsCharArray_FormattedDouble("e" + i, value);
				TestValue_ToStringAsCharArray_FormattedDouble("E" + i, value);
				TestValue_ToStringAsCharArray_FormattedDouble("F" + i, value);
				TestValue_ToStringAsCharArray_FormattedDouble("G" + i, value);
				TestValue_ToStringAsCharArray_FormattedDouble("N" + i, value);
				TestValue_ToStringAsCharArray_FormattedDouble("P" + i, value);
			}
		}

		private static void WarmUpFastNumberFormatter()
		{
			lock (BigBuffer)
			{
				// Warm up internal buffers of FastNumberFormatter
				for (int iDecimal = 0; iDecimal < 15; iDecimal++)
				{
					float.MaxValue.ToStringAsCharArray("C" + iDecimal, BigBuffer);
					//float.MaxValue.ToStringAsCharArray("D" + iDecimal, BigBuffer); Integers only
					float.MaxValue.ToStringAsCharArray("e" + iDecimal, BigBuffer);
					float.MaxValue.ToStringAsCharArray("E" + iDecimal, BigBuffer);
					float.MaxValue.ToStringAsCharArray("F" + iDecimal, BigBuffer);
					float.MaxValue.ToStringAsCharArray("G" + iDecimal, BigBuffer);
					float.MaxValue.ToStringAsCharArray("N" + iDecimal, BigBuffer);
					float.MaxValue.ToStringAsCharArray("P" + iDecimal, BigBuffer);
					//float.MaxValue.ToStringAsCharArray("X" + iDecimal, BigBuffer); Integers only

					double.MaxValue.ToStringAsCharArray("C" + iDecimal, BigBuffer);
					//double.MaxValue.ToStringAsCharArray("D" + iDecimal, BigBuffer); Integers only
					double.MaxValue.ToStringAsCharArray("e" + iDecimal, BigBuffer);
					double.MaxValue.ToStringAsCharArray("E" + iDecimal, BigBuffer);
					double.MaxValue.ToStringAsCharArray("F" + iDecimal, BigBuffer);
					double.MaxValue.ToStringAsCharArray("G" + iDecimal, BigBuffer);
					double.MaxValue.ToStringAsCharArray("N" + iDecimal, BigBuffer);
					double.MaxValue.ToStringAsCharArray("P" + iDecimal, BigBuffer);
					//double.MaxValue.ToStringAsCharArray("X" + iDecimal, BigBuffer); Integers only

					Int32.MaxValue.ToStringAsCharArray("C" + iDecimal, BigBuffer);
					Int32.MaxValue.ToStringAsCharArray("D" + iDecimal, BigBuffer);
					Int32.MaxValue.ToStringAsCharArray("e" + iDecimal, BigBuffer);
					Int32.MaxValue.ToStringAsCharArray("E" + iDecimal, BigBuffer);
					Int32.MaxValue.ToStringAsCharArray("F" + iDecimal, BigBuffer);
					Int32.MaxValue.ToStringAsCharArray("G" + iDecimal, BigBuffer);
					Int32.MaxValue.ToStringAsCharArray("N" + iDecimal, BigBuffer);
					Int32.MaxValue.ToStringAsCharArray("P" + iDecimal, BigBuffer);
					Int32.MaxValue.ToStringAsCharArray("X" + iDecimal, BigBuffer);

					Int64.MaxValue.ToStringAsCharArray("C" + iDecimal, BigBuffer);
					Int64.MaxValue.ToStringAsCharArray("D" + iDecimal, BigBuffer);
					Int64.MaxValue.ToStringAsCharArray("e" + iDecimal, BigBuffer);
					Int64.MaxValue.ToStringAsCharArray("E" + iDecimal, BigBuffer);
					Int64.MaxValue.ToStringAsCharArray("F" + iDecimal, BigBuffer);
					Int64.MaxValue.ToStringAsCharArray("G" + iDecimal, BigBuffer);
					Int64.MaxValue.ToStringAsCharArray("N" + iDecimal, BigBuffer);
					Int64.MaxValue.ToStringAsCharArray("P" + iDecimal, BigBuffer);
					Int64.MaxValue.ToStringAsCharArray("X" + iDecimal, BigBuffer);
				}
			}
		}

		private static void TestValue_ToStringAsCharArray_FormattedInt(string format, Int64 value)
		{
			TestValue_ToStringAsCharArray_FormattedValue(format, value, (_value, _format) => _value.ToString(_format), (_value, _format, _chars) => _value.ToStringAsCharArray(_format, _chars));
			TestValue_ToStringAsCharArray_FormattedValue(format, (Int32)value, (_value, _format) => _value.ToString(_format), (_value, _format, _chars) => _value.ToStringAsCharArray(_format, _chars));
		}

		private static void TestValue_ToStringAsCharArray_FormattedDouble(string format, double value)
		{
			TestValue_ToStringAsCharArray_FormattedValue(format, value, (_value, _format) => _value.ToString(_format), (_value, _format, _chars) => _value.ToStringAsCharArray(_format, _chars));
			TestValue_ToStringAsCharArray_FormattedValue(format, (float)value, (_value, _format) => _value.ToString(_format), (_value, _format, _chars) => _value.ToStringAsCharArray(_format, _chars));
		}

		/// <summary>
		/// The trick here is that convertToExpectedString and convertToCharArray
		/// won't allocate any memory while providing the test functionality for
		/// various types of values. The allocation problems can easily be seen
		/// with ReSharper's 'Heap Allocations Viewer' plugin.
		///
		/// Though admittedly the lines where this method is called looks a bit silly.
		/// </summary>
		private static void TestValue_ToStringAsCharArray_FormattedValue<T>(string format, T value, Func<T, string, string> convertToExpectedString, Func<T, string, char[], int> convertToCharArray)
		{
			lock (BigBuffer)
			{
				var valueAsUnformattedString = value.ToString();
				BigBuffer.Clear();
				UnityTestTools.BeginMemoryCheck();
				var length = convertToCharArray(value, format, BigBuffer);
				if (UnityTestTools.EndMemoryCheck())
					Assert.Fail($"Memory allocated while converting value '{valueAsUnformattedString}' with format '{format}' to string resulting '{BigBuffer.ConvertToString(0, length)}'.");
				var resultString = BigBuffer.ConvertToString(0, length);
				var expectedString = convertToExpectedString(value, format);
				if (!expectedString.Equals(resultString))
				{
					Log.Error($"Erroneous value generated while converting value '{valueAsUnformattedString}' with format '{format}' to string resulting '{BigBuffer.ConvertToString(0, length)}'.");
				}
				Assert.AreEqual(expectedString, resultString);
				Assert.AreEqual(expectedString.Length, length);
			}
		}

		#endregion

		#region Conversions - Time

		[Test]
		public void ToStringMinutesSecondsMillisecondsFromSeconds()
		{
			Assert.AreEqual("0:00.000", (0d).ToStringMinutesSecondsMillisecondsFromSeconds());
			Assert.AreEqual("0:01.000", (1d).ToStringMinutesSecondsMillisecondsFromSeconds());
			Assert.AreEqual("0:02.000", (2d).ToStringMinutesSecondsMillisecondsFromSeconds());
			Assert.AreEqual("0:09.000", (9d).ToStringMinutesSecondsMillisecondsFromSeconds());
			Assert.AreEqual("0:10.000", (10d).ToStringMinutesSecondsMillisecondsFromSeconds());
			Assert.AreEqual("0:11.000", (11d).ToStringMinutesSecondsMillisecondsFromSeconds());
			Assert.AreEqual("0:59.000", (59d).ToStringMinutesSecondsMillisecondsFromSeconds());
			Assert.AreEqual("1:00.000", (60d).ToStringMinutesSecondsMillisecondsFromSeconds());
			Assert.AreEqual("1:01.000", (61d).ToStringMinutesSecondsMillisecondsFromSeconds());
			Assert.AreEqual("1:02.000", (62d).ToStringMinutesSecondsMillisecondsFromSeconds());
			Assert.AreEqual("1:59.000", (119d).ToStringMinutesSecondsMillisecondsFromSeconds());
			Assert.AreEqual("2:02.000", (122d).ToStringMinutesSecondsMillisecondsFromSeconds());
			Assert.AreEqual("9:59.000", (599d).ToStringMinutesSecondsMillisecondsFromSeconds());
			Assert.AreEqual("10:00.000", (600d).ToStringMinutesSecondsMillisecondsFromSeconds());
			Assert.AreEqual("11:00.000", (660d).ToStringMinutesSecondsMillisecondsFromSeconds());

			Assert.AreEqual("0:00.001", (0.001d).ToStringMinutesSecondsMillisecondsFromSeconds());
			Assert.AreEqual("0:00.001", (0.0014d).ToStringMinutesSecondsMillisecondsFromSeconds());
			Assert.AreEqual("0:00.002", (0.0016d).ToStringMinutesSecondsMillisecondsFromSeconds());
			Assert.AreEqual("0:00.999", (0.9994d).ToStringMinutesSecondsMillisecondsFromSeconds());
			Assert.AreEqual("0:01.000", (0.9996d).ToStringMinutesSecondsMillisecondsFromSeconds());
			Assert.AreEqual("0:01.001", (1.0014d).ToStringMinutesSecondsMillisecondsFromSeconds());
			Assert.AreEqual("0:01.002", (1.0016d).ToStringMinutesSecondsMillisecondsFromSeconds());
			Assert.AreEqual("0:10.001", (10.0014d).ToStringMinutesSecondsMillisecondsFromSeconds());
			Assert.AreEqual("0:10.002", (10.0016d).ToStringMinutesSecondsMillisecondsFromSeconds());
			Assert.AreEqual("0:59.001", (59.0014d).ToStringMinutesSecondsMillisecondsFromSeconds());
			Assert.AreEqual("0:59.002", (59.0016d).ToStringMinutesSecondsMillisecondsFromSeconds());
			Assert.AreEqual("0:59.999", (59.9994d).ToStringMinutesSecondsMillisecondsFromSeconds());
			Assert.AreEqual("1:00.000", (59.9996d).ToStringMinutesSecondsMillisecondsFromSeconds());
			Assert.AreEqual("1:59.999", (119.9994d).ToStringMinutesSecondsMillisecondsFromSeconds());
			Assert.AreEqual("2:00.000", (119.9996d).ToStringMinutesSecondsMillisecondsFromSeconds());
		}

		[Test]
		public void ToStringMillisecondsFromSeconds()
		{
			Assert.AreEqual("0.000", (0d).ToStringMillisecondsFromSeconds());
			Assert.AreEqual("1000.000", (1d).ToStringMillisecondsFromSeconds());
			Assert.AreEqual("2000.000", (2d).ToStringMillisecondsFromSeconds());
			Assert.AreEqual("9000.000", (9d).ToStringMillisecondsFromSeconds());
			Assert.AreEqual("10000.000", (10d).ToStringMillisecondsFromSeconds());
			Assert.AreEqual("11000.000", (11d).ToStringMillisecondsFromSeconds());
			Assert.AreEqual("59000.000", (59d).ToStringMillisecondsFromSeconds());
			Assert.AreEqual("60000.000", (60d).ToStringMillisecondsFromSeconds());
			Assert.AreEqual("61000.000", (61d).ToStringMillisecondsFromSeconds());
			Assert.AreEqual("99000.000", (99d).ToStringMillisecondsFromSeconds());
			Assert.AreEqual("100000.000", (100d).ToStringMillisecondsFromSeconds());
			Assert.AreEqual("1000000.000", (1000d).ToStringMillisecondsFromSeconds());
			Assert.AreEqual("10000000.000", (10000d).ToStringMillisecondsFromSeconds());

			Assert.AreEqual("0.001", (0.000001d).ToStringMillisecondsFromSeconds());
			Assert.AreEqual("1.000", (0.001d).ToStringMillisecondsFromSeconds());
			Assert.AreEqual("1.001", (0.001001d).ToStringMillisecondsFromSeconds());
			Assert.AreEqual("1.001", (0.0010014d).ToStringMillisecondsFromSeconds());
			Assert.AreEqual("1.002", (0.0010016d).ToStringMillisecondsFromSeconds());
			Assert.AreEqual("1.999", (0.0019994d).ToStringMillisecondsFromSeconds());
			Assert.AreEqual("2.000", (0.0019996d).ToStringMillisecondsFromSeconds());
		}

		#endregion

		#region Hash

		[Test]
		public static void GetHashCodeGuaranteed()
		{
			var valueBag = new HashSet<string>(); // This will allow ignoring of checking the hashes for the same generated values.
			var history = new Dictionary<int, string>(100000); // This will be used for checking if the hash is generated before. Also it will be used for logging the previously generated hash value.

			// ReSharper disable once AssignNullToNotNullAttribute
			Assert.Throws<ArgumentNullException>(() => ((string)null).GetHashCodeGuaranteed());

			TestValue_GetHashCodeGuaranteed(valueBag, history, "", 757602046);
			TestValue_GetHashCodeGuaranteed(valueBag, history, " ", -842352768);
			TestValue_GetHashCodeGuaranteed(valueBag, history, "\t", -842352729);
			TestValue_GetHashCodeGuaranteed(valueBag, history, "a", -842352705);
			TestValue_GetHashCodeGuaranteed(valueBag, history, "ab", -840386625);
			TestValue_GetHashCodeGuaranteed(valueBag, history, "abc", 536991770);
			TestValue_GetHashCodeGuaranteed(valueBag, history, "1", -842352753);
			TestValue_GetHashCodeGuaranteed(valueBag, history, "a1", -843466817);
			TestValue_GetHashCodeGuaranteed(valueBag, history, "1a", -840321137);
			TestValue_GetHashCodeGuaranteed(valueBag, history, "aa", -840321089);
			TestValue_GetHashCodeGuaranteed(valueBag, history, "aaa", -625742108);
			TestValue_GetHashCodeGuaranteed(valueBag, history, "11", -843466865);
			TestValue_GetHashCodeGuaranteed(valueBag, history, "111", 1508494276);
			TestValue_GetHashCodeGuaranteed(valueBag, history, "12", -843532401);
			TestValue_GetHashCodeGuaranteed(valueBag, history, "123", -1623739142);
			TestValue_GetHashCodeGuaranteed(valueBag, history, "bvuYGfg823tbn181", -2132762692);

			/*
			// Just add some random values and expect them to not collide.
			// Of course it will collide randomly. So this is just for fun experimentation.
			// Seems like the collision will be rare below 5.000 items.
			// Above that, we start to see collisions.
			// Above 100.000, we rarely see the test succeeds without collision.
			UnityRandomTools.RandomizeGenerator();
			var buffer = new char[10];
			for (int i = 0; i < 100000; i++)
			{
				UnityRandomTools.FillRandomly(buffer);
				var value = buffer.ConvertToString(0, Random.Range(1, 10));
				if (!TestValue_GetHashCodeGuaranteed(valueBag, history, value))
					i--;
			}
			*/
		}

		//private static void CreateLog(System.Text.StringBuilder builder, string value)
		//{
		//	var hash = value.GetHashCodeGuaranteed();
		//	builder.AppendLine($@"TestValue_GetHashCodeGuaranteed(history, ""{value}"", {hash});");
		//}

		private static bool TestValue_GetHashCodeGuaranteed(HashSet<string> valueBag, Dictionary<int, string> history, string value)
		{
			if (!valueBag.Add(value))
				return false;

			var hash = value.GetHashCodeGuaranteed();

			// Just make sure there will be no collision with our basic dataset.
			// If it's not the case, something is really going sideways.
			if (history.ContainsKey(hash))
				throw new Exception($"Collision detected at iteration '{history.Count}' between values '{history[hash]}' and '{value}'");
			history.Add(hash, value);

			return true;
		}

		private static bool TestValue_GetHashCodeGuaranteed(HashSet<string> valueBag, Dictionary<int, string> history, string value, int expectedHash)
		{
			if (!valueBag.Add(value))
				return false;

			var hash = value.GetHashCodeGuaranteed();

			// Just make sure there will be no collision with our basic dataset.
			// If it's not the case, something is really going sideways.
			if (history.ContainsKey(hash))
				throw new Exception($"Collision detected at iteration '{history.Count}' between values '{history[hash]}' and '{value}'");
			history.Add(hash, value);

			Assert.AreEqual(expectedHash, hash);

			return true;
		}

		#endregion

		#region Smart Format

		[Test]
		public void SmartFormat()
		{
			TestSmartFormat(null, "", false, false, false, -1, -1);
			TestSmartFormat("", "", false, false, false, -1, -1);
			TestSmartFormat("asd", "asd", false, false, false, -1, -1);

			// trim
			TestSmartFormat(" ", " ", false, false, false, -1, -1);
			TestSmartFormat(" ", "", true, false, false, -1, -1);
			TestSmartFormat("   ", "", true, false, false, -1, -1);
			TestSmartFormat("   \n   \r\n   ", "", true, false, false, -1, -1);
			TestSmartFormat(" asd ", " asd ", false, false, false, -1, -1);
			TestSmartFormat(" asd ", "asd", true, false, false, -1, -1);
			TestSmartFormat("\nasd\n", "\nasd\n", false, false, false, -1, -1);
			TestSmartFormat("\nasd\n", "asd", true, false, false, -1, -1);
			TestSmartFormat(
				"  \r\n \r\n\n  asd  \r\n \r\n\n  ",
				"  \r\n \r\n\n  asd  \r\n \r\n\n  ",
				false, false, false, -1, -1);
			TestSmartFormat(
				"  \r\n \r\n\n  asd  \r\n \r\n\n  ",
				"asd",
				true, false, false, -1, -1);

			// maxAllowedConsecutiveLineEndings
			TestSmartFormat(
				"LINE 1\n\n\n\nLINE 2",
				"LINE 1\n\nLINE 2",
				false, false, false, 2, -1);
			TestSmartFormat(
				"LINE 1\n\n\n\nLINE 2\n\n\nLINE 3\n\nLINE 4",
				"LINE 1\n\nLINE 2\n\nLINE 3\n\nLINE 4",
				false, false, false, 2, -1);

			// trimEndOfEachLine
			TestSmartFormat(
				"  LINE 1   \n \n  \n      \nLINE 2   ",
				"  LINE 1\n\n\n\nLINE 2",
				false, true, false, -1, -1);
			TestSmartFormat(
				"  LINE 1   \n  LINE 2   ",
				"  LINE 1\n  LINE 2",
				false, true, false, -1, -1);
			TestSmartFormat(
				"  LINE 1  ",
				"  LINE 1",
				false, true, false, -1, -1);

			// trimEndOfEachLine combined with maxAllowedConsecutiveLineEndings
			TestSmartFormat(
				"  LINE 1   \n \n  \n      \nLINE 2   ",
				"  LINE 1\n\nLINE 2",
				false, true, false, 2, -1);

			// !trimEndOfEachLine combined with maxAllowedConsecutiveLineEndings
			TestSmartFormat(
				"  LINE 1   \n \n  \n      \nLINE 2   ",
				"  LINE 1   \n \n  \n      \nLINE 2   ",
				false, false, false, 2, -1);
		}

		private static void TestSmartFormat(string input, string expected, bool trim, bool trimEndOfEachLine, bool normalizeLineEndings, int maxAllowedConsecutiveLineEndings, int maxLength)
		{
			//Log.Info("------------------------------------------ before   (A: input  B: expected)");
			//Log.Info("A: '" + input.ToHexStringFancy() + "\n" + "'" + input + "'");
			//Log.Info("B: '" + expected.ToHexStringFancy() + "\n" + "'" + expected + "'");
			StringTools.SmartFormat(ref input, trim, trimEndOfEachLine, normalizeLineEndings, maxAllowedConsecutiveLineEndings, maxLength);
			//Log.Info("--------------- after   (A: input  B: expected)");
			//Log.Info("A: " + input.ToHexStringFancy() + "\n" + "'" + input + "'");
			//Log.Info("B: " + expected.ToHexStringFancy() + "\n" + "'" + expected + "'");
			Assert.AreEqual(expected, input);
		}

		#endregion

		#region Tools

		private static char[] BigBuffer = new char[500];

		#endregion
	}

}
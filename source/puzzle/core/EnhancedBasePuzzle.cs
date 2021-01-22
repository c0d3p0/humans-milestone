using System.Text;

using Godot;
using Godot.Collections;


public abstract class EnhancedBasePuzzle : BasePuzzle
{
	protected StringBuilder Shuffle(string text)
	{
		StringBuilder sb = new StringBuilder(text.Trim());
		char[] chars = new char[text.Length];
		int sbIndex, charIndex = 0;

		while(sb.Length > 0)
		{
			sbIndex = this.RandiRange(rng, 0, sb.Length - 1);
			chars[charIndex++] = sb[sbIndex];
			sb.Remove(sbIndex, 1);
		}

		return new StringBuilder().Append(chars);
	}

	protected byte[] RandomizeNumbers(int amount, byte rngStart, byte rngEnd)
	{
		byte[] numbers = new byte[amount];

		for(byte i = 0; i < amount; i++)
			numbers[i] = (byte) this.RandiRange(rng, rngStart, rngEnd);

		return numbers;
	}

	protected char[] RandomizeFromText(StringBuilder text, int amount)
	{
		return RandomizeFromText(text, amount, 0, (byte) (text.Length - 1));
	}

	protected char[] RandomizeFromText(StringBuilder text, int amount, byte rngStart, byte rngEnd)
	{
		char[] chars = new char[amount];

		for(byte i = 0; i < amount; i++)
			chars[i] = text[this.RandiRange(rng, rngStart, rngEnd)];

		return chars;
	}

	protected char[] RandomizeNonRepeatingFromText(StringBuilder text, int amount)
	{
		return RandomizeNonRepeatingFromText(text, amount, 0, (byte) (text.Length - 1));
	}

	protected char[] RandomizeNonRepeatingFromText(StringBuilder text,
			int amount, byte rngStart, byte rngEnd)
	{
		char[] chars = new char[amount];
		StringBuilder available = new StringBuilder().Append(text);
		int random;

		for(byte i = 0; i < amount; i++)
		{
			random = this.RandiRange(rng, rngStart, rngEnd - i);
			chars[i] = available[random];
			available.Remove(random, 1);
		}

		return chars;
	}

	protected byte[] RandomizeNonRepeatingNumbers(int amount,
			byte rngStart, byte rngEnd)
	{
		byte[] numbers = new byte[amount];
		Dictionary<byte, object> numberSet = new Dictionary<byte, object>();
		int generated = 0;

		while(generated < amount)
		{
			numbers[generated] = (byte) this.RandiRange(rng, rngStart, rngEnd);

			if(!numberSet.ContainsKey(numbers[generated]))
				numberSet.Add(numbers[generated++], null);
		}

		return numbers;
	}

	protected Vector2[] RandomizeNonRepeatingPairs(int amount,
			byte rngStart, byte rngEnd)
	{
		Vector2[] pairs = new Vector2[amount];
		Dictionary<Vector2, object> pairSet = new Dictionary<Vector2, object>();
		int n1, n2;
		int generated = 0;

		while(generated < amount)
		{
			n1 = this.RandiRange(rng, rngStart, rngEnd);
			n2 = this.RandiRange(rng, rngStart, rngEnd);
			pairs[generated] = new Vector2(n1, n2);

			if(!pairSet.ContainsKey(pairs[generated]))
			{
				if(n1 != n2)
					pairSet.Add(new Vector2(n2, n1), null);

				pairSet.Add(pairs[generated], null);
				generated++;
			}
		}

		return pairs;
	}

	protected PuzzleInputPage CreateNumericPuzzleInputPage()
	{
		PuzzleInputPage page = new PuzzleInputPage();
		char c;

		for(byte i = 0; i < 9; i++)
		{
			c = ConvertToDigitChar(7 + (i % 3) - ((i / 3) * 3));
			page.AddPuzzleInput((byte) (SystemGUIID.BTN_KB_KEY0 + i), c);
		}

		page.AddPuzzleInput(SystemGUIID.BTN_KB_KEY10, '0');
		return page;
	}

	protected PuzzleInputPage CreateTextInputPage(StringBuilder text)
	{
		PuzzleInputPage page = new PuzzleInputPage();
		byte limit = text.Length < 13 ? (byte) text.Length : (byte) 12;

		for(byte i = 0; i < limit; i++)
			page.AddPuzzleInput((byte) (SystemGUIID.BTN_KB_KEY0 + i), text[i]);

		return page;
	}

	protected PuzzleInputPage CreateXFormatTextInputPage(StringBuilder text)
	{
		PuzzleInputPage page = new PuzzleInputPage();
		
		for(byte i = 0; i < 5; i++)
			page.AddPuzzleInput((byte) (SystemGUIID.BTN_KB_KEY0 + (i * 2)), text[i]);

		return page;
	}

	protected PuzzleInputPage CreateSquareFormatTextInputPage(StringBuilder text)
	{
		PuzzleInputPage page = new PuzzleInputPage();
		byte pageIndex = SystemGUIID.BTN_KB_KEY0;

		for(byte i = 0; i < text.Length; i++)
		{	
			page.AddPuzzleInput(pageIndex++, text[i]);

			if(pageIndex == SystemGUIID.BTN_KB_KEY4)
				pageIndex++;
		}

		return page;
	}

	protected string ReverseNumberString(int number)
	{
		StringBuilder result = new StringBuilder();
		string sum = number.ToString("D2");
		
		for(int i = sum.Length - 1; i > -1; i--)
			result.Append(sum[i]);

		return result.ToString();
	}

	protected T[] ReverseArray<T>(T[] array)
	{
		T[] reverseArray = new T[array.Length];
		int lastIndex = array.Length - 1;
		
		for(int i = lastIndex; i > -1; i--)
			reverseArray[i] = array[lastIndex - i];

		return reverseArray;
	}

	protected void CopyNumericArrayToCorrectAnswer(byte[] numbers)
	{
		correctAnswer = new char[numbers.Length];

		for(int i = 0; i < numbers.Length; i++)
			correctAnswer[i] = ConvertToDigitChar(numbers[i]);
	}

	protected override void InitializePuzzleInputsMap()
	{
		CreateNumericPuzzleInputsMap();
	}

	protected void CreateNumericPuzzleInputsMap()
	{
		puzzleInputPageMap.Add(0, CreateNumericPuzzleInputPage());
	}

	protected void CreateAlphabetPuzzleInputsMap()
	{
		puzzleInputPageMap.Add(0, CreateTextInputPage(new StringBuilder("ABCDEFGHIJKL")));
		puzzleInputPageMap.Add(1, CreateTextInputPage(new StringBuilder("MNOPQRSTUVWX")));
		puzzleInputPageMap.Add(2, CreateTextInputPage(new StringBuilder("YZ")));
	}

	protected void CreateXFormatSymbolsPuzzleInputsMap()
	{
		StringBuilder sb = GetSymbolCharacters();
		int pageAmount = sb.Length / 5;

		for(byte i = 0; i < pageAmount; i++)
		{
			puzzleInputPageMap.Add(i, CreateXFormatTextInputPage(
					new StringBuilder(sb.ToString(i * 5, 5))));
		}
	}

	protected void CreateSquareFormatHexPuzzleInputsMap()
	{
		StringBuilder sb = GetHexCharacters();
		int pageAmount = sb.Length / 8;

		for(byte i = 0; i < pageAmount; i++)
		{
			puzzleInputPageMap.Add(i, CreateSquareFormatTextInputPage(
					new StringBuilder(sb.ToString(i * 8, 8))));
		}
	}

	protected StringBuilder GetDecimalCharacters()
	{
		return new StringBuilder("0123456789");
	}

	protected StringBuilder GetHexCharacters()
	{
		return new StringBuilder("0123456789ABCDEF");
	}

	protected StringBuilder GetSymbolCharacters()
	{
		return new StringBuilder("§#$&@¢£¥µ€†‡Ððþ");
	}

	protected StringBuilder GetAlphabetCharacters()
	{
		return new StringBuilder("ABCDEFGHIJKLMNOPQRSTUVWXYZ");
	}
	
	protected char ConvertToDigitChar(int number)
	{
		return (char)('0' + number);
	}

	protected char ConvertToDigitChar(float number)
	{
		return (char)('0' + (int) (number));
	}

	// protected char ConvertToUpperLetter(int index)
	// {
	// 	return (char)('A' + (int) (index));
	// }

	// protected byte ConvertUpperLetterToIndex(char letter)
	// {
	// 	return (byte) (letter - 'A');
	// }

	protected int ConvertCharacterToIndex(char character, StringBuilder characters)
	{
		return characters.ToString().IndexOf(character);
	}
}

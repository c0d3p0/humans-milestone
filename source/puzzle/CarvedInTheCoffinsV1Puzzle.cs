using System.Text;

using Godot;


public class CarvedInTheCoffinsV1Puzzle : EnhancedBasePuzzle
{
	protected PuzzleContent CreatePuzzlePage(char passwordChar)
	{
		StringBuilder[] coffins = CreateCoffins(
				CreateBaseCoffinsContent(passwordChar), passwordChar);
		StringBuilder pageText = new StringBuilder();
		InsertCoffinLine1or3(pageText, coffins, 0, 0);
		InsertCoffinLine2(pageText, coffins, 0);
		InsertCoffinLine1or3(pageText, coffins, 0, 5);
		InsertCoffinLine4(pageText, coffins, 0);
		pageText.Append('\n');
		InsertCoffinLine4(pageText, coffins, 4);
		InsertCoffinLine1or3(pageText, coffins, 4, 5);
		InsertCoffinLine2(pageText, coffins, 4);
		InsertCoffinLine1or3(pageText, coffins, 4, 0);
		return new PuzzleContent(pageText.ToString().Trim());
	}
		
	protected StringBuilder[] CreateCoffins(StringBuilder content, char passwordChar)
	{
		StringBuilder[] coffins = new StringBuilder[COFFINS_AMOUNT];
		byte index = 0, randomIndex;

		for(int i = 0; i < coffins.Length; i++)
		{
			coffins[i] = new StringBuilder();

			while(coffins[i].Length < coffinBaseCharAmount)
				coffins[i].Append(content[index++]);

			if(coffinBaseCharAmount < COFFIN_LENGTH)
			{
				randomIndex = (byte) this.RandiRange(rng, 0, COFFIN_LENGTH - 1);
				coffins[i].Insert(randomIndex, passwordChar);
			}
		}

		return coffins;
	}

	protected StringBuilder CreateBaseCoffinsContent(char passwordChar)
	{
		StringBuilder baseContent = new StringBuilder();
		StringBuilder baseChar = new StringBuilder().Append(characters);
		baseChar.Remove(ConvertCharacterToIndex(passwordChar, characters), 1);
		byte removeAmount = randomAmount;
		StringBuilder randomChars;

		for(byte i = 0; i < CHAR_MIN_FREQUENCY; i++)
			baseContent.Append(Shuffle(baseChar.ToString()));

		while(removeAmount > 0)
		{
			randomChars = GetRandomCharacters(baseChar);
			removeAmount -= (byte) randomChars.Length;
			baseContent.Append(GetRandomCharacters(baseChar));
		}

		if(operation == APPEAR_IN_ONLY_ONE_COFFIN)
			baseContent.Insert(this.RandiRange(rng, 0, characters.Length - 1), passwordChar);

		return baseContent;
	}

	protected void InsertCoffinLine1or3(StringBuilder page,
			StringBuilder[] coffins, byte startIndex, byte charIndex)
	{
		byte limit = (byte) (startIndex + 4);

		for(byte i = startIndex; i < limit; i++)
		{
			page.Append(coffins[i][charIndex]);
			page.Append(coffins[i][charIndex + 1]);
			page.Append('\u0020').Append('\u0020');
		}

		page.Remove(page.Length - 2, 2).Append('\n');
	}

	protected void InsertCoffinLine2(StringBuilder page,
			StringBuilder[] coffins, byte startIndex)
	{
		byte limit = (byte) (startIndex + 4);

		for(byte i = startIndex; i < limit; i++)
		{
			page.Append(coffins[i][2]);
			page.Append(coffins[i][3]);
			page.Append(coffins[i][4]);
			page.Append('\u0020');
		}

		page.Remove(page.Length - 1, 1).Append('\n');
	}

	protected void InsertCoffinLine4(StringBuilder page,
			StringBuilder[] coffins, byte startIndex)
	{
		byte limit = (byte) (startIndex + 4);

		for(byte i = startIndex; i < limit; i++)
		{
			page.Append(coffins[i][7]);
			page.Append('\u0020');
			page.Append('\u0020');
			page.Append('\u0020');
		}

		page.Append('\n');
	}

	protected StringBuilder GetRandomCharacters(StringBuilder characters)
	{
		StringBuilder random = new StringBuilder().Append(characters);
		
		while(random.Length > randomAmount)
			random.Remove(this.RandiRange(rng, 0, random.Length - 1), 1); 

		return random;
	}

	protected byte GetRandomOperation()
	{
		return (byte) this.RandiRange(rng, REPEAT_IN_ALL_COFFINS,
				APPEAR_IN_ONLY_ONE_COFFIN);
	}

	protected void ClearAttributes()
	{
		characters.Clear();
		characters = null;
	}

	protected void InitializeAttributes()
	{
		characters = GetHexCharacters();
		correctAnswer = RandomizeFromText(characters, 4);
		operation = GetRandomOperation();
		randomAmount = TOTAL_FREQUENCY - BASE_CHAR_AMOUNT;
		coffinBaseCharAmount = COFFIN_LENGTH;

		if(operation ==	REPEAT_IN_ALL_COFFINS)
		{
			randomAmount -= COFFIN_LENGTH;
			coffinBaseCharAmount = COFFIN_LENGTH - 1;
		}
		else if(operation == APPEAR_IN_ONLY_ONE_COFFIN)
			randomAmount--;
	}

	protected override void InitializePuzzle()
	{
		byte index = 0;
		InitializeAttributes();
		puzzleContentMap.Add(index++,
				new PuzzleContent("Carved In The Coffins v1"));
		
		for(byte i = 0; i < correctAnswer.Length; i++)
			puzzleContentMap.Add(index++, CreatePuzzlePage(correctAnswer[i]));	

		puzzleContentMap.Add(index++, CreateExtraPage1());
		puzzleContentMap.Add(index++, CreateExtraPage2());
		puzzleStatusPageId = index;
	
		if(OS.IsDebugBuild())
			PrintPuzzleDebug();

		ClearAttributes();
	}

	protected override void InitializePuzzleInputsMap()
	{
		CreateSquareFormatHexPuzzleInputsMap();
	}

	protected PuzzleContent CreateExtraPage1()
	{
		StringBuilder sb = new StringBuilder();
		sb.Append("Beings are born, beings die. Some might think very few ");
		sb.Append("beings do things that will be remembered as a legacy.");
		return new PuzzleContent(sb.ToString());
	}

	protected PuzzleContent CreateExtraPage2()
	{
		StringBuilder sb = new StringBuilder();
		sb.Append("But everyone leaves behind traces of their existence. ");
		sb.Append("Sometimes there are connections, sometimes there aren't.");
		return new PuzzleContent(sb.ToString());
	}

	protected void PrintPuzzleDebug()
	{
		GD.PushWarning("========== Puzzle Debug ==============");
		GD.PushWarning("");
		GD.PushWarning("Puzzle: Carved In The Coffins V1");
		GD.PushWarning("Operation: " + operation);
		GD.PushWarning("");
		int index;

		for(byte i = 1; i <= correctAnswer.Length; i++)
		{
			byte[] frequency = new byte[characters.Length];
			string pageText = puzzleContentMap[i].text;
			GD.PushWarning("Page: " + i);
			
			for(byte j = 0; j < pageText.Length; j++)
			{
				index = ConvertCharacterToIndex(pageText[j], characters);

				if(index > -1)
					frequency[index]++;
			}

			for(byte j = 0; j < frequency.Length; j++)
				GD.PushWarning(characters[j] + ": " + frequency[j]);

			GD.PushWarning("");
		}

		GD.PushWarning("CorrectAnswer: " + new string(correctAnswer));
		GD.PushWarning("");
		GD.PushWarning("========== Puzzle Debug ==============");
		GD.PushWarning("");
	}


	protected StringBuilder characters;
	protected byte operation;
	protected byte coffinBaseCharAmount;
	protected byte randomAmount;

	protected const byte REPEAT_IN_ALL_COFFINS = 0;
	protected const byte DONT_APPEAR_IN_ANY_COFFIN = 1;
	protected const byte APPEAR_IN_ONLY_ONE_COFFIN = 2;

	protected const byte TOTAL_FREQUENCY = 64;
	protected const byte CHARS_LENGTH = 16;
	protected const byte COFFIN_LENGTH = 8;
	protected const byte COFFINS_AMOUNT = 8;
	protected const byte BASE_CHAR_AMOUNT = CHAR_MIN_FREQUENCY * (CHARS_LENGTH - 1);
	protected const byte CHAR_MIN_FREQUENCY =
			((TOTAL_FREQUENCY - COFFIN_LENGTH) / (CHARS_LENGTH - 1));
}



/*
	Puzzle Idea

	1: 4 hex digits from 0 to F will be randomly generated to represent the
		 4 digits password.

	2: Each of the 4 digits will result in one text page. Each page will
	   help to find one character of the password.

	3: The text page will contain 8 coffins made of digits. the hint to
	   find the character of the password present in the coffins will be
		 defined by 3 operations.

	4: The 3 operations will be: the password character will appear in all
	   coffins; the password character will never appear in any coffin; the
		 password character will appear only once in only one random coffin.
	
	5: 1 operation will be randomly selected.
*/

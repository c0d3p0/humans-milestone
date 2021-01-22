using System.Text;

using Godot;


public class NotSoHiddenV1Puzzle : EnhancedBasePuzzle
{	
	protected byte[] CreateDigitFrequency(byte answerDigit)
	{
		byte[] frequency = new byte[10];
		byte digitsLeft = (byte) (DIGIT_AMOUNT - answerDigit);
		byte randomDigit;
		frequency[answerDigit] = answerDigit;

		if(answerDigit != 1)
		{
			frequency[1] = 2;
			digitsLeft -= 2;
		}

		for(byte i = 0; i < frequency.Length; i++)
		{
			if(i != answerDigit)
				IncreaseFrequency(frequency, i, ref digitsLeft);
		}

		while(digitsLeft > 0)
		{
			randomDigit = (byte) (this.RandiRange(rng, 0, 9));

			if(randomDigit != answerDigit)
				IncreaseFrequency(frequency, randomDigit, ref digitsLeft);
		}

		return frequency;
	}

	protected void IncreaseFrequency(byte[] frequency, byte digit, ref byte digitsLeft)
	{
		byte max = MAX_FREQUENCY < digitsLeft ? MAX_FREQUENCY : digitsLeft;
		byte currentAmount = (byte) (this.RandiRange(rng, 1, max));
		frequency[digit] += currentAmount;
		digitsLeft -= currentAmount;

		if(frequency[digit] == digit)
		{
			frequency[digit]--;
			digitsLeft++;
		}
	}

	protected PuzzleContent CreateTextPage(byte answerDigit)
	{
		byte[] df = CreateDigitFrequency(answerDigit);
		StringBuilder sb = new StringBuilder();
		byte charsLeft = DIGIT_AMOUNT, index = 0, lineLeft = LINE_LENGTH, remove;

		while(charsLeft > 0)
		{
			remove = df[index] <= lineLeft ? df[index] : lineLeft;
			sb.Append(ConvertToDigitChar(index), remove);
			df[index] -= remove;
			lineLeft -= remove;
			charsLeft -= remove;

			if(df[index] == 0)
				index++;

			if(lineLeft == 0)
				lineLeft = LINE_LENGTH;
		}

		sb = Shuffle(sb.ToString().Trim());

		for(int i = 0; i < LINE_AMOUNT - 1; i++) // Insert LINE_AMOUNT - 1 line breaks
			sb.Insert((LINE_LENGTH * (i + 1)) + i, '\n');

		return new PuzzleContent(sb.ToString().Trim());
	}

	protected void ClearAttributes()
	{
		characters.Clear();
		characters = null;
		answer = null;
	}

	protected void InitializeAttributes()
	{
		characters = GetDecimalCharacters();
		answer = RandomizeNumbers(4, 0, 9);
	}

	protected override void InitializePuzzle()
	{
		byte index = 0;
		InitializeAttributes();
		CopyNumericArrayToCorrectAnswer(answer);
		puzzleContentMap.Add(index++, new PuzzleContent("Not So Hidden v1"));

		for(byte i = 0; i < answer.Length; i++)
			puzzleContentMap.Add(index++, CreateTextPage(answer[i]));

		puzzleContentMap.Add(index++, CreateExtraPage());
		puzzleStatusPageId = index;

		if(OS.IsDebugBuild())
			PrintPuzzleDebug();

		ClearAttributes();
	}

	protected PuzzleContent CreateExtraPage()
	{
		StringBuilder sb = new StringBuilder();
		sb.Append("Keep in mind that the biggest secrets are those ");
		sb.Append("hidding in plain sight.");
		return new PuzzleContent(sb.ToString());
	}

	protected void PrintPuzzleDebug()
	{
		GD.PushWarning("========== Puzzle Debug ==============");
		GD.PushWarning("");
		GD.PushWarning("Puzzle: Not So Hidden V1");
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
	protected byte[] answer;

	protected const byte DIGIT_AMOUNT = 96;
	protected const byte MAX_FREQUENCY = 10;
	protected const byte LINE_LENGTH = 12;
	protected const byte LINE_AMOUNT = 8;
}


/*
	Puzzle Idea


	1: 4 numbers from 0 to 9 will be randomly generated to represent the 4
	   digits password.

	2: Each of the 4 numbers will result in one text page. Each page will
	   help to find one digit of the password.

	3: The text page will contain 96 characters, all of them being a digit
	   from 0 to 9.
		 
	4: The hint to find the password digit int the page is counting the
	   frequency of each digit, if the frequency of the digit is equals to
		 the digit itself, then it is the password digit. There will be not
		 more than 1 digit with the frequency equals to itself.
		 Ex: 4 appearing 4 times, 7 appearing 7 times, 0 appearing 0 times.
*/

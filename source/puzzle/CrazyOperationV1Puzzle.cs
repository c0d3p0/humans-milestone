using System.Text;

using Godot;


public class CrazyOperationV1Puzzle : EnhancedBasePuzzle
{
	protected PuzzleContent CreatePage(Vector2[] pairs, byte[] operations)
	{
		StringBuilder cosb = new StringBuilder();
		int index;

		for(byte i = 0; i < pairs.Length; i++)
		{
			index = i * 2;
			cosb.Append(i < 4 ? pairs[i].x.ToString() : "?");
			cosb.Append(" (CO) ");
			cosb.Append(i < 4 ? pairs[i].y.ToString() : "?");
			cosb.Append(" = ");
			cosb.Append(GetOperationResult(pairs[i], operations));
			cosb.Append('\n');
		}

		return new PuzzleContent(cosb.ToString().Trim());
	}

	protected string GetOperationResult(Vector2 pair, byte[] operations)
	{
		StringBuilder result = new StringBuilder();
		short value = 0;

		for(int i = 0; i < operations.Length; i++)
		{
			if(operations[i] == DIVISION_FIRST_2_DIGITS)
			{
				value = (short) (pair.x / pair.y * 100);
				value = value > 99 ? (short) (value / 10) : value;
			}
			else if(operations[i] == ADD_AND_REVERSE)
				value = (short) (pair.x + pair.y);
			else if(operations[i] == ABS_SUB_AND_REVERSE)
				value = (short) Mathf.Abs(pair.x - pair.y);
			else if(operations[i] == MULT_AND_REVERSE)
				value = (short) (pair.x * pair.y);

			if(operations[i] < DIVISION_FIRST_2_DIGITS)
				result.Append(ReverseNumberString(value));
			else
				result.Append(value);
		}

		return result.ToString();
	}

	protected void InitializeAnswer(Vector2[] pairs)
	{
		correctAnswer = new char[4];
		byte ci = 0;

		for(int i = pairs.Length - 2; i < pairs.Length; i ++)
		{
			correctAnswer[ci++] = ConvertToDigitChar(pairs[i].x);
			correctAnswer[ci++] = ConvertToDigitChar(pairs[i].y);
		}
	}

	protected override void InitializePuzzle()
	{
		byte index = 0;
		Vector2[] pairs = RandomizeNonRepeatingPairs(6, 1, 9);
		byte[] ops = RandomizeNonRepeatingNumbers(2,
				ADD_AND_REVERSE, DIVISION_FIRST_2_DIGITS);
		puzzleContentMap.Add(index++, new PuzzleContent("Crazy Operation v1"));
		puzzleContentMap.Add(index++, CreatePage(pairs, ops));
		puzzleContentMap.Add(index++, CreateExtraPage());
		puzzleStatusPageId = index;
		InitializeAnswer(pairs);

		if(OS.IsDebugBuild())
			PrintPuzzleDebug();
	}

	protected PuzzleContent CreateExtraPage()
	{
		StringBuilder sb = new StringBuilder();
		sb.Append("In 2020, your own Physics says that there are 4 ");
		sb.Append("fundamental forces of nature, it's such a coincidence.");
		return new PuzzleContent(sb.ToString());
	} 

	protected void PrintPuzzleDebug()
	{
		GD.PushWarning("========== Puzzle Debug ==============");
		GD.PushWarning("");
		GD.PushWarning("Puzzle: Crazy Operation V1");
		GD.PushWarning("CorrectAnswer: " + new string(correctAnswer));
		GD.PushWarning("");
		GD.PushWarning("========== Puzzle Debug ==============");
		GD.PushWarning("");
	}


	protected const byte ADD_AND_REVERSE = 0;
	protected const byte ABS_SUB_AND_REVERSE = 1;
	protected const byte MULT_AND_REVERSE = 2;
	protected const byte DIVISION_FIRST_2_DIGITS = 3;
}

/*
	Puzzle Idea


	1: 6 pairs of numbers from 0 to 9 will be randomly generated. The fisrt
	   4 pairs will be used to help to find out the password, the other 2
		 will be used to represent the password.

	2: 2 of 4 operations will be randomly selected to be used to produce
	   a 4 digit result. The operations are sum, absolute subtraction,
		 multiplication and a division that will get the first 2 non zero
		 digits. The result of the operation will be defined by a two digits
		 number and it will be reversed for the first 3 operations.

	3: From the pairs of randomized numbers, a operation with the format
	   "N1 (CO) N2 = 4_DIGIT_RESULT" will be generated in the pages as a
		 hint to find the password.

	4: From the pairs of randomized numbers, 3 of them will be used as
	   example, showing what numbers they are and what result they will
		 produce.

	5: From the last 2 pairs, the numbers will be replaced by ? because
	   they are the characters of the password and only the result of the
		 operation will be shown.
*/
using System.Text;

using Godot;


public class CrazyOperationV2Puzzle : EnhancedBasePuzzle
{
	protected PuzzleContent CreatePage(Vector2[] pairs, int operation, int index)
	{
		StringBuilder cosb = new StringBuilder();
		int pairAmount = 4;
		int pairIndex = index * pairAmount;

		for(byte i = 0; i < pairAmount; i++)
		{
			cosb.Append(i != 2 ? pairs[pairIndex + i].x.ToString() : "?");
			cosb.Append(" (CO) ");
			cosb.Append(i != 3 ? pairs[pairIndex + i].y.ToString() : "?");
			cosb.Append(" = ");
			cosb.Append(GetOperationResult(pairs[pairIndex + i], operation));
			cosb.Append('\n');
		}

		return new PuzzleContent(cosb.ToString().Trim());
	}

	protected string GetOperationResult(Vector2 pair, int operation)
	{
		StringBuilder result = new StringBuilder();
		short value = 0;

		if(operation == ADD_AND_REVERSE)
			value = (short) (pair.x + pair.y);
		else if(operation == SUB_AND_REVERSE)
			value = (short) (pair.x - pair.y);
		else if(operation == MULT_AND_REVERSE)
			value = (short) (pair.x * pair.y);
		else if(operation == CONCAT_AND_REVERSE)
			value = (short) ((pair.x * 10) + pair.y);

		if(value > -1)
		{
			result.Append(' ');
			result.Append(ReverseNumberString(value));
		}
		else
		{
			result.Append('-');
			result.Append(ReverseNumberString(Mathf.Abs(value)));
		}
		

		return result.ToString();
	}

	protected void InitializeAnswer(Vector2[] pairs, int digitAmount)
	{
		correctAnswer = new char[digitAmount];
		byte ci = 0;

		for(int i = 2; i < pairs.Length; i += 4)
		{
			correctAnswer[ci++] = ConvertToDigitChar(pairs[i].x);
			correctAnswer[ci++] = ConvertToDigitChar(pairs[i + 1].y);
		}
	}

	protected override void InitializePuzzle()
	{
		byte index = 0;
		byte digitAmount = 12;
		byte pageAmount = (byte) (digitAmount / 2);
		Vector2[] pairs = RandomizeNonRepeatingPairs(4 * pageAmount, 0, 9);
		int op = this.RandiRange(rng, ADD_AND_REVERSE, CONCAT_AND_REVERSE);
		puzzleContentMap.Add(index++, new PuzzleContent("Crazy Operation\nv2"));

		for(int i = 0; i < pageAmount; i++)
			puzzleContentMap.Add(index++, CreatePage(pairs, op, i));

		puzzleContentMap.Add(index++, CreateExtraPage());
		puzzleStatusPageId = index;
		InitializeAnswer(pairs, digitAmount);

		if(OS.IsDebugBuild())
			PrintPuzzleDebug();
	}

	protected PuzzleContent CreateExtraPage()
	{
		StringBuilder sb = new StringBuilder();
		sb.Append("It is always mathematics the angel capable of ");
		sb.Append("describing the universe with such precision. ");
		return new PuzzleContent(sb.ToString());
	} 

	protected void PrintPuzzleDebug()
	{
		GD.PushWarning("========== Puzzle Debug ==============");
		GD.PushWarning("");
		GD.PushWarning("Puzzle: Crazy Operation V2");
		GD.PushWarning("CorrectAnswer: " + new string(correctAnswer));
		GD.PushWarning("");
		GD.PushWarning("========== Puzzle Debug ==============");
		GD.PushWarning("");
	}


	protected const byte ADD_AND_REVERSE = 0;
	protected const byte SUB_AND_REVERSE = 1;
	protected const byte MULT_AND_REVERSE = 2;
	protected const byte CONCAT_AND_REVERSE = 3;
}

/*
	Puzzle Idea


	1:	24 pairs of numbers from 0 to 9 will be randomly generated. 
			Each page will have 4 pairs, the last 2 pairs will have 
			1 hidden digit that will be used by the password. The first 
			2 pairs will be used as a hint so the player can find the answer.

	2:	1 of the 4 operations will be randomly selected to produce
			a 2 digit result. The operations are sum, subtraction,
			multiplication and concatenation of the digits.

	3:	From the pairs of randomized numbers, a operation with the format
			"N1 (CO) N2 = 2_DIGIT_RESULT" will be generated in the pages as a
			hint to find the password. The 2 digit result will also be the
			reversed result.

	4:	From each page of 4 pairs, the first 2 will be used as example, 
			showing what numbers they are and what result they will produce.

	5:	From each page of 4 pairs, the last 2 pairs will have at least 1 
			number that will be replaced by ?, they are a digit of the password.
*/

using System.Text;

using Godot;
using Godot.Collections;


public class KeysToTheTreasureV1Puzzle : EnhancedBasePuzzle
{
	protected StringBuilder[] GenerateTreasureChestLines(char passwordCharacter)
	{
		int pattern, halfCharAmount;
		byte[] validIndexes;

		if(operation == ROW_PATTERN)
		{
			pattern = ROW_LINE_AMOUNT;
			halfCharAmount = COLUMN_LINE_AMOUNT / 2;
			validIndexes = passwordRowValidIndexes;
		}
		else
		{
			pattern = COLUMN_LINE_AMOUNT;
			halfCharAmount = ROW_LINE_AMOUNT / 2;
			validIndexes = passwordColValidIndexes;
		}

		int passLineIndex = validIndexes[this.RandiRange(rng, 0, validIndexes.Length - 1)];
		StringBuilder[] lines = new StringBuilder[pattern];
		char[] half1, half2;
		char mid;

		for(int i = 0; i < pattern; i++)
		{
			lines[i] = new StringBuilder();

			if(i != passLineIndex)
			{
				half1 = RandomizeFromText(symbols, halfCharAmount);

				do
					half2 = RandomizeFromText(symbols, halfCharAmount);
				while(IsReverseEquals(half1, half2));

				mid = symbols[this.RandiRange(rng, 0, (byte) (symbols.Length - 1))];
				lines[i].Append(half1).Append(mid).Append(half2);
			}
			else
			{
				half1 = RandomizeFromText(symbols, halfCharAmount);
				lines[i].Append(half1).Append(passwordCharacter);
				lines[i].Append(ReverseArray(half1));
			}
		}

		return lines;		
	}

	protected PuzzleContent CreatePage(char passwordCharacter)
	{
		StringBuilder[] lines = GenerateTreasureChestLines(passwordCharacter);
		StringBuilder page = new StringBuilder();
		StringBuilder line;

		for(int i = 0; i < ROW_LINE_AMOUNT; i++)
		{
			line = new StringBuilder();

			for(int j = 0; j < COLUMN_LINE_AMOUNT; j++)
			{
				if(blankIndexMap.ContainsKey(GetBlankIndexKey(i, j)))
					line.Append("  ");
				else if(operation == ROW_PATTERN)
					line.Append(lines[i][j]).Append(' ');
				else if(operation == COLUMN_PATTERN)
					line.Append(lines[j][i]).Append(' ');
			}
			
			page.Append(line.ToString().Trim());
			page.Append('\n');
		}

		page.Remove(page.Length - 1, 1);
		return new PuzzleContent(page.ToString());
	}

	protected bool IsReverseEquals<T>(T[] array1, T[] array2)
	{
		if(array1.Length != array2.Length)
			return false;

		for(int i = 2, j = array1.Length - 3; i < array1.Length; i++, j--)
		{
			if(!array1[i].Equals(array2[j]))
				return false;
		}

		return true;
	}
	
	protected byte GetBlankIndexKey(int row, int column)
	{
		return (byte) ((row * 10) + column);
	}

	protected void ClearAttributes()
	{
		symbols.Clear();
		blankIndexMap.Clear();
		symbols = null;
		blankIndexMap = null;
		passwordRowValidIndexes = null;
		passwordColValidIndexes = null;
	}

	protected void InitializeAttributes()
	{
		symbols = GetSymbolCharacters();
		passwordRowValidIndexes = new byte[]{0, 1, 3, 5, 6};
		passwordColValidIndexes = new byte[]{2, 3, 5, 6};
		operation = (byte) this.RandiRange(rng, ROW_PATTERN, COLUMN_PATTERN);
		correctAnswer = RandomizeFromText(symbols, 6);
	}

	protected void InitializeBlankIndexMap()
	{
		blankIndexMap = new Dictionary<byte, object>();

		blankIndexMap.Add(GetBlankIndexKey(0, 0), null);
		blankIndexMap.Add(GetBlankIndexKey(0, 1), null);
		blankIndexMap.Add(GetBlankIndexKey(0, 7), null);
		blankIndexMap.Add(GetBlankIndexKey(0, 8), null);

		blankIndexMap.Add(GetBlankIndexKey(1, 0), null);
		blankIndexMap.Add(GetBlankIndexKey(1, 8), null);

		blankIndexMap.Add(GetBlankIndexKey(2, 4), null);

		blankIndexMap.Add(GetBlankIndexKey(4, 4), null);
	}

	protected override void InitializePuzzle()
	{
		byte index = 0;
		InitializeAttributes();
		InitializeBlankIndexMap();
		puzzleContentMap.Add(index++, new PuzzleContent("Keys To The Treasure v1"));

		for(byte i = 0; i < correctAnswer.Length; i++)
			puzzleContentMap.Add(index++, CreatePage(correctAnswer[i]));	

		puzzleContentMap.Add(index++, CreateExtraPage());
		puzzleStatusPageId = index;
		ClearAttributes();

		if(OS.IsDebugBuild())
			PrintPuzzleDebug();
	}

	protected override void InitializePuzzleInputsMap()
	{
		CreateXFormatSymbolsPuzzleInputsMap();
	}

	protected PuzzleContent CreateExtraPage()
	{
		StringBuilder sb = new StringBuilder();
		sb.Append("It is not money that makes someone rich, but knowledge. Anywhere");
		sb.Append(" you look you can gain new knowledge, even in symmetric things.");
		return new PuzzleContent(sb.ToString());
	} 

	protected void PrintPuzzleDebug()
	{
		GD.PushWarning("========== Puzzle Debug ==============");
		GD.PushWarning("");
		GD.PushWarning("Puzzle: Keys To The Treasure V1");
		GD.PushWarning("CorrectAnswer: " + new string(correctAnswer));
		GD.PushWarning("");
		GD.PushWarning("========== Puzzle Debug ==============");
		GD.PushWarning("");
	}


	protected StringBuilder symbols;
	protected byte operation;
	protected byte[] passwordRowValidIndexes;
	protected byte[] passwordColValidIndexes;
	protected Dictionary<byte, object> blankIndexMap;

	protected const byte ROW_PATTERN = 0;
	protected const byte COLUMN_PATTERN = 1;
	protected const byte ROW_LINE_AMOUNT = 7;
	protected const byte COLUMN_LINE_AMOUNT = 9;
}


/*
	Puzzle Idea


	1: 4 characters of the symbols "#$&@¢£¥µ€†‡ÐÞðþ" will be randomly
	   generated to represent the 4 characters of the password. 

	2: Each of the 4 characters will result in one text page. Each page will
	   help to find one character of the password.

	3: The text page will contain characters in a pattern that forms an image
	   of a treasure chest.

	4: 1 of 2 operations will be randomly selected to be used to create
	   the lines or columns of the treasure chest. The 2 operations are
		 ROW_PATTERN and COLUMN_PATTERN.
		 
	5: The password character will be hidden in a randomly selected
	   palindrome horizontal or vertical line, based in the randomly
		 generated operation ROW or COLUMN. The lines that are not hidding the
		 password character will not be a palindrome line.

	6: The format of the chest is like the one bellow.
			   * * * * *
		   * * * * * * *
	   * * * *   * * * *
	   * * * * * * * * *
	   * * * *   * * * *
		 * * * * * * * * *
		 * * * * * * * * *
*/

using System;
using System.Text;


public class AnyDividedBy0Puzzle : EnhancedBasePuzzle
{
	protected PuzzleContent CreateContentPage(String content)
	{
		StringBuilder sb = new StringBuilder();
		sb.Append(content).Append(" ÷ 0");
		return new PuzzleContent(sb.ToString());
	}

	protected override void InitializePuzzle()
	{
		byte index = 0;
		byte[] numbers = RandomizeNonRepeatingNumbers(12, 1, 99);
		puzzleContentMap.Add(index++, CreateContentPage("Any"));
		correctAnswer = new char[0];

		for(byte i = 0; i < numbers.Length; i++)
			puzzleContentMap.Add(index++, CreateContentPage(numbers[i].ToString()));

		puzzleContentMap.Add(index++, CreateExtraPage());
		puzzleStatusPageId = index;
	}

	protected PuzzleContent CreateExtraPage()
	{
		StringBuilder sb = new StringBuilder();
		sb.Append("Sometimes there are solutions to some problems ");
		sb.Append("that don't look like a solution at all.");
		return new PuzzleContent(sb.ToString());
	}
}


/*
	Puzzle Idea (Sample Puzzle)


	1: 12 numbers from 0 to 10 will be randomly generated to help to
	   find the password.

	2: From the randomized numbers, 12 pages will be created and the
	   answer to the content of each page will be a character in the
		 password.

	3: Each of the 12 numbers will be divided by 0 and any number
	   divided by 0 is undefinided.
	
	4: The password is undefined, so if you can't define the password
	   you shouldn't type any character.		 
*/

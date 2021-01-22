using System.Text;

using Godot;


public class GuidedByTheEvidenceV1Puzzle : EnhancedBasePuzzle
{
	protected PuzzleContent CreateHintPage(int pageIndex)
	{
		HashList<StringBuilder> lineList = new HashList<StringBuilder>();
		StringBuilder cl = new StringBuilder().Append(correctAnswer[pageIndex * 2]);
		cl.Append('-', 10).Append(correctAnswer[(pageIndex * 2) + 1]).Append('\n');
		lineList.Add(cl);
		StringBuilder l;
		sbyte mln;
		int ci;

		for(int i = 0; i < 4; i++)
		{
			mln = GetRandomLinkNumber();
			ci = mln > 0 ? this.RandiRange(rng, 0, 12) : this.RandiRange(rng, 13, 25);
			l = new StringBuilder().Append(alphabet[ci]).Append('-', 10);
			l.Append(System.Convert.ToChar(alphabet[ci] + mln)).Append('\n');
			lineList.Add(l);
		}

		return new PuzzleContent(GetShuffledPageContent(lineList).ToString().Trim());
	}

	protected StringBuilder GetShuffledPageContent(HashList<StringBuilder> lineList)
	{
		StringBuilder pageContent = new StringBuilder();
		int random;

		while(lineList.Count > 0)
		{
			random = this.RandiRange(rng, 0, lineList.Count - 1);
			pageContent.Append(lineList[random]);
			lineList.RemoveAt(random);
		}

		return pageContent;
	}

	protected sbyte GetRandomLinkNumber()
	{
		int random = this.RandiRange(rng, 0, linkNumberList.Count - 1);
		sbyte randomLinkNumber = linkNumberList[random];
		linkNumberList.RemoveAt(random);
		return randomLinkNumber;
	}

	protected void CreatePages()
	{
		byte index = 0;
		int pages = correctAnswer.Length / 2;
		puzzleContentMap.Add(index++,
				new PuzzleContent("Guided By The Evidence v1"));

		for(byte i = 0; i < pages; i++)
			puzzleContentMap.Add(index++, CreateHintPage(i));

		puzzleContentMap.Add(index++, CreateExtraPage1());
		puzzleContentMap.Add(index++, CreateExtraPage2());
		puzzleStatusPageId = index;
	}

	protected void CreateCorrectAnswer(char[] coreChars)
	{
		correctAnswer = new char[coreChars.Length * 2];

		for(int i = 0; i < coreChars.Length; i++)
		{
			correctAnswer[i * 2] = coreChars[i];
			correctAnswer[(i * 2) + 1] =
					System.Convert.ToChar(coreChars[i] + passLinkNumber);
		}
	}

	protected void CreateLinkNumberList()
	{
		linkNumberList = new HashList<sbyte>();

		for(sbyte i = 1; i < 14; i++)
		{
			linkNumberList.Add(i);
			linkNumberList.Add((sbyte) (-i));
		}

		linkNumberList.Remove(passLinkNumber);
	}
	
	protected override void InitializePuzzle()
	{
		alphabet = GetAlphabetCharacters();
		passLinkNumber = (sbyte) this.RandiRange(rng, 1, 13);
		passLinkNumber *= (sbyte) (1 - (this.RandiRange(rng, 0, 1) * 2));
		CreateLinkNumberList();
		char[] coreChars = passLinkNumber > 0 ?
				RandomizeNonRepeatingFromText(alphabet, 3, 0, 12) :
				RandomizeNonRepeatingFromText(alphabet, 3, 13, 25);
		CreateLinkNumberList();
		CreateCorrectAnswer(coreChars);
		CreatePages();

		if(OS.IsDebugBuild())
			PrintPuzzleDebug();		
	}

	protected override void InitializePuzzleInputsMap()
	{
		CreateAlphabetPuzzleInputsMap();
	}

	private PuzzleContent CreateExtraPage1()
	{
		StringBuilder sb = new StringBuilder();
		
		sb.Append("Several intelligent species like us and even humans ");
		sb.Append("found a way to get as close as we can to the truth.");
		return new PuzzleContent(sb.ToString());
	}

	private PuzzleContent CreateExtraPage2()
	{
		StringBuilder sb = new StringBuilder();
		sb.Append("We all do it by checking uncorrelated sources that ");
		sb.Append("has matching results.");
		return new PuzzleContent(sb.ToString());
	}

	protected void PrintPuzzleDebug()
	{
		GD.PushWarning("========== Puzzle Debug ==============");
		GD.PushWarning("");
		GD.PushWarning("Puzzle: Guided By The Evidence V1");
		GD.PushWarning("PaswordLinkNumber: " + passLinkNumber);
		GD.PushWarning("CorrectAnswer: " + new string(correctAnswer));
		GD.PushWarning("");
		GD.PushWarning("========== Puzzle Debug ==============");
		GD.PushWarning("");
	}


	protected sbyte passLinkNumber;
	protected StringBuilder alphabet;
	protected HashList<sbyte> linkNumberList;
}


/*
	Puzzle Idea

	1: The puzzle idea is about pairs of characters from A - Z. The difference
		 between the characters in a pair will represent a "Link Number" (LN).

	2: A "Link Number" from 1 - 13, positive or negative, will be randomized
		 to help create all pairs of characters that will form the password.

	3: 3 letters will be randomized, if the "Link Number" is positive, the
		 letters will be from A to M, if it is negative the letters will be
		 from N to Z.
	
	4: The other letter from all links will be the a random letter plus the
		 "Link Number", so the password will be:
		 C1, C1 + LN, C2, C2 + LN, C3, C3 + LN
	
	5: 3 pages will be generated with 7 pairs of characters each. It will
		 have 1 pair with 2 linked characters from the password and 6 other
		 misleading pairs.
	
	6: The misleading pairs can be formed by any "Link Number", positive
		 or negative.
		
	7: All misleading "Link Numbers" will appear only once in the puzzle
		 and only the "Link Number" of the password will appear in all pages,
		 being the hint for the password.
*/


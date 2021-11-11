using System.Text;

using Godot;
using Godot.Collections;


public class KnowledgeGivenByTheBookV1Puzzle : EnhancedBasePuzzle
{
	protected PuzzleContent GenerateBookPage(byte pageNumber)
	{
		StringBuilder page = new StringBuilder();
		byte subjectIndex = (byte) this.RandiRange(rng, 0, --pageLeft);
		string[] words = GeneratePageWords();

		if(subjectIndex < subjectList.Count)
		{
			int wordIndex = this.RandiRange(rng, 0, words.Length - 1);
			words[wordIndex] = subjectList[subjectIndex];
			subjectList.RemoveAt(subjectIndex);
			puzzleAnswer.Append(words[wordIndex][pageNumber - 1]);
		}

		for(byte i = 0; i < words.Length; i++)
			page.Append(words[i]).Append(' ');

		page.Remove(page.Length - 1, 1).Append('\n');
		page.Append("\n                ").Append(pageNumber);
		return new PuzzleContent(page.ToString());
	}

	protected string[] GeneratePageWords()
	{
		string[] words = new string[PAGE_LINES * WORDS_PER_LINE];
		
		for(byte i = 0; i < words.Length; i++)
			words[i] = GenerateNotInSubjectsWord();

		return words;
	}

	protected void GenerateRandomSubjects()
	{
		subjectList = new Array<string>();

		for(byte i = 0; i < SUBJECT_AMOUNT; i++)
			subjectList.Add(GenerateNotInSubjectsWord());
	}

	protected PuzzleContent GenerateSubjectPage()
	{
		StringBuilder page = new StringBuilder();
		page.Append("Subjects\n\n\n");

		for(byte i = 0; i < subjectList.Count; i++)
			page.Append(subjectList[i].ToString()).Append('\n');

		page.Append("\n\n");
		return new PuzzleContent(page.ToString());
	}

	protected string GenerateNotInSubjectsWord()
	{
		return GenerateNotInSubjectsWord(CHAR_AMOUNT, CHAR_AMOUNT);
	}

	protected string GenerateNotInSubjectsWord(int min, int max)
	{
		string word;

		do
		{
			word = new string(this.RandomizeFromText(symbols,
					this.RandiRange(rng, min, max)));
		}
		while(subjectList.Contains(word));

		return word;
	}

	protected void ClearAttributes()
	{
		symbols.Clear();
		subjectList.Clear();
		puzzleAnswer.Clear();
		symbols = null;
		subjectList = null;
		puzzleAnswer = null;
	}

	protected void InitializeAttributes()
	{
		symbols = GetSymbolCharacters();
		puzzleAnswer = new StringBuilder();
		pageLeft = PAGE_AMOUNT;
		correctAnswer = new char[ANSWER_LENGTH];
	}

	protected override void InitializePuzzle()
	{
		byte index = 0;
		InitializeAttributes();
		GenerateRandomSubjects();
		puzzleContentMap.Add(index++,
				new PuzzleContent("Knowledge Given By The Book v1"));
		puzzleContentMap.Add(index++, GenerateSubjectPage());

		for(byte i = 1; i <= PAGE_AMOUNT; i++)
			puzzleContentMap.Add(index++, GenerateBookPage(i));	

		puzzleContentMap.Add(index++, CreateExtraPage());
		puzzleStatusPageId = index;
		puzzleAnswer.CopyTo(0, correctAnswer, 0, ANSWER_LENGTH);

		if(OS.IsDebugBuild())
			PrintPuzzleDebug();

		ClearAttributes();
	}

	protected override void InitializePuzzleInputsMap()
	{
		CreateXFormatSymbolsPuzzleInputsMap();
	}

	protected PuzzleContent CreateExtraPage()
	{
		StringBuilder sb = new StringBuilder();
		sb.Append("The credibility of any information is defined by");
		sb.Append(" its precision!");
		return new PuzzleContent(sb.ToString());
	} 

	protected void PrintPuzzleDebug()
	{
		GD.PushWarning("========== Puzzle Debug ==============");
		GD.PushWarning("");
		GD.PushWarning("Puzzle: Knowledge Given By The Book V1");

		PuzzleContent pcPage;
		string[] subjects = puzzleContentMap[1].text.Trim().Split("\n");

		for(byte i = 0; i < 5; i++)
		{
			pcPage = puzzleContentMap[(byte) (i + 2)];

			for(byte j = 1; j < subjects.Length; j++)
			{
				if(pcPage.text.Contains(subjects[j]))
				{
					GD.PushWarning("Page " + (i + 1) + " contains word " + j);
					GD.PushWarning("Extract Character " + (i + 1) + " from word");
				}
			}
		}

		GD.PushWarning("CorrectAnswer: " + new string(correctAnswer));
		GD.PushWarning("");
		GD.PushWarning("========== Puzzle Debug ==============");
		GD.PushWarning("");
	}


	protected StringBuilder symbols;
	protected StringBuilder puzzleAnswer;
	protected Array<string> subjectList;
	protected byte pageLeft;

	protected const byte CHAR_AMOUNT = 5;
	protected const byte PAGE_AMOUNT = 5;
	protected const byte SUBJECT_AMOUNT = 3;
	protected const byte WORDS_PER_LINE = 3;
	protected const byte PAGE_LINES = 7;
	protected const byte ANSWER_LENGTH = 3;
}


/*
	Puzzle Idea


	1: 3 words with 5 symbols of "#$&@¢£¥µ€†‡ÐÞðþ" will be randomly generated.
	   One of the characters in each word will be part of the password.

	2: 5 pages full of words with 5 of the same symbols will be randomly
	   generated.

	3: 3 of the 5 pages will have one of the 3 words. The character of the word
	   that is in the same position of the number of the page will be the password
		 character.

	4: Pages will have at maximum 7 lines with 3 words.
*/

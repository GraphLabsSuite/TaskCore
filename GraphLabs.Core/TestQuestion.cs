using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

#if false
namespace GraphLabs.StdGraph
{
    public class TestQuestion
    {
        private string wording;
        public string Question
        {
            get
            {
                return wording;
            }
        }
        private ReadOnlyCollection<string> choiseSet;

        public ReadOnlyCollection<string> Answers
        {
            get
            {
                return choiseSet;
            }
        }

        private int rightChoise;

        public int CorrectAnswer
        {
            get
            {
                return rightChoise;
            }
        }

        public TestQuestion(string question, ReadOnlyCollection<string> answers, int CorrectAnsIndex)
        {
            wording = question;
            choiseSet = answers;
            rightChoise = CorrectAnsIndex;
        }
    }
}
#endif
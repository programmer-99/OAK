using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System;

namespace OAK
{
    /// <summary>
    /// This class only deals with the lexical logic all grammar is separated in another class
    /// </summary>
    public class LexicalAnalyzer
    {
        public LexicalAnalyzer()
        {
            Dict = new Grammar();
        }

        public Grammar Dict { get; set; }

        /// <summary>
        /// Tokenize the code according to the available grammar
        /// </summary>
        /// <param name="rawText"></param>
        /// <returns></returns>
        public List<Token> Analyze(string rawText)
        {
            var tokens = new List<Token>();

            //main ind to track the current position
            int postion = 0;
            int noOflines = 1;
            string text = rawText;
            int length = rawText.Length;
            while (postion < length)
            {
               // Remove spaces,tabs and new noOfLines from front of string and increase the current ind
                var items = TrimStart(text);
                noOflines += items.Item3;
                var count = items.Item1 + items.Item2 + items.Item3;
                postion += count;
                text = text.Substring(count);

                //if we are at the end of text
                if (string.IsNullOrEmpty(text))
                    break;

                Token token;

                //check keyword
                token = IsKeyword(text, noOflines);

                //check punctuators
                if (token == null)
                    token = IsPunctuator(text, noOflines);

                //Check string
                if (token == null)
                    token = IsString(text, noOflines);

                //Check comment
                if (token == null)
                {
                    var val = IsComment(text, noOflines);
                    token = val.Item1;
                    noOflines += val.Item2;
                }




                //check operator
                if (token == null)
                    token = IsOperator(text, noOflines);
                if (token == null)
                {
                    //now we have to break the word in order to validate it as arithmetic operators or identifier
                    var word = BreakWord(text);
                    token = IsDouble(word, noOflines);
                    if (token == null)
                        token = IsInt(word, noOflines);
                    if (token == null)
                        token = IsBool(word, noOflines);
                    if (token == null)
                        token = IsIdentifier(word, noOflines);
                    if (token == null)
                        token = new Token(ClassPart.INVALID, word, noOflines);
                }
                //increase current ind
                postion += token.Value.Length;
                //remove the token from text
                text = text.Substring(token.Value.Length);
                //add token
                tokens.Add(token);
            }
            return tokens;
        }

        private (int, int, int) TrimStart(string text)
        {
            int spaceCount = 0;
            int lineCount = 0;
            int tabCount = 0;
            for (int i = 0; i < text.Length; i++)
            {
                if (text[i] == ' ')
                {
                    spaceCount++;
                }
                else if (text[i] == '\n')
                {
                    lineCount++;
                }
                else if (text[i] == '\t' || text[i] == '\r')
                {
                    tabCount++;
                }
                else
                    break;
            }
            return (spaceCount, tabCount, lineCount);
        }

        private string BreakWord(string text)
        {
            //check for work breakers and punctuators
            //start from ind 1 cause we have already checked the punctuator and operator before, so they won't be present at start at least
            if ( (text.Length > 1 && text[0] == '.'
                && Regex.Match(text[0].ToString(), "^[0-9]$").Success))
            {
                //only accepts 1 dot (for double only)
                int dotCount = 0;
                for (int i = 1; i < text.Length; i++)
                {
                    
                    if (text[i] == '.') dotCount++;
                    if (!Regex.Match(text[i].ToString(), "^[0-9]$").Success || (text[i] == '.' && dotCount == 2))
                    {
                        dotCount = 0;
                        return text.Substring(0, i);
                    }
                }
            }
            else
            {
                for (int i = 1; i < text.Length; i++)
                {
                    if (Dict.WordBreakers.Contains(text[i]) || Dict.Punctuators.Keys.Contains(text[i].ToString()))
                    {
                        //if word breaker is a dot make sure the word is not a double
                        if (text[i] != '.' || (text[i] == '.' && !Regex.Match(text[i + 1].ToString(), "^[0-9]$").Success))
                        return text.Substring(0, i);
                        
                    }
                    else
                    {
                        //check for operators
                        foreach (var op in Dict.Operators.Keys)
                        {
                            //match the operators
                            if (text.Substring(i).StartsWith(op))
                            {
                                return text.Substring(0, i);
                            }
                        }
                    }
                }
            }
            return text;
        }

        private Token IsKeyword(string text, int lineNumber)
        {
            Token token = null;
            foreach (var keyword in Dict.Keywords.Keys)
            {
                //check if text starts with keyword and the keyword not end with any alphabet or digit
                if (text.StartsWith(keyword) && (text.Length == keyword.Length || !Regex.Match(text.Substring(keyword.Length, 1), "^[a-zA-Z_][a-zA-Z0-9_]*$").Success)) //!Regex.Match(text.Substring(keyword.Length, 1), "^[A-Za-z0-9]+$").Success)
                {
                    var classPart = Dict.Keywords[keyword];
                    token = new Token(classPart, keyword, lineNumber);
                    break;
                }
            }
            return token;
        }

        private Token IsPunctuator(string text, int lineNumber)
        {
            Token token = null;
            foreach (var punctuator in Dict.Punctuators.Keys)
            {
                if (text.StartsWith(punctuator))
                {
                    //don't take DOT as a punctuator if it has preceding digits, cause it will be a double constant in that case
                    if (text[0] == '.')
                    {
                        if (text.Length > 1 && !Regex.Match(text[1].ToString(), "^[0-9]$").Success)
                        {
                            token = new Token(Dict.Punctuators[punctuator], punctuator, lineNumber);
                            break;
                        }
                    }
                    else
                    {
                        var classPart = Dict.Punctuators[punctuator];
                        token = new Token(classPart, punctuator, lineNumber);
                        break;
                    }
                }
            }
            return token;
        }

        private Token IsOperator(string text, int lineNumber)
        {
            Token token = null;
            foreach (var op in Dict.Operators.Keys)
            {
                if (text.StartsWith(op))
                {
                    //we only except plus and minus operator which don't have a dot or number after them, cause we have to differentiate between +/- sign and operator
                    if (op == "+" || op == "-")
                    {
                        if (text[1] != '.' && !Regex.Match(text[1].ToString(), "^[0-9]$").Success)
                        {
                            var classPart = Dict.Operators[op];
                            token = new Token(classPart, op, lineNumber);
                            break;
                        }
                    }
                    else
                    {
                        var classPart = Dict.Operators[op];
                        token = new Token(classPart, op, lineNumber);
                        break;
                    }
                }
            }
            return token;
        }

        private Token IsString(string text, int lineNumber)
        {

            Token token = null;
            //true when we find an invalid escape sequence \
            bool isInvalid = false;
            //if it starts with double quote, search for the ending quote 
            if (text.StartsWith("\"") && text.Length > 1)
            {
                for (int i = 1; i < text.Length; i++)
                {
                    if (text[i] == '\\')
                    {
                        //isInvalid = true;
                    }
                    //l_num break
                    else if (text[i] == '\r' || text[i] == '\n')
                    {
                        //don't include the new l_num character in the value part
                        token = new Token(ClassPart.INVALID, text.Substring(0, i), lineNumber);
                        break;
                    }
                    //find the end of string and also deal the escape condition
                    if (text[i] == '\"' && text[i - 1] != '\\')
                    {
                        if (isInvalid)
                            return new Token(ClassPart.INVALID, text.Substring(0, i + 1), lineNumber);
                        return new Token(ClassPart.STRING_CONSTANT, text.Substring(0, i + 1), lineNumber);
                    }
                    //if text ends and we don't find the ending quote for string, means string is not closed
                    if (i == text.Length - 1)
                        token = new Token(ClassPart.INVALID, text, lineNumber);
                }
            }
            //if it starts with single quote, search for the ending single 
            else if (text.StartsWith("\'") && text.Length > 1)
            {
                for (int i = 1; i < text.Length; i++)
                {
                    if (text[i] == '\\')
                    {
                        isInvalid = true;
                    }
                    //l_num break
                    else if (text[i] == '\r' || text[i] == '\n')
                    {
                        //don't include the new l_num character in the value part
                        token = new Token(ClassPart.INVALID, text.Substring(0, i), lineNumber);
                        break;
                    }
                    //find the end of string and also deal the escape condition
                    if (text[i] == '\'' && text[i - 1] != '\\')
                    {
                        if (isInvalid)
                            return new Token(ClassPart.INVALID, text.Substring(0, i + 1), lineNumber);
                        return new Token(ClassPart.STRING_CONSTANT, text.Substring(0, i + 1), lineNumber);
                    }
                    //if text ends and we don't find the ending quote for string, means string is not closed
                    if (i == text.Length - 1)
                        token = new Token(ClassPart.INVALID, text, lineNumber);
                }
            }
            //if string is too short and incomplete
            else if ((text.StartsWith("\"") || text.StartsWith("'")) && text.Length < 2)
            {
                token = new Token(ClassPart.INVALID, text, lineNumber);
            }

            return token;
        }

        private (Token, int) IsComment(string text, int lineNumber)
        {
            Token token = null;
            int lineCount = 0;

            //for single l_num comment
            if (text.StartsWith("//") && text.Length > 1)
            {
                for (int i = 1; i < text.Length; i++)
                {
                    if (text[i] == '\n')
                    {
                        token = new Token(ClassPart.SINGLE_LINE_COMMENT, text.Substring(0, i), lineNumber);
                        break;
                    }
                    //if we couldn't find a l_num break means its the end of string
                    if (i == text.Length - 1)
                    {
                        token = new Token(ClassPart.SINGLE_LINE_COMMENT, text, lineNumber);
                    }
                }
            }
            //for multi l_num comment
            else if (text.StartsWith("/*") && text.Length > 4)
            {
                for (int i = 2; i < text.Length - 1; i++)
                {
                    if (text[i] == '\n')
                    {
                        lineCount++;
                    }
                    //find the end of multi l_num comment
                    if (text[i] == '*' && text[i + 1] == '/')
                    {
                        token = new Token(ClassPart.MULTI_LINE_COMMENT, text.Substring(0, i + 2), lineNumber + lineCount);
                        break;
                    }
                    //if text ends and we don't find the closing comment tag
                    if (i == text.Length - 2)
                        token = new Token(ClassPart.INVALID, text, lineNumber);
                }
            }
            //if text is too short and comment is incomplete i.e not closed
            else if (text.StartsWith("/*") && text.Length < 5)
            {
                token = new Token(ClassPart.INVALID, text, lineNumber);
            }

            return (token, lineCount);
        }
        private Token IsDouble(string word, int lineNumber)
        {
            if (Regex.Match(word, "^[+-]?[0-9]*[.][0-9]+[d]?$").Success)
            {
                return new Token(ClassPart.DOUBLE_CONSTANT, word, lineNumber);
            }
            return null;
        }

        private Token IsInt(string word, int lineNumber)
        {
            if (Regex.Match(word, "^[+-]?[0-9]+$").Success)
            {
                return new Token(ClassPart.INT_CONSTANT, word, lineNumber);
            }
            return null;
        }


        private Token IsBool(string word, int lineNumber)
        {
            if (word == "true" || word == "false")
            {
                return new Token(ClassPart.BOOL_CONSTANT, word, lineNumber);
            }
            return null;
        }

        private Token IsIdentifier(string word, int lineNumber)
        {
            if (Regex.Match(word, "^[_]?[A-Za-z]+([_]*[A-Za-z0-9]*)*[_]*$").Success)
            {
                return new Token(ClassPart.IDENTIFIER, word, lineNumber);
            }
            return null;
        }
    }
}
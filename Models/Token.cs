namespace OAK
{
    public class Token
    {
        /// <summary>
        /// Model class for token
        /// </summary>
        /// <param name="classPart"></param>
        /// <param name="value"></param>
        /// <param name="lineNumber"></param>
        public Token(ClassPart classPart, string value, int lineNumber)
        {
            ClassPart = classPart;
            Value = value;
            LineNumber = lineNumber;
        }

        public ClassPart ClassPart { get; set; }
        public string Value { get; set; }
        public int LineNumber { get; set; }

        public string ToCSVString()
        {
            return $"{LineNumber},{ClassPart},{Value}\r\n";
        }
        public override string ToString()
        {
            if (ClassPart == ClassPart.MULTI_LINE_COMMENT)
                return null;


            if (ClassPart == ClassPart.SINGLE_LINE_COMMENT)
                return null;


            return $" (LINE NUMBER: {LineNumber},CLASS PART: {ClassPart},VALUE: '{Value}')";
        }

    }
}

using System.Collections.Generic;

namespace OAK
{
    /// <summary>
    /// Grammar of the Compiler
    /// Mostly uses dictionaries to store the Class part with keywords, punctuators and operators
    /// </summary>
    public class Grammar
    {
        public char[] WordBreakers { get; set; } = new char[]
        {
            ' ',
            '\n',
            '\t',
            '\r',
            '.',
            '"',
            '\'',
            
        };

        //Order is IMPORTANT
        public Dictionary<string, ClassPart> Keywords { get; set; } = new Dictionary<string, ClassPart>()
        {
            {"break",     ClassPart.BREAK},
            {"continue",  ClassPart.CONTINUE},
            {"class",     ClassPart.CLASS},
            {"catch",     ClassPart.CATCH},
            {"else",      ClassPart.ELSE },
            {"default",   ClassPart.DEFAULT},
            {"if",        ClassPart.IF},
            {"for",       ClassPart.FOR},
            {"using",     ClassPart.USING},
            {"new",       ClassPart.NEW},
            {"void",      ClassPart.VOID},
            {"try",       ClassPart.TRY},
            {"return",    ClassPart.RETURN},
            {"static",    ClassPart.STATIC},
            {"this",      ClassPart.THIS},
            {"while",     ClassPart.WHILE},
            {"public",    ClassPart.PUBLIC},
            {"private",   ClassPart.PRIVATE},
            {"abstract",  ClassPart.ABSTRACT},                                        
            {"sealed",    ClassPart.SEALED},
            {"protected", ClassPart.PROTECTED},
            /*{"internal",ClassPart.INTERNAL},*/
            {"Main",      ClassPart.MAIN},

            //Data Types
            { "int",ClassPart.INTEGER },
            { "string",ClassPart.STRING },
            { "double",ClassPart.DOUBLE },
            { "bool", ClassPart.BOOL },


            {"do",  ClassPart.DO },
            {"in",   ClassPart.IN},
        };

        
        public Dictionary<string, ClassPart> Punctuators { get; set; } = new Dictionary<string, ClassPart>()
        {
            { ";", ClassPart.SEMI_COLON},
            { " ", ClassPart.WHITESPACE},
            { ":", ClassPart.COLON },
            { ",", ClassPart.COMMA },
            { ".", ClassPart.DOT },
            { "(", ClassPart.OPENING_PARANTHESES },
            { ")", ClassPart.CLOSING_PARANTHESES },
            { "{", ClassPart.OPENING_CURLY_BRACKET },
            { "}", ClassPart.CLOSING_CURLY_BRACKET },
            { "[", ClassPart.OPENING_SQUARE_BRACKET },
            { "]", ClassPart.CLOSING_SQUARE_BRACKET },
        };

        /// <summary>
        /// NOTE: The order of operators is IMPORTANT, it is used for checking
        /// </summary>
        public Dictionary<string, ClassPart> Operators { get; set; } = new Dictionary<string, ClassPart>()
        {
            //Assignment Operators
            {"+=",ClassPart.COMPOUND_EQUAL_PLUS},
            {"-=",ClassPart.COMPOUND_EQUAL_MINUS},
            {"/=",ClassPart.COMPOUND_EQUAL_DIVIDE},
            {"*=",ClassPart.COMPOUND_EQUAL_MULTIPLY},
            

            //Increment/Decrement Operators
            {"++",ClassPart.INCREMENT_DECREMENT_PLUS},
            {"--",ClassPart.INCREMENT_DECREMENT_MINUS},

            //Arithmetic Operators
            {"+",ClassPart.PLUS_MINUS},
            {"-",ClassPart.PLUS_MINUS},
            {"*",ClassPart.MULTIPLY_DIVIDE_MODULUS},
            {"/",ClassPart.MULTIPLY_DIVIDE_MODULUS},
            {"%",ClassPart.MULTIPLY_DIVIDE_MODULUS},

            //Logical Operators
            {"&&",ClassPart.AND},
            {"||",ClassPart.OR},
            {"<=",ClassPart.RELATIONAL_OPERATOR},
            {">=",ClassPart.RELATIONAL_OPERATOR},
            {"!=",ClassPart.RELATIONAL_OPERATOR},
            {"==",ClassPart.RELATIONAL_OPERATOR},
            {"!",ClassPart.NOT},
            {">",ClassPart.RELATIONAL_OPERATOR},
            {"<",ClassPart.RELATIONAL_OPERATOR},


            //Equal
            {"=",ClassPart.EQUAL},

            //End Marker
            {"0/", ClassPart.END_MARKER},
        };
    }
}
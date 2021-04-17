namespace OAK
{
    /// All class parts are available as Enum to avoid spelling mistakes and easy handling
   
    public enum ClassPart
    {
        INVALID,
        IDENTIFIER,

        //KEYWORDS
        BREAK,
        CONTINUE,
        DO,
        CLASS,
        CATCH,
        ELSE,
        DEFAULT,
        IF,
        FOR,
        USING,
        NEW,
        VOID,
        TRY,
        RETURN,
        IN,
        STATIC,
        THIS,
        WHILE,
        PUBLIC,
        PRIVATE,
        PROTECTED,
        INTERNAL,
        ABSTRACT,
        SEALED,
        MAIN,

        //DATA TYPES
        //DATA_TYPE,
        INTEGER,
        STRING,
        BOOL,
        DOUBLE,

        //CONSTANTS
        INT_CONSTANT,
        DOUBLE_CONSTANT,
        STRING_CONSTANT,
        BOOL_CONSTANT,

        //PUNCTUATORS
        OPENING_PARANTHESES,
        CLOSING_PARANTHESES,

        OPENING_CURLY_BRACKET,
        CLOSING_CURLY_BRACKET,

        CLOSING_SQUARE_BRACKET,
        OPENING_SQUARE_BRACKET,

        COLON,
        SEMI_COLON,
        COMMA,
        DOT,
        WHITESPACE,

        //ARITHMETIC OPERATORS
        PLUS_MINUS,
        MULTIPLY_DIVIDE_MODULUS,

        //ASSIGNMENT OPERATORS
        COMPOUND_EQUAL_PLUS,
        COMPOUND_EQUAL_MINUS,
        COMPOUND_EQUAL_MULTIPLY,
        COMPOUND_EQUAL_DIVIDE,
        EQUAL,

        //INCREMENT/DECREMENT OPERATOR
        INCREMENT_DECREMENT_PLUS,
        INCREMENT_DECREMENT_MINUS,

        //LOGICAL OPERATOS
        AND,
        OR,
        NOT,
        RELATIONAL_OPERATOR,

        //COMMENTS
        SINGLE_LINE_COMMENT,
        MULTI_LINE_COMMENT,


        //END MARKER
        END_MARKER,
    }
}
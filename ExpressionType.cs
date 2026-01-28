namespace Parser
{
    public enum ExpressionType
    {
        Add, 
        Sub, 
        Mul, 
        Div,
        GE,
        LE, 
        EQ, 
        NE,


        FloatDecl, 
        BoolDecl, 
        FloatAssign,
        BoolAssign, 
 
        Input,
        Output,


        IfExpression,
        Then,
        Else,
        Condition,
        ForExpression,
        Body,


    }
}
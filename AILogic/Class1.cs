using GameRules;

namespace AILogic
{
    public interface IStateEvalutator
    {
           public float EvaluateState(BoardState state);

            public int HashState(BoardState state);

            public int ID();
    }

}
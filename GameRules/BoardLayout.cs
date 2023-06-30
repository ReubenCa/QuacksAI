namespace GameRules
{
    public static class BoardLayout
    {
        private static readonly (int Money, int VP, bool Ruby)[] Board = {(0,0,false),(1,0,false),(2,0,false),(3,0,false),(4,0,false),(5,0,true)
    ,(6,1,false),(7,1,false),(8,1,false),(9,1,true),(10,2,false),(11,2,false),(12,2,false),(13,2,true),(14,3,false),(15,3,false),
    (15,3,true),(16,3,false),(16,4,false),(17,4,false),(17,4,true),(18,4,false),(18,5,false),(19,5,false),(19,5,true),(20,5,false),
    (20,6,false),(21,6,false),(21,6,true),(22,7,false),(23,7,false),(23,8,false),(24,8,false),(24,8,true),(25,9,false),(25,9,true),(26,9,false),
    (26,10,false),(27,10,true),(28,11,false),(28,11,true),(29,11,false),(29,12,false),(30,12,false),(30,12,true),(31,12,false),(31,13,false),(32,13,false),
    (32,13,true),(33,14,false),(33,14,true),(35,15,false)};

        public static int GetMoney(int space)
        {
            return Board[space].Money;
        }

        public static int GetVP(int space)
        {
            return Board[space].VP;
        }

        public static bool GetRuby(int space)
        {
            return Board[space].Ruby;
        }
    }
}
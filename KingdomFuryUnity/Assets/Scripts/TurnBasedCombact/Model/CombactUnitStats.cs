namespace TurnBasedCombact.Model
{
    [System.Serializable]
    public class CombactUnitStats
    {
        public int health; // Max player health
        public int impetus;   // Max player Impetus
    
        public int attack; //
        public int defence; // [0-1] diminuisce i danni subiti da un attacco
    
        public int speed; // [0-1] probabilita' di schivare un attacco?
        public int affinity; // [-1, 1] probabilita' di attacco critico o debole
    }
}
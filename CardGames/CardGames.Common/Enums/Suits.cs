using System.ComponentModel;

namespace CardGames.Common.Enums
{
    public enum Suits
    {
        [Description("S")]
        Spades,
        [Description("H")]
        Hearts,
        [Description("D")]
        Diamonds,
        [Description("C")]
        Clubs
    }

    public enum Colors
    {
        Red,
        Black
    }
}

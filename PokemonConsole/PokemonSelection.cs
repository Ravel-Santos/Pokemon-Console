namespace PokemonConsole;

public class PokemonSelection
{
    public static void DrawnSelection(int sel, Pokemon[] lista, bool choosingPlayer, Pokemon playerWasChosen)
    {
        Console.Clear();

        string leftMargin = "     ";
        int spaceBetweenSprites = 28;
        
        Console.WriteLine("----------------- BATTLE SELECTION -----------------");
        Console.WriteLine("");
        
        string[] spriteLeft = choosingPlayer ? lista[sel]._sprite : playerWasChosen._sprite;
        string[] spriteRigth = choosingPlayer ? SpritePokebola() : lista[sel]._sprite;

        for (int i = 0; i < 5; i++)
        {
            string esq = (i < spriteLeft.Length) ? spriteLeft[i] : "";
            string dir = (i < spriteRigth.Length) ? spriteRigth[i] : "";

            Console.WriteLine(leftMargin + esq.PadRight(spaceBetweenSprites) + dir);
        }

        Console.WriteLine("");
        Console.WriteLine("    [PLAYER]                      [ENEMY]");
        Console.WriteLine("------------------------------------------------------");
        
        string menuIndent = choosingPlayer ? leftMargin : leftMargin + "".PadRight(spaceBetweenSprites);

        for (int i = 0; i < lista.Length; i++)
        {
            if (i == sel)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine(menuIndent + $"> [{lista[i]._name}] <");
                Console.ResetColor();
            }
            else
            {
                Console.WriteLine(menuIndent + $"   {lista[i]._name}");
            }
        }
    }
    
    static string[] SpritePokebola()
    {
        return new string[]
        {
            "   .---.   ",
            "  /     \\  ",
            " |  -*-  | ",
            "  \\     /  ",
            "   '---'   "
        };
    }
}
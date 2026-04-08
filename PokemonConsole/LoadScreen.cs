namespace PokemonConsole;

public class LoadScreen
{
    public void DisplayLoadingScreen()
    {
        Console.Clear();
        
        string loadMsg = "PREPARING BATTLE ARENA...";

        for (int i = 0; i < 4; i++) Console.WriteLine();
        
        Console.WriteLine(loadMsg.PadLeft(7));
        Console.WriteLine();
        
        int sizeMax = 25;
        
        Console.SetCursorPosition(0, Console.CursorTop);
        Console.Write("[");
        
        Console.ForegroundColor = ConsoleColor.Cyan;
        
        for (int i = 0; i < sizeMax; i++)
        {
            Console.Write("█");
            Thread.Sleep(50); 
        }
        
        Console.ResetColor();
        Console.Write("]"); 
        
        Console.WriteLine("\n\nLOADING COMPLETE!");
        
        Thread.Sleep(1200); 
    }
}
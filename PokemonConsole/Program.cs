using  System;
using System.Threading;
using PokemonConsole;

class Program
{
    static void Main()
    {
        Console.CursorVisible = false;
        Console.Clear();

        Pokemon[] listaPokemons = new[] { PokemonList(0), PokemonList(1), PokemonList(2) };
        
        PokemonSelection selection = new PokemonSelection();
        LoadScreen loadScreen = new LoadScreen();
        
        int selPlayer = 0;
        while(true) {
            PokemonSelection.DrawnSelection(selPlayer, listaPokemons, true, null);
            var key = Console.ReadKey(true).Key;
            if(key == ConsoleKey.Enter) break;
            if(key == ConsoleKey.UpArrow) selPlayer = (selPlayer == 0) ? 2 : selPlayer - 1;
            if(key == ConsoleKey.DownArrow) selPlayer = (selPlayer == 2) ? 0 : selPlayer + 1;
        }
        Pokemon player = PokemonList(selPlayer);

        Thread.Sleep(100);

        int selEnemy = 0;
        while(true) {
            PokemonSelection.DrawnSelection(selEnemy, listaPokemons, false, player);
            var tecla = Console.ReadKey(true).Key;
            if(tecla == ConsoleKey.Enter) break;
            if(tecla == ConsoleKey.UpArrow) selEnemy = (selEnemy == 0) ? 2 : selEnemy - 1;
            if(tecla == ConsoleKey.DownArrow) selEnemy = (selEnemy == 2) ? 0 : selEnemy + 1;
        }
        
        Pokemon enemy = PokemonList(selEnemy);
        
        Thread.Sleep(100);
        
        // End of selection

        player = PokemonList(selPlayer);
        enemy = PokemonList(selEnemy);
        
        loadScreen.DisplayLoadingScreen(); // Loading screen
        
        DrawnScree(player, enemy, "The battle has begun!", false, false); 
        Console.WriteLine("\nPress any key to fight...");
        Console.ReadKey();

        while (player._currentHp > 0 && enemy._currentHp > 0)
        {
            player.ProcessTurnEffects(); 
            if (player._currentHp <= 0) break;
            
            if (player.CanAttack())
            {
                int sel = BattleMenu(player, enemy);
                ExecuteAttack(player, enemy, player._scams[sel], true);
            }
            Thread.Sleep(1000);

            enemy.ProcessTurnEffects();
            if (enemy._currentHp <= 0) break;

            if (enemy.CanAttack())
            {
                Random rand = new Random();
                int attack = rand.Next(enemy._scams.Length);
                ExecuteAttack(enemy, player, enemy._scams[attack], false);
            }
            Thread.Sleep(1000);
            
            if (player._currentHp <= 0) break;
            if (enemy._currentHp <= 0) break;
        }

        int selOptions = 0;
        DrawnOptionsMenu(selOptions, player._currentHp > 0 ? player : enemy, player._currentHp > 0);
    }
    
    static void DrawnOptionsMenu(int sel, Pokemon p, bool win)
    {
        while(true) {
            Console.Clear();
            Console.WriteLine("-------------------------------------------------");
            string leftMargin = "                     ";
        
            for (int line = 0; line < 4; line++)
            {
                string dir = (line < p._sprite.Length) ? p._sprite[line] : "";
                Console.WriteLine(leftMargin + dir);
            }
        
            Console.WriteLine();
        
            string title = win ? "You Win!" : "You were defeated!";
            Console.WriteLine($"----------------- {title.ToUpper()} -----------------");
            Console.WriteLine();

            string[] menuOptions = new[] { "Back to Selection", "Quit Game" }; 
            
            for (int i = 0; i < menuOptions.Length; i++)
            {
                if (i == sel)
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine(leftMargin + $"> [{menuOptions[i]}] <");
                    Console.ResetColor();
                }
                else
                {
                    Console.WriteLine(leftMargin + $"   {menuOptions[i]}");
                }
            }
            
            var tecla = Console.ReadKey(true).Key;
            if(tecla == ConsoleKey.Enter) break;
            if(tecla == ConsoleKey.UpArrow) sel = (sel == 0) ? 1 : sel - 1;
            if(tecla == ConsoleKey.DownArrow) sel = (sel == 1) ? 0 : sel + 1;
        }
        
        switch (sel)
        {
            case 0: Main(); break;
            case 1: 
                Console.Clear();
                Console.WriteLine("\n PROGRAM TERMINATED!");
                Environment.Exit(0); 
                break;
        }
    }

    static int BattleMenu(Pokemon p, Pokemon i)
    {
        int sel = 0;
        while (true)
        {
            DrawnScree(p, i, null);
            Console.WriteLine("\n SELECT A MOVE:");

            for (int j = 0; j < p._scams.Length; j++)
            {
                if (j == sel)
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    string linhaOpcao = $"> [{p._scams[j]._name}] <";
                    
                    Console.Write(linhaOpcao.PadRight(25));

                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine($"│ Info: {p._scams[j]._description}");
                    Console.ResetColor();
                }
                else
                {
                    Console.WriteLine($"  {p._scams[j]._name}");
                }
            }

            var key = Console.ReadKey(true).Key;
            int count = p._scams.Length;
            
            if (key == ConsoleKey.UpArrow) 
                sel = (sel - 1 + count) % count;
            if (key == ConsoleKey.DownArrow) 
                sel = (sel + 1) % count;
            
            if (key == ConsoleKey.Enter) 
                return sel;
        }
    }

    static void ExecuteAttack(Pokemon cause, Pokemon target, Pokemon.Scams scam, bool isPlayer)
    {
        Pokemon player = isPlayer ? cause : target;
        Pokemon enemy = isPlayer ? target : cause;
        
        for (int i = 0; i < 3; i++)
        {
            DrawnScree(player, enemy, $"{cause._name} used {scam._name}!", !isPlayer, isPlayer);
            Thread.Sleep(150);
            
            DrawnScree(player, enemy, $"{cause._name} used {scam._name}!", false, false);
            Thread.Sleep(150);
        }

        int totalDamage = cause._attack + scam._damage;
        target.TakeDamage(totalDamage);
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($" -> caused {totalDamage} damage!");

        switch (scam._effect)
        {
            case Pokemon.AttackEffect.Cure:
                int cure = 15;
                cause._currentHp = Math.Min(cause._hpMax, cause._currentHp + cure);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($" -> restored {cure} HP!"); 
            break;
            case Pokemon.AttackEffect.Drain:
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine(" -> The move applied a draining effect!");
                target.ApplySeed(scam._durationEffect);
                break;
            case Pokemon.AttackEffect.Paralyze:
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine(" -> The move paralyzed the opponent!");
                target.ApplyParalysis(scam._durationEffect);
            break;
            case Pokemon.AttackEffect.Burn:
                target.ApplyBurn(scam._durationEffect);
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(" -> The move caused a burn!");
            break;
            case Pokemon.AttackEffect.IgnoreDefense:
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine(" -> The move pierced through defenses!");
                target.TakeDamage(4);
            break;
        }
        
        Console.ResetColor();
        Thread.Sleep(1200);
    }

    static void DrawnScree(Pokemon p, Pokemon i, string msg, bool hidePlayer = false, bool hideEnemy = false)
    {
        Console.Clear();
        Console.WriteLine("-------------------------------------------------");

        for (int line = 0; line < 4; line++)
        {
            string pLine = hidePlayer? "      " : p._sprite[line];
            string iLine = hideEnemy? "      " : i._sprite[line];
            Console.WriteLine(pLine.PadRight(40) + iLine);
        }
        
        Console.WriteLine("-------------------------------------------------");
        
        ShowStats(p);
        ShowStats(i);
        
        Console.WriteLine("-------------------------------------------------");
        if(!string.IsNullOrEmpty(msg)) Console.WriteLine($"\n {msg}");
    }

    static void ShowStats(Pokemon p)
    {
        Console.Write($"{p._name.PadRight((12))} HP: [");
        float perc = (float) p._currentHp / p._hpMax;
        int block = (int)(perc * 10);

        if (perc > 0.5) Console.ForegroundColor = ConsoleColor.Green;
        else if(perc > 0.2)  Console.ForegroundColor = ConsoleColor.Yellow;
        else Console.ForegroundColor = ConsoleColor.Red;
        for (int i = 0; i < 10; i++)
        {
            Console.Write(i < block? "█" : "-");
        }
        
        Console.ResetColor();
        Console.WriteLine($"] {p._currentHp}/{p._hpMax}");
    }

    static Pokemon PokemonList(int index)
    {
        switch (index)
        {
            case 0 :
                Pokemon.Scams thunderShock = new Pokemon.Scams("Thunder Shock", "Throws lightning bolts. High paralysis chance.", 12, Pokemon.AttackEffect.Paralyze, 1);
                Pokemon.Scams quickAttack = new Pokemon.Scams("Quick Attack", "A simple and fast physical attack!", 8);
                Pokemon.Scams ironTail = new Pokemon.Scams("Iron Tail", "Spins and hits with a hardened tail!", 15);
                
                return new (
                    "Pikachu", 
                    90, 
                    22, 
                    PokemonSprites(index), 
                    new[] { thunderShock, quickAttack, ironTail }
                );
            case 1:
            {
                Pokemon.Scams ember = new Pokemon.Scams("Ember", "Spits out small flames!", 12, Pokemon.AttackEffect.Burn, 8);
                Pokemon.Scams scratch = new Pokemon.Scams("Scratch", "Uses sharp claws to cut!", 10);
                Pokemon.Scams growl = new Pokemon.Scams("Growl", "A breath of energy that ignores defenses!", 18, Pokemon.AttackEffect.IgnoreDefense);
                
                return new (
                    "Charmander", 
                    100, 
                    18, 
                    PokemonSprites(index), 
                    new[] { ember, scratch, growl }
                );
            }
            case 2:
            {
                Pokemon.Scams vineWhip = new Pokemon.Scams("Vine Whip", "Attacks with green whips. Damage: 22", 10);
                Pokemon.Scams razorLeaf = new Pokemon.Scams("Razor Leaf", "Throws sharp leaves. Damage: 15", 15);
                Pokemon.Scams leechSeed = new Pokemon.Scams("Leech Seed", "Heals the user while dealing damage.", 8,
                    Pokemon.AttackEffect.Drain, 5);

                return new(
                    "Bulbasaur",
                    120,
                    14,
                    PokemonSprites(index),
                    new[] { vineWhip, razorLeaf, leechSeed }
                );
            }
        }

        return new Pokemon();
    }

    static string[] PokemonSprites(int index)
    {
        switch (index)
        {
            case 0: return new string[]
            {
                "  \\|/  ",
                "  o.o  ",
                " > ^ < ",
                " /   \\ "
            }; break;
            case 1: return new string[]
            {
                "  (o.o) ",
                "  /|_|\\ ",
                "  \\ / ",
                " (__) "
            }; break;
            case 2: return new string[]
            {
                "  .---. ",
                " ( o.o )",
                "  |___| ",
                " /     \\"
            }; break;
        }
        
        return null;
    }
}
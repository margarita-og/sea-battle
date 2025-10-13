using System;
namespace SeaBattle
{
    class Program
    {
        static void PrintField(char[,] field)
        {
            Console.WriteLine("    A B C D E F G H I J");
            Console.WriteLine("  ┌─────────────────────┐");
            for (int i = 0; i < 10; i++)
            {
                Console.Write($"{i + 1,2}│ ");

                for (int j = 0; j < 10; j++)
                {
                    Console.Write(field[i, j] + " ");
                }

                Console.WriteLine("│");
            }

            Console.WriteLine("  └─────────────────────┘");
        }

        static void PrintBothFields(char[,] field1, char[,] field2)
        {
            Console.WriteLine("    Поле первого игрока        Поле второго игрока\n");
            Console.WriteLine("    A B C D E F G H I J        A B C D E F G H I J");
            Console.WriteLine("  ┌─────────────────────┐    ┌─────────────────────┐");
            for (int i = 0; i < 10; i++)
            {
                Console.Write($"{i + 1,2}│ ");

                for (int j = 0; j < 10; j++)
                {
                    Console.Write(field1[i, j] + " ");
                }

                Console.Write("│");

                Console.Write($"  {i + 1,2}│ ");

                for (int j = 0; j < 10; j++)
                {
                    Console.Write(field2[i, j] + " ");
                }

                Console.WriteLine("│");
            }

            Console.WriteLine("  └─────────────────────┘    └─────────────────────┘");
        }

        static void EditField(string coord, char[,] fieldWithShips, char[,] fieldInGame)
        {
            int savedX = Console.CursorLeft;
            int savedY = Console.CursorTop;

            char letter = coord[0];
            string numberStr = coord.Substring(1);
            int number = int.Parse(numberStr);
            int x, y;
            if (fieldWithShips[number - 1, letter - 'A'] == 's')
            {
                fieldInGame[number - 1, letter - 'A'] = 'X';
                x = 2 * (letter - 'A') + 4;
                y = number - 1 + 4;
                Console.SetCursorPosition(x, y);
                Console.Write('X');
            }
            else
            {
                fieldInGame[number - 1, letter - 'A'] = 'O';
                x = 2 * (letter - 'A') + 4;
                y = number - 1 + 4;
                Console.SetCursorPosition(x, y);
                Console.Write('O');
            }
            Console.SetCursorPosition(savedX, savedY);
        }

        static bool PlayerMoveStatus(string coord, char[,] fieldWithShips)
        {
            char letter = coord[0];
            string numberStr = coord.Substring(1);
            int number = int.Parse(numberStr);
            if (fieldWithShips[number - 1, letter - 'A'] == 's')
            {
                return true;
            }
            return false;
        }

        static bool ProcessingPlayersMove(char [,] fieldWithShips, char [,] fieldInGame, int defaultX, int defaultY)
        {
            string coordinate = Console.ReadLine();
            int currentX;
            EditField(coordinate, fieldWithShips, fieldInGame);
            if (PlayerMoveStatus(coordinate, fieldWithShips))
            {
                Console.SetCursorPosition(defaultX, defaultY);
                Console.Write("Попал! ");
                currentX = Console.CursorLeft;
                Console.SetCursorPosition(currentX, defaultY);
            }
            else
            {
                Console.SetCursorPosition(defaultX, defaultY);
                Console.Write("Промах. ");
                currentX = Console.CursorLeft;
                Console.SetCursorPosition(currentX, defaultY);
            }
            if (PlayerMoveStatus(coordinate, fieldWithShips))
            {
                return true;
            }
            return false;
        }
        
        static void SetShipBound(char[,] field)
        {
            for (int i = 0; i < 10; i++)
            {
                if (field[i, 1] == 's') field[i, 0] = 'x';
                for (int j = 1; j < 9; j++)
                {
                    if (field[i, j] == 's') continue;
                    if (field[i, j - 1] == 's' || field[i, j + 1] == 's') field[i, 0] = 'x';
                }
                if (field[i, 8] == 's') field[i, 9] = 'x';
            }
        }
        static void CleanFromBounds(char[,] field)
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; i < 10; i++)
                {
                    if (field[i, j] != 's') field[i, j] = '~';
                }
            }
        }
        static void CoordsIntoField(char[,] field, string input)
        {
            string[] coordinates = input.Split(' ');
            foreach (string coord in coordinates)
            {
                char letter = coord[0];
                string numberStr = coord.Substring(1);
                int number = int.Parse(numberStr);
                if (field[number - 1, letter - 'A'] != 'x')
                {
                    field[number - 1, letter - 'A'] = 's';
                }
            }
        }

        static void FieldSetup(char[,] field)
        {
            Console.WriteLine("Укажите через пробел координаты клеток, где будет находиться корабль из четырёх клеток\n");
            PrintField(field);
            string input = Console.ReadLine();
            CoordsIntoField(field, input);
            Console.Clear();
            Console.WriteLine("Укажите через пробел координаты клеток, где будет находиться первый корабль из трёх клеток, а также координаты второго на новой строке\n");
            PrintField(field);
            for (int i = 0; i < 2; i++)
            {
                input = Console.ReadLine();
                CoordsIntoField(field, input);
            }
            Console.Clear();
            Console.WriteLine("Укажите через пробел координаты клеток, где будет находиться первый корабль из двух клеток, а также координаты второго и третьего на новых строчках\n");
            PrintField(field);
            for (int i = 0; i < 3; i++)
            {
                input = Console.ReadLine();
                CoordsIntoField(field, input);
            }
            Console.Clear();
            Console.WriteLine("Укажите через пробел координаты клеток, где будет находиться первый корабль из одной клетки, а также координаты второго, третьего и четвёртого на новых строчках\n");
            PrintField(field);
            for (int i = 0; i < 4; i++)
            {
                input = Console.ReadLine();
                CoordsIntoField(field, input);
            }
            Console.Clear();
            Console.WriteLine("Ваше поле выглядит так:\n");
            PrintField(field);
        }
        static void Main(string[] args)
        {
            Console.Clear();
            Console.WriteLine("Добро пожаловать в игру морской бой!");
            Console.WriteLine("Передайте устройство первому игроку для заполнения поля кораблями\n");
            Console.WriteLine("Для продолжения нажмите Enter");
            Console.ReadLine();
            Console.Clear();
            char[,] firstPlayerField = new char[10, 10];
            char[,] secondPlayerField = new char[10, 10];
            char[,] firstPlayerFieldInGame = new char[10, 10];
            char[,] secondPlayerFieldInGame = new char[10, 10];
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    firstPlayerField[i, j] = '~';
                    secondPlayerField[i, j] = '~';
                    firstPlayerFieldInGame[i, j] = '~';
                    secondPlayerFieldInGame[i, j] = '~';
                }
            }
            //FieldSetup(firstPlayerField);
            Console.WriteLine("Нажмите Enter и передайте устройство второму игроку");
            Console.ReadLine();
            Console.Clear();
            //FieldSetup(secondPlayerField);
            PrintBothFields(firstPlayerFieldInGame, secondPlayerFieldInGame);
            bool firstPlayerTurn = false;

            int defaultX = Console.CursorLeft;
            int defaultY = Console.CursorTop;
            int currentX;
            while (true)
            {
                string coordinate;
                if (firstPlayerTurn)
                {
                    Console.WriteLine("Ход первого игрока, введите координату, куда будете бить");
                    if (!ProcessingPlayersMove(secondPlayerField, secondPlayerFieldInGame, defaultX, defaultY))
                    {
                        firstPlayerTurn = false;
                    }
                }
                else
                {
                    Console.WriteLine("Ход второго игрока, введите координату, куда будете бить");
                    if (!ProcessingPlayersMove(firstPlayerField, firstPlayerFieldInGame, defaultX, defaultY))
                    {
                        firstPlayerTurn = true;
                    }
                }
                // break;
            }
        }
    }
}


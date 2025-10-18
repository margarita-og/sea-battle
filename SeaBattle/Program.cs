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
                    if (field[i, j] == 's')
                    {
                        Console.Write("■ ");
                    }
                    else
                    {
                        Console.Write(field[i, j] + " ");
                    }
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

        static void EditField(int playersNumber, string coord, char[,] fieldWithShips, char[,] fieldInGame)
        {
            int savedX = Console.CursorLeft;
            int savedY = Console.CursorTop;

            char letter = coord[0];
            string numberStr = coord.Substring(1);
            int number = int.Parse(numberStr);
            int x_coord = number - 1;
            int y_coord = letter - 'A';
            int x, y;
            if (playersNumber == 1)
            {
                x = 2 * (y_coord) + 4;
                y = x_coord + 4;
            }
            else
            {
                x = 27 + 2 * (y_coord) + 4;
                y = x_coord + 4;
            }
            Console.SetCursorPosition(x, y);
            if (fieldWithShips[x_coord, y_coord] == 's')
            {
                fieldInGame[number - 1, letter - 'A'] = 'X';
                if (ShipDestroyed(x_coord, y_coord, fieldWithShips, fieldInGame))
                {
                    PrintDestroyedShip(playersNumber, x_coord, y_coord, fieldWithShips);
                }
                else
                {
                    Console.Write('X');
                }
            }
            else
            {
                fieldInGame[number - 1, letter - 'A'] = 'O';
                Console.Write('O');
            }

            Console.SetCursorPosition(savedX, savedY);
        }

        static string PlayerMoveStatus(string coord, char[,] fieldWithShips, char[,] fieldInGame)
        {
            char letter = coord[0];
            string numberStr = coord.Substring(1);
            int number = int.Parse(numberStr);
            int x_coord = number - 1;
            int y_coord = letter - 'A';
            if (fieldWithShips[x_coord, y_coord] == 's')
            {
                if (ShipDestroyed(x_coord, y_coord, fieldWithShips, fieldInGame))
                {
                    return "destroy";
                }
                return "hit";
            }
            return "miss";
        }

        static bool ProcessingPlayersMove(int playersNumber, char[,] fieldWithShips, char[,] fieldInGame, int defaultX, int defaultY)
        {
            int countOfLinesWithErrors = 0;
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(defaultX, defaultY+1);
            string coordinate = Console.ReadLine().ToUpper();
            string result = CheckingSingleCoordinate(coordinate);
            while (result != "ok")
            {
                Console.WriteLine(result);
                coordinate = Console.ReadLine().ToUpper();
                result = CheckingSingleCoordinate(coordinate);
                countOfLinesWithErrors += 2;
            }
            int currentX;
            EditField(playersNumber, coordinate, fieldWithShips, fieldInGame);
            Console.SetCursorPosition(defaultX, defaultY);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(defaultX, defaultY);
            if (PlayerMoveStatus(coordinate, fieldWithShips, fieldInGame) == "hit")
            {
                Console.Write("Попал! ");
                currentX = Console.CursorLeft;
                if (countOfLinesWithErrors != 0)
                {
                    for (int i = 2; i < countOfLinesWithErrors+2; i++)
                    {
                        Console.SetCursorPosition(defaultX, defaultY+i);
                        Console.Write(new string(' ', Console.WindowWidth));
                    }
                }
                
                Console.SetCursorPosition(currentX, defaultY);
                return true;
            }
            else if (PlayerMoveStatus(coordinate, fieldWithShips, fieldInGame) == "destroy")
            {
                Console.Write("Уничтожен! ");
                currentX = Console.CursorLeft;
                if (countOfLinesWithErrors != 0)
                {
                    for (int i = 2; i < countOfLinesWithErrors+2; i++)
                    {
                        Console.SetCursorPosition(defaultX, defaultY+i);
                        Console.Write(new string(' ', Console.WindowWidth));
                    }
                }
                Console.SetCursorPosition(currentX, defaultY);
                return true;
            }
            else
            {
                Console.Write("Промах. ");
                currentX = Console.CursorLeft;
                if (countOfLinesWithErrors != 0)
                {
                    for (int i = 2; i < countOfLinesWithErrors+2; i++)
                    {
                        Console.SetCursorPosition(defaultX, defaultY+i);
                        Console.Write(new string(' ', Console.WindowWidth));
                    }
                }
                Console.SetCursorPosition(currentX, defaultY);
                return false;
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
        static string CheckingSingleCoordinate(string input)
        {
            string acceptableLetters = "ABCDEFGHIJ";
            string[] coordinate = input.Split(' ');
            if (coordinate.Length > 1)
            {
                return "Введённая координата должна быть единственной";
            }
            string coord = coordinate[0];
            char letter = coord[0];
            string numberStr = coord.Substring(1);
            int number;
            if (int.TryParse(numberStr, out number))
            {
                if (!acceptableLetters.Contains(letter))
                {
                    return "Некорректный ввод, буква должна быть от A до J";
                }
                if (number <= 0 || number > 10)
                {
                    return "Некорректный ввод, число должно быть от 1 до 10";
                }
            }
            else
            {
                return "Некорректный ввод, введите координату вида A1-J10";
            }
            return "ok";
        }
        static string CheckingInput(string input, char[,] field, int lenOfShip)
        {
            string acceptableLetters = "ABCDEFGHIJ";
            List<(int x, int y)> inputCoordinates = new List<(int x, int y)>();
            string[] curShip = input.Split(' ');
            HashSet<string> uniqueCoordinates = new HashSet<string>(curShip);
            if (uniqueCoordinates.Count < lenOfShip)
            {
                return $"Вы ввели недостаточное количество координат для корбля длины {lenOfShip}, или они повторяются";
            }
            if (uniqueCoordinates.Count > lenOfShip)
            {
                return $"Вы ввели слишком много координат для корбля длины {lenOfShip}";
            }
            foreach (string coord in curShip)
            {
                char letter = coord[0];
                string numberStr = coord.Substring(1);
                int number;
                if (int.TryParse(numberStr, out number))
                {
                    if (!acceptableLetters.Contains(letter))
                    {
                        return "Некорректный ввод, буква должна быть от A до J";
                    }
                    if (number <= 0 || number > 10)
                    {
                        return "Некорректный ввод, число должно быть от 1 до 10";
                    }
                }
                else
                {
                    if (lenOfShip == 1)
                    {
                        return "Некорректный ввод, введите координату вида A1-J10";
                    }
                    return "Некорректный ввод, введите координаты вида A1-J10 через пробел";
                }
                inputCoordinates.Add((number - 1, letter - 'A'));

            }
            if (lenOfShip != 1)
            {
                int common_x = inputCoordinates[0].x;
                int common_x_count = 0;
                int common_y = inputCoordinates[0].y;
                int common_y_count = 0;
                foreach (var coord in inputCoordinates)
                {
                    if (coord.x == common_x)
                    {
                        common_x_count++;
                    }
                    if (coord.y == common_y)
                    {
                        common_y_count++;
                    }
                }
                if (!(common_x_count == lenOfShip || common_y_count == lenOfShip))
                {
                    return "Корабль должен целиком лежать в одном ряду или столбце, введите другие координаты";
                }
                if (common_x_count == lenOfShip)
                {
                    var sorted = inputCoordinates.OrderBy(coord => coord.x).ToList();
                    for (int i = 1; i < lenOfShip; i++)
                    {
                        if (sorted[i].y != sorted[i - 1].y + 1)
                        {
                            return "Между вашими координатами есть пустые клетки, введите другие";
                        }
                    }
                }
                else
                {
                    var sorted = inputCoordinates.OrderBy(coord => coord.y).ToList();
                    for (int i = 1; i < lenOfShip; i++)
                    {
                        if (sorted[i].x != sorted[i - 1].x + 1)
                        {
                            return "Между вашими координатами есть пустые клетки, введите другие";
                        }
                    }
                }
            }
            foreach (var coord in inputCoordinates)
            {
                for (int i = -1; i < 2; i++)
                {
                    for (int j = -1; j < 2; j++)
                    {
                        if (coord.x+i >= 0 && coord.x+i < 10 && coord.y+j >= 0 && coord.y+j < 10)
                        {
                            if (field[coord.x+i, coord.y+j] == 's')
                            {
                                return "Корабль не может быть построен по вашим координатам, так как он будет касаться другого вашего корабля, введите новые";
                            }
                        }
                    }
                }
            }
            return "ok";
        }

        static void FieldSetup(char[,] field)
        {
            Console.WriteLine("Укажите через пробел координаты клеток, где будет находиться корабль из четырёх клеток\n");
            PrintField(field);
            string input = Console.ReadLine().ToUpper();
            string checkResult = CheckingInput(input, field, 4);
            while (checkResult != "ok")
            {
                Console.WriteLine(checkResult);
                input = Console.ReadLine().ToUpper();
                checkResult = CheckingInput(input, field, 4);
            }
            CoordsIntoField(field, input);
            Console.Clear();
            Console.WriteLine("Укажите через пробел координаты клеток, где будет находиться первый корабль из трёх клеток, а также координаты второго на новой строке\n");
            PrintField(field);
            for (int i = 0; i < 2; i++)
            {
                input = Console.ReadLine().ToUpper();
                checkResult = CheckingInput(input, field, 3);
                while (checkResult != "ok")
                {
                    Console.WriteLine(checkResult);
                    input = Console.ReadLine().ToUpper();
                    checkResult = CheckingInput(input, field, 3);
                }
                CoordsIntoField(field, input);
            }
            Console.Clear();
            Console.WriteLine("Укажите через пробел координаты клеток, где будет находиться первый корабль из двух клеток, а также координаты второго и третьего на новых строчках\n");
            PrintField(field);
            for (int i = 0; i < 3; i++)
            {
                input = Console.ReadLine().ToUpper();
                checkResult = CheckingInput(input, field, 2);
                while (checkResult != "ok")
                {
                    Console.WriteLine(checkResult);
                    input = Console.ReadLine().ToUpper();
                    checkResult = CheckingInput(input, field, 2);
                }
                CoordsIntoField(field, input);
            }
            Console.Clear();
            Console.WriteLine("Укажите через пробел координаты клеток, где будет находиться первый корабль из одной клетки, а также координаты второго, третьего и четвёртого на новых строчках\n");
            PrintField(field);
            for (int i = 0; i < 4; i++)
            {
                input = Console.ReadLine().ToUpper();
                checkResult = CheckingInput(input, field, 1);
                while (checkResult != "ok")
                {
                    Console.WriteLine(checkResult);
                    input = Console.ReadLine().ToUpper();
                    checkResult = CheckingInput(input, field, 1);
                }
                CoordsIntoField(field, input);
            }
            Console.Clear();
            Console.WriteLine("Ваше поле выглядит так:\n");
            PrintField(field);
        }
        static List<(int x, int y)> CurrentShip(int x, int y, char[,] fieldWithShips)
        {
            List<(int x, int y)> currentShipCoordinates = new List<(int, int)>();
            int y_iterator = y;
            int x_iterator = x;
            while (y_iterator < 10)
            {
                if (fieldWithShips[x, y_iterator] == 's')
                {
                    currentShipCoordinates.Add((x, y_iterator));
                    y_iterator++;
                }
                else
                {
                    break;
                }
            }
            y_iterator = y - 1;
            while (y_iterator >= 0)
            {
                if (fieldWithShips[x, y_iterator] == 's')
                {
                    currentShipCoordinates.Add((x, y_iterator));
                    y_iterator--;
                }
                else
                {
                    break;
                }
            }
            x_iterator = x + 1;
            while (x_iterator < 10)
            {
                if (fieldWithShips[x_iterator, y] == 's')
                {
                    currentShipCoordinates.Add((x_iterator, y));
                    x_iterator++;
                }
                else
                {
                    break;
                }
            }
            x_iterator = x - 1;
            while (x_iterator >= 0)
            {
                if (fieldWithShips[x_iterator, y] == 's')
                {
                    currentShipCoordinates.Add((x_iterator, y));
                    x_iterator--;
                }
                else
                {
                    break;
                }
            }
            return currentShipCoordinates;
        }
        static void PrintDestroyedShip(int playersNumber, int x_coord, int y_coord, char [,] fieldWithShips)
        {
            List<(int x, int y)> currentShipCoordinates = CurrentShip(x_coord, y_coord, fieldWithShips);
            int x, y;
            foreach (var coord in currentShipCoordinates)
            {
                if (playersNumber == 1)
                {
                    x = 2 * (coord.y) + 4;
                    y = coord.x + 4;
                }
                else
                {
                    x = 27 + 2 * (coord.y) + 4;
                    y = coord.x + 4;
                }
                Console.SetCursorPosition(x, y);
                Console.Write('#');
            }
            foreach (var coord in currentShipCoordinates)
            {
                for (int i = -1; i < 2; i++)
                {
                    for (int j = -1; j < 2; j++)
                    {
                        if (coord.x + i >= 0 && coord.x + i < 10 && coord.y + j >= 0 && coord.y + j < 10)
                        {
                            if (fieldWithShips[coord.x + i, coord.y + j] != 's')
                            {
                                if (playersNumber == 1)
                                {
                                    x = 2 * (coord.y+j) + 4;
                                    y = coord.x+i + 4;
                                }
                                else
                                {
                                    x = 27 + 2 * (coord.y+j) + 4;
                                    y = coord.x+i + 4;
                                }
                                Console.SetCursorPosition(x, y);
                                Console.Write('O');
                            }
                        }
                    }
                }
            }
        }

        static bool ShipDestroyed(int x, int y, char [,] fieldWithShips, char [,] fieldInGame)
        {
            List<(int x, int y)> currentShipCoordinates = CurrentShip(x, y, fieldWithShips);
            foreach (var coord in currentShipCoordinates)
            {
                if (fieldInGame[coord.x, coord.y] != 'X' && fieldInGame[coord.x, coord.y] != '#')
                {
                    return false;
                }
            }
            foreach (var coord in currentShipCoordinates)
            {
                fieldInGame[coord.x, coord.y] = '#';
            }
            return true;
        }

        static int GameIsOver(char [,] firstFieldInGame, char [,] secondFieldInGame)
        {
            int FIELD_SIZE = 10;
            int hashtagCount1 = 0;
            int hashtagCount2 = 0;
            for (int i = 0; i < FIELD_SIZE; i++)
            {
                for (int j = 0; j < FIELD_SIZE; j++)
                {
                    if (firstFieldInGame[i, j] == '#')
                    {
                        hashtagCount1++;
                    }
                    if (secondFieldInGame[i, j] == '#')
                    {
                        hashtagCount2++;
                    }
                }
            }
            if (hashtagCount1 == 20)
            {
                return 2;
            }
            if (hashtagCount2 == 20)
            {
                return 1;
            }
            return 0;
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
            FieldSetup(firstPlayerField);
            Console.WriteLine("Нажмите Enter и передайте устройство второму игроку");
            Console.ReadLine();
            Console.Clear();
            FieldSetup(secondPlayerField);
            Console.WriteLine("Нажмите Enter, чтобы начать бой");
            Console.ReadLine();
            Console.Clear();
            PrintBothFields(firstPlayerFieldInGame, secondPlayerFieldInGame);
            bool firstPlayerTurn = true;

            int defaultX = Console.CursorLeft;
            int defaultY = Console.CursorTop;
            int currentX;
            while (true)
            {
                string coordinate;
                if (firstPlayerTurn)
                {
                    Console.WriteLine("Ход первого игрока, введите координату, куда будете бить");
                    if (!ProcessingPlayersMove(2, secondPlayerField, secondPlayerFieldInGame, defaultX, defaultY))
                    {
                        firstPlayerTurn = false;
                    }
                }
                else
                {
                    Console.WriteLine("Ход второго игрока, введите координату, куда будете бить");
                    if (!ProcessingPlayersMove(1, firstPlayerField, firstPlayerFieldInGame, defaultX, defaultY))
                    {
                        firstPlayerTurn = true;
                    }
                }
                int result = GameIsOver(firstPlayerFieldInGame, secondPlayerFieldInGame);
                if (result != 0)
                {
                    if (result == 1)
                    {
                        Console.SetCursorPosition(defaultX, defaultY);
                        Console.WriteLine("Первый игрок победил!");
                    }
                    else
                    {
                        Console.SetCursorPosition(defaultX, defaultY);
                        Console.WriteLine("Второй игрок победил!");
                    }
                    break;
                }
            }
        }
    }
}
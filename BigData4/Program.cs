using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
    static void Main()
    {
        // Ініціалізація матриць
        int[,] matrixA = { { 1, 2, 3 }, { 4, 5, 6 } };
        int[,] matrixB = { { 7, 8 }, { 9, 10 }, { 11, 12 } };

        // Перетворення матриць у формат для MapReduce
        List<KeyValuePair<int, int>> mapInput = MapInput(matrixA, matrixB);

        // Застосування MapReduce
        List<KeyValuePair<int, int>> reduceOutput = MapReduce(mapInput, matrixA, matrixB);

        // Виведення результату
        Console.WriteLine("Результат матриці:");
        PrintMatrix(reduceOutput);

        Console.ReadKey();
    }

    // Генерує список пар індексів для кожної комбінації рядка і стовпця матриці
    static List<KeyValuePair<int, int>> MapInput(int[,] matrixA, int[,] matrixB)
    {
        List<KeyValuePair<int, int>> mapInput = new List<KeyValuePair<int, int>>();

        for (int i = 0; i < matrixA.GetLength(0); i++)
        {
            for (int j = 0; j < matrixB.GetLength(1); j++)
            {
                mapInput.Add(new KeyValuePair<int, int>(i, j));
            }
        }

        return mapInput;
    }

    // Застосовує функцію Map до кожної пари індексів та отримує результати
    static List<KeyValuePair<int, int>> MapReduce(List<KeyValuePair<int, int>> mapInput, int[,] matrixA, int[,] matrixB)
    {
        // Застосування Map
        var mapOutput = mapInput.Select(item => Map(item, matrixA, matrixB));

        // Групування за ключем та застосування Reduce
        var reduceOutput = mapOutput.GroupBy(item => item.Key)
                                    .Select(group => Reduce(group.Key, group.Select(item => item.Value)));

        return reduceOutput.ToList();
    }

    // Виконує множення матриці A на матрицю B за заданими індексами
    static KeyValuePair<int, int> Map(KeyValuePair<int, int> input, int[,] matrixA, int[,] matrixB)
    {
        int row = input.Key;
        int col = input.Value;

        int sum = 0;
        for (int k = 0; k < matrixA.GetLength(1); k++)
        {
            sum += matrixA[row, k] * matrixB[k, col];
        }

        return new KeyValuePair<int, int>(row, sum);
    }

    // Виконує сумування значень за однаковим ключем (індексом рядка)
    static KeyValuePair<int, int> Reduce(int key, IEnumerable<int> values)
    {
        int sum = values.Sum();
        return new KeyValuePair<int, int>(key, sum);
    }

    // Виводить результат у вигляді матриці
    static void PrintMatrix(List<KeyValuePair<int, int>> matrix)
    {
        foreach (var item in matrix)
        {
            Console.WriteLine($"Рядок: {item.Key}, Значення: {item.Value}");
        }
    }
}
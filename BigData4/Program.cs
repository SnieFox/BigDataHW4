using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
    static void Main()
    {
        // Задаємо розміри матриць
        int[,] matrixA = { { 1, 2 }, { 3, 4 } };
        int[,] matrixB = { { 5, 6 }, { 7, 8 } };

        // Викликаємо MapReduce для множення матриць
        int[,] result = MapReduceMatrixMultiply(matrixA, matrixB);

        // Виводимо результат
        Console.WriteLine("Result Matrix:");
        PrintMatrix(result);
    }

    static int[,] MapReduceMatrixMultiply(int[,] matrixA, int[,] matrixB)
    {
        int rowsA = matrixA.GetLength(0);
        int colsA = matrixA.GetLength(1);
        int rowsB = matrixB.GetLength(0);
        int colsB = matrixB.GetLength(1);

        if (colsA != rowsB)
        {
            throw new InvalidOperationException("Неможливо виконати множення матриць. Кількість стовпців першої матриці не дорівнює кількості рядків другої.");
        }

        // Створюємо список для результатів
        List<Tuple<int, int, int>> intermediateResults = new List<Tuple<int, int, int>>();

        // Map: Генеруємо проміжні значення для кожної пари (i, k, A[i, k] * B[k, j])
        for (int i = 0; i < rowsA; i++)
        {
            for (int j = 0; j < colsB; j++)
            {
                for (int k = 0; k < colsA; k++)
                {
                    intermediateResults.Add(Tuple.Create(i, j, matrixA[i, k] * matrixB[k, j]));
                }
            }
        }

        // Reduce: Сумуємо проміжні значення для кожної пари (i, j)
        var groupedResults = intermediateResults.GroupBy(tuple => new { tuple.Item1, tuple.Item2 });

        // Створюємо результуючу матрицю
        int[,] resultMatrix = new int[rowsA, colsB];

        foreach (var group in groupedResults)
        {
            int i = group.Key.Item1;
            int j = group.Key.Item2;
            int sum = group.Sum(tuple => tuple.Item3);
            resultMatrix[i, j] = sum;
        }

        return resultMatrix;
    }

    static void PrintMatrix(int[,] matrix)
    {
        int rows = matrix.GetLength(0);
        int cols = matrix.GetLength(1);

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                Console.Write(matrix[i, j] + " ");
            }
            Console.WriteLine();
        }
    }
}
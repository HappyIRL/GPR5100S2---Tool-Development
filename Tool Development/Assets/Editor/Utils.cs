using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils
{
	public static Transform[,] TransposeArray(Transform[,] input)
	{
		Transform[,] result = new Transform[input.GetLength(0), input.GetLength(1)];
		for (int x = 0; x < input.GetLength(0); x++)
		{
			for (int y = 0; y < input.GetLength(1); y++)
			{
				result[x, y] = input[y, x];
			}
		}
		return result;
	}
}

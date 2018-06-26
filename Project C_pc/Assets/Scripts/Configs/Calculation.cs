using System.Collections;
using UnityEngine;
public class Calculation
{
	public Calculation ()
	{
	}

	public static int ArrayMin(int[][] nums){
		int min=nums[0][0];
		for (int i = 0; i < nums.Length; i++) {
			for (int j = 0; j < nums [i].Length; j++) {
				if (nums [i] [j] < min)
					min = nums [i] [j];
			}
		}
		return min;
	}

	public static int GetMyRandomForSeed(int min,int max){
        int totalWeight=0;
        for (int i = 0; i < max; i++)
        {
			if (i >= min)
				totalWeight += Configs.levelWeight [i];
			else
				totalWeight += Configs.levelWeight [min + 3];
        }
        int r = Random.Range(0, totalWeight);
        totalWeight = 0;
        for (int i = 0; i < max; i++)
        {
			if (i >= min)
				totalWeight += Configs.levelWeight [i];
			else
				totalWeight += Configs.levelWeight [min + 3];
            if (r <= totalWeight)
                return i;
        }
        return 0;
	}
}



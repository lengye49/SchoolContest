using System;


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
}



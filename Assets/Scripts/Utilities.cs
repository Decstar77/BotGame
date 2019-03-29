using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utilities{

	public static float Norimalize(float value, float min, float max)
	{
		return (value - min) / (max - min);
	}

}

using UnityEngine;
using System.Collections;

/**
 * 2014.9
 * @author Xiangyu Guo
 * */
public class City : MonoBehaviour
{

		private string name;
		private double tempture;
	
		public City (string name, double tempture)
		{
				this.name = name;
				this.tempture = tempture;
		}

		public Color getWeatherColor ()
		{
		
				Color color = Constants.c_0;
		
				if (tempture > 0) {
			
						if (tempture > 0 && tempture < 4)
								color = Constants.c_4;
						else if (tempture >= 4 && tempture < 8) 
								color = Constants.c_8;
						else if (tempture >= 8 && tempture < 12) 
								color = Constants.c_12;
						else if (tempture >= 12 && tempture < 16) 
								color = Constants.c_16;
						else if (tempture >= 16 && tempture < 20) 
								color = Constants.c_20;
						else if (tempture >= 20 && tempture < 24) 
								color = Constants.c_24;
						else if (tempture >= 24 && tempture < 28) 
								color = Constants.c_28;
						else if (tempture >= 28 && tempture < 32) 
								color = Constants.c_32;
						else if (tempture >= 32 && tempture < 36) 
								color = Constants.c_36;
						else if (tempture >= 36 && tempture < 40) 
								color = Constants.c_40;
						else 
								color = Constants.c_44;
				}
				if (tempture < 0) {
			
						if (tempture >= -4 && tempture < 0)
								color = Constants.c_0_4;
						else if (tempture >= -8 && tempture < -4) 
								color = Constants.c_0_8;
						else if (tempture >= -12 && tempture < -8) 
								color = Constants.c_0_12;
						else if (tempture >= -16 && tempture < -12) 
								color = Constants.c_0_16;
						else if (tempture >= -20 && tempture < -16) 
								color = Constants.c_0_20;
						else if (tempture >= -24 && tempture < -20) 
								color = Constants.c_0_24;
						else 
								color = Constants.c_0_28;
				}
		
				return color;
		}

		public string getName ()
		{
				return name;
		}

		public string getTempture ()
		{
				return "当前温度： " + this.tempture + "℃";
		}
	
		public Vector3 getSize ()
		{		
				return new Vector3 (0.1f, 0.1f, 0.1f);
		}

		public Vector3 getScaleSize (Transform transform)
		{		
				return transform.localScale * 0.5f;
		}

		// Use this for initialization
		void Start ()
		{
	
		}
	
		// Update is called once per frame
		void Update ()
		{
	
		}
}

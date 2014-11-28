using UnityEngine;
using System.Collections;

/**
 * 2014.9
 * @author Xiangyu Guo
 * */
public class ChangeColor : MonoBehaviour
{
	
		public Vector3 oldScale = new Vector3 (0.1f, 0.1f, 0.1f);
		public bool isMouseOn;
		public string name;
		public string id;
		public string tempture;

		void OnMouseOver ()
		{
				isMouseOn = true;
		}

		void OnMouseExit ()
		{
				isMouseOn = false;
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

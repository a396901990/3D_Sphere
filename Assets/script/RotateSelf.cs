using UnityEngine;
using System.Collections;

/**
 * 2014.9
 * @author Xiangyu Guo
 * */
public class RotateSelf : MonoBehaviour
{
	
		//是否被拖拽//     
		private bool onDrag = false; 
		//旋转速度//    
		private float speed = 3f;  
		//阻尼速度//     
		private float tempSpeed; 
		//鼠标沿水平方向移动的增量//  
		private float axisX;  
		//鼠标沿竖直方向移动的增量//
		private float axisY;    
		//滑动距离（鼠标）
		private float cXY;

		//球体属性
		private float r = 10f;
		private float size = 2.2f;
		private float distance = 10f;
		public bool isCity = false;
		public ArrayList citys;
		public GameObject textObject;
		public Transform cameraTarget;

		void Start ()
		{
				citys = new ArrayList ();

				foreach (string cityName in Constants.pros) {
						citys.Add (new City (cityName, Random.Range (-10, 30)));
				}
				createSphere (citys.Count);
		}

		// 设置名称
		public void createSphere (float N)
		{

				float inc = Mathf.PI * (3 - Mathf.Sqrt (5));
				float off = 2 / N;
				for (int k = 0; k < (N); k++) {
						float y = k * off - 1 + (off / 2);
						float r = Mathf.Sqrt (1 - y * y);
						float phi = k * inc;
						Vector3 pos = new Vector3 ((Mathf.Cos (phi) * r * size), y * size, Mathf.Sin (phi) * r * size); 
			
						GameObject text = (GameObject)Instantiate (textObject, pos, Quaternion.identity);
						ChangeColor changeColor = text.GetComponent<ChangeColor> ();
						text.transform.parent = gameObject.transform;
			
						City city = citys [k] as City;
						TextMesh tm = (TextMesh)text.GetComponent<TextMesh> ();

						tm.text = city.getName ();
				

						tm.color = city.getWeatherColor ();
						text.transform.localScale = city.getSize ();
						text.GetComponent<ChangeColor> ().oldScale = text.transform.localScale;
		
						foreach (Transform child in text.transform) {
				

								TextMesh tm2 = (TextMesh)child.GetComponent<TextMesh> ();
								tm2.text = city.getTempture ();
								tm2.color = city.getWeatherColor ();
			
				
						}
				}
		}
	
		//鼠标移动的距离    
		void OnMouseDown ()
		{   
				//接受鼠标按下的事件// 
				axisX = 0f;
				axisY = 0f;   
		}

		//鼠标拖拽时的操作
		void OnMouseDrag ()
		{  
				onDrag = true;
				axisX = -Input.GetAxis ("Mouse X"); 
				axisY = Input.GetAxis ("Mouse Y");

				cXY = Mathf.Sqrt (axisX * axisX + axisY * axisY); 
				//计算鼠标移动的长度// 
				if (cXY == 0f) { 
						cXY = 1f; 
				}     
		}      

		//计算阻尼速度（鼠标）
		float Rigid ()
		{  
				if (onDrag) { 
						tempSpeed = speed;
				} else { 
						if (tempSpeed > 0) {  
								//通过除以鼠标移动长度实现拖拽越长速度减缓越慢    
								if (cXY != 0) {
										tempSpeed -= speed * 2 * Time.deltaTime / cXY; 
								}
						} else {
								tempSpeed = 0; 
						}         
				}  
				return tempSpeed; 
		}

		void Update ()
		{  

				// 计算角度,透明度,颜色
				foreach (Transform child in gameObject.transform) {
						child.LookAt (new Vector3 (cameraTarget.position.x, cameraTarget.position.y, -cameraTarget.position.z));
						float dis = Vector3.Distance (child.position, cameraTarget.transform.position);
						Color c = child.GetComponent<Renderer>().material.color;
						float alpha = Mathf.Abs (dis - r) / 5f + 0.1f;

						child.GetComponent<Renderer>().material.color = new Color (c.r, c.g, c.b, alpha);
			
						foreach (Transform cc in child.transform) {
								cc.GetComponent<Renderer>().material.color = new Color (c.r, c.g, c.b, alpha);
						}
			
				}
	
				// 找出选中对象
				foreach (Transform child in gameObject.transform) {
						ChangeColor changeColor = child.GetComponent<ChangeColor> ();
						float disC_Camera = Vector3.Distance (child.position, cameraTarget.position);
						float disCamera_P = Vector3.Distance (cameraTarget.position, transform.position);
						float disC_P = Vector3.Distance (child.position, transform.position);
						float dis = disCamera_P - disC_P;
	
						if ((disC_Camera > dis - 0.1f) && (disC_Camera < dis + 0.1f) && !onDrag && tempSpeed == 0f) {
								child.localScale = 1.5f * child.GetComponent<ChangeColor> ().oldScale;
						} else {
								child.localScale = child.GetComponent<ChangeColor> ().oldScale;	
						}
				}

				gameObject.transform.Rotate (new Vector3 (axisY, axisX, 0) * Rigid (), Space.World); 
	
				if (!Input.GetMouseButton (0)) { 
						onDrag = false; 
				}     
		} 
	
}
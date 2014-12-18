using UnityEngine;
using System.Collections;

/**
 * 2014.9
 * @author Xiangyu Guo
 * */
public class Camera : MonoBehaviour {

	private string cityIds;

	private int currentID = 0;

	private Hashtable proLabel;

	private Hashtable cityLabel;

	private string proName;

	private string cityName;

	public string labelText;

	public string labelText2;

	public string labelText3;

	public string labelLeftDetail;

	public string labelRightDetail;

	public Hashtable Labels;

	private RotateSelf mySphere;

	void Start () {
		cityLabel = Constants.initLabel (cityLabel);
		mySphere = GameObject.FindWithTag ("Sphere").GetComponent<RotateSelf> ();
		Screen.orientation = ScreenOrientation.Portrait;
		labelText3 = Constants.welcome;
		InvokeRepeating("setHelpLabel", 1,5);
	}

	// 回退
	void back(string str) {
		if(mySphere.isCity){
			mySphere.setNewCity(Constants.proIds, false);
			labelText3 = Constants.welcome;
		}
		labelLeftDetail = "";
		labelRightDetail = "";
	}

	// 详细
	void detail(string str) {

		string currentCity = mySphere.currentCity;
		string currentCityID = mySphere.currentCityID;
		bool isCity = mySphere.isCity;
		Hashtable allCityID = mySphere.allCityID;

		if (isCity)
		{
			mySphere.getDetail(currentCityID);
		}
		else
		{
			mySphere.setNewCity((string[])allCityID[currentCity],true);
		}
	}

	// 显示详细城市天气
	public void showDetail (Detail detail) {
		labelLeftDetail = detail.getWeatherDetail();
		labelRightDetail = detail.getIndexDetail();
	}
	
	void OnGUI() {
		GUIStyle aa=new GUIStyle();
		aa.normal.background = null;		    
		aa.alignment = TextAnchor.MiddleCenter;
		aa.normal.textColor=new Color(1,1,1); 
		aa.fontSize = 40;       
		GUI.Label(new Rect(0, 0, Screen.width, 150), labelText, aa);

		GUIStyle bb=new GUIStyle();
		bb.normal.background = null;		    
		bb.alignment = TextAnchor.MiddleCenter;
		bb.normal.textColor=new Color(1,1,1);   
		bb.fontSize = 20;       
		GUI.Label(new Rect(0, 0, Screen.width, 250), labelText2, bb);

		GUIStyle cc=new GUIStyle();
		cc.normal.background = null;		    
		cc.alignment = TextAnchor.MiddleCenter;
		cc.normal.textColor=new Color(1,1,1);   
		cc.fontSize = 20;       
		GUI.Label(new Rect(0, 0, Screen.width, 50), labelText3, cc);

		
		GUIStyle detailL=new GUIStyle();
		detailL.normal.background = null;		    
		detailL.alignment = TextAnchor.MiddleCenter;
		detailL.normal.textColor=new Color(1,1,1);   
		detailL.fontSize = 15;       
		GUI.Label(new Rect(0, 0, Screen.width/2, 300), labelLeftDetail, detailL);

		GUIStyle detailR=new GUIStyle();
		detailR.normal.background = null;		    
		detailR.alignment = TextAnchor.MiddleCenter;
		detailR.normal.textColor=new Color(1,1,1);   
		detailR.fontSize = 15;       
		GUI.Label(new Rect(Screen.width/2, 0, Screen.width/2, 300), labelRightDetail, detailR);
		// for test
		if(GUILayout.Button("详细",GUILayout.Height(50)))
		{
			detail("");
		}

		if(GUILayout.Button("后退",GUILayout.Height(50)))
		{
			back("");
		}
	}

	// Update is called once per frame
	void Update () {

		// 返回键
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			back("");
		}

		// Home键
		if (Input.GetKeyDown(KeyCode.Home) )
		{

		}
	}

	// 当前城市是否是省
	public bool isPro (string cityName) {
		
		foreach (string pro in Constants.pros) {
			if (cityName.Equals (pro)) {
				return true;
			}
		}
		
		return false;
	}

	IEnumerator wait(int seconds, string labelWord) {
		yield return new WaitForSeconds (seconds);
		labelText3 = labelWord;
	}

	public void setHelpLabel() {
		currentID = currentID == 3 ? 0 : ++currentID;
		labelText3 = (string)cityLabel[currentID];
	}

}

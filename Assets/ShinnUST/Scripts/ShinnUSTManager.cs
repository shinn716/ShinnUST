using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[RequireComponent(typeof(URGData))]
public class ShinnUSTManager : MonoBehaviour {

    #region URG Render
    [SerializeField]
    GameObject UrgDevice;
    Urg urg;
    #endregion

    Texture2D guiBackground;
    bool showGui = false;

    [Header("Touch Area")]
    public GameObject touchArea;
    [HideInInspector] public Vector2 TAoffPos;
    [HideInInspector] public Vector2 scaleRate;
    public static Vector4 touchArea_value;

    [Header("USTmode")]
    public KeyCode ShowSetting = KeyCode.Q;
    public string USTObjs_Name = "ust01";
    public bool Enable = false;
    public bool MainUST = true;
    public bool showMouse;

    public Vector2 guiPos = new Vector2(200, 600);
    public float step = .1f;
    public float step_rotate = .5f;
    public float step_scale = .01f;

    public GUIStyle _style;
    [Header("USTData")]
    public PlayerPrefsData data;

    //----call xml
    URGData _readXml;
    Vector3 urgScale;
    float px;
    float py;
    float scalex;
    float scaley;
    float rot;

    void Start () 
	{
        guiBackground = (Texture2D)Resources.Load("GUIBackground");
        _readXml = this.GetComponent<URGData> ();
		touchArea.GetComponent<SpriteRenderer> ().color = new Color (1, 1, 1, 1f);

		_readXml.init();


        if (!Enable) {
			
			urg = UrgDevice.GetComponent<Urg> ();
			urg.init ();

            data.urgPosOffsetx = PlayerPrefs.GetFloat(USTObjs_Name + " URG Offset X");
            data.urgPosOffsety = PlayerPrefs.GetFloat(USTObjs_Name + " URG Offset Y");
            data.urgScalex = PlayerPrefs.GetFloat(USTObjs_Name + " Scalex", data.urgScalex);
            data.urgScaley = PlayerPrefs.GetFloat(USTObjs_Name + " Scaley", data.urgScaley);

            data.urgRotate = PlayerPrefs.GetFloat(USTObjs_Name + " Rotate", data.urgRotate);
            
            data.guiScale = PlayerPrefs.GetInt(USTObjs_Name + " GUI Scale");

            urg.PosOffset = new Vector3(data.urgPosOffsetx, data.urgPosOffsety);
            urg._Scale = new Vector3(data.urgScalex, data.urgScaley);
            urg.Rotate = data.urgRotate;

            if (MainUST)
            {
                data.TAoffPosx = PlayerPrefs.GetFloat("Touch AreaPosX");
                data.TAoffPosy = PlayerPrefs.GetFloat("Touch AreaPosY");
                data.scaleRatex = PlayerPrefs.GetFloat("Touch Width");
                data.scaleRatey = PlayerPrefs.GetFloat("Touch Height");

                TAoffPos.x = data.TAoffPosx;
                TAoffPos.y = data.TAoffPosy;
                scaleRate.x = data.scaleRatex;
                scaleRate.y = data.scaleRatey;
            }
            
			urg.DrawMesh = false;

			if (urg != null) {
				urg.Connect ();
			}

			px = urg.PosOffset.x;
			py = urg.PosOffset.y;

			scalex = urgScale.x;
			scaley = urgScale.y;
            
            _style.fontSize = data.guiScale;

        } else {
			touchArea.GetComponent<SpriteRenderer> ().color = new Color (1, 1, 1, 0f);
		}
			
	}

	void Update () 
	{

        if (Input.GetKeyDown("/"))
        {
            showMouse = !showMouse;
        }

        if (showMouse)
            Cursor.visible = true;
        else
            Cursor.visible = false;


        if (Input.GetKeyDown(ShowSetting))
            showGui = !showGui;

        if (showGui)
        {

            if (Input.GetKeyDown(KeyCode.F8))
            {
                print("URG disconnect");
                urg.Disconnect();
            }
            if (Input.GetKeyDown(KeyCode.F7))
            {
                print("URG connect");
                urg.Connect();
            }
            if (Input.GetKeyDown(KeyCode.F12))
            {
                print("Default data.");
                Default();
            }




            if (!Enable && MainUST)
                touchAreaFuct();


            if (Input.GetKey(KeyCode.UpArrow))
            {
                py += step;
                Vector3 pos = new Vector3(urg.PosOffset.x, py, urg.PosOffset.z);
                urg.PosOffset = pos;
            }
            if (Input.GetKey(KeyCode.DownArrow))
            {
                py -= step;
                Vector3 pos = new Vector3(urg.PosOffset.x, py, urg.PosOffset.z);
                urg.PosOffset = pos;
            }
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                px -= step;
                Vector3 pos = new Vector3(px, urg.PosOffset.y, urg.PosOffset.z);
                urg.PosOffset = pos;
            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
                px += step;
                Vector3 pos = new Vector3(px, urg.PosOffset.y, urg.PosOffset.z);
                urg.PosOffset = pos;
            }


            if (Input.GetKey(KeyCode.Z))
            {
                scalex -= step_scale;
                Vector3 pos = new Vector3(scalex, urg._Scale.y, urg._Scale.z);
                urg._Scale = pos;
            }

            if (Input.GetKey(KeyCode.X))
            {
                scalex += step_scale;
                Vector3 pos = new Vector3(scalex, urg._Scale.y, urg._Scale.z);
                urg._Scale = pos;
            }



            if (Input.GetKey(KeyCode.A))
            {
                scaley -= step_scale;
                Vector3 pos = new Vector3(urg._Scale.x, scaley, urg._Scale.z);
                urg._Scale = pos;
            }

            if (Input.GetKey(KeyCode.S))
            {
                scaley += step_scale;
                Vector3 pos = new Vector3(urg._Scale.x, scaley, urg._Scale.z);
                urg._Scale = pos;
            }

            if (Input.GetKey(KeyCode.C))
            {
                rot -= step_rotate;
                urg.Rotate = rot;
            }

            if (Input.GetKey(KeyCode.V))
            {
                rot += step_rotate;
                urg.Rotate = rot;
            }

            if (Input.GetKeyDown("="))
            {
                step *= 1.25f;
                step_scale *= 1.25f;
                step_rotate *= 1.25f;
            }
            if (Input.GetKeyDown("-"))
            {
                step /= 1.25f;
                step_scale /= 1.25f;
                step_rotate /= 1.25f;
            }

        }


    }

	void touchAreaFuct()
	{
		touchArea.transform.position = new Vector3 (TAoffPos.x, TAoffPos.y, touchArea.transform.position.z);
		touchArea.transform.localScale = new Vector3 (scaleRate.x, scaleRate.y);

		float tax = touchArea.transform.position.x;
		float taw = (touchArea.GetComponent<SpriteRenderer> ().bounds.extents.x) ;

		float tay = touchArea.transform.position.y;
		float tah = (touchArea.GetComponent<SpriteRenderer> ().bounds.extents.y) ;

		touchArea_value = new Vector4 ( (tax-taw), (tax+taw), (tay+tah), (tay-tah) );
	}

	void OnGUI()
	{

		if (!Enable) {
		
			if (showGui) {
				touchArea.GetComponent<SpriteRenderer> ().color = Color.white;

				GUIStyle style = new GUIStyle ();
				GUIStyleState styleState = new GUIStyleState ();

			
				styleState.background = guiBackground;
				styleState.textColor = Color.white;
				style.normal = styleState;
               


                GUI.color = new Color (1, 1, 1, 0.7f);
				GUILayout.BeginArea (new Rect (guiPos.x, 0, guiPos.y, Screen.height), style);

				GUILayout.Space (20);
				GUILayout.Label (" URG Setting ('"+ ShowSetting +" ) ", _style);
                GUILayout.Label (" 'f7 UST Connect' 'f8 UST Disconnect' 'f12 Default' ) ", _style);

                var urgStatus = urg.IsConnected ? "Connected" : "Not Connected";

                _style.normal.textColor = Color.green;
                GUILayout.Label (" " + USTObjs_Name + " URG Status :  " + urgStatus, _style);

                _style.normal.textColor = Color.white;
                GUILayout.Label (" StartStep :  " + Urg._startstep + "  EndStep :  " + Urg._endstep, _style);
                GUILayout.Label(" StepCount : " + Urg._stepCount, _style);

                GUILayout.Space(20);
                
                GUILayout.Label(" GUI Sca : " + _style.fontSize.ToString(""), _style);
                _style.fontSize = (int) GUILayout.HorizontalSlider(_style.fontSize, 0, 60);

                GUILayout.Space (20);

				if (GUILayout.Button ("Show URG Data"))
					urg.DrawMesh = true;

				if (GUILayout.Button ("Hide URG Data"))
					urg.DrawMesh = false;

				GUILayout.Space (20);


				Vector3 pos;
				GUILayout.Label ("URG Offset X (Arrow key):  " + urg.PosOffset.x, _style);
				pos.x = GUILayout.HorizontalSlider (urg.PosOffset.x, -200, 200);

				GUILayout.Label ("URG Offset Y (Arrow key):  " + urg.PosOffset.y, _style);
				pos.y = GUILayout.HorizontalSlider (urg.PosOffset.y, -300, 1000);
				urg.PosOffset = new Vector3(pos.x, pos.y, 0);

				px = pos.x;
				py = pos.y;

				Vector3 temp;
				GUILayout.Label (" Scale X (Z,X):  " + urg._Scale.x.ToString ("0.000"), _style);
				temp.x = GUILayout.HorizontalSlider (urg._Scale.x, 0f, 0.5f);

				GUILayout.Label (" Scale Y (A,S):  " + urg._Scale.y.ToString ("0.000"), _style);
				temp.y = GUILayout.HorizontalSlider (urg._Scale.y, 0f, 0.5f);
				urg._Scale = new Vector3(temp.x, temp.y, 1);

				scalex = temp.x;
				scaley = temp.y;


				GUILayout.Label (" Rotate (C,V) :  " + urg.Rotate.ToString ("0"), _style);
				urg.Rotate = GUILayout.HorizontalSlider (urg.Rotate, -360, 360);

				GUILayout.Space (20);

                if (MainUST)
                {

                    GUILayout.Label(" Touch AreaPosX :  " + TAoffPos.x.ToString("0.0"), _style);
                    TAoffPos.x = GUILayout.HorizontalSlider(TAoffPos.x, -200f, 200f);

                    GUILayout.Label(" Touch AreaPosY :  " + TAoffPos.y.ToString("0.0"), _style);
                    TAoffPos.y = GUILayout.HorizontalSlider(TAoffPos.y, -200f, 200f);


                    GUILayout.Label(" Touch Width :  " + scaleRate.x.ToString("0.0"), _style);
                    scaleRate.x = GUILayout.HorizontalSlider(scaleRate.x, 0.1f, 100);

                    GUILayout.Label(" Touch Height :  " + scaleRate.y.ToString("0.0"), _style);
                    scaleRate.y = GUILayout.HorizontalSlider(scaleRate.y, 0.1f, 100);

                    GUILayout.Space(20);

                }
                
                if (GUILayout.Button ("SAVE"))
					SaveData ();

                GUILayout.Label("FPS : " + (1 / Time.deltaTime).ToString("0.00"), _style);


                GUILayout.Space(40);
                if (GUILayout.Button("DEFAULT"))
                    Default();


                GUILayout.EndArea ();

			} else {
				touchArea.GetComponent<SpriteRenderer> ().color = new Color (1, 1, 1, 0);
			}

		}
			
	}

    void SaveData()
    {


        data.urgPosOffsetx = urg.PosOffset.x;
        data.urgPosOffsety = urg.PosOffset.y;
        data.urgScalex = urg._Scale.x;
        data.urgScaley = urg._Scale.y;
        data.urgRotate = urg.Rotate;

        //data.guiPos = guiPos.x;
        data.guiScale = _style.fontSize;

        PlayerPrefs.SetFloat(USTObjs_Name + " URG Offset X", urg.PosOffset.x);
        PlayerPrefs.SetFloat(USTObjs_Name + " URG Offset Y", urg.PosOffset.y);
        PlayerPrefs.SetFloat(USTObjs_Name + " Scalex", urg._Scale.x);
        PlayerPrefs.SetFloat(USTObjs_Name + " Scaley", urg._Scale.y);
        PlayerPrefs.SetFloat(USTObjs_Name + " Rotate", urg.Rotate);
        //PlayerPrefs.SetFloat(name + " GUI Position", data.guiPos);
        PlayerPrefs.SetInt(USTObjs_Name + " GUI Scale", data.guiScale);


        if (MainUST)
        {

            data.TAoffPosx = TAoffPos.x;
            data.TAoffPosy = TAoffPos.y;
            data.scaleRatex = scaleRate.x;
            data.scaleRatey = scaleRate.y;

            //PlayerPrefs.SetFloat ("DetectRange", urg.DetectRange);
            PlayerPrefs.SetFloat("Touch AreaPosX", TAoffPos.x);
            PlayerPrefs.SetFloat("Touch AreaPosY", TAoffPos.y);
            PlayerPrefs.SetFloat("Touch Width", scaleRate.x);
            PlayerPrefs.SetFloat("Touch Height", scaleRate.y);

        }

    }

    void Default()
    {

        data.urgPosOffsetx = 7.75f;
        data.urgPosOffsety = 30.25f;
        data.urgScalex = .015f;
        data.urgScaley = .015f;
        data.urgRotate = 19;
        //data.guiPos = 200;
        data.guiScale = 12;

        PlayerPrefs.SetFloat(USTObjs_Name + " URG Offset X", data.urgPosOffsetx);
        PlayerPrefs.SetFloat(USTObjs_Name + " URG Offset Y", data.urgPosOffsety);
        PlayerPrefs.SetFloat(USTObjs_Name + " Scalex", data.urgScalex);
        PlayerPrefs.SetFloat(USTObjs_Name + " Scaley", data.urgScaley);
        PlayerPrefs.SetFloat(USTObjs_Name + " Rotate", data.urgRotate);
        //PlayerPrefs.SetFloat(name + " GUI Position", data.guiPos);
        PlayerPrefs.SetFloat(USTObjs_Name + " GUI Scale", data.guiScale);

        if (MainUST)
        {
            data.TAoffPosx = 0;
            data.TAoffPosy = 0;
            data.scaleRatex = 11.9f;
            data.scaleRatey = 7.6f;

            PlayerPrefs.SetFloat("Touch AreaPosX", data.TAoffPosx);
            PlayerPrefs.SetFloat("Touch AreaPosY", data.TAoffPosy);
            PlayerPrefs.SetFloat("Touch Width", data.scaleRatex);
            PlayerPrefs.SetFloat("Touch Height", data.scaleRatey);
        }


    }

    void OnApplicationQuit()
	{
		Debug.Log ("Application end");
		PlayerPrefs.Save ();
	}
}

[System.Serializable]
public struct PlayerPrefsData
{

    public float urgPosOffsetx;
    public float urgPosOffsety;
    public float urgScalex;
    public float urgScaley;
    public float urgRotate;

    public float TAoffPosx;
    public float TAoffPosy;
    public float scaleRatex;
    public float scaleRatey;

    //public float guiPos;
    public int guiScale;
}

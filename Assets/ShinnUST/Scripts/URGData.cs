using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using System;

public class URGData : MonoBehaviour
{

    public enum FilePath
    {
        AbsoluteLocation,
        RelativeLocation
    }

    public FilePath pathstate;
    public string Absolutefilepath = "C:/TAO_iWall/UrgData.xml";
    
    int xml_startstep;
    int xml_endstep;
    int xml_stepCount360;

    public void init()
    {
        if (pathstate == FilePath.AbsoluteLocation)
        {
            LoadFromXml(Absolutefilepath);
            Urg._startstep = xml_startstep;
            Urg._endstep = xml_endstep;
            Urg._stepCount = xml_stepCount360;
        }
        else if (pathstate == FilePath.RelativeLocation)
        {
            string assets = (Application.streamingAssetsPath + "/UrgData.xml").ToString();
            LoadFromXml(assets);
            Urg._startstep = xml_startstep;
            Urg._endstep = xml_endstep;
            Urg._stepCount = xml_stepCount360;
        }
    }

    private void LoadFromXml(string path)
    {

        XmlDocument xmlDoc = new XmlDocument();

        if (File.Exists(path))
        {
            xmlDoc.Load(path);
            XmlNodeList transformList_urg = xmlDoc.GetElementsByTagName("URGStep");

            foreach (XmlNode transformInfo in transformList_urg)
            {

                XmlNodeList transformcontent = transformInfo.ChildNodes;
                foreach (XmlNode transformItens in transformcontent)
                {

                    if (transformItens.Name == "StartStep")
                        xml_startstep = int.Parse(transformItens.InnerText);

                    if (transformItens.Name == "EndStep")
                        xml_endstep = int.Parse(transformItens.InnerText);

                    if (transformItens.Name == "StepCount360")
                        xml_stepCount360 = int.Parse(transformItens.InnerText);

                }
            }
        }

    }
}
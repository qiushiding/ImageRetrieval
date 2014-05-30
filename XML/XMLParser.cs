using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Common;

namespace XML
{
    public class XMLParser
    {
        public static string readXml(string filePath)
        {
            StringBuilder label = new StringBuilder();
            List<string> ExcludeNodes = new List<string> { "diagram", "layer", "connector" };
            string xpath = "mxGraphModel/root";
            XmlNodeList nodeList = XmlHelper.GetChileNodesByXpath(filePath, xpath);
            if (nodeList == null || nodeList.Count == 0)
                return null;

            foreach (XmlNode node in nodeList)
            {
                if (ExcludeNodes.Contains(node.Name.ToLower()))
                {
                    continue;
                }
                foreach (XmlAttribute att in node.Attributes)
                {
                    if (att.Name.ToLower().Equals("label"))
                    {
                        label.Append(att.Value + " ; ");
                    }
                }
            }
            return label.ToString();
        }

    }
}

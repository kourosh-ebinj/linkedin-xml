using System.Collections.Specialized;
using System.Xml;
using System.Xml.Xsl;

namespace LinkedIn_XML.Helpers;

public static class XMLHelper
{
    public static string RunXSLT(
        string xsltFilePath,
        XmlDocument inputXML,
        Dictionary<string, string> parameters = null,
        bool debug = false)
    {
        if (inputXML == null)
            throw new Exception("InputXML parameter can not be null.");

        var transform = new XslCompiledTransform(debug);

        transform.Load(xsltFilePath);

        using (var sw = new StringWriter())
        using (var tw = new XmlTextWriter(sw))
        {
            if (parameters == null)
            {
                transform.Transform(inputXML, tw);
            }
            else
            {
                var args = new XsltArgumentList();
                foreach (var param in parameters)
                {
                    args.AddParam(param.Key, string.Empty,
                        !string.IsNullOrEmpty(param.Value) ? param.Value : string.Empty);
                }

                transform.Transform(inputXML, args, tw);
            }

            return sw.ToString();
        }
    }

    public static XmlNodeList? GetXPathResultNodes(string xmlData, string xPath)
    {
        var doc = new XmlDocument();
        doc.LoadXml(xmlData);
        return GetXPathResultNodes(doc, xPath);
    }

    public static XmlNodeList? GetXPathResultNodes(XmlDocument xmlData, string xPath)
    {
        return xmlData.SelectNodes(xPath);
    }

    public static IEnumerable<T> GetXPathResult<T>(XmlDocument xmlData, string xPath, Func<XmlNode, T> valueSelector)
    {
        var result = new List<T>();

        var nodes = GetXPathResultNodes(xmlData, xPath);
        if (nodes is null)
            return result;

        foreach (XmlNode node in nodes)
            result.Add(valueSelector(node));

        return result;
    }

    public static IEnumerable<string> GetXPathResult(string xPath, string xmlData)
    {
        var doc = new XmlDocument();
        doc.LoadXml(xmlData);

        return GetXPathResult(doc, xPath, e => e.InnerText);
    }

    public static XmlDocument CreateXMLDocument(string rootElementName, string saveToPath = "")
    {
        if (string.IsNullOrWhiteSpace(rootElementName))
            rootElementName = "root";

        var xmlDoc = new XmlDocument();
        var declaration = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", string.Empty);
        xmlDoc.AppendChild(declaration);

        if (!string.IsNullOrEmpty(rootElementName))
        {
            var newElement = xmlDoc.CreateElement(rootElementName);
            xmlDoc.AppendChild(newElement);
        }

        if (!string.IsNullOrWhiteSpace(saveToPath))
            xmlDoc.Save(saveToPath);

        return xmlDoc;
    }

    public static XmlDocument GetXMLDocument(string xmlData)
    {
        var xmlDoc = new XmlDocument();
        xmlDoc.Load(xmlData);

        return xmlDoc;
    }

    public static XmlDocument GetXMLDocument(string filePath, string rootElementName)
    {
        if (File.Exists(filePath))
        {
            var xmlDoc = new XmlDocument();
            xmlDoc.Load(filePath);
            return xmlDoc;
        }

        return CreateXMLDocument(rootElementName, filePath);
    }
}

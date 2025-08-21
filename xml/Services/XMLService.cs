using LinkedIn_XML.Helpers;
using System.Xml;
using System.Xml.Linq;

namespace LinkedIn_XML.Services
{
    public interface IXMLService 
    {

    }

    public class XMLService: IXMLService
    {
        public XmlDocument GetXmlDocument(string xmlData)
        {
            var doc = XMLHelper.GetXMLDocument(xmlData);

            return doc;
        }

        public XmlElement CreateXmlElement(string elementName, string innerText = null)
        {
            XmlDocument doc = new XmlDocument();
            XmlElement element = doc.CreateElement(elementName);
            if (!string.IsNullOrEmpty(innerText))
            {
                element.InnerText = innerText;
            }
            return element;
        }
    }
}

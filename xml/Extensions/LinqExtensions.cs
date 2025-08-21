using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace LinkedIn_XML.Helpers;

public static class LinqExtensions
{
    public static XElement? E(this XNode node, string xpath, IXmlNamespaceResolver? ns = null)
        => node.XPathSelectElement(xpath, ns);
    public static IEnumerable<XElement> Es(this XNode node, string xpath, IXmlNamespaceResolver? ns = null)
        => node.XPathSelectElements(xpath, ns);
    public static T Eval<T>(this XNode node, string xpath, IXmlNamespaceResolver? ns = null)
        => (T)node.XPathEvaluate(xpath, ns);
}

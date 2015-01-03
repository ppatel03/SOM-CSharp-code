/*
 * Module Operations:
 * ------------------
 * This module defines the following classes:
 *   LinqDisplay - Contains methods to extract type and package analysis data
 *                  from xml
 */
/* Required Files:
 *   Constants.cs
 *   FinalOutput.cs
 *   IClientService.cs
 *   IServerService.cs
 *   Parser
 *   Storage
 *    
 *   
 * Maintenance History:
 * --------------------
 * ver 1.0 : 25 Nov 2014
 * - first release
 *
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
namespace CodeAnalysis
{
    public class LinqDisplay
    {
        /**
         * Constructs the Type Analysis Results and Package Analysis Results
         * */
        public string constructOutput(string relationXml, string packageXml)
        {
            StringBuilder sb = new StringBuilder();
            if (null != relationXml && relationXml.Trim().Length > 0)
            {
                sb.Append("Type Analysis Results").Append("\n");
                sb.Append("=====================").Append("\n");
                XDocument doc1 = XDocument.Parse(relationXml);
                var filename2 = from e in doc1.Elements("Relationships") select e;
                IEnumerable<XElement> elListr = from el in filename2.Descendants("File") select el;
                if (elListr == null || elListr.Count() == 0)
                {
                    sb.Append("No Type Analysis Results to Display").Append("\n\n\n");
                }
                //browse through each element and show meaningfull output
                foreach (XElement el in elListr)
                {
                    sb.Append("file name " + (string)el.Attribute("Name")).Append("\n");
                    IEnumerable<XElement> elList2 = from e in el.Elements("Class") select e;
                    foreach (XElement ell in elList2)
                    {
                        sb.Append("  class name " + (string)ell.Attribute("Name"));
                        sb.Append("  depends on " + (string)ell.Attribute("RelatedTo"));
                        sb.Append("  relationship type " + (string)ell.Attribute("RelType"));
                    }
                }
            }
            if (null != packageXml && packageXml.Trim().Length>0)
            {
                sb.Append("\nPackage Analysis Results").Append("\n");
                sb.Append("========================").Append("\n");
                XDocument doc = XDocument.Parse(packageXml);
                var filename = from e in doc.Elements("PackageDependency") select e;
                IEnumerable<XElement> elList = from el in filename.Descendants("Package") select el;
                if (elList == null || elList.Count() == 0)
                {
                    sb.Append("No Package Analysis Results to display").Append("\n\n\n");
                }
                foreach (XElement el in elList)
                {
                    sb.Append("package name " + (string)el.Attribute("Name")).Append("\n");
                    IEnumerable<XElement> elList2 = from e in el.Elements("DependOn") select e;
                    foreach (XElement ell in elList2)
                        sb.Append("       depends on " + (string)ell.Attribute("Name")).Append("\n");
                }
            }
            return sb.ToString();
        }
        
        /**
         * Constructs the output through Linq queries
         * */
        public void displaythroughLinq(string relationXml, string packageXml)
        {
            //using Xelement to browse through each node
            if (null != packageXml && packageXml.Trim().Length >0) { 
                XDocument doc = XDocument.Parse(packageXml);
                var filename = from e in doc.Elements("PackageDependency") select e;
                IEnumerable<XElement> elList = from el in filename.Descendants("Package") select el;
                foreach (XElement el in elList)
                {
                    Console.WriteLine("package name " + (string)el.Attribute("Name"));
                    IEnumerable<XElement> elList2 = from e in el.Elements("DependOn") select e;
                    foreach (XElement ell in elList2)
                        Console.WriteLine("       depends on " + (string)ell.Attribute("Name"));
                }
            }
            if (null != relationXml && relationXml.Trim().Length > 0)
            { 
                XDocument doc1 = XDocument.Parse(relationXml);
                var filename2 = from e in doc1.Elements("Relationships") select e;
                IEnumerable<XElement> elListr = from el in filename2.Descendants("File") select el;

                foreach (XElement el in elListr)
                {
                    Console.WriteLine("file name " + (string)el.Attribute("Name"));
                    IEnumerable<XElement> elList2 = from e in el.Elements("Class") select e;
                    foreach (XElement ell in elList2)
                    {
                        Console.WriteLine("  class name " + (string)ell.Attribute("Name"));
                        Console.Write("  depends on " + (string)ell.Attribute("RelatedTo"));
                        Console.WriteLine("  relationship type " + (string)ell.Attribute("RelType"));
                    }
                }
            }            
        }
/**
 *  Stub to load sample XML and constructing the output
 * */
#if(CLIENTSERVICE_ANALYZER)
        static void Main(string[] args)
        {
            XDocument doc1 = XDocument.Load("typeRelation.xml");
            XDocument doc2 = XDocument.Load("packageDependency.xml");
            LinqDisplay linqDisplay = new LinqDisplay();
            linqDisplay.constructOutput(doc1.ToString(), doc2.ToString());
        }
#endif
    }

}

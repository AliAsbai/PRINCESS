using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;

/**
 *  authors:
 *          @Ali Asbai
 *          @Clara Hällgren
 *          @Olivia Höft
 *          
 **/

namespace PRINCESS.model.screenScraper
{
    [DataContract]
    public class HtmlElement
    {

        [DataMember]
        public string element;
        [DataMember]
        public bool removeCharsProp;
        [DataMember]
        public bool IgnoreTitleKeyProp;

        [DataMember]
        public List<string> ignoredKeys;

        [DataMember]
        public List<string> removeChars;

        public HtmlElement()
        {
            SetElement("");
            SetRemoveCharsProp(false);
            SetIgnoreTitleKeyProp(false);
            ignoredKeys = new List<string>();
            removeChars = new List<string>();
        }

        public HtmlElement(string Element)
        {
            if (Element == null || Element.Trim() == "") throw new ArgumentNullException();
            this.SetElement(Element);
            SetRemoveCharsProp(false);
            SetIgnoreTitleKeyProp(false);
            ignoredKeys = new List<string>();
            removeChars = new List<string>();
        }

        public string GetElement()
        {
            return element;
        }

        public void SetElement(string value)
        {
            element = value;
        }

        public bool GetRemoveCharsProp()
        {
            return removeCharsProp;
        }

        public void SetRemoveCharsProp(bool value)
        {
            removeCharsProp = value;
        }

        public bool GetIgnoreTitleKeyProp()
        {
            return IgnoreTitleKeyProp;
        }

        public void SetIgnoreTitleKeyProp(bool value)
        {
            IgnoreTitleKeyProp = value;
        }

        public string RemoveChars(string text)
        {
            string s = text.ToLower();
            foreach(string c in removeChars)
            {
               s = s.Replace(c.ToLower(), " ").Trim();
            }
            return s;
        }

        public string HandleLazyContainer(string type, string lazyContainer)
        {
            if (lazyContainer == null || type == null) return null; // <-- exception
            string[] sElement = this.element.Split('/');
            char[] lzContainer = lazyContainer.ToCharArray();
            Array.Reverse(lzContainer);
            string xml = "";
            foreach (char c in lzContainer)
            {
                lazyContainer = lazyContainer.Remove(lazyContainer.Length - 1);
                if (c == '/') break;
                else xml = string.Concat(c, xml);
            }
            Array.Reverse(lzContainer);
            if (lzContainer[0] == '/' && lzContainer[1] == '/' && char.IsLetterOrDigit(lzContainer[2]))
            {
                string result = lazyContainer + "[" + xml + "='" + type + "']";
                for (int i = 0; i < sElement.Length; i++)
                {
                    if (sElement[i] != "")
                    {
                        result = string.Concat(result, sElement[i]);
                    }
                    else if (i != 0)
                    {
                        result = string.Concat(result, "//");
                    }
                }
                return result;
            }
            return null; // <-- exception
        }

        public bool CheckIgnoredKey(string title)
        {
            string trim = Regex.Replace(title.ToLower(), @"\s+", "");
            foreach (string s in ignoredKeys)
            {
                if (trim.Contains(Regex.Replace(s.ToLower(), @"\s+", ""))) return true;
            }
            return false;
        }

        public List<string> getRemoveChars()
        {
            return removeChars;
        }

        public void setRemoveChars(List<string> l)
        {
            removeChars = l;
        }

        public void addRemoveChar(string c)
        {
            removeChars.Add(c);
        }

        public List<string> getIgnoredKeys()
        {
            return ignoredKeys;
        }

        public void setIgnoredKeys(List<string> l)
        {
            ignoredKeys = l;
        }

        public void addIgnoredKeys(string key)
        {
            ignoredKeys.Add(key);
        }

    }
}

using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace BookRecommender.DataManipulation.WikiData
{
    /// <summary>
    /// Wikidata specifics implementation of IParser
    /// </summary>
    class WikiDataJsonParser : IParser
    {
        public List<Dictionary<string, string>> Parse(string data)
        {
            var jObject = JObject.Parse(data);

            var head = jObject?["head"]?["vars"]?.Children();

            var variables = new List<string>();
            var returnList = new List<Dictionary<string, string>>();

            //Load variables from json to list
            foreach (var variable in head)
            {
                variables.Add((string)variable);
            }

            //If there is no variable in json, there is also no data
            if (variables.Count == 0)
            {
                return null;
            }

            var objects = jObject?["results"]?["bindings"];
            foreach (var obj in objects)
            {

                var objectList = new Dictionary<string, string>();

                //try to retrive data for every variable, else get empty string
                foreach (var variable in variables)
                {
                    var value = (string)obj?[variable]?["value"];
                    if (value == null)
                    {
                        objectList.Add(variable, string.Empty);
                    }
                    else
                    {
                        objectList.Add(variable, value);
                    }
                }
                returnList.Add(objectList);
            }
            return returnList;
        }
    }
}
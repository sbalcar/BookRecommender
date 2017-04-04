using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using BookRecommender.Models;
using BookRecommender.DataManipulation;
using System.Threading.Tasks;

namespace BookRecommender.Controllers
{
    public class AjaxController : Controller
    {
        public async Task<IActionResult> SparqlData(string entityUri)
        {
            var additionalData = await DataMiner.GetAdditionalDataAsync(entityUri);
            return PartialView("AdditionalSparqlData", additionalData);
        }
        public async Task<string> DynamicImage(string entityUri)
        {
            var db = new BookRecommenderContext();
            IGoogleImg dbObj = db.Books.Where(b => b.Uri == entityUri)?.FirstOrDefault();
            if (dbObj == null)
            {
                dbObj = db.Authors.Where(a => a.Uri == entityUri)?.FirstOrDefault();
            }
            if (dbObj == null)
            {
                return "";
            }
            return await dbObj.TryToGetImgUrlAsync();
        }
        public string[] QueryAutoComplete(string query)
        {
            // return "ahoj jak se vede".Split(' ');
            return SearchEngine.Autocomplete(new BookRecommenderContext(), query, 10).ToArray();
        }
        public IActionResult Recommendation(string type, int data)
        {
            List<int> recList;

            switch (type)
            {
                case "bookPage":
                case "test":
                    recList = new RecommenderEngine().RecommendBookSimilar(data);
                    break;
                default:
                    return View("Error");
            }
            if(recList.Count != 6)
            {
                return View("Error");
            }
            var recommendations = new List<Recommendation>();

            foreach(var bookId in recList){
                recommendations.Add(new Recommendation(bookId));
            }
            return PartialView("Recommendation", recommendations);

            // var db = new BookRecommenderContext();
            // Random rnd = new Random();
            // var books = db.Books.Where(b => !string.IsNullOrEmpty(b.NameEn)).Select(b => b.BookId).OrderBy(x => rnd.Next()).Take(6).ToList();
            // var data = new List<Recommendation>();
            // foreach(var i in books){
            //     data.Add(new Recommendation(i));
            // }
            // return PartialView("Recommendation", data);
        }
    }
}

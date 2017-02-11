using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using BookRecommender.DataManipulation;
using BookRecommender.Models;
using BookRecommender.Models.Database;

namespace BookRecommender.DataManipulation.WikiData
{

    class WikiDataEndpointMiner : SparqlEndPointMiner
    {
        IMiner miner = new WikiDataMiner();
        IParser parser = new WikiDataCsvParser();

        protected override List<Dictionary<string, string>> Execute(string query)
        {
            // Mine data from endpoint
            var rawData = miner.MineData(query);
            // Proccess data
            return parser.Parse(rawData);
        }

        //------------------------------------------------------------------------------------------------------------------------------------
        // Books
        //------------------------------------------------------------------------------------------------------------------------------------
        public override void UpdateBooks(List<int> methodsList)
        {
            if (methodsList == null || methodsList.Count == 0)
            {
                base.UpdateDatabase(GetBooksUri(), SaveBooksUri);
                base.UpdateDatabase(GetBooksTitleLabelCsLabelEn(), SaveBooksTitleLabelCsLabelEn);
                base.UpdateDatabase(GetBooksNamesByLangOfOrigin(), SaveBooksNamesByLangOfOrigin);
                base.UpdateDatabase(GetBooksNamesByAuthorCountryLang(), SaveBooksNamesByAuthorCountryLang);
                base.UpdateDatabase(GetBooksLabelsAll(), SaveBookTitleWithNoOtherName);
                base.UpdateDatabase(GetBooksIdentifiers(), SaveBooksIdentifiers);
            }
            else
            {
                if (methodsList.Contains(0))
                {
                    base.UpdateDatabase(GetBooksUri(), SaveBooksUri);
                }
                if (methodsList.Contains(1))
                {
                    base.UpdateDatabase(GetBooksTitleLabelCsLabelEn(), SaveBooksTitleLabelCsLabelEn);
                }
                if (methodsList.Contains(2))
                {
                    base.UpdateDatabase(GetBooksNamesByLangOfOrigin(), SaveBooksNamesByLangOfOrigin);
                }
                if (methodsList.Contains(3))
                {
                    base.UpdateDatabase(GetBooksNamesByAuthorCountryLang(), SaveBooksNamesByAuthorCountryLang);
                }
                if (methodsList.Contains(4))
                {
                    base.UpdateDatabase(GetBooksLabelsAll(), SaveBookTitleWithNoOtherName);
                }
                if (methodsList.Contains(5))
                {
                    base.UpdateDatabase(GetBooksIdentifiers(), SaveBooksIdentifiers);
                }
            }
        }

        IEnumerable<string> GetBooksUri()
        {
            // Query which loads all books and returns uri
            // wdt:P31 - instance of
            // wd:Q571 - book
            var query = @"SELECT ?book
                        WHERE {
                            ?book wdt:P31 wd:Q571.
                        }";
            var result = Execute(query);
            foreach (var line in result)
            {
                yield return line["book"];
            }
            yield break;
        }

        void SaveBooksUri(string line, BookRecommenderContext db)
        {
            // Check for uniqueness
            if (db.Books.Where(b => b.Uri == line).Count() > 0)
            {
                return;
            }
            db.Books.Add(new Book
            {
                Uri = line
            });
        }

        IEnumerable<(string uri, string title, string labelCs, string labelEn)> GetBooksTitleLabelCsLabelEn()
        {
            // Simple query to retrieve title and labels from books
            // wdt:P1476 - title
            // rdfs:label - standard label - multiple languages, need to filter relevant
            var query = @"SELECT ?book ?title ?label_cs ?label_en
                        WHERE {
                            ?book wdt:P31 wd:Q571.
                            OPTIONAL{
                                ?book wdt:P1476 ?title.
                            }
                            OPTIONAL{
                                ?book rdfs:label ?label_cs.
                                    FILTER(LANG(?label_cs) = ""cs"")
                            }
                            OPTIONAL{
                                ?book rdfs:label ?label_en.
  	                                FILTER(LANG(?label_en) = ""en"")
                            }
                        }";
            var result = Execute(query);
            foreach (var line in result)
            {
                yield return (line["book"], line["title"], line["label_cs"], line["label_en"]);
            }
            yield break;
        }
        void SaveBooksTitleLabelCsLabelEn((string uri, string title, string labelCs, string labelEn) line, BookRecommenderContext db)
        {

            var book = db.Books.Where(b => b.Uri == line.uri)?.FirstOrDefault();
            if (book == null)
            {
                System.Console.WriteLine("book not in database: " + line.uri);
                return;
            }
            book.Title = line.title;
            book.NameCs = line.labelCs;
            book.NameEn = line.labelEn;

            db.Books.Update(book);
        }
        //------------------------------------------------------------------------------------------------------------------------------------
        IEnumerable<(string uri, string langCode, string label)> GetBooksNamesByLangOfOrigin()
        {
            // Get the original name by original language of work property
            // wdt:P364 - original language of work
            // wdt:P424 - Wikimedia language code
            var query = @"SELECT ?book ?lang_code ?label
                        WHERE {
                            ?book wdt:P31 wd:Q571.
                            ?book wdt:P364 ?origLangOfWork.
                            ?origLangOfWork wdt:P424 ?lang_code.
                            ?book rdfs:label ?label.
                                FILTER(LANG(?label) = ?lang_code)
                        }";
            var result = Execute(query);
            foreach (var line in result)
            {
                yield return (line["book"], line["lang_code"], line["label"]);
            }
            yield break;
        }
        void SaveBooksNamesByLangOfOrigin((string uri, string langCode, string label) line, BookRecommenderContext db)
        {
            var book = db.Books.Where(b => b.Uri == line.uri)?.FirstOrDefault();
            if (book == null)
            {
                System.Console.WriteLine("book not in database: " + line.uri);
                return;
            }
            book.OrigLang = line.langCode;
            book.NameOrig = line.label;
            db.Books.Update(book);
        }
        //------------------------------------------------------------------------------------------------------------------------------------        
        IEnumerable<(string uri, string countryLangCode, string label)> GetBooksNamesByAuthorCountryLang()
        {
            // Try to get original name trough nationality of its author
            // Most endpoint heavy query
            // wdt:P50 - author
            // wdt:P27 - country of citizenship
            // wdt:P37 - official language
            // wdt:P424 - Wikimedia language code
            var query = @"SELECT ?item ?country_lang_code ?label
                            WHERE {
                                ?item wdt:P31 wd:Q571.
                                ?item wdt:P50 ?author.
                                ?author wdt:P27 ?country_citiz.
                                ?country_citiz wdt:P37 ?country_lang.
                                ?country_lang wdt:P424 ?country_lang_code.
                            
                                ?item rdfs:label ?label.
                                    FILTER(LANG(?label) = ?country_lang_code)
                            }";
            var result = Execute(query);
            foreach (var line in result)
            {
                yield return (line["item"], line["country_lang_code"], line["label"]);
            }
            yield break;
        }
        void SaveBooksNamesByAuthorCountryLang((string uri, string countryLangCode, string label) line, BookRecommenderContext db)
        {
            var book = db.Books.Where(b => b.Uri == line.uri)?.FirstOrDefault();
            if (book == null)
            {
                System.Console.WriteLine("book not in database: " + line.uri);
                return;
            }
            book.OrigLang = line.countryLangCode;
            book.NameOrig = line.label;
            db.Books.Update(book);
        }
        //------------------------------------------------------------------------------------------------------------------------------------
        IEnumerable<(string uri, string label)> GetBooksLabelsAll()
        {
            // Query to fill out books with no name
            // just use the first one, there are almost no books with no name at this point in mining
            var query = @"SELECT ?item ?label
                            WHERE {
                                ?item wdt:P31 wd:Q571.
                                ?item rdfs:label ?label.
                            }";
            var result = Execute(query);
            foreach (var line in result)
            {
                yield return (line["item"], line["label"]);
            }
            yield break;
        }

        bool BookHasNoName(Book b)
        {
            return (string.IsNullOrEmpty(b.Title) &&
                    string.IsNullOrEmpty(b.NameOrig) &&
                    string.IsNullOrEmpty(b.NameCs) &&
                    string.IsNullOrEmpty(b.NameEn));
        }
        void SaveBookTitleWithNoOtherName((string uri, string label) line, BookRecommenderContext db)
        {
            var book = db.Books.Where(b => b.Uri == line.uri)?.FirstOrDefault();
            if (book == null)
            {
                System.Console.WriteLine("book not in database: " + line.uri);
                return;
            }
            if (BookHasNoName(book))
            {
                book.Title = line.label;
                db.Books.Update(book);
            }

        }
        //------------------------------------------------------------------------------------------------------------------------------------ 

        IEnumerable<(string uri, string isbn10, string isbn13, string gndId, string openLibId, string freeBase)> GetBooksIdentifiers()
        {
            // Query to obtain the identifiers
            // wdt:P957 - ISBN-10
            // wdt:P212 - ISBN-13
            // wdt:P227 - GND ID
            // wdt:P648 - Open Library ID
            // wdt:P646 - Freebase ID
            var query = @"SELECT ?item ?ISBN10 ?ISBN13 ?GND_id ?open_lib_id ?free_base
                            WHERE {
                                ?item wdt:P31 wd:Q571.
                                OPTIONAL{
                                    ?item wdt:P957 ?ISBN10.
                                }
                                OPTIONAL{
                                    ?item wdt:P212  ?ISBN13.
                                }
                                OPTIONAL{
                                    ?item wdt:P227 ?GND_id.
                                }
                                OPTIONAL{
                                    ?item wdt:P648 ?open_lib_id.
                                }
                                OPTIONAL{
                                    ?item wdt:P646 ?free_base.
                                }
                            }";
            var result = Execute(query);
            foreach (var line in result)
            {
                yield return (line["item"], line["ISBN10"], line["ISBN13"], line["GND_id"], line["open_lib_id"], line["free_base"]);
            }
            yield break;
        }
        void SaveBooksIdentifiers((string uri, string isbn10, string isbn13, string gndId, string openLibId, string freeBase) line, BookRecommenderContext db)
        {
            var book = db.Books.Where(b => b.Uri == line.uri)?.FirstOrDefault();
            if (book == null)
            {
                System.Console.WriteLine("book not in database: " + line.uri);
                return;
            }

            book.ISBN10 = line.isbn10;
            book.ISBN13 = line.isbn13;
            book.GndId = line.gndId;
            book.OpenLibId = line.openLibId;
            book.FreeBase = line.freeBase;
            db.Books.Update(book);

        }
        //------------------------------------------------------------------------------------------------------------------------------------
        // Authors
        //------------------------------------------------------------------------------------------------------------------------------------

        public override void UpdateAuthors(List<int> methodsList)
        {
            if (methodsList == null || methodsList.Count == 0)
            {
                base.UpdateDatabase(GetAuthorsUri(), SaveAuthorsUri);
                base.UpdateDatabase(GetAuthorsData(), SaveAuthorsData);
                base.UpdateDatabase(GetAuthorsData2(), SaveAuthorsData2);
                base.UpdateDatabase(GetAuthorBookRelations(), SaveAuthorBookRelations);
            }
            else
            {
                if (methodsList.Contains(0))
                {
                    base.UpdateDatabase(GetAuthorsUri(), SaveAuthorsUri);
                }
                if (methodsList.Contains(1))
                {
                    base.UpdateDatabase(GetAuthorsData(), SaveAuthorsData);
                }
                if (methodsList.Contains(2))
                {
                    base.UpdateDatabase(GetAuthorsData2(), SaveAuthorsData2);
                }
                if (methodsList.Contains(3))
                {
                    base.UpdateDatabase(GetAuthorBookRelations(), SaveAuthorBookRelations);
                }
            }
        }

        IEnumerable<string> GetAuthorsUri()
        {
            // Retrieve all authors of some book
            var query = @"SELECT DISTINCT ?author
                            WHERE {
                            ?item wdt:P31 wd:Q571.
                            ?item wdt:P50 ?author.
                        }";
            var result = Execute(query);
            foreach (var line in result)
            {
                yield return line["author"];
            }
            yield break;
        }
        void SaveAuthorsUri(string uri, BookRecommenderContext db)
        {
            if (db.Authors.Where(a => a.Uri == uri).Count() > 0)
            {
                return;
            }
            db.Authors.Add(new Author
            {
                Uri = uri
            });
        }
        //------------------------------------------------------------------------------------------------------------------------------------
        IEnumerable<(string uri, string labelEn, string labelCs)> GetAuthorsData()
        {
            // Mine labels
            var query = @"SELECT DISTINCT ?author ?author_label_en ?author_label_cs
                            WHERE {
                            ?item wdt:P31 wd:Q571.
                            ?item wdt:P50 ?author.
                            OPTIONAL{
                                ?author rdfs:label ?author_label_en.
                                    FILTER(LANG(?author_label_en) = ""en"")
                            }
                            OPTIONAL{
                                ?author rdfs:label ?author_label_cs.
                                    FILTER(LANG(?author_label_cs) = ""cs"")
                            }
                            
                        }";
            var result = Execute(query);
            foreach (var line in result)
            {
                yield return (line["author"], line["author_label_en"], line["author_label_cs"]);
            }
            yield break;
        }
        void SaveAuthorsData((string uri, string labelEn, string labelCs) line, BookRecommenderContext db)
        {
            var author = db.Authors.Where(a => a.Uri == line.uri)?.FirstOrDefault();
            if (author == null)
            {
                System.Console.WriteLine("author not in database: " + line.uri);
                return;
            }

            author.Uri = line.uri;
            author.NameEn = line.labelEn;
            author.NameCs = line.labelCs;

            db.Authors.Update(author);
        }
        //------------------------------------------------------------------------------------------------------------------------------------
        IEnumerable<(string uri, string dateOfBirth, string dateOfDeath, string sex)> GetAuthorsData2()
        {
            // wdt:P569 - date of birth
            // wdt:P570 - date of death
            // wdt:P21 - sex or gender
            // sex is an uri - so just take english label
            var query = @"SELECT DISTINCT ?author ?date_of_birth ?date_of_death  ?sex
                            WHERE {
                            ?item wdt:P31 wd:Q571.
                            ?item wdt:P50 ?author.
                            OPTIONAL{
                                ?author wdt:P569 ?date_of_birth.
                            }
                            OPTIONAL{
                                ?author wdt:P570 ?date_of_death.
                            }
                            OPTIONAL{
                                ?author wdt:P21 ?sexObj.
                                ?sexObj rdfs:label ?sex.
                                        FILTER(LANG(?sex) = ""en"")
                            }
                            
                        }";
            var result = Execute(query);
            foreach (var line in result)
            {
                yield return (line["author"], line["date_of_birth"], line["date_of_death"], line["sex"]);
            }
            yield break;
        }
        void SaveAuthorsData2((string uri, string dateOfBirth, string dateOfDeath, string sex) line, BookRecommenderContext db)
        {
            var author = db.Authors.Where(a => a.Uri == line.uri)?.FirstOrDefault();
            if (author == null)
            {
                System.Console.WriteLine("author not in database: " + line.uri);
                return;
            }

            author.Uri = line.uri;

            author.DateBirth = HistoricalDateTime.FromWikiData(line.dateOfBirth);
            author.DateDeath = HistoricalDateTime.FromWikiData(line.dateOfDeath);

            if (!string.IsNullOrEmpty(line.sex))
            {
                if (line.sex == "male")
                {
                    author.Sex = SexType.Male;
                }
                if (line.sex == "female")
                {
                    author.Sex = SexType.Female;
                }
            }
            db.Authors.Update(author);
        }
        //------------------------------------------------------------------------------------------------------------------------------------

        IEnumerable<(string uriBook, string uriAuthor)> GetAuthorBookRelations()
        {
            var query = @"SELECT DISTINCT ?book ?author
                            WHERE {
                            ?book wdt:P31 wd:Q571.
                            ?book wdt:P50 ?author.
                        }";
            var result = Execute(query);
            foreach (var line in result)
            {
                yield return (line["book"], line["author"]);
            }
            yield break;
        }
        void SaveAuthorBookRelations((string uriBook, string uriAuthor) line, BookRecommenderContext db)
        {
            var book = db.Books.Where(b => b.Uri == line.uriBook)?.FirstOrDefault();
            var author = db.Authors.Where(a => a.Uri == line.uriAuthor)?.FirstOrDefault();
            if (book == null || author == null)
            {
                return;
            }

            if (db.BooksAuthors.Where(x => x == new BookAuthor(book, author)) != null)
            {
                return;
            }

            book.AddAuthor(author, db);
        }
        //------------------------------------------------------------------------------------------------------------------------------------
        // Characters
        //------------------------------------------------------------------------------------------------------------------------------------

        public override void UpdateCharacters(List<int> methodsList)
        {
            if (methodsList == null || methodsList.Count == 0)
            {
                base.UpdateDatabase(GetCharacters(), SaveCharacters);
                base.UpdateDatabase(GetCharacterBookRelations(), SaveCharacterBookRelations);

            }
            else
            {
                if (methodsList.Contains(0))
                {
                    base.UpdateDatabase(GetCharacters(), SaveCharacters);
                }
                if (methodsList.Contains(1))
                {
                    base.UpdateDatabase(GetCharacterBookRelations(), SaveCharacterBookRelations);
                }
            }
        }
        //------------------------------------------------------------------------------------------------------------------------------------
        IEnumerable<(string uriCharacter, string nameCs, string nameEn)> GetCharacters()
        {
            var query = @"SELECT DISTINCT ?character ?label_cs ?label_en
                            WHERE {
                            ?book wdt:P31 wd:Q571.
                            ?book wdt:P674 ?character.
                            OPTIONAL {
                                ?character rdfs:label ?label_en.
                                    FILTER(LANG(?label_en) = ""en"")
                            }
                            OPTIONAL {
                                ?character rdfs:label ?label_cs.
                                    FILTER(LANG(?label_cs) = ""cs"")
                            }
                        }";
            var result = Execute(query);
            foreach (var line in result)
            {
                yield return (line["character"], line["label_cs"], line["label_en"]);
            }
            yield break;
        }
        void SaveCharacters((string uriCharacter, string nameCs, string nameEn) line, BookRecommenderContext db)
        {
            var character = db.Characters.Where(c => c.Uri == line.uriCharacter)?.FirstOrDefault();

            if (character == null)
            {
                // We dont want to have genre without english name
                if (string.IsNullOrEmpty(line.nameEn))
                {
                    return;
                }
                db.Characters.Add(new Character()
                {
                    Uri = line.uriCharacter,
                    NameCs = line.nameCs,
                    NameEn = line.nameEn
                });
            }
            else
            {
                character.NameCs = line.nameCs;
                character.NameEn = line.nameEn;
                db.Characters.Update(character);
            }
        }
        //------------------------------------------------------------------------------------------------------------------------------------
        IEnumerable<(string uriBook, string uriCharacter)> GetCharacterBookRelations()
        {
            var query = @"SELECT DISTINCT ?book ?character
                            WHERE {
                            ?book wdt:P31 wd:Q571.
                            ?book wdt:P674 ?character.
                        }";
            var result = Execute(query);
            foreach (var line in result)
            {
                yield return (line["book"], line["character"]);
            }
            yield break;
        }
        void SaveCharacterBookRelations((string uriBook, string uriCharacter) line, BookRecommenderContext db)
        {
            var book = db.Books.Where(b => b.Uri == line.uriBook)?.FirstOrDefault();
            var character = db.Characters.Where(c => c.Uri == line.uriCharacter)?.FirstOrDefault();
            if (book == null || character == null)
            {
                return;
            }
            if (db.BooksCharacters.Where(x => x == new BookCharacter(book, character)) != null)
            {
                return;
            }

            book.AddCharacter(character, db);
        }

        // ------------------------------------------------------------------------------------------------------------------------------------
        // Genres
        // ------------------------------------------------------------------------------------------------------------------------------------
        public override void UpdateGenres(List<int> methodsList)
        {
            if (methodsList == null || methodsList.Count == 0)
            {
                base.UpdateDatabase(GetGenres(), SaveGenres);
                base.UpdateDatabase(GetGenreBookRelations(), SaveGenreBookRelations);
            }
            else
            {
                if (methodsList.Contains(0))
                {
                    base.UpdateDatabase(GetGenres(), SaveGenres);
                }
                if (methodsList.Contains(1))
                {
                    base.UpdateDatabase(GetGenreBookRelations(), SaveGenreBookRelations);
                }
            }
        }
        //------------------------------------------------------------------------------------------------------------------------------------
        IEnumerable<(string uriGenre, string nameCs, string nameEn)> GetGenres()
        {
            var query = @"SELECT DISTINCT ?genre ?label_cs ?label_en
                            WHERE {
                            ?book wdt:P31 wd:Q571.
                            ?book wdt:P136 ?genre.
                            OPTIONAL {
                                ?genre rdfs:label ?label_en.
                                    FILTER(LANG(?label_en) = ""en"")
                            }
                            OPTIONAL {
                                ?genre rdfs:label ?label_cs.
                                    FILTER(LANG(?label_cs) = ""cs"")
                            }
                        }";
            var result = Execute(query);
            foreach (var line in result)
            {
                yield return (line["genre"], line["label_cs"], line["label_en"]);
            }
            yield break;
        }
        void SaveGenres((string uriGenre, string nameCs, string nameEn) line, BookRecommenderContext db)
        {
            var genre = db.Genres.Where(g => g.Uri == line.uriGenre)?.FirstOrDefault();

            if (genre == null)
            {
                // We dont want to have genre without english name
                if (string.IsNullOrEmpty(line.nameEn))
                {
                    return;
                }
                db.Genres.Add(new Genre()
                {
                    Uri = line.uriGenre,
                    NameCs = line.nameCs,
                    NameEn = line.nameEn
                });
            }
            else
            {
                genre.NameCs = line.nameCs;
                genre.NameEn = line.nameEn;
                db.Genres.Update(genre);
            }
        }
        //------------------------------------------------------------------------------------------------------------------------------------
        IEnumerable<(string uriBook, string uriGenre)> GetGenreBookRelations()
        {
            var query = @"SELECT DISTINCT ?book ?genre
                            WHERE {
                            ?book wdt:P31 wd:Q571.
                            ?book wdt:P136 ?genre.
                        }";
            var result = Execute(query);
            foreach (var line in result)
            {
                yield return (line["book"], line["genre"]);
            }
            yield break;
        }
        void SaveGenreBookRelations((string uriBook, string uriGenre) line, BookRecommenderContext db)
        {
            var book = db.Books.Where(b => b.Uri == line.uriBook)?.FirstOrDefault();
            var genre = db.Genres.Where(c => c.Uri == line.uriGenre)?.FirstOrDefault();
            if (book == null || genre == null)
            {
                return;
            }

            if (db.BooksGenres.Where(x => x == new BookGenre(book, genre)) != null)
            {
                return;
            }

            book.AddGenre(genre, db);
        }
    }
}
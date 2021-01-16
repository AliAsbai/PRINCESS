using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using PRINCESS.model.screenScraper;
using PRINCESS.model;
using System.Text;
using static PRINCESS.model.screenScraper.Movie;

/** authors:
 *          @Anna Mann
 *          @Ali Asbai
 *          @Clara Hällgren
 *          @Gabriel Vega
 *          @Olivia Höft
 *  
 **/

namespace PRINCESS.controller
{
    public class PrincessDb
    {
        private static SqlConnection con;

        public static Boolean connect()
        {
            SqlConnectionStringBuilder builder;
            try
            {
                if (con == null)
                {
                    builder = new SqlConnectionStringBuilder();
                    builder.DataSource = "princess-comet-sql.database.windows.net";
                    builder.UserID = "princessuser";
                    builder.Password = "mCp+Vrx:]VZwR43>n!aJ^qSe";
                    builder.InitialCatalog = "Princess";

                    con = new SqlConnection(builder.ConnectionString);

                    con.Open();
                    return true;
                }
            }
            catch (SqlException ex)
            {
                con.Close();
                Console.WriteLine("Exception i connect:\n" + ex.GetType());
            }
            con.Close();
            return false;
        }

        public static User CheckUser(string login, string password)
        {
            String query = "SELECT email, password, name, usertype FROM Users WHERE email = '" + login + "';";
            try
            {
                using (SqlCommand command = new SqlCommand(query, con))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string email = reader.GetString(0);
                            string pass = reader.GetString(1);
                            string name = reader.GetString(2);
                            string usertype = reader.GetString(3);

                            if (password.Equals(pass))
                            {
                                User u = new User { Password = pass, Name = name, Email = email, Usertype = usertype };
                                return u;
                            }
                            else
                            {
                                throw new Exception("Wrong password.");
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Connection closed, could not find user.");
                Console.WriteLine(e.ToString());
                return null;
            }
            return null;
        }

        public static void disconnect()
        {
            if (con != null)
            {
                con.Close();
                con = null;
                Console.WriteLine("Successfully disconnected!");
            }
        }

        public static List<Review> searchReviewByTitle(string searchTitle, string sortBy)
        {
            String query = "SELECT Review.MovieID, Title, ReviewText, Writer, Rating, Review.DomainURL, PublishDate, Domain.URL, Priority " +
                "FROM Movie INNER JOIN Review ON Movie.MovieID = Review.MovieID AND Title LIKE '%" + searchTitle + "%' " +
                "INNER JOIN Domain ON Domain.URL = Review.DomainURL ";

            if (sortBy != null)
            {
                query += "ORDER BY " + sortBy + " ASC";
            }
            return searchReview(query);
        }

        public static List<Review> searchReviewByDomain(String searchDomain, string sortBy)
        {
            String query = "SELECT Review.MovieID, Title, ReviewText, Writer, Rating, Review.DomainURL, PublishDate, Domain.URL, Priority " +
                "FROM Movie INNER JOIN Review ON Movie.MovieID = Review.MovieID AND DomainUrl LIKE '%" + searchDomain + "%' " +
                "INNER JOIN Domain ON Domain.URL = Review.DomainURL ";
            if (sortBy != null)
            {
                query += "ORDER BY " + sortBy + " ASC";
            }
            return searchReview(query);
        }

        public static List<Review> searchReviewByWriter(String searchWriter, string sortBy)
        {
            String query = "SELECT Review.MovieID, Title, ReviewText, Writer, Rating, Review.DomainURL, PublishDate, Domain.URL, Priority " +
                "FROM Movie INNER JOIN Review ON Movie.MovieID = Review.MovieID AND Writer LIKE '%" + searchWriter + "%' " +
                "INNER JOIN Domain ON Domain.URL = Review.DomainURL ";
            if (sortBy != null)
            {
                query += "ORDER BY " + sortBy + " ASC";
            }
            return searchReview(query);
        }

        private static List<Review> searchReview(string query)
        {
            List<Review> reviews = new List<Review>();
            try
            {
                using (SqlCommand command = new SqlCommand(query, con))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string movieID = reader.GetString(0);
                            string title = reader.GetString(1);
                            string reviewText = reader.GetString(2);
                            string writer = reader.GetString(3);
                            string rating = reader.GetString(4);
                            string domainUrl = reader.GetString(5);
                            string publishDate = reader.GetString(6);
                            string url = reader.GetString(7);
                            int priority = reader.GetInt32(8);

                            Review review = new Review(url, movieID, title, reviewText, writer, publishDate, rating, domainUrl, priority);
                            reviews.Add(review);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return reviews;
        }

        public static void insertDomain(Domain domain)
        {
            SqlTransaction transaction = con.BeginTransaction();

            try
            {
                using (SqlCommand command = con.CreateCommand())
                {
                    command.Connection = con;
                    command.Transaction = transaction;

                    command.CommandText = "IF NOT EXISTS (SELECT URL FROM Domain WHERE URL = '" + domain.getURL() + "') " +
                        "INSERT INTO Domain(Object, URL, Priority, Country) VALUES('"
                        + escapeApostrophe(SerializeToXml.SerializeObjectToXml<Domain>(domain)) + "', '"
                        + domain.getURL() + "', "
                        + domain.getPriority() + ", '"
                        + domain.getCountry() + "');";
                    command.ExecuteNonQuery();
                    transaction.Commit();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                if (con != null)
                {
                    transaction.Rollback();
                }
            }
            finally
            {
                transaction.Dispose();
            }
        }

        public static List<Domain> getDomains()
        {
            List<Domain> domains = new List<Domain>();
            try
            {
                String query = "SELECT * FROM Domain;";
                using (SqlCommand command = new SqlCommand(query, con))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            domains.Add(SerializeToXml.DeSerializeXmlToObject<Domain>(reader.GetString(0)));
                            domains[domains.Count - 1].setURL(reader.GetString(1));
                            domains[domains.Count - 1].setDomainURL(reader.GetString(1));
                            domains[domains.Count - 1].setCountry(reader.GetString(3));
                            domains[domains.Count - 1].setPriority(reader.GetInt32(2));
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return domains;
        }

        private static string escapeApostrophe(string s)
        {
            StringBuilder builder = new StringBuilder();
            foreach (char c in s)
            {
                builder.Append(c);
                if (c == '\'') builder.Append('\'');
            }
            return builder.ToString();
        }

        public static void insertReview(Review review)
        {
            SqlTransaction transaction = con.BeginTransaction();

            try
            {
                using (SqlCommand command = con.CreateCommand())
                {
                    command.Connection = con;
                    command.Transaction = transaction;

                    if (!movieExists(review.ImdbID))
                    {
                        Movie m = getMovie(review.ImdbID);
                        command.CommandText = "INSERT INTO Movie (MovieID, Title, Genre, ProductionCountry, ReleaseDate) VALUES ('"
                        + m.imdbID + "', '" + m.Title + "', '" + m.Genre + "', '" + m.Country + "', '" + m.Released + "');";

                        command.ExecuteNonQuery();
                    }

                    command.CommandText = "IF NOT EXISTS (SELECT URL FROM Review WHERE URL = '" + review.Url + "')" +
                        "INSERT INTO Review (URL, MovieID, ReviewText, Writer, Rating, DomainURL, PublishDate) VALUES ('"
                        + review.Url + "', '" + review.ImdbID + "', '" + review.ReviewText + "', '" + review.Writer + "', '" + review.Rating
                        + "', '" + review.DomainUrl + "' , '" + review.PublishDate + "');";
                    command.ExecuteNonQuery();
                    transaction.Commit();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                if (con != null)
                {
                    transaction.Rollback();
                }
            }
            finally
            {
                transaction.Dispose();
            }
        }

        private static bool movieExists(string imdbID)
        {
            List<string> result = new List<string>();
            try
            {
                String query = "SELECT MovieID FROM Movie WHERE MovieID = '" + imdbID + "';";
                using (SqlCommand command = new SqlCommand(query, con))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result.Add(reader.GetString(0));
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return result.Count == 1;
        }

        public static void alterDomain(Domain domain)
        {
            SqlTransaction transaction = con.BeginTransaction();

            try
            {
                using (SqlCommand command = con.CreateCommand())
                {
                    command.Connection = con;
                    command.Transaction = transaction;

                    command.CommandText = "IF EXISTS (SELECT * FROM Domain WHERE URL = '" + domain.getDomainURL() + "')" +
                        "UPDATE Domain SET Object = '" + escapeApostrophe(SerializeToXml.SerializeObjectToXml<Domain>(domain)) + "' WHERE URL = '" + domain.getDomainURL() + "';";
                    command.ExecuteNonQuery();
                    transaction.Commit();
                }
            }
            catch (SqlException ex)
            {
                if (con != null)
                {
                    transaction.Rollback();
                }
                throw ex;
            }
            finally
            {
                transaction.Dispose();
            }
        }

        public static void insertQuote(QuoteFormat q)
        {
            SqlTransaction transaction = con.BeginTransaction();

            try
            {
                using (SqlCommand command = con.CreateCommand())
                {
                    command.Connection = con;
                    command.Transaction = transaction;

                    command.CommandText = "IF NOT EXISTS (SELECT * FROM Quotes WHERE domainURL = '" + q.Review.DomainUrl + "' AND URL = '" + q.Review.Url
                                                                                            + "' AND Quote = '" + q.Quote + "' AND UserID = '" + q.User + "')" +
                        "INSERT INTO Quotes(domainURL, URL, Quote, UserID) VALUES('"
                        + escapeApostrophe(q.Review.DomainUrl) + "', '"
                        + escapeApostrophe(q.Review.Url) + "', '"
                        + escapeApostrophe(q.Quote) + "', '"
                        + escapeApostrophe(q.User) + "');";
                    command.ExecuteNonQuery();
                    transaction.Commit();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                if (con != null)
                {
                    transaction.Rollback();
                }
            }
            finally
            {
                transaction.Dispose();
            }
        }

    }
}
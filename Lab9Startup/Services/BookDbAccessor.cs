/*
 * ﻿/*
 * Course: CPRG-211-C
 * Lab: Database Connection
 * Author: Jirch Tumbokon
 * When: Winter 2025
 * Purpose: This program connects to a MySQL database and performs CRUD operations on a books table.
 *          It integrates an AddBook method to add a book to the database, a GetBooks method to retrieve all books,
 *          GetBook method to retrieve a specific book, an UpdateBook method to update a book's details and
 *          DeleteBook method to remove a book from the database.
 */

using Dapper;
using Lab9Startup.Models;
using MySqlConnector;

namespace Lab9Startup.Services
{
    public class BookDbAccessor
    {
        protected MySqlConnection connection;

        public BookDbAccessor()
        {
            // get environemnt variable
            //string ?dbHost = Environment.GetEnvironmentVariable("DB_HOST");
            //string ?dbUser = Environment.GetEnvironmentVariable("DB_USER");
            //string ?dbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD");
            string dbHost = "localhost";
            string dbUser = "root";
            string dbPassword = "password";

            var builder = new MySqlConnectionStringBuilder
            {
                Server = dbHost,
                UserID = dbUser,
                Password = dbPassword,
                Database = "library", // Use maria db to create a database called library
            };

            connection = new MySqlConnection(builder.ConnectionString);
        }

        /// <summary>
        /// Initialize the database and create the books table
        /// </summary>
        public void InitializeDatabase()
        {
            connection.Open();

            var sql = @"CREATE TABLE IF NOT EXISTS books (
                BookId VARCHAR(36) PRIMARY KEY,
                Title VARCHAR(255) NOT NULL,
                Author VARCHAR(255) NOT NULL,
                Description TEXT,
                Category VARCHAR(255)
            )";

            connection.Execute(sql);

            connection.Close();
        }

        /// <summary>
        /// Implement the AddBook method to add a book to the database
        /// </summary>
        /// <param name="book"></param>
        public void AddBook(Book book)
        {
            // insert a new book into the database
            var sql = @"INSERT INTO books (BookId, Title, Author, Description, Category) 
                VALUES (@BookId, @Title, @Author, @Description, @Category)";

            connection.Open();

            connection.Execute(sql, new
            {
                book.BookId,
                book.Title,
                book.Author,
                book.Description,
                book.Category});

            connection.Close();
        }


        /// <summary>
        /// Implement the GetBooks method to get all books from the database
        /// </summary>
        /// <returns></returns>
        public List<Book> GetBooks()
        {
            // get all books from the database
            var sql = "SELECT * FROM books";

            connection.Open();

            // execute the SQL query and map the result to a list of Book objects
            var books = connection.Query<Book>(sql).ToList();

            connection.Close();

            return books;
        }


        /// <summary>
        /// Implement the GetBook method to get a book from the database
        /// </summary>
        /// <param name="bookId"></param>
        /// <returns></returns>
        public Book? GetBook(string bookId)
        {
            // get a book from the database based on the bookId
            var sql = "SELECT * FROM books WHERE BookId = @BookId";

            connection.Open();

            // execute the SQL query and map the result to a Book object
            var book = connection.Query<Book>(sql, new { BookId = bookId }).FirstOrDefault();

            connection.Close();

            return book;
        }

        /// <summary>
        /// Implement the UpdateBook method to update a book in the database
        /// </summary>
        /// <param name="book"></param>
        public void UpdateBook(Book book)
        {
            // update a book in the database
            var sql = @"UPDATE books 
                        SET Title = @Title, 
                            Author = @Author, 
                            Description = @Description, 
                            Category = @Category 
                        WHERE BookId = @BookId";

            connection.Open();

            connection.Execute(sql, new
            {
                book.BookId,
                book.Title,
                book.Author,
                book.Description,
                book.Category
            });

            connection.Close();
        }

        /// <summary>
        /// Implement the DeleteBook method to delete a book from the database
        /// </summary>
        /// <param name="bookId"></param>
        public void DeleteBook(string bookId)
        {
            // delete a book from the database based on the bookId
            var sql = "DELETE FROM books WHERE BookId = @BookId";

            connection.Open();

            connection.Execute(sql, new { BookId = bookId });

            connection.Close();
        }
    }
}

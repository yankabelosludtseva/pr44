using LibraryCatalog.Classes;
using LibraryCatalog.Context;
using LibraryCatalog.Models;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;

namespace LibraryCatalog.ViewModels
{
    public class VM_Books : Notification
    {
        public LibraryContext Context { get; set; }
        public ObservableCollection<Book> Books { get; set; }
        public RealyCommand OnAddBook { get; set; }

        public VM_Books()
        {
            Context = new LibraryContext();

            try
            {
                var list = Context.Books.OrderBy(b => b.IsAvailable).ToList();

                foreach (var book in list)
                {
                    book.Context = Context;
                    book.ViewModel = this;
                }

                Books = new ObservableCollection<Book>(list);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                Books = new ObservableCollection<Book>();
            }

            OnAddBook = new RealyCommand(AddNewBook);
        }

        private void AddNewBook(object parameter)
        {
            var newBook = new Book
            {
                Title = " ",
                Author = " ",
                Year = DateTime.Now.Year,
                Genre = " ",
                Description = "",
                IsAvailable = true,
                Context = Context,
                ViewModel = this
            };

            Books.Add(newBook);
            Context.Books.Add(newBook);

            try
            {
                Context.SaveChanges();
                
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка добавления: {ex.Message}", "Ошибка");
                Books.Remove(newBook);
            }
        }
    }
}
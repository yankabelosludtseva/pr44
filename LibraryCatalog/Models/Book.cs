using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using System.Windows;
using LibraryCatalog.Classes;
using LibraryCatalog.Context;
using LibraryCatalog.ViewModels;
using Microsoft.EntityFrameworkCore;
using Schema = System.ComponentModel.DataAnnotations.Schema;

namespace LibraryCatalog.Models
{
    public class Book : Notification
    {
        [Key]
        public int Id { get; set; }

        [Schema.NotMapped]
        public LibraryContext Context { get; set; }

        [Schema.NotMapped]
        public VM_Books ViewModel { get; set; }

        private string title;
        public string Title
        {
            get => title;
            set
            {
                if (string.IsNullOrEmpty(value)) { title = value; OnPropertyChanged(); return; }

                if (!Regex.IsMatch(value, @"^(.{1,100})$"))
                    MessageBox.Show("Название: от 1 до 100 символов.", "Ошибка ввода");
                else { title = value; OnPropertyChanged(); }
            }
        }

        private string author;
        public string Author
        {
            get => author;
            set
            {
                if (string.IsNullOrEmpty(value)) { author = value; OnPropertyChanged(); return; }
                if (!Regex.IsMatch(value, @"^(.{1,50})$"))
                    MessageBox.Show("Автор: от 1 до 50 символов.", "Ошибка ввода");
                else { author = value; OnPropertyChanged(); }
            }
        }

        private int year;
        public int Year
        {
            get => year;
            set
            {
                if (value < 1000 || value > DateTime.Now.Year)
                    MessageBox.Show("Год должен быть в диапазоне 1000 - " + DateTime.Now.Year, "Ошибка ввода");
                else { year = value; OnPropertyChanged(); }
            }
        }

        private string genre;
        public string Genre
        {
            get => genre;
            set
            {
                if (string.IsNullOrEmpty(value)) { genre = value; OnPropertyChanged(); return; }
                if (!Regex.IsMatch(value, @"^(.{1,30})$"))
                    MessageBox.Show("Жанр: от 1 до 30 символов.", "Ошибка ввода");
                else { genre = value; OnPropertyChanged(); }
            }
        }

        private string description;
        public string Description
        {
            get => description;
            set
            {
                if (string.IsNullOrEmpty(value)) { description = value; OnPropertyChanged(); return; }
                if (!Regex.IsMatch(value, @"^(.{1,1000})$"))
                    MessageBox.Show("Описание: не более 1000 символов.", "Ошибка ввода");
                else { description = value; OnPropertyChanged(); }
            }
        }

        private bool isAvailable;
        public bool IsAvailable
        {
            get => isAvailable;
            set { isAvailable = value; OnPropertyChanged(); OnPropertyChanged(nameof(StatusText)); }
        }

        [Schema.NotMapped] private bool isEditMode;
        [Schema.NotMapped]
        public bool IsEditMode
        {
            get => isEditMode;
            set { isEditMode = value; OnPropertyChanged(); OnPropertyChanged(nameof(EditButtonText)); }
        }

        [Schema.NotMapped]
        public string EditButtonText => IsEditMode ? "Сохранить" : "Изменить";

        [Schema.NotMapped]
        public string StatusText => IsAvailable ? "В наличии" : "Выдана";

        [Schema.NotMapped]
        public RealyCommand OnEdit => new RealyCommand(_ =>
        {
            IsEditMode = !IsEditMode;
            if (!IsEditMode)
            {
                if (Context == null)
                {
                    MessageBox.Show("Ошибка подключения к базе данных", "Ошибка");
                    return;
                }

                try
                {
                    if (Id == 0)
                        Context.Books.Add(this);
                    else
                        Context.Entry(this).State = EntityState.Modified;
                    Context.SaveChanges();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка сохранения: {ex.Message}", "Ошибка");
                }
            }
        });

        [Schema.NotMapped]
        public RealyCommand OnDelete => new RealyCommand(_ =>
        {
            if (MessageBox.Show("Удалить книгу?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                if (ViewModel == null)
                {
                    MessageBox.Show("Ошибка: не найден контекст приложения", "Ошибка");
                    return;
                }

                if (Context == null)
                {
                    MessageBox.Show("Ошибка подключения к базе данных", "Ошибка");
                    return;
                }

                try
                {
                    ViewModel.Books.Remove(this);
                    Context.Remove(this);
                    Context.SaveChanges();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка удаления: {ex.Message}", "Ошибка");
                    if (!ViewModel.Books.Contains(this))
                        ViewModel.Books.Add(this);
                }
            }
        });

        [Schema.NotMapped]
        public RealyCommand OnToggleStatus => new RealyCommand(_ =>
        {
            IsAvailable = !IsAvailable;
            if (!IsEditMode)
            {
                if (Context == null)
                {
                    MessageBox.Show("Ошибка подключения к базе данных", "Ошибка");
                    return;
                }

                try
                {
                    Context.Entry(this).State = EntityState.Modified;
                    Context.SaveChanges();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка изменения статуса: {ex.Message}", "Ошибка");
                }
            }
        });
    }
}
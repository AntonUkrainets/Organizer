using BusinessOrganizer.Windows;
using Server.Constants;
using System;
using System.Collections.Generic;
using System.Windows.Media;
using Telerik.Windows.Controls;

namespace BusinessOrganizer.Extensions
{
    /// <summary>
    /// класс расширения категории
    /// </summary>
    public static class CategoryExtensions
    {
        /// <summary>
        /// объявление коллекции категорий
        /// </summary>
        private static Dictionary<int?, KeyValuePair<string, string>> categories;

        /// <summary>
        /// конструктор
        /// </summary>
        static CategoryExtensions()
        {
            //создание нового экземпляра коллекции категорий
            categories = new Dictionary<int?, KeyValuePair<string, string>>();

            //добавление в коллекцию категорий желтого цвета
            categories.Add(1, new KeyValuePair<string, string>(
               Windows.Colors.Yellow, Windows.Color.Yellow));

            //добавление в коллекцию категорий зеленого цвета
            categories.Add(2, new KeyValuePair<string, string>(
                Windows.Colors.Green, Windows.Color.Green));

            //добавление в коллекцию категорий фиолетового цвета
            categories.Add(3, new KeyValuePair<string, string>(
                Windows.Colors.Purple, Windows.Color.Purple));

            //добавление в коллекцию категорий розового цвета
            categories.Add(4, new KeyValuePair<string, string>(
                Windows.Colors.Pink, Windows.Color.Pink));
        }

        /// <summary>
        /// Получение категории по id категории
        /// </summary>
        /// <param name="categoryId">id категории, которую нужно преобразовать с саму категорию</param>
        /// <returns></returns>
        public static Category GetCategory(this int? categoryId)
        {
            //если категория не была выбрана или равняется null
            if (categoryId == 0
                || categoryId == null)
                return null;//вернуть null

            //иначе
            return new Category()//вернуть новую категорию
            {//установка кисти 
                CategoryBrush = categories[categoryId].Key.GetBrush(),
                CategoryName = categories[categoryId].Value
            };            
        }

        /// <summary>
        /// преобразование цвета типа string в тип Brush
        /// </summary>
        /// <param name="color">цвет, который необходимо преобразовать</param>
        /// <returns></returns>
        public static Brush GetBrush(this string color)
        {
            //вернуть новый Brush
            return (Brush)new BrushConverter().ConvertFromString(color);
        }

        /// <summary>
        /// получение id категории по выбранной категории
        /// </summary>
        /// <param name="category">категория, по которой необходимо получить id</param>
        /// <returns></returns>
        public static int? GetCategoryId(this ICategory category)
        {
            //если категория не была выбрана
            if (category == null)
                return null;//вернуть null

            //сравнение по имени категории
            switch (category.CategoryName)
            {
                //если была выбрана желтая категория
                case "Yellow":
                    return Categories.Yellow;//вернуть 1

                    //если была выбрана зеленая категория
                case "Green":
                    return Categories.Green;//вернуть 2

                    //если была выбрана фиолетовая категория
                case "Purple":
                    return Categories.Purple;//вернуть 3

                    //если была выбрана розовая категория
                case "Pink":
                    return Categories.Pink;//вернуть 4

                default:
                    throw new NotSupportedException(); 
            }
        }
    }
}
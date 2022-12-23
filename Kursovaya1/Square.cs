using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Kursovaya1
{
    class Square
    {
        public BitmapImage img { get; set; }
        public int whichGem { get; set; } // ЗАМЕНИТЬ НАЗВАНИЕ ПЕРЕМЕННОЙ 

        public Button gem3 = new Button();

        int width = 50;

        public Square(int whichGem, int tag)
        {
            this.whichGem = whichGem;

            gem3 = new Button();
            gem3.Tag = tag;
            gem3.Width = width;
            gem3.Height = width;
            gem3.Content = "";
            gem3.Background = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
            gem3.Margin = new Thickness(0);
            gem3.BorderThickness = new Thickness(0);

        }
    }
}

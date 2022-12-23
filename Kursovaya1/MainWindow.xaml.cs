using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Kursovaya1
{
    public partial class MainWindow : Window
    {
       
        int width = 60;

        DispatcherTimer timer1 = new DispatcherTimer();
        TimeSpan time1;

        BitmapImage[] gemImg = new BitmapImage[]
        {
            new BitmapImage(new Uri(@"pack://application:,,,/Gems/blue.png", UriKind.Absolute)),
            new BitmapImage(new Uri(@"pack://application:,,,/Gems/green.png", UriKind.Absolute)),
            new BitmapImage(new Uri(@"pack://application:,,,/Gems/pink.png", UriKind.Absolute)),
            new BitmapImage(new Uri(@"pack://application:,,,/Gems/red.png", UriKind.Absolute)),
            new BitmapImage(new Uri(@"pack://application:,,,/Gems/yell.png", UriKind.Absolute)),         
        };

        Square[,] field = new Square[sizeField1, sizeField1];
        Logic logic;

        const int sizeField1 = 10;
        const int empty = -1;
        const int missscore = 10 * ((sizeField1 - 2) * 3 * sizeField1 * 2);
       
        public MainWindow()
        {
            InitializeComponent();
            grid.Rows = sizeField1;
            grid.Columns = sizeField1;

            grid.Width = sizeField1 * (width + 4);
            grid.Height = sizeField1 * (width + 4);

           // grid.Margin = new Thickness(5, 5, 5, 5);
            grid.Background = new SolidColorBrush(Color.FromArgb(100, 100, 150, 100));
            logic = new Logic(field);

            timer1.Interval = new TimeSpan(0, 0, 0, 1);
            timer1.Tick += timer1_Tick;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            time1 = time1.Subtract(new TimeSpan(0, 0, 0, 1));

            lb.Content = $"Time: {time1.ToString()}";

            if (time1.Seconds == 0)
            {
                timer1.Stop();
                MessageBox.Show("GameOver!!!");
            }
        }

        private void Falling(object sender, EventArgs args)
        {
            Application.Current.Dispatcher.Invoke(delegate
            {
                Update();
            });
        }

        void Update()
        {
            for (int i = 0; i < sizeField1; i++)
                for (int j = 0; j < sizeField1; j++)
                {
                    StackPanel stack = new StackPanel();

                    int gem1 = field[i, j].whichGem;
                    if (gem1 != empty)
                    {
                        BitmapImage image = gemImg[gem1];
                        stack = getPanel(image);

                    }

                    field[i, j].gem3.Content = stack;
                }

            points.Content = Convert.ToString(logic.getScore() - missscore);
           
        }

        StackPanel getPanel(BitmapImage picture)
        {
            StackPanel stackPanel = new StackPanel();
            Image image = new Image();
            image.Source = picture;
            stackPanel.Children.Add(image);
            stackPanel.Margin = new Thickness(0);

            return stackPanel;
        }


        private void Btn_Click(object sender, RoutedEventArgs e)
        {
            int index = (int)(((Button)sender).Tag); //!!!!!!!!!!!!

            int i = index % sizeField1;
            int j = index / sizeField1;

            logic.moveCell(i, j);

            points.Content = Convert.ToString(logic.getScore() - missscore);
            Update();
        }


        private void Start_Click(object sender, RoutedEventArgs e)
        {
            grid.Children.Clear();
            if (logic.getScore() == 0)
            { }
            else
                winTable.Items.Add(playername.Text.ToString() + "            " + points.Content.ToString());

            string name = Convert.ToString(playername.Text.ToString());
            if (name != "Player: ")
            {
                for (int i = 0; i < sizeField1; i++)
                    for (int j = 0; j < sizeField1; j++)
                    {
                        field[i, j] = new Square(empty, i + j * sizeField1);
                        StackPanel stackPanel = new StackPanel();
                        stackPanel.Margin = new Thickness(1);
                        field[i, j].gem3.Click += Btn_Click;
                        grid.Children.Add(field[i, j].gem3);
                    }
                logic.GameSetScore(0);
                logic.Falling += Falling;
                Update();
                logic.BeginFalling();
                time1 = new TimeSpan(0, 0, 1, 0);
                lb.Content = $"Time: {time1.ToString()}";
                timer1.Start();
            }
            else
                MessageBox.Show("Player's name:");
        }
    }
}

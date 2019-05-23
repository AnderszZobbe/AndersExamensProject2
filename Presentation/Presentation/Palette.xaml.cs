using Presentation.Properties;
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
using System.Windows.Shapes;
using Xceed.Wpf.Toolkit;

namespace Presentation
{
    /// <summary>
    /// Interaction logic for Palette.xaml
    /// </summary>
    public partial class Palette : Window
    {
        public Palette()
        {
            InitializeComponent();

            System.Drawing.Color color = Settings.Default.Workday;
            daywork.SelectedColor = Color.FromArgb(color.A, color.R, color.G, color.B);

            color = Settings.Default.Worknight;
            nightwork.SelectedColor = Color.FromArgb(color.A, color.R, color.G, color.B);

            color = Settings.Default.FridayFree;
            fridayfree.SelectedColor = Color.FromArgb(color.A, color.R, color.G, color.B);

            color = Settings.Default.Weekend;
            weekend.SelectedColor = Color.FromArgb(color.A, color.R, color.G, color.B);

            color = Settings.Default.Holiday;
            holiday.SelectedColor = Color.FromArgb(color.A, color.R, color.G, color.B);
        }

        private void DayworkSelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            if (sender is ColorPicker colorPicker)
            {
                if (colorPicker.SelectedColor.HasValue)
                {
                    Color color = colorPicker.SelectedColor.Value;
                    Settings.Default.Workday = System.Drawing.Color.FromArgb(color.A, color.R, color.G, color.B);
                    Settings.Default.Save();
                }
            }

            Update();
        }

        private void NightworkSelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            if (sender is ColorPicker colorPicker)
            {
                if (colorPicker.SelectedColor.HasValue)
                {
                    Color color = colorPicker.SelectedColor.Value;
                    Settings.Default.Worknight = System.Drawing.Color.FromArgb(color.A, color.R, color.G, color.B);
                    Settings.Default.Save();
                }
            }

            Update();
        }

        private void FridayfreeSelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            if (sender is ColorPicker colorPicker)
            {
                if (colorPicker.SelectedColor.HasValue)
                {
                    Color color = colorPicker.SelectedColor.Value;
                    Settings.Default.FridayFree = System.Drawing.Color.FromArgb(color.A, color.R, color.G, color.B);
                    Settings.Default.Save();
                }
            }

            Update();
        }

        private void WeekendSelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            if (sender is ColorPicker colorPicker)
            {
                if (colorPicker.SelectedColor.HasValue)
                {
                    Color color = colorPicker.SelectedColor.Value;
                    Settings.Default.Weekend = System.Drawing.Color.FromArgb(color.A, color.R, color.G, color.B);
                    Settings.Default.Save();
                }
            }

            Update();
        }

        private void HolidaySelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            if (sender is ColorPicker colorPicker)
            {
                if (colorPicker.SelectedColor.HasValue)
                {
                    Color color = colorPicker.SelectedColor.Value;
                    Settings.Default.Holiday = System.Drawing.Color.FromArgb(color.A, color.R, color.G, color.B);
                    Settings.Default.Save();
                }
            }

            Update();
        }

        private void Update()
        {
            if (Owner is WorkteamOverview wo)
            {
                wo.InitializeGrid();
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (Owner is WorkteamOverview wo)
            {
                wo.palette = null;
            }
        }
    }
}

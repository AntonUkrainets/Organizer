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
using Telerik.Windows.Controls.ScheduleView;

namespace BusinessOrganizer.Windows
{
    /// <summary>
    /// Логика взаимодействия для ReminderWindow.xaml
    /// </summary>
    public partial class ReminderWindow : Window
    {
        public ReminderWindow(Appointment appointment)
        {
            InitializeComponent();

            this.SubjectTextBlock.Text = appointment.Subject;

            var startTime = appointment.Start;
            var endTime = appointment.End;

            this.TimeTextBlock.Text =
                string.Format($"{startTime.Hour}:{startTime.Minute} - {endTime.Hour}:{endTime.Minute}");
        }

        private void DismissButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close(false);
        }

        private void SnoozeButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close(true);
        }

        private void Close(bool result)
        {
            this.DataContext = result;
            this.Close();
        }
    }
}
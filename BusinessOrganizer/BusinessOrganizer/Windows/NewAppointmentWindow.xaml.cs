using BusinessOrganizer.Windows;
using Server;
using Server.Constants;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Telerik.Windows.Controls.ScheduleView;
using Telerik.Windows.Controls;
using BusinessOrganizer.Extensions;

namespace BusinessOrganizer
{
    /// <summary>
    /// Логика взаимодействия для NewAppointment.xaml
    /// </summary>
    public partial class NewAppointmentWindow : Window
    {
        private ObservableCollection<Appointment> appointments;

        private Appointment currentAppointment;
        private Server.DataModel.Recurrence recurrence;

        private int? importance;

        public NewAppointmentWindow(
            ObservableCollection<Appointment> appointments,
            Appointment currentAppointment,
            Server.DataModel.Recurrence recurrence)
        {
            this.DataContext = this;

            InitializeComponent();

            this.TbSubject.Text = currentAppointment.Subject;
            this.TbDescription.Text = currentAppointment.Body;
            this.StartTimeDatePicker.SelectedValue = currentAppointment.Start;
            this.EndTimeDatePicker.SelectedValue = currentAppointment.End;
            this.IsAllDayEventRadioButton.IsChecked = currentAppointment.IsAllDayEvent;

            this.appointments = appointments;
            this.recurrence = recurrence;

            this.currentAppointment = currentAppointment;

            SetCategory();
            SetTimeMarker();
            SetImportance();
        }

        private void SetCategory()
        {
            if (this.currentAppointment.Category == null)
                return;

            if (this.currentAppointment.Category.CategoryName == "Yellow")
                this.YellowCategory.IsSelected = true;

            else if (this.currentAppointment.Category.CategoryName == "Green")
                this.GreenCategory.IsSelected = true;

            else if (this.currentAppointment.Category.CategoryName == "Purple")
                this.PurpleCategory.IsSelected = true;

            else if (this.currentAppointment.Category.CategoryName == "Pink")
                this.PinkCategory.IsSelected = true;
        }

        private void SetTimeMarker()
        {
            if (currentAppointment.TimeMarker == null)
                return;

            if (this.currentAppointment.TimeMarker.TimeMarkerName == TimeMarker.Free.TimeMarkerName)
                this.FreeTimeMarker.IsSelected = true;
            
            else if (this.currentAppointment.TimeMarker.TimeMarkerName == TimeMarker.Busy.TimeMarkerName)
                this.BusyTimeMarker.IsSelected = true;

            else if (this.currentAppointment.TimeMarker.TimeMarkerName == TimeMarker.Tentative.TimeMarkerName)
                this.TentativeTimeMarker.IsSelected = true;

            else if (this.currentAppointment.TimeMarker.TimeMarkerName == TimeMarker.OutOfOffice.TimeMarkerName)
                this.OutOfOfficeTimeMarker.IsSelected = true;
        }

        private void SetImportance()
        {
            if (currentAppointment == null
                || currentAppointment.Importance == Importance.None)
                return;

            if (currentAppointment.Importance == Importance.High)
            {
                this.importance = Priorities.High;
                this.HighImportanceButton.Background = Windows.Colors.LightSkyBlue.GetBrush();
            }
            else if (currentAppointment.Importance == Importance.Low)
            {
                this.importance = Priorities.Low;
                this.LowImportanceButton.Background = Windows.Colors.LightSkyBlue.GetBrush();
            }
        }

        /// <summary>
        /// Set count reccurence of appointment
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EditReccurenceButton_Click(object sender, RoutedEventArgs e)
        {
            CountReccurenceOfAppointmentWindow recurrenceWindow =
                new CountReccurenceOfAppointmentWindow(
                    this.StartTimeDatePicker.SelectedValue,
                    this.EndTimeDatePicker.SelectedValue,
                    this.currentAppointment,
                    this.recurrence);
            
            if (recurrenceWindow.ShowDialog() == true)
            {
                if (!string.IsNullOrEmpty(recurrence.Frequency))
                {
                    this.StartTimeDatePicker.IsEnabled = false;
                    this.EndTimeDatePicker.IsEnabled = false;

                    this.IsAllDayEventRadioButton.IsEnabled = false;
                }

            }
        }

        /// <summary>
        /// Create New Appointment
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            currentAppointment.Subject = this.TbSubject.Text;
            currentAppointment.Body = this.TbDescription.Text;

            if (this.StartTimeDatePicker.SelectedValue.HasValue)
                currentAppointment.Start = this.StartTimeDatePicker.SelectedValue.Value;

            if (this.EndTimeDatePicker.SelectedValue.HasValue)
                currentAppointment.End = this.EndTimeDatePicker.SelectedValue.Value;

            if (this.IsAllDayEventRadioButton.IsChecked.HasValue)
                currentAppointment.IsAllDayEvent = this.IsAllDayEventRadioButton.IsChecked.Value;

            this.currentAppointment.Importance = this.importance.GetImportance();

            int? categoryId = this.CategoriesComboBox.SelectedIndex;
            this.currentAppointment.Category = categoryId.GetCategory();

            int? timeMarkerId = this.TimeMarkersComboBox.SelectedIndex;
            this.currentAppointment.TimeMarker = timeMarkerId.GetTimeMarker();

            this.Close(true);
        }  

        /// <summary>
        /// Close the program
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close(false);
        }

        private void HighImportanceButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.importance != Priorities.High)
            {
                this.importance = Priorities.High;
                this.HighImportanceButton.Background = Windows.Colors.LightSkyBlue.GetBrush();

                this.LowImportanceButton.Background = Windows.Colors.White.GetBrush();
            }
            else
            {
                this.importance = Priorities.Normal;
                this.HighImportanceButton.Background = Windows.Colors.White.GetBrush();
            }
        }

        private void LowImportanceButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.importance != Priorities.Low)
            {
                this.importance = Priorities.Low;
                this.LowImportanceButton.Background =
                    CategoryExtensions.GetBrush(Windows.Colors.LightSkyBlue);

                this.HighImportanceButton.Background =
                    CategoryExtensions.GetBrush(Windows.Colors.White);
            }
            else
            {
                this.importance = Priorities.Normal;
                this.LowImportanceButton.Background =
                    CategoryExtensions.GetBrush(Windows.Colors.White);
            }
        }

        private void IsAllDayEventRadioButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.IsAllDayEventRadioButton.IsChecked == true)
            {
                this.StartTimeDatePicker.InputMode = InputMode.DatePicker;
                this.EndTimeDatePicker.InputMode = InputMode.DatePicker;
            }
            else
            {
                this.StartTimeDatePicker.InputMode = InputMode.DateTimePicker;
                this.EndTimeDatePicker.InputMode = InputMode.DateTimePicker;
            }
        }

        private void Close(bool result)
        {
            this.DialogResult = result;
            this.Close();
        }
    }
}
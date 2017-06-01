using BusinessOrganizer.Windows;
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

namespace BusinessOrganizer
{
    /// <summary>
    /// Логика взаимодействия для CountReccurenceOfAppointmentWindow.xaml
    /// </summary>
    public partial class CountReccurenceOfAppointmentWindow : Window
    {
        public Server.DataModel.Recurrence recurrence = null;
        private Appointment appointment = null;

        private DateTime? startTime = null;
        private DateTime? endTime = null;

        public CountReccurenceOfAppointmentWindow(
            DateTime? startTime,
            DateTime? endTime,
            Appointment appointment,
            Server.DataModel.Recurrence recurrence)
        {
            InitializeComponent();

            this.recurrence = recurrence;

            this.appointment = appointment;

            this.startTime = startTime;
            this.endTime = endTime;

            SetDateTime(startTime, endTime);

            if (this.recurrence.AppointmentId==0)
            {                
                SetDayIsToday();
                SetMonthIsNow();
            }

            InitializeDailyComponents();
            InitializeWeeklyComponents();
        }

        private void InitializeTimeComponents()
        {

        }

        private void InitializeDailyComponents()
        {
            if (this.recurrence.Frequency != "Daily")
                return;

            if (this.recurrence.Interval == 5)
            {
                this.DailyEveryRadioButton.IsChecked = false;
                this.DailyEveryNumeric.IsEnabled = false;

                this.DailyEveryWeekdayRadioButton.IsChecked = true;
            }

            else
            {
                this.DailyEveryNumeric.Value = this.recurrence.Interval;
                this.DailyEveryWeekdayRadioButton.IsChecked = false;
            }
        }

        private void InitializeWeeklyComponents()
        {
            if (this.recurrence.Frequency != "Weekly")
                return;

            this.WeeklyNumeric.Value = this.recurrence.Interval;

            if (this.recurrence.DaysOfWeekMask == null)
                return;

            InitializeWeeklyDays();
        }

        private void InitializeWeeklyDays()
        {
            List<string> days = this.recurrence.DaysOfWeekMask.Split(',').ToList();

            foreach (var day in days)
            {
                switch (day)
                {
                    case "Monday":
                        this.MondayCheckBox.IsChecked = true;
                        break;
                    case "Tuesday":
                        this.TuesdayCheckBox.IsChecked = true;
                        break;
                    case "Wednesday":
                        this.WednesdayCheckBox.IsChecked = true;
                        break;
                    case "Thursday":
                        this.ThursdayCheckBox.IsChecked = true;
                        break;
                    case "Friday":
                        this.FridayCheckBox.IsChecked = true;
                        break;
                    case "Saturday":
                        this.SaturdayCheckBox.IsChecked = true;
                        break;
                    case "Sunday":
                        this.SundayCheckBox.IsChecked = true;
                        break;
                    default:
                        break;
                }
            }
        }

        private void SetDateTime(DateTime? startTime, DateTime? endTime)
        {
            DateTime dateTimeNow = DateTime.Now;

            this.StartTimePicker.SelectedValue = startTime;
            this.EndTimePicker.SelectedValue = endTime;

            this.StartDatePicker.SelectedDate = dateTimeNow;
            this.EndByDatePicker.SelectedDate = dateTimeNow.AddDays(10);

            this.MonthlyDayNumeric.Value = dateTimeNow.Day;
        }

        private void SetMonthIsNow()
        {
            int month = DateTime.Now.Month;

            this.YearlyEveryComboBox.SelectedIndex = month - 1;
            this.YearlyOfDayRadComboBox.SelectedIndex = month - 1;

            switch (month)
            {
                case 1:
                    this.JanuaryComboBox.IsSelected = true;
                    break;
                case 2:
                    this.FebruaryComboBox.IsSelected = true;
                    break;
                case 3:
                    this.MarchComboBox.IsSelected = true;
                    break;
                case 4:
                    this.AprilComboBox.IsSelected = true;
                    break;
                case 5:
                    this.MayComboBox.IsSelected = true;
                    break;
                case 6:
                    this.JuneComboBox.IsSelected = true;
                    break;
                case 7:
                    this.JulyComboBox.IsSelected = true;
                    break;
                case 8:
                    this.AugustComboBox.IsSelected = true;
                    break;
                case 9:
                    this.SeptemberComboBox.IsSelected = true;
                    break;
                case 10:
                    this.OctoberComboBox.IsSelected = true;
                    break;
                case 11:
                    this.NovemberComboBox.IsSelected = true;
                    break;
                case 12:
                    this.DecemberComboBox.IsSelected = true;
                    break;
                default:
                    break;
            }
        }

        private void SetDayIsToday()
        {
            DayOfWeek day = DateTime.Now.DayOfWeek;

            this.MonthlyTheSecondDayComboBox.SelectedIndex = (int)day;
            this.MonthlyTheSecondDayComboBox.SelectedIndex = (int)day;

            this.YearlyEveryNumeric.Value = DateTime.Now.Day;
            this.YearlyTheSecondDayComboBox.SelectedIndex = (int)day;

            switch (day)
            {
                case DayOfWeek.Monday:
                    this.MondayCheckBox.IsChecked = true;
                    this.MondayComboBox.IsSelected = true;
                    this.YearlyMondayComboBox.IsSelected = true;
                    break;

                case DayOfWeek.Tuesday:
                    this.TuesdayCheckBox.IsChecked = true;
                    this.TuesdayComboBox.IsSelected = true;
                    this.YearlyTuesdayComboBox.IsSelected = true;
                    break;

                case DayOfWeek.Wednesday:
                    this.WednesdayCheckBox.IsChecked = true;
                    this.WednesdayComboBox.IsSelected = true;
                    this.YearlyWednesdayComboBox.IsSelected = true;
                    break;

                case DayOfWeek.Thursday:
                    this.ThursdayCheckBox.IsChecked = true;
                    this.ThursdayComboBox.IsSelected = true;
                    this.YearlyThursdayComboBox.IsSelected = true;
                    break;

                case DayOfWeek.Friday:
                    this.FridayCheckBox.IsChecked = true;
                    this.FridayComboBox.IsSelected = true;
                    this.YearlyFridayComboBox.IsSelected = true;
                    break;

                case DayOfWeek.Saturday:
                    this.SaturdayCheckBox.IsChecked = true;
                    this.SaturdayComboBox.IsSelected = true;
                    this.YearlySaturdayComboBox.IsSelected = true;
                    break;

                case DayOfWeek.Sunday:
                    this.SundayCheckBox.IsChecked = true;
                    this.SundayComboBox.IsSelected = true;
                    this.YearlySundayComboBox.IsSelected = true;
                    break;

                default:
                    break;
            }
        }

        private void WeeklyButton_Click(object sender, RoutedEventArgs e)
        {
            this.WeeklyPanel.Visibility = Visibility.Visible;

            DailyVisibility();
            MonthlyVisibility();
            YearlyVisibility();
        }

        private void MonthlyButton_Click(object sender, RoutedEventArgs e)
        {
            this.MonthlyPanel.Visibility = Visibility.Visible;

            DailyVisibility();
            WeeklyVisibility();
            YearlyVisibility();
        }

        private void DailyVisibility()
        {
            this.DailyPanel.Visibility = Visibility.Collapsed;

            if(this.recurrence.Frequency=="Daily")
            {
                InitializeDailyComponents();
            }
            else
            {
                this.DailyEveryWeekdayRadioButton.IsChecked = false;
                this.DailyEveryRadioButton.IsChecked = true;
                this.DailyEveryNumeric.IsEnabled = true;
            }
        }

        private void WeeklyVisibility()
        {
            this.WeeklyPanel.Visibility = Visibility.Collapsed;
        }

        private void MonthlyVisibility()
        {
            this.MonthlyPanel.Visibility = Visibility.Collapsed;

            this.MonthlyTheRadioButton.IsChecked = false;
            this.MonthlydayRadioButton.IsChecked = true;

            this.MonthlyDayNumeric.IsEnabled = true;
            this.MonthlyDayOfEveryNumeric.IsEnabled = true;

            this.MonthlyTheDayComboBox.IsEnabled = false;
            this.MonthlyTheSecondDayComboBox.IsEnabled = false;
            this.MonthlyOfEveryNumeric.IsEnabled = false;
        }

        private void YearlyVisibility()
        {
            this.YearlyPanel.Visibility = Visibility.Collapsed;

            this.YearlyTheRadioButton.IsChecked = false;
            this.YearlyEveryRadioButton.IsChecked = true;

            this.YearlyEveryComboBox.IsEnabled = true;
            this.YearlyEveryNumeric.IsEnabled = true;

            this.YearlyTheDayComboBox.IsEnabled = false;
            this.YearlyTheSecondDayComboBox.IsEnabled = false;
            this.YearlyOfDayRadComboBox.IsEnabled = false;
        }

        private void DailyEveryRadioButton_Click(object sender, RoutedEventArgs e)
        {
            this.DailyEveryNumeric.IsEnabled = true;
        }

        private void MonthlydayRadioButton_Click(object sender, RoutedEventArgs e)
        {
            this.MonthlyDayNumeric.IsEnabled = true;
            this.MonthlyDayOfEveryNumeric.IsEnabled = true;

            MonthlyTheDayComboBox.IsEnabled = false;
            MonthlyTheSecondDayComboBox.IsEnabled = false;
            MonthlyOfEveryNumeric.IsEnabled = false;
        }

        private void MonthlyTheRadioButton_Click(object sender, RoutedEventArgs e)
        {
            this.MonthlyDayNumeric.IsEnabled = false;
            this.MonthlyDayOfEveryNumeric.IsEnabled = false;

            MonthlyTheDayComboBox.IsEnabled = true;
            MonthlyTheSecondDayComboBox.IsEnabled = true;
            MonthlyOfEveryNumeric.IsEnabled = true;
        }

        private void YearlyTheRadioButton_Click(object sender, RoutedEventArgs e)
        {
            this.YearlyEveryComboBox.IsEnabled = false;
            this.YearlyEveryNumeric.IsEnabled = false;

            this.YearlyTheDayComboBox.IsEnabled = true;
            this.YearlyTheSecondDayComboBox.IsEnabled = true;
            this.YearlyOfDayRadComboBox.IsEnabled = true;
        }

        private void YearlyEveryRadioButton_Click(object sender, RoutedEventArgs e)
        {
            this.YearlyEveryComboBox.IsEnabled = true;
            this.YearlyEveryNumeric.IsEnabled = true;

            this.YearlyTheDayComboBox.IsEnabled = false;
            this.YearlyTheSecondDayComboBox.IsEnabled = false;
            this.YearlyOfDayRadComboBox.IsEnabled = false;
        }

        private void NoEndDateRadioButton_Click(object sender, RoutedEventArgs e)
        {
            this.CountRecurrencesNumeric.IsEnabled = false;

            this.EndByDatePicker.IsEnabled = false;
        }

        private void EndAfterRadioButton_Click(object sender, RoutedEventArgs e)
        {
            this.CountRecurrencesNumeric.IsEnabled = true;

            this.EndByDatePicker.IsEnabled = false;
        }

        private void EndByRadioButton_Click(object sender, RoutedEventArgs e)
        {
            this.CountRecurrencesNumeric.IsEnabled = false;

            this.EndByDatePicker.IsEnabled = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close(false);
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.DailyRadioButton.IsChecked == true)
                GetDailyMetadata();

            else if (this.WeeklyButton.IsChecked == true)
                GetWeeklyMetadata();

            else if (this.MonthlyButton.IsChecked == true)
                GetMonthlyMetadata();

            else if (this.YearlyRadioButton.IsChecked == true)
                GetYearlyMetadata();

            this.appointment.Start = this.StartTimePicker.SelectedValue.Value;

            if (this.EndByRadioButton.IsChecked == true)
                this.appointment.End = this.EndByDatePicker.SelectedValue.Value;

            this.appointment.End = this.EndTimePicker.SelectedValue.Value;

            this.Close(true);
        }

        private void Close(bool result)
        {
            this.DialogResult = result;
            this.Close();
        }

        private void GetDailyMetadata()
        {
            if (this.DailyRadioButton.IsChecked == true)
            {
                if (this.DailyEveryRadioButton.IsChecked == true)
                {
                    this.recurrence.Interval =
                        Convert.ToInt32(this.DailyEveryNumeric.Value);

                    this.recurrence.Frequency =
                        RecurrenceFrequency.Daily.ToString();

                    this.recurrence.DaysOfWeekMask =
                        RecurrenceDays.EveryDay.ToString();

                    this.recurrence.FirstDayOfWeek =
                        DayOfWeek.Monday.ToString();

                    //set count recurrences
                    if (this.EndAfterRadioButton.IsChecked == true)
                        this.recurrence.MaxOccurrences =
                            Convert.ToInt32(this.CountRecurrencesNumeric.Value);

                    else if (this.EndByRadioButton.IsChecked == true)
                    {
                        this.recurrence.RecursUntil =
                            this.EndByDatePicker.SelectedValue.Value
                                .AddHours(this.EndTimePicker.SelectedValue.Value.Hour)
                                .AddMinutes(this.EndTimePicker.SelectedValue.Value.Minute);
                    }

                }
                else if (this.DailyEveryWeekdayRadioButton.IsChecked == true)
                {
                    this.recurrence.Frequency =
                        RecurrenceFrequency.Weekly.ToString();

                    this.recurrence.DaysOfWeekMask =
                        RecurrenceDays.WeekDays.ToString();

                    this.recurrence.FirstDayOfWeek =
                        DayOfWeek.Monday.ToString();

                    this.recurrence.Interval = 5;

                    //set count recurrences
                    if (this.EndAfterRadioButton.IsChecked == true)
                        this.recurrence.MaxOccurrences =
                            Convert.ToInt32(this.CountRecurrencesNumeric.Value);
                }
            }
        }

        private void GetWeeklyMetadata()
        {
            this.recurrence.DaysOfWeekMask = GetSelectedDays();

            this.recurrence.FirstDayOfWeek =
                        DayOfWeek.Monday.ToString();

            this.recurrence.Frequency =
                        RecurrenceFrequency.Weekly.ToString();

            this.recurrence.Interval = Convert.ToInt32(this.WeeklyNumeric.Value);

            //set count recurrences
            if (this.EndAfterRadioButton.IsChecked == true)
                this.recurrence.MaxOccurrences =
                    Convert.ToInt32(this.CountRecurrencesNumeric.Value);

            else if (this.EndByRadioButton.IsChecked == true)
            {
                this.recurrence.RecursUntil =
                    this.EndByDatePicker.SelectedValue.Value
                        .AddHours(this.EndTimePicker.SelectedValue.Value.Hour)
                        .AddMinutes(this.EndTimePicker.SelectedValue.Value.Minute);
            }
        }

        private string GetSelectedDays()
        {
            List<string> days = new List<string>();

            if (this.MondayCheckBox.IsChecked == true)
                days.Add(DayOfWeek.Monday.ToString());

            if (this.TuesdayCheckBox.IsChecked == true)
                days.Add(DayOfWeek.Tuesday.ToString());

            if (this.WednesdayCheckBox.IsChecked == true)
                days.Add(DayOfWeek.Wednesday.ToString());

            if (this.ThursdayCheckBox.IsChecked == true)
                days.Add(DayOfWeek.Thursday.ToString());

            if (this.FridayCheckBox.IsChecked == true)
                days.Add(DayOfWeek.Friday.ToString());

            if (this.SaturdayCheckBox.IsChecked == true)
                days.Add(DayOfWeek.Saturday.ToString());

            if (this.SundayCheckBox.IsChecked == true)
                days.Add(DayOfWeek.Sunday.ToString());

            return string.Join(",", days);
        }

        private void GetMonthlyMetadata()
        {
            if (this.MonthlydayRadioButton.IsChecked == true)
            {
                this.recurrence.DaysOfMonth = this.MonthlyDayNumeric.Value.ToString();
                this.recurrence.FirstDayOfWeek = DayOfWeek.Monday.ToString();
                this.recurrence.Frequency = RecurrenceFrequency.Monthly.ToString();
                this.recurrence.Interval = Convert.ToInt32(this.MonthlyOfEveryNumeric.Value);

                //set count recurrences
                if (this.EndAfterRadioButton.IsChecked == true)
                    this.recurrence.MaxOccurrences =
                        Convert.ToInt32(this.CountRecurrencesNumeric.Value);

                else if (this.EndByRadioButton.IsChecked == true)
                {
                    this.recurrence.RecursUntil =
                        this.EndByDatePicker.SelectedValue.Value
                            .AddHours(this.EndTimePicker.SelectedValue.Value.Hour)
                            .AddMinutes(this.EndTimePicker.SelectedValue.Value.Minute);
                }
            }
            else if(this.MonthlyTheRadioButton.IsChecked==true)
            {
                this.recurrence.DayOrdinal = this.MonthlyTheDayComboBox.SelectedIndex + 1;

                this.recurrence.DaysOfWeekMask =
                    GetSelectedDay(this.MonthlyTheSecondDayComboBox.SelectedIndex + 1);

                this.recurrence.FirstDayOfWeek = DayOfWeek.Monday.ToString();
                this.recurrence.Frequency = RecurrenceFrequency.Monthly.ToString();
                this.recurrence.Interval = Convert.ToInt32(this.MonthlyOfEveryNumeric.Value);

                //set count recurrences
                if (this.EndAfterRadioButton.IsChecked == true)
                    this.recurrence.MaxOccurrences =
                        Convert.ToInt32(this.CountRecurrencesNumeric.Value);

                else if (this.EndByRadioButton.IsChecked == true)
                {
                    this.recurrence.RecursUntil =
                        this.EndByDatePicker.SelectedValue.Value
                            .AddHours(this.EndTimePicker.SelectedValue.Value.Hour)
                            .AddMinutes(this.EndTimePicker.SelectedValue.Value.Minute);
                }
            }
        }

        private void GetYearlyMetadata()
        {//first point
            if (this.YearlyEveryRadioButton.IsChecked == true)
            {
                this.recurrence.Frequency =
                       RecurrenceFrequency.Yearly.ToString();

                this.recurrence.FirstDayOfWeek =
                    DayOfWeek.Monday.ToString();

                this.recurrence.DaysOfWeekMask =
                        RecurrenceDays.None.ToString();

                this.recurrence.MonthOfYear =
                    this.YearlyEveryComboBox.SelectedIndex + 1;

                this.recurrence.Interval = Convert.ToInt32(this.DailyEveryNumeric.Value);

                this.recurrence.DaysOfMonth = this.YearlyEveryNumeric.Value.ToString();

                this.recurrence.RecursUntil = GetTimeRecursUntil();
            }//last point
            else if (this.YearlyTheRadioButton.IsChecked == true)
            {
                this.recurrence.Interval = Convert.ToInt32(this.DailyEveryNumeric.Value);

                this.recurrence.MonthOfYear =
                    this.YearlyEveryComboBox.SelectedIndex + 1;

                this.recurrence.Frequency =
                       RecurrenceFrequency.Yearly.ToString();

                this.recurrence.FirstDayOfWeek =
                    DayOfWeek.Monday.ToString();

                this.recurrence.DaysOfWeekMask = 
                    GetSelectedDay(this.YearlyTheSecondDayComboBox.SelectedIndex + 1);

                this.recurrence.DayOrdinal = this.YearlyTheDayComboBox.SelectedIndex + 1;

                if (this.EndAfterRadioButton.IsChecked == true)
                    this.recurrence.MaxOccurrences =
                        Convert.ToInt32(this.CountRecurrencesNumeric.Value);

                this.recurrence.RecursUntil = GetTimeRecursUntil();
            }
        }

        private DateTime? GetTimeRecursUntil()
        {
            if (this.EndByRadioButton.IsChecked == true)
               return this.EndByDatePicker.SelectedValue.Value
                          .AddHours(this.EndTimePicker.SelectedValue.Value.Hour)
                          .AddMinutes(this.EndTimePicker.SelectedValue.Value.Minute);

            return null;
        }

        private string GetSelectedDay(int index)
        {
            switch (index)
            {
                case 1:
                    return "Monday";
                case 2:
                    return "Tuesday";
                case 3:
                    return "Wednesday";
                case 4:
                    return "Thursday";
                case 5:
                    return "Friday";
                case 6:
                    return "Saturday";
                case 7:
                    return "Sunday";
                case 8:
                    return "EveryDay";
                case 9:
                    return "WeekDays";
                case 10:
                    return "WeekendDays";
                default:
                    return null;
            }
        }

        private void DailyRadioButton_Click(object sender, RoutedEventArgs e)
        {
            this.DailyPanel.Visibility = Visibility.Visible;

            WeeklyVisibility();
            MonthlyVisibility();
            YearlyVisibility();
        }

        private void DailyEveryWeekdayRadioButton_Click(object sender, RoutedEventArgs e)
        {
            this.DailyEveryNumeric.IsEnabled = false;
        }

        private void YearlyRadioButton_Click(object sender, RoutedEventArgs e)
        {
            this.YearlyPanel.Visibility = Visibility.Visible;

            DailyVisibility();
            WeeklyVisibility();
            MonthlyVisibility();
        }
    }
}
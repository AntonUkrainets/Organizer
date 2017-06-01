using System;
using System.Collections.Generic;
using System.Globalization;
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
using System.Windows.Markup;
using Telerik.Windows.Controls.ScheduleView;
using Telerik.Windows.Controls;
using System.Collections.ObjectModel;
using Server;
using Server.Constants;
using BusinessOrganizer.Windows;
using BusinessOrganizer.Extensions;
using System.Windows.Threading;
using System.Diagnostics;

namespace BusinessOrganizer
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// таймер для отслеживания событий
        /// </summary>
        private DispatcherTimer dispatcherTimer;

        #region коллекция событий в расписании

        private ObservableCollection<Appointment> appointments;

        public ObservableCollection<Appointment> Appointments
        {
            get
            {
                if (this.appointments == null)
                    this.appointments = new ObservableCollection<Appointment>();

                return this.appointments;
            }
        }

        #endregion

        #region свойство для выбора типа дня в расписании

        public int ActiveViewDefinition
        {
            get { return (int)GetValue(ActiveViewDefinitionProperty); }
            set { SetValue(ActiveViewDefinitionProperty, value); }
        }

        //паттерн проектирования подписчик = издатель Observer
        public static readonly DependencyProperty ActiveViewDefinitionProperty =
            DependencyProperty.Register(
                "ActiveViewDefinition",
                typeof(int),
                typeof(MainWindow),
                new PropertyMetadata(0));

        #endregion

        // свойство для выбора слота в расписании
        public Slot selectedSlot { get; set; }

        public MainWindow()
        {
            this.InitializeComponent();

            //привязка данных к главному окну
            this.DataContext = this;

            //событие при загрузки окна
            this.Loaded += this.MainWindow_Loaded;

            //вид расписания
            this.ActiveViewDefinition = 0;

            //выбор слота в расписании
            this.scheduleView.AppointmentSelectionChanged += this.ScheduleView_AppointmentSelectionChanged;

            //выбор пустого слота с событием в расписании
            this.scheduleView.RequestBringIntoView += this.ScheduleView_RequestBringIntoView;

            //растягивание времени события
            this.scheduleView.AppointmentEdited += this.ScheduleView_AppointmentEdited;

            //получение событий из бд и добавление их коллекцию
            this.GetAppointmens();

            //наблюдение за началом событий
            this.InitWatcher();
        }

        /// <summary>
        /// наблюдение за началом событий
        /// </summary>
        private void InitWatcher()
        {
            //создание нового экземляра таймера
            this.dispatcherTimer = new DispatcherTimer();

            //событие на каждое срабатывание тика
            this.dispatcherTimer.Tick += this.DispatcherTimer_Tick;

            //интервал срабатывания таймера = 1 минута
            this.dispatcherTimer.Interval = TimeSpan.FromMinutes(1);

            //запуск таймера
            this.dispatcherTimer.Start();
        }

        /// <summary>
        /// событие при каждом тике
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            //округленное время с точностью до минуты
            var timeNow = DateTime.Now.RoundUp(TimeSpan.FromMinutes(1));

            //прохождение каждого события в коллекции событий
            foreach (var appointment in Appointments)
            {
                //получение округленного времени таймера с точностью до минуты
                var startTime = appointment.Start.Round(TimeSpan.FromMinutes(1));

                //сравнение текущего времени события с временем таймера
                if (startTime == timeNow)
                {
                    //если совпадает, то создание новый экземпляр окна напоминания
                    ReminderWindow window = new ReminderWindow(appointment);

                    //и открывает его
                    window.Show();
                }
            }
        }

        /// <summary>
        /// срабатываниет при растягивание события в слоте
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ScheduleView_AppointmentEdited(object sender, AppointmentEditedEventArgs e)
        {
            //получение выбранного события в расписании
            var currentAppointment = this.scheduleView.CurrentAppointment as Appointment;

            //конвертация выбранного события в событие доменной модели
            Server.DataModel.Appointment appointmentDomainModel =
                    currentAppointment.ToDomainModel();

            //изменение события доменной модели в базе данных
            OrganizerRepository.Change(appointmentDomainModel);
        }

        /// <summary>
        /// получение событий из базы данных
        /// </summary>
        private void GetAppointmens()
        {
            //получение коллекции событий доменной модели
            var appointmentsDomainModel = OrganizerRepository.GetAppointmens();

            //прохождение по каждому событию в коллекцию для добавления его в коллекцию событий
            foreach (var appoinmentDomainModel in appointmentsDomainModel)
            {
                //конвертация события доменной модели в событие расписания
                var appointment = appoinmentDomainModel.ToAppointment();

                //добавление в коллекцию события
                this.Appointments.Add(appointment);
            }
        }

        /// <summary>
        /// срабатывает при выборе пустого слота в расписании
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ScheduleView_RequestBringIntoView(object sender, RequestBringIntoViewEventArgs e)
        {
            //активация кнопки создания события
            this.NewAppointmentButton.IsEnabled = true;
        }

        /// <summary>
        /// срабатывает при выборе события в расписании
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ScheduleView_AppointmentSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //активация кнопки удаления события
            this.DeleteAppointmentButton.IsEnabled = true;

            //активация кнопок маркеров времени
            this.FreeTimeMarker.IsEnabled = true;
            this.BusyTimeMarker.IsEnabled = true;
            this.TentativeTimeMarker.IsEnabled = true;
            this.OutOfOfficeTimeMarker.IsEnabled = true;

            //активация кнопок категории
            this.YellowCategory.IsEnabled = true;
            this.GreenCategory.IsEnabled = true;
            this.PurpleCategory.IsEnabled = true;
            this.PinkCategory.IsEnabled = true;
        }

        /// <summary>
        /// ???
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //установка начального видимого времени в расписании
            this.scheduleView.FirstVisibleTime = TimeSpan.FromHours(8);
        }

        /// <summary>
        /// событие при двойном клике по выбранному слоту
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RadScheduleView_ShowDialog(object sender, ShowDialogEventArgs e)
        {
            //отмена использования стандартного окна редактирования
            e.Cancel = true;

            //создание события по выбранному времени слота в расписании
            AppointmentDialogViewModel appointmentDialogViewModel =
                e.DialogViewModel as AppointmentDialogViewModel;

            //если выбранный слот равен нулю
            if (appointmentDialogViewModel == null)
                return;//пропустить

            this.scheduleView.Cancel();

            //если выбранный слот необходимо добавить
            if (appointmentDialogViewModel.ViewMode == AppointmentViewMode.Add)
            {
                //создать новое событие
                this.CreateNewAppointment();
            }
            else//иначе
            {
                //редактировать выбранное событие
                this.EditCurrentAppointment();
            }
        }

        /// <summary>
        /// кнопка создание нового события
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NewAppointmentButton_Click(object sender, RoutedEventArgs e)
        {
            //запрос на выполнения метода создания нового события
            this.CreateNewAppointment();
        }

        /// <summary>
        /// создание нового события
        /// </summary>
        private void CreateNewAppointment()
        {
            //получить выбранный слот
            Slot slot = this.selectedSlot as Slot;

            //создание нового экземпляра начального времени начала события
            //и округление его с точностью до минуты
            DateTime startTime = DateTime.Now.RoundUp(TimeSpan.FromHours(1));

            //создание нового экземпляра конечного времени с увеличением его на 1 час
            DateTime endTime = startTime.AddHours(1);

            //если выбранный слот не равляется null
            if (slot != null)
            {
                //установка начального времени выбранного слота
                startTime = slot.Start;

                //установка конечного времени выбранного слота
                endTime = slot.End;
            }

            //создание нового события
            Appointment newAppointment = new Appointment()
            {
                //установка уникального id
                UniqueId = 0.ToString(),

                //установка начального времени события
                Start = startTime,

                //установка конечного времени события
                End = endTime
            };

            //создание нового экземпляра частоты повторений
            Server.DataModel.Recurrence recurrence = new Server.DataModel.Recurrence();

            //и присваивание ему Guid Id
            recurrence.Id = Guid.NewGuid();

            //создание нового события
            NewAppointmentWindow newAppointmentWindow = new NewAppointmentWindow(
                this.Appointments,
                newAppointment,
                recurrence);

            //если пользователь нажал ОК(создание нового события)
            if (newAppointmentWindow.ShowDialog() == true)
            {
                //добавить новое событие в коллекцию событий
                this.Appointments.Add(newAppointment);

                //конвертировать событие в доменную модель события для добавление в базу данных
                Server.DataModel.Appointment appointmentDomainModel =
                    newAppointment.ToDomainModel();

                //сохранение события и повторения в базу данных
                OrganizerRepository.Save(appointmentDomainModel, recurrence);

                //присваивание уникального id событию, события доменной модели
                newAppointment.UniqueId = appointmentDomainModel.Id.ToString();

                //присваивание повторению id события доменной модели
                recurrence.AppointmentId = appointmentDomainModel.Id;

                //сохранить все выбранные изменения в компонент расписания
                this.scheduleView.Commit();
            }
        }

        /// <summary>
        /// редактирование выбранного события
        /// </summary>
        private void EditCurrentAppointment()
        {
            //создание нового экземпляра выбранного события
            var currentAppointment = this.scheduleView.CurrentAppointment as Appointment;

            //получение id выбранного события
            int appointmentId = int.Parse(currentAppointment.UniqueId);

            //получение повторений для события по id
            Server.DataModel.Recurrence recurrence = OrganizerRepository.GetRecurrence(appointmentId);

            //начало редактирование текущего события в компоненте расписания
            this.scheduleView.BeginEdit(currentAppointment);

            //создание окна нового события
            NewAppointmentWindow newAppointmentWindow = new NewAppointmentWindow(
                this.Appointments,
                currentAppointment,
                recurrence);

            //если пользователь нажал ОК(редактирование события)
            if (newAppointmentWindow.ShowDialog() == true)
            {
                //сохранить все изменения, полученные из нового окна
                this.scheduleView.Commit();

                //конвертация текущего события в доменную модель
                Server.DataModel.Appointment appointmentDomainModel =
                    currentAppointment.ToDomainModel();

                //изменение события и его повторений в базе данных
                OrganizerRepository.Change(appointmentDomainModel, recurrence);
            }
            else//иначе
            {
                //отменить все изменения
                this.scheduleView.Cancel();
            }
        }

        /// <summary>
        /// выбор текущего дня в расписании и календаре
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TodayButton_Click(object sender, RoutedEventArgs e)
        {
            //установка текущего дня в расписании и календаре
            this.scheduleView.CurrentDate = DateTime.Now;
            this.calendar.SelectedDate = DateTime.Now;
            this.calendar.DisplayDate = DateTime.Now;
        }

        /// <summary>
        /// кнопка увеличения временного периода
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void IncreaseButton_Click(object sender, RoutedEventArgs e)
        {
            //метод увеличения временного периода
            this.IncreasePeriod();
        }

        /// <summary>
        /// кнопка уменьшения временного периода
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DecreaseButton_Click(object sender, RoutedEventArgs e)
        {
            //метод уменьшения временного периода
            this.DescreasePeriod();
        }

        /// <summary>
        /// кнопка следующего диапазона временного периода
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NextRangeButton_Click(object sender, RoutedEventArgs e)
        {
            //метод следующего диапазона временного периода
            this.IncreasePeriod();
        }

        /// <summary>
        /// увеличение временного периода
        /// </summary>
        private void IncreasePeriod()
        {
            //создание нового экземпляра времени отступа от выбранного времени в календаре
            TimeSpan shift = TimeSpan.MinValue;

            //переменная выбранного активного вида расписания
            switch (this.ActiveViewDefinition)
            {
                //если был выбран вид дня
                //увеличить отступ дней на 1
                case 0:
                    shift = TimeSpan.FromDays(1);
                    break;
                //если был выбран вид недели
                //увеличить отступ дней на 7
                case 1:
                    shift = TimeSpan.FromDays(7);
                    break;
                //если был выбран вид рабочей недели
                //увеличить отступ дней на 7
                case 2:
                    shift = TimeSpan.FromDays(5);
                    break;
                //если был выбран вид месяца
                //увеличить отступ дней на 30
                case 3:
                    shift = TimeSpan.FromDays(30);
                    break;
                //если был выбран вид временной шкалы
                //увеличить отступ дней на 7
                case 4:
                    shift = TimeSpan.FromDays(7);
                    break;
                default:
                    break;
            }

            //создание нового экземпляра выбранного времени с определенным кол-вом отступов
            var selectedDateTime = scheduleView.CurrentDate.Add(shift);

            //установка выбранной даты в расписании выбранным DateTime
            this.scheduleView.CurrentDate = selectedDateTime;

            //установка выбранной даты в календаре выбранным DateTime
            this.calendar.DisplayDate = selectedDateTime;
        }

        /// <summary>
        /// уменьшение временного периода
        /// </summary>
        private void DescreasePeriod()
        {
            //создание нового экземпляра времени отступа от выбранного времени в календаре
            TimeSpan shift = TimeSpan.MinValue;

            //переменная выбранного активного вида расписания
            switch (this.ActiveViewDefinition)
            {
                //если был выбран вид дня
                //уменьшить отступ дней на 1
                case 0:
                    shift = TimeSpan.FromDays(-1);
                    break;
                //если был выбран вид недели
                //уменьшить отступ дней на 7
                case 1:
                    shift = TimeSpan.FromDays(-7);
                    break;
                //если был выбран вид рабочей недели
                //уменьшить отступ дней на 7
                case 2:
                    shift = TimeSpan.FromDays(-5);
                    break;
                //если был выбран вид месяца
                //уменьшить отступ дней на 30
                case 3:
                    shift = TimeSpan.FromDays(-30);
                    break;
                //если был выбран вид временной шкалы
                //уменьшить отступ дней на 7
                case 4:
                    shift = TimeSpan.FromDays(-7);
                    break;
                default:
                    break;
            }

            //создание переменной для присваивания текущей даты + отступы
            var selectedDateTime = scheduleView.CurrentDate.Add(shift);

            //присваивание расписанию, полученной даты с переменной selectedDateTime
            this.scheduleView.CurrentDate = selectedDateTime;

            //присваивание календарю, полученной даты с переменной selectedDateTime
            this.calendar.DisplayDate = selectedDateTime;
        }

        /// <summary>
        /// вид дня
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DayViewButton_Click(object sender, RoutedEventArgs e)
        {
            //установка вида дня на 0
            this.ActiveViewDefinition = 0;

            //деактивация кнопок
            this.DayViewButton.IsEnabled = false;

            //активация кнопок
            this.WeekViewButton.IsEnabled = true;
            this.WorkViewButton.IsEnabled = true;
            this.MonthViewButton.IsEnabled = true;
            this.TimelineViewButton.IsEnabled = true;
        }

        private void WeekViewButton_Click(object sender, RoutedEventArgs e)
        {
            this.ActiveViewDefinition = 1;

            //button deactive
            this.WeekViewButton.IsEnabled = false;

            //button active
            this.DayViewButton.IsEnabled = true;
            this.WorkViewButton.IsEnabled = true;
            this.MonthViewButton.IsEnabled = true;
            this.TimelineViewButton.IsEnabled = true;
        }

        private void WorkViewButton_Click(object sender, RoutedEventArgs e)
        {
            this.ActiveViewDefinition = 2;

            //button deactive
            this.WorkViewButton.IsEnabled = false;

            //button active
            this.DayViewButton.IsEnabled = true;
            this.WeekViewButton.IsEnabled = true;
            this.MonthViewButton.IsEnabled = true;
            this.TimelineViewButton.IsEnabled = true;
        }

        private void MonthViewButton_Click(object sender, RoutedEventArgs e)
        {
            this.ActiveViewDefinition = 3;

            //button deactive
            this.MonthViewButton.IsEnabled = true;

            //button active
            this.DayViewButton.IsEnabled = true;
            this.WeekViewButton.IsEnabled = true;
            this.WorkViewButton.IsEnabled = true;
            this.TimelineViewButton.IsEnabled = true;
        }

        private void TimelineViewButton_Click(object sender, RoutedEventArgs e)
        {
            this.ActiveViewDefinition = 4;

            //button deactive
            this.TimelineViewButton.IsEnabled = true;

            //button active
            this.DayViewButton.IsEnabled = true;
            this.WeekViewButton.IsEnabled = true;
            this.WorkViewButton.IsEnabled = true;
            this.MonthViewButton.IsEnabled = true;
        }

        /// <summary>
        /// удаление выбранного события
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteAppointmentButton_Click(object sender, RoutedEventArgs e)
        {
            //создание нового экземпляра выбранного события
            Appointment appointment = this.scheduleView.CurrentAppointment as Appointment;

            //удаление выбранного события из коллекции событий
            this.Appointments.Remove(appointment);

            //преобразование переменной типа string в переменную типа int
            int appointmentId = int.Parse(appointment.UniqueId);

            //удаление события по id в базе данных
            OrganizerRepository.Delete(appointmentId);
        }

        /// <summary>
        /// изменение выбранного события
        /// </summary>
        /// <param name="currentAppointment">выбранное событие</param>
        private void ChangeAppointment(Appointment currentAppointment)
        {
            //конвертация выбранного события в доменную модель            
            Server.DataModel.Appointment appointmentDomainModel =
                   currentAppointment.ToDomainModel();

            //изменение события доменной модели в базе данных
            OrganizerRepository.Change(appointmentDomainModel);
        }

        /// <summary>
        /// получение цвета временного маркера события
        /// </summary>
        /// <param name="marker">тип маркера</param>
        /// <returns></returns>
        private string GetTimeMarker(int marker)
        {
            switch (marker)
            {
                case 1:
                    return Windows.Color.Free;
                case 2:
                    return Windows.Color.Tentative;
                case 3:
                    return Windows.Color.Busy;
                case 4:
                    return Windows.Color.OutOfOffice;
                default:
                    return null;
            }
        }

        /// <summary>
        /// установка временного маркера в расписании
        /// </summary>
        /// <param name="scheduleAppointment">выбранное событие</param>
        /// <param name="marker">имя маркера</param>
        private void SetTimeMarker(Appointment scheduleAppointment, string marker)
        {
            if (scheduleAppointment == null)
                return;

            scheduleAppointment.TimeMarker =
                this.scheduleView.TimeMarkersSource.Cast<TimeMarker>()
                    .FirstOrDefault(timeMarker => timeMarker.TimeMarkerName == marker);
        }

        /// <summary>
        /// кнопка установки выбранного события в Free time marker
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FreeTimeMarker_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            //если выбранный слот в расписании не равен null
            if (this.scheduleView.SelectedSlot == null)
            {
                //создание нового экземпляра выбранного события в расписании
                Appointment appointment = this.scheduleView.SelectedAppointment as Appointment;

                //получения цвета временного маркера
                string marker = this.GetTimeMarker(TimeMarkers.Free);

                //начало редактирование события в расписании
                this.scheduleView.BeginEdit(appointment);

                //установка выбранному событию временного маркера
                this.SetTimeMarker(appointment, marker);

                //сохранение всех изменений в расписании
                this.scheduleView.Commit();

                //занесения всех изменений по выбранному событию в базу данных
                this.ChangeAppointment(appointment);
            }
            else
            {   //если слот не был выбран
                System.Windows.Forms.MessageBox.Show("Please, select an appointment first.");
            }
        }

        /// <summary>
        /// кнопка установки выбранного события в Busy time marker
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BusyTimeMarker_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            //если выбранный слот в расписании не равен null
            if (this.scheduleView.SelectedSlot == null)
            {
                //создание нового экземпляра выбранного события в расписании
                Appointment appointment = this.scheduleView.SelectedAppointment as Appointment;

                //получения цвета временного маркера
                string marker = this.GetTimeMarker(TimeMarkers.Busy);

                //начало редактирование события в расписании
                this.scheduleView.BeginEdit(appointment);

                //установка выбранному событию временного маркера
                this.SetTimeMarker(appointment, marker);

                //сохранение всех изменений в расписании
                this.scheduleView.Commit();

                //занесения всех изменений по выбранному событию в базу данных
                this.ChangeAppointment(appointment);
            }
            else
            {   //если слот не был выбран
                System.Windows.Forms.MessageBox.Show("Please, select an appointment first.");
            }
        }

        /// <summary>
        /// кнопка установки выбранного события в Tentative time marker
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TentativeTimeMarker_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            //если выбранный слот в расписании не равен null
            if (this.scheduleView.SelectedSlot == null)
            {
                //создание нового экземпляра выбранного события в расписании
                Appointment appointment = this.scheduleView.SelectedAppointment as Appointment;

                //получения цвета временного маркера
                string marker = this.GetTimeMarker(TimeMarkers.Tentative);

                //начало редактирование события в расписании
                this.scheduleView.BeginEdit(appointment);

                //установка выбранному событию временного маркера
                this.SetTimeMarker(appointment, marker);

                //сохранение всех изменений в расписании
                this.scheduleView.Commit();

                //занесения всех изменений по выбранному событию в базу данных
                this.ChangeAppointment(appointment);
            }
            else
            {
                //если слот не был выбран
                System.Windows.Forms.MessageBox.Show("Please, select an appointment first.");
            }
        }

        /// <summary>
        /// кнопка установки выбранного события в Out Of Office time marker
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OutOfOfficeTimeMarker_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            //если выбранный слот в расписании не равен null
            if (this.scheduleView.SelectedSlot == null)
            {
                //создание нового экземпляра выбранного события в расписании
                Appointment appointment = this.scheduleView.SelectedAppointment as Appointment;

                //получения цвета временного маркера
                string marker = this.GetTimeMarker(TimeMarkers.OutOfOffice);

                //начало редактирование события в расписании
                this.scheduleView.BeginEdit(appointment);

                //установка выбранному событию временного маркера
                this.SetTimeMarker(appointment, marker);

                //сохранение всех изменений в расписании
                this.scheduleView.Commit();

                //занесения всех изменений по выбранному событию в базу данных
                this.ChangeAppointment(appointment);
            }
            else
            {   //если слот не был выбран
                System.Windows.Forms.MessageBox.Show("Please, select an appointment first.");
            }
        }

        /// <summary>
        /// получение цвета категории события
        /// </summary>
        /// <param name="categoryId">тип категории</param>
        /// <returns></returns>
        private string GetCategory(int categoryId)
        {
            switch (categoryId)
            {
                case 1:
                    return Windows.Color.Yellow;
                case 2:
                    return Windows.Color.Green;
                case 3:
                    return Windows.Color.Purple;
                case 4:
                    return Windows.Color.Pink;
                default:
                    throw new ArgumentException();//return null;
            }
        }

        /// <summary>
        /// установка категории событию
        /// </summary>
        /// <param name="scheduleAppointment">выбранное событие</param>
        /// <param name="category">тип категории</param>
        private void SetCategory(Appointment scheduleAppointment, int? category)
        {
            //если выбранное расписание не равно null
            if (scheduleAppointment == null)
                return;

            var categor = category.GetCategory();
            var categoryName = categor.CategoryName;

            scheduleAppointment.Category =
                this.scheduleView.CategoriesSource.Cast<Category>()
                    .FirstOrDefault(Category =>
                    Category.CategoryName == categoryName);

        }

        /// <summary>
        /// кнопка установки желтой категории выбранному событию
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void YellowCategory_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {   
            //если выбранный слот 
            if (this.scheduleView.SelectedSlot == null)
            {
                //создание нового экземпляра выбранного события в расписании
                Appointment appointment = this.scheduleView.SelectedAppointment as Appointment;

                //начало редактирование события в расписании
                this.scheduleView.BeginEdit(appointment);

                //получение типа категории
                int? category = Categories.Yellow;

                //установка типа категории выбранному событию
                this.SetCategory(appointment, category);

                //сохранение всех изменений в расписании
                this.scheduleView.Commit();

                //занесения всех изменений по выбранному событию в базу данных
                this.ChangeAppointment(appointment);
            }
            else
            {   //если слот не был выбран
                System.Windows.Forms.MessageBox.Show("Please, select an appointment first.");
            }
        }

        /// <summary>
        /// кнопка установки зеленой категории выбранному событию
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GreenCategory_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            if (this.scheduleView.SelectedSlot == null)
            {
                //создание нового экземпляра выбранного события в расписании
                Appointment appointment = this.scheduleView.SelectedAppointment as Appointment;

                //начало редактирование события в расписании
                this.scheduleView.BeginEdit(appointment);

                //получение типа категории
                int category = Categories.Green;

                //установка типа категории выбранному событию
                this.SetCategory(appointment, category);

                //сохранение всех изменений в расписании
                this.scheduleView.Commit();

                this.ChangeAppointment(appointment);
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Please, select an appointment first.");
            }
        }

        /// <summary>
        /// кнопка установки фиолетовой категории выбранному событию
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PurpleCategory_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            if (this.scheduleView.SelectedSlot == null)
            {
                //создание нового экземпляра выбранного события в расписании
                Appointment appointment = this.scheduleView.SelectedAppointment as Appointment;

                //получение типа категории
                int category = Categories.Purple;

                //получение типа категории
                this.scheduleView.BeginEdit(appointment);

                //установка типа категории выбранному событию
                this.SetCategory(appointment, category);

                //сохранение всех изменений в расписании
                this.scheduleView.Commit();

                this.ChangeAppointment(appointment);
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Please, select an appointment first.");
            }
        }

        /// <summary>
        /// кнопка установки розовой категории выбранному событию
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PinkCategory_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            if (this.scheduleView.SelectedSlot == null)
            {
                //создание нового экземпляра выбранного события в расписании
                Appointment appointment = this.scheduleView.SelectedAppointment as Appointment;

                //получение типа категории
                int category = Categories.Pink;

                //получение типа категории
                this.scheduleView.BeginEdit(appointment);

                //установка типа категории выбранному событию
                this.SetCategory(appointment, category);

                //сохранение всех изменений в расписании
                this.scheduleView.Commit();

                this.ChangeAppointment(appointment);
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Please, select an appointment first.");
            }
        }
    }
}
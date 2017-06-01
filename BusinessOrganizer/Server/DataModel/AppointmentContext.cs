namespace Server.DataModel
{
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class OrganizerContext : DbContext
    {
        // Контекст настроен для использования строки подключения "Appointments" из файла конфигурации  
        // приложения (App.config или Web.config). По умолчанию эта строка подключения указывает на базу данных 
        // "Server.DataModel.Appointments" в экземпляре LocalDb. 
        // 
        // Если требуется выбрать другую базу данных или поставщик базы данных, измените строку подключения "Appointments" 
        // в файле конфигурации приложения.
        public OrganizerContext()
            : base("name=Appointments")
        {
        }

        // Добавьте DbSet для каждого типа сущности, который требуется включить в модель. Дополнительные сведения 
        // о настройке и использовании модели Code First см. в статье http://go.microsoft.com/fwlink/?LinkId=390109.

        // public virtual DbSet<MyEntity> MyEntities { get; set; }

        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Priority> Priorities { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<TimeMarker> TimeMarkers { get; set; }
        public DbSet<Recurrence> Recurrences { get; set; }
    }
}
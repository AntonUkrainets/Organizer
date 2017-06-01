using Telerik.Windows.Controls.ScheduleView;

namespace BusinessOrganizer.Extensions
{
    /// <summary>
    /// класс расширения приоритета
    /// </summary>
    public static class PriorityExtensions
    {
        /// <summary>
        /// получение id выбранного приоритета
        /// </summary>
        /// <param name="priority">приоритет, по которому необходимо получить id</param>
        /// <returns></returns>
        public static int? GetPriorityId(this Importance priority)
        {
            //если полученный приоритет
            switch (priority)
            {
                //является высоким
                case Importance.High:
                    return 1;//вернуть 1

                //является низким
                case Importance.Low:
                    return 3;//вернуть 3
                default://иначе вернуть 2, является нормальным приоритетом
                    return 2;
            }
        }

        /// <summary>
        /// получение приоритета по выбранному id
        /// </summary>
        /// <param name="priorityId">id, по которому необходимо получить приоритет</param>
        /// <returns></returns>
        public static Importance GetImportance(this int? priorityId)
        {
            //если priorityId
            switch (priorityId)
            {
                //равен 1
                case 1:
                    return Importance.High;//вернуть высокий приоритет
                
                //равен 3
                case 3:
                    return Importance.Low;//вернуть низкий приоритет

                //иначе
                default:
                    return Importance.None;//вернуть нормальный приоритет(или никакой)
            }
        }
    }
}
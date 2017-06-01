using Server.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.Windows.Controls;

namespace BusinessOrganizer.Extensions
{
    /// <summary>
    /// класс расширения временных маркеров
    /// </summary>
    public static class TimeMarkerExtensions
    {
        /// <summary>
        /// получение из id маркера тип ITimeMarker
        /// </summary>
        /// <param name="timeMarkerId">id, по которому необходимо получить маркер</param>
        /// <returns></returns>
        public static ITimeMarker GetTimeMarker(this int? timeMarkerId)
        {
            //если id маркера
            switch (timeMarkerId)
            {
                //равно 1
                case 1:
                    return TimeMarker.Free;//вернуть свободный маркер
                
                //равно 2
                case 2:
                    return TimeMarker.Busy;//вернуть маркер занят
                
                //равно 3
                case 3:
                    return TimeMarker.Tentative;//вернуть неопределенный маркер

                //равно 4
                case 4:
                    return TimeMarker.OutOfOffice;//вернуть вне дома
                default://если маркер не был выбран
                    return null;//вернуть null
            }
        }

        /// <summary>
        /// получение из типа ITimeMarker, id маркера
        /// </summary>
        /// <param name="timeMarker">маркер, по которому необходимо получить id</param>
        /// <returns></returns>
        public static int? GetTimeMarkerId(this ITimeMarker timeMarker)
        {
            //если маркер не был создан
            if (timeMarker == null)
                return null;//вернуть null

            //если имя выбранного маркера совпадает с маркером Free
            if (timeMarker.TimeMarkerName == TimeMarker.Free.TimeMarkerName)
                return TimeMarkers.Free;//вернуть маркер Free

            //если же выбранного маркера совпадает с маркером Busy
            else if (timeMarker.TimeMarkerName == TimeMarker.Busy.TimeMarkerName)
                return TimeMarkers.Busy;//вернуть маркер Busy

            //если же выбранного маркера совпадает с маркером Tentative
            else if (timeMarker.TimeMarkerName == TimeMarker.Tentative.TimeMarkerName)
                return TimeMarkers.Tentative;//вернуть маркер Tentative

            //если же выбранного маркера совпадает с маркером OutOfOffice
            else if (timeMarker.TimeMarkerName == TimeMarker.OutOfOffice.TimeMarkerName)
                return TimeMarkers.OutOfOffice;//вернуть маркер OutOfOffice            

            //иначе вернуть null
            return null;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;

namespace UpdateToGraphVSIX
{
    /// <summary>
    /// struct storage exchange api name and graph url
    /// </summary>
    public class ApiMappingItem
    {
        public string GraphApiName { get; set; }
        public string GraphURL { get; set; }
        public string EWSApiName { get; set; }
        public string EWSMethodSignature { get; set; }
        public string Namespace
        {
            get; set;
        }
        public string ClassName
        {
            get
            {
                return EWSMethodSignature.Substring(0, EWSMethodSignature.LastIndexOf("."));
            }
        }
        public string MothedName
        {
            get
            {
                return EWSMethodSignature.Substring(EWSMethodSignature.LastIndexOf(".") + 1);
            }
        }
    }

    public class ExchangeMapToGraph
    {
        public static IEnumerable<ApiMappingItem> ExchangeMapToGraphList
        {
            get
            {
                ApiMappingItem[] result = new ApiMappingItem[]
                {
                    new ApiMappingItem{ Namespace="Microsoft.Exchange.WebServices.Data",EWSMethodSignature="Appointment.Accept", EWSApiName = @"Accept", GraphURL = @"https://developer.microsoft.com/en-us/graph/docs/api-reference/v1.0/api/event_accept"},
                    new ApiMappingItem{ Namespace="Microsoft.Exchange.WebServices.Data",EWSMethodSignature="Appointment.AcceptTentatively", EWSApiName = @"AcceptTentatively", GraphURL = @"https://developer.microsoft.com/en-us/graph/docs/api-reference/v1.0/api/event_tentativelyaccept"},
                    new ApiMappingItem{ Namespace="Microsoft.Exchange.WebServices.Data",EWSMethodSignature="Appointment.Decline", EWSApiName = @"Decline", GraphURL = @"https://developer.microsoft.com/en-us/graph/docs/api-reference/v1.0/api/event_decline"},

                    new ApiMappingItem{ Namespace="Microsoft.Exchange.WebServices.Data",EWSMethodSignature="Appointment.Bind", EWSApiName = @"Appointment.Bind", GraphURL = @"https://developer.microsoft.com/en-us/graph/docs/api-reference/v1.0/api/event_get"},
                    new ApiMappingItem{ Namespace="Microsoft.Exchange.WebServices.Data",EWSMethodSignature="Appointment.Save", EWSApiName = @"Save", GraphURL = @"https://developer.microsoft.com/en-us/graph/docs/api-reference/v1.0/api/user_post_events"},
                    new ApiMappingItem{ Namespace="Microsoft.Exchange.WebServices.Data",EWSMethodSignature="Appointment.Update", EWSApiName = @"Update", GraphURL = @"https://developer.microsoft.com/en-us/graph/docs/api-reference/v1.0/api/event_update"},
                    new ApiMappingItem{ Namespace="Microsoft.Exchange.WebServices.Data",EWSMethodSignature="CalendarFolder.FindAppointments", EWSApiName = @"FindAppointments", GraphURL = @"https://developer.microsoft.com/en-us/graph/docs/api-reference/v1.0/api/user_list_events"},

                    new ApiMappingItem{ Namespace="Microsoft.Exchange.WebServices.Data",EWSMethodSignature="CalendarFolder.Bind", EWSApiName = @"CalendarFolder.Bind", GraphURL = @"https://developer.microsoft.com/en-us/graph/docs/api-reference/v1.0/api/calendar_get"},
                    new ApiMappingItem{ Namespace="Microsoft.Exchange.WebServices.Data",EWSMethodSignature="EmailMessage.Bind", EWSApiName = @"EmailMessage.Bind", GraphURL = @"https://developer.microsoft.com/en-us/graph/docs/api-reference/v1.0/api/message_get"},
                    new ApiMappingItem{ Namespace="Microsoft.Exchange.WebServices.Data",EWSMethodSignature="EmailMessage.Send", EWSApiName = @"Send", GraphURL = @"https://developer.microsoft.com/en-us/graph/docs/api-reference/v1.0/api/user_sendmail"},

                    new ApiMappingItem{ Namespace="Microsoft.Exchange.WebServices.Data",EWSMethodSignature="ExchangeService.DeleteItems", EWSApiName = @"DeleteItems", GraphURL = @"https://developer.microsoft.com/en-us/graph/docs/api-reference/v1.0/api/message_delete"},
                    //new ApiMappingItem{ Namespace="Microsoft.Exchange.WebServices.Data",EWSMethodSignature="ExchangeService.DeleteItems", EWSApiName = @"DeleteItems", GraphURL = @"https://developer.microsoft.com/en-us/graph/docs/api-reference/v1.0/api/event_delete"},

                    new ApiMappingItem{ Namespace="Microsoft.Exchange.WebServices.Data",EWSMethodSignature="ExchangeService.FindFolders", EWSApiName = @"FindFolders", GraphURL = @"https://developer.microsoft.com/en-us/graph/docs/api-reference/v1.0/api/user_list_calendars"},
                    new ApiMappingItem{ Namespace="Microsoft.Exchange.WebServices.Data",EWSMethodSignature="ExchangeService.FindItems", EWSApiName = @"FindItems", GraphURL = @"https://developer.microsoft.com/en-us/graph/docs/api-reference/v1.0/api/user_list_mailfolders"},
                    new ApiMappingItem{ Namespace="Microsoft.Exchange.WebServices.Data",EWSMethodSignature="ExchangeService.GetPeopleInsights", EWSApiName = @"GetPeopleInsights", GraphURL = @"https://developer.microsoft.com/en-us/graph/docs/api-reference/v1.0/api/user_get"},

                    new ApiMappingItem{ Namespace="Microsoft.Exchange.WebServices.Data",EWSMethodSignature="Folder.Bind", EWSApiName = @"Folder.Bind", GraphURL = @"https://developer.microsoft.com/en-us/graph/docs/api-reference/v1.0/api/mailfolder_get"},
                    new ApiMappingItem{ Namespace="Microsoft.Exchange.WebServices.Data",EWSMethodSignature="Folder.Delete", EWSApiName = @"Delete", GraphURL = @"https://developer.microsoft.com/en-us/graph/docs/api-reference/v1.0/api/mailfolder_delete"},
                    new ApiMappingItem{ Namespace="Microsoft.Exchange.WebServices.Data",EWSMethodSignature="Folder.FindFolders", EWSApiName = @"Folder", GraphURL = @"https://developer.microsoft.com/en-us/graph/docs/api-reference/v1.0/resources/mailfolder" },
                    new ApiMappingItem{ Namespace="Microsoft.Exchange.WebServices.Data",EWSMethodSignature="Folder.Save", EWSApiName = @"Save", GraphURL = @"https://developer.microsoft.com/en-us/graph/docs/api-reference/v1.0/api/user_post_mailfolders"},

                    new ApiMappingItem{ Namespace="Microsoft.Exchange.WebServices.Data",EWSMethodSignature="Folder.Update", EWSApiName = @"Update", GraphURL = @"https://developer.microsoft.com/en-us/graph/docs/api-reference/v1.0/api/mailfolder_update"},

                    new ApiMappingItem{ Namespace="Microsoft.Exchange.WebServices.Data",EWSMethodSignature="Item.Copy", EWSApiName = @"Copy", GraphURL = @"https://developer.microsoft.com/en-us/graph/docs/api-reference/v1.0/api/message_copy"},
                    new ApiMappingItem{ Namespace="Microsoft.Exchange.WebServices.Data",EWSMethodSignature="Item.Move", EWSApiName = @"Move", GraphURL = @"https://developer.microsoft.com/en-us/graph/docs/api-reference/v1.0/api/message_move"},
                    new ApiMappingItem{Namespace="Microsoft.Exchange.WebServices.Data", EWSMethodSignature="Item.Update", EWSApiName = @"Update", GraphURL = @"https://developer.microsoft.com/en-us/graph/docs/api-reference/v1.0/api/message_update"},

                    // new ApiMappingItem{Namespace="System.Net", EWSMethodSignature="NetworkCredential.NetworkCredential", EWSApiName = @"NetworkCredential", GraphURL = @"https://developer.microsoft.com/en-us/graph/docs/concepts/auth_overview"},
                

                };
                return result;
            }
        }

        private static IEnumerable<string> EWSApiNameList { get; } =
            ExchangeMapToGraphList.Select(m => m.EWSApiName).Distinct();
        public static IEnumerable<string> GetEWSApiNameList()
        {
            return EWSApiNameList;
        }

        public static ApiMappingItem MatchAccordinglyGraph(string EWSMethodSignature)
        {
            if (string.IsNullOrWhiteSpace(EWSMethodSignature))
            {
                return null;
            }

            ApiMappingItem result;

            try
            {
                result = ExchangeMapToGraphList.FirstOrDefault(x => x.EWSMethodSignature.Contains(EWSMethodSignature));
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }

    }
}

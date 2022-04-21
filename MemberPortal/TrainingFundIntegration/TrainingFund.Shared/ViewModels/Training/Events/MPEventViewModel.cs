using System;
using System.Collections.Generic;

namespace TrainingFund.Shared.ViewModels.Training.Events
{
    public class MPEventViewModel
    {
        public string Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public MPGenericLinkButtonViewModel Name { get; set; }
        public string NameSide { get; set; }
        public string SubName { get; set; }
        public bool isExclamationIcon { get; set; }
        public bool isFlairNameSide { get; set; }
        public bool isRecurringEvent { get; set; }
        public string FooterText { get; set; }
        public List<MPGenericLinkButtonViewModel> FooterLinks { get; set; }
        public string Image { get; set; }
        public int V3PersonId { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string ContactEmail { get; set; }
        public string ContactName { get; set; }
        public string ContactPhone { get; set; }
        public string Country { get; set; }
        public string County { get; set; }
        public string LocationName { get; set; }
        public string RoomName { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}

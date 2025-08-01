using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthScapeMAUI.Models
{
    public class Customer
    {
        public long Id { get; set; }
        public string? Title { get; set; }
        public string? Slug { get; set; }
        public string? Abbrv { get; set; }
        public string? Description { get; set; }
        public string? PaymentGatewayCustomerId { get; set; }
        public string? Logo { get; set; }
        public string? Address1 { get; set; }
        public string? Address2 { get; set; }
        public string? State { get; set; }
        public string? City { get; set; }
        public string? PostalCode { get; set; }
        public string? WebsiteUri { get; set; }
        public VendorType VendorType { get; set; }
        public long? LocationCount { get; set; }
        public MembershipType MembershipType { get; set; }
        public bool CanAdjustSellPrice { get; set; }
        public DateTime Created { get; set; }
    }

    public enum VendorType
    {
        [Description("Customer")]
        [Display(Name = "Buying, Selling, and/or Tracking Products")]
        Customer = 0,

        [Description("Furniture Dealer")]
        [Display(Name = "Furniture Dealer")]
        FurnitureDealer = 1,

        [Description("Architect")]
        [Display(Name = "Architect")]
        Architect = 2,

        [Description("Interior Designer")]
        [Display(Name = "Interior Designer")]
        InteriorDesign = 3,

        [Description("Electrician")]
        [Display(Name = "Electrician")]
        Electrician = 4,

        [Description("Moving Vendor")]
        [Display(Name = "Moving Vendor")]
        MovingVendor = 5,

        [Description("Donation")]
        [Display(Name = "Donation")]
        Donation = 6,

        [Description("Reupholster")]
        [Display(Name = "Reupholster")]
        Reupholster = 7,

        [Description("Manufacturer")]
        [Display(Name = "Manufacturer")]
        Manufacturer = 8,



        All = 99,
    }

    public enum MembershipType
    {
        Standard = 0,
        Enterprise = 1,
        EnterpriseAudit = 2
    }
}

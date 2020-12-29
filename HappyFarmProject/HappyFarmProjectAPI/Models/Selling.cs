//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace HappyFarmProjectAPI.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Selling
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Selling()
        {
            this.CustomerFeedbacks = new HashSet<CustomerFeedback>();
            this.SellingActivities = new HashSet<SellingActivity>();
            this.SellingDetails = new HashSet<SellingDetail>();
        }
    
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int SellingStatusId { get; set; }
        public int CustomerAddressId { get; set; }
        public int PromoId { get; set; }
        public string RecipientName { get; set; }
        public string RecipientPhone { get; set; }
        public byte[] PaymentImage { get; set; }
        public decimal ShippingCharges { get; set; }
        public decimal TotalSalePrice { get; set; }
        public System.DateTime DateTime { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CustomerFeedback> CustomerFeedbacks { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SellingActivity> SellingActivities { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SellingDetail> SellingDetails { get; set; }
    }
}
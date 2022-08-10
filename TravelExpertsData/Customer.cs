using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace TravelExpertsData
{
    [Index(nameof(AgentId), Name = "EmployeesCustomers")]
    public partial class Customer
    {
        public Customer()
        {
            Bookings = new HashSet<Booking>();
            CreditCards = new HashSet<CreditCard>();
            CustomersRewards = new HashSet<CustomersReward>();
        }

        [Key]
        public int CustomerId { get; set; }
        
        [Required(ErrorMessage = "Please enter your first name.")]
        [StringLength(25)]
        public string CustFirstName { get; set; }

        [Required(ErrorMessage = "Please enter your last name.")]
        [StringLength(25)]
        public string CustLastName { get; set; }

        [Required(ErrorMessage = "Please enter your address.")]
        [StringLength(75)]
        public string CustAddress { get; set; }

        [Required(ErrorMessage = "Please enter your city.")]
        [StringLength(50)]
        public string CustCity { get; set; }

        [Required(ErrorMessage = "Please enter your province.")]
        [StringLength(2)]
        public string CustProv { get; set; }
        
        [Required(ErrorMessage = "Please enter your postal code.")]
        [RegularExpression("^([A-Z][0-9][A-Z]) ([0-9][A-Z][0-9])$", ErrorMessage = "Please enter postal code in format X1X 1X1.")]
        [StringLength(7)]
        public string CustPostal { get; set; }

        [Required(ErrorMessage = "Please enter your country.")]
        [StringLength(25)]
        public string CustCountry { get; set; }

        [Required(ErrorMessage = "Please enter your home phone.")]
        [RegularExpression("^[0-9]{10}$", ErrorMessage = "Please enter 10 numeric digits without space.")]
        [StringLength(20)]
        public string CustHomePhone { get; set; }

        //[Required]
        [RegularExpression("^[0-9]{10}$", ErrorMessage = "Please enter 10 numeric digits without space.")]
        [StringLength(20)]
        public string CustBusPhone { get; set; }

        //[Required]
        [StringLength(50)]
        public string CustEmail { get; set; }

        public int? AgentId { get; set; }

        [Required(ErrorMessage = "Please enter a user ID.")]
        [StringLength(25)]
        public string UserId { get; set; }

        [Required(ErrorMessage = "Please enter a password.")]
        [Compare("ConfirmPassword")]
        [StringLength(25)]
        public string Password { get; set; }

        [Display(Name = "Confirm Password")]
        [Required(ErrorMessage = "Please confirm your password.")]
        [NotMapped]
        public string ConfirmPassword { get; set; }

        [ForeignKey(nameof(AgentId))]
        [InverseProperty("Customers")]
        public virtual Agent Agent { get; set; }
        [InverseProperty(nameof(Booking.Customer))]
        public virtual ICollection<Booking> Bookings { get; set; }
        [InverseProperty(nameof(CreditCard.Customer))]
        public virtual ICollection<CreditCard> CreditCards { get; set; }
        [InverseProperty(nameof(CustomersReward.Customer))]
        public virtual ICollection<CustomersReward> CustomersRewards { get; set; }
    }
}

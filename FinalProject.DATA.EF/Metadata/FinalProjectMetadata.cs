using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.DATA.EF
{
    public class LocationMetaData
    {
        [Required]
        [Display(Name = "Daycare Location Name")]
        [StringLength(50, ErrorMessage = "*Location Name: Max 50 Characters")]
        public string LocationName { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "*Address: Max 100 Characters")]
        public string Address { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "*City: Max 100 Characters")]
        public string City { get; set; }
        [Required]
        [StringLength(2, ErrorMessage = "*State: Max 2 Characters")]
        public string State { get; set; }
        [Required]
        [Display(Name = "Zip Code")]
        [StringLength(5, ErrorMessage = "*Zip Code: Max 5 Characters")]
        public string ZipCode { get; set; }
        [Required]
        [Display(Name = "Reservation Limit")]
        [StringLength(255, ErrorMessage = "*Reservation Limit: Max 255 Characters")]
        public byte ReservationLimit { get; set; }
    }

    [MetadataType(typeof(LocationMetaData))]
    public partial class Location
    {
        [Display(Name = "Location Name + (Reservation Limit)")]
        public string NameLimit
        {
            get { return LocationName + " + (" + ReservationLimit + ")"; }
        }
    }


    public class PetMetaData
    {
        [Required]
        [Display(Name = "Pet Name")]
        [StringLength(50, ErrorMessage = "*Pet Name: Max 50 Characters")]
        public string AssetName { get; set; }
        [Required]
        [Display(Name = "Owner Name")]
        public string OwnerId { get; set; }
        [Display(Name = "Pet Photo")]
        public string AssetPhoto { get; set; }
        [Display(Name = "Special Pet Notes")]
        [UIHint("MultilineText")]
        public string SpecialNotes { get; set; }
        [Display(Name = "Current Pet?")]
        public bool IsActive { get; set; }
        [Required]
        [Display(Name = "Date Added")]
        [DisplayFormat(DataFormatString = "{0:d}, ApplyFormatInEditMode=true")]
        public System.DateTime DateAdded { get; set; }
    }

    [MetadataType(typeof(PetMetaData))]
    public partial class Pet
    {

    }


    public partial class ReservationMetaData
    {
        [Display(Name = "Pet Name")]
        public int PetId { get; set; }
        [Display(Name = "Location Name")]
        public int LocationId { get; set; }
        [Required]
        [Display(Name = "Reservation Date")]
        [DisplayFormat(DataFormatString = "{0:d}, ApplyFormatInEditMode=true")]
        public System.DateTime ReservationDate { get; set; }
        [Display(Name = "Special Requests")]
        [UIHint("MultilineText")]
        public string SpecialRequests { get; set; }
    }

    [MetadataType(typeof(ReservationMetaData))]
    public partial class Reservation
    {

    }


    public partial class UserDetailMetadata
    {
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Display(Name = "Phone Number")]
        public string Phone { get; set; }
    }

    [MetadataType(typeof(UserDetailMetadata))]
    public partial class UserDetail
    {
        public string FullName
        {
            get { return FirstName + " " + LastName; }
        }
    }

}

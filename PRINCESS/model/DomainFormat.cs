using System.ComponentModel.DataAnnotations;

/**
 *  authors:
 *          @Anna Mann
 *          @Olivia Höft
 *          
 **/

namespace PRINCESS.model
{
    public class DomainFormat
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Review { get; set; }

        [Required]
        public string Writer { get; set; }

        [Required]
        public string Rating { get; set; }

        [Required]
        public string Menu { get; set; }

        [Required]
        public string URL { get; set; }

        [Required]
        public int Priority { get; set; }

        [Required]
        public string Country { get; set; }

        public bool removeChars { get; set; }

        public bool LazyContainer { get; set; }

        public string LcValue { get; set; }

        public bool McValue { get; set; }

        public bool NextButton { get; set; }

        public string NbValue { get; set; }

        public bool IgnoreTitleKey { get; set; }

        public string ItkValue { get; set; }

        public bool Paywall { get; set; }

        public string Login { get; set; }

        public string Password { get; set; }
    }
}

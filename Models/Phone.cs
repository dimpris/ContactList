﻿namespace ContactList.Models
{
    public class Phone
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public PhoneType Type { get; set; }
        public Contact Contact { get; set; }
    }
}

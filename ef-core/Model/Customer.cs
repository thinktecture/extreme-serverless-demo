﻿using System;
using System.Collections.Generic;

namespace Serverless.Model
{
    public partial class Customer
    {
        public Customer()
        {
            CustomerAddres = new HashSet<CustomerAddress>();
            SalesOrderHeaders = new HashSet<SalesOrderHeader>();
        }

        public int CustomerId { get; set; }
        public bool NameStyle { get; set; }
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Suffix { get; set; }
        public string CompanyName { get; set; }
        public string SalesPerson { get; set; }
        public string EmailAddress { get; set; }
        public string Phone { get; set; }
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }
        public Guid Rowguid { get; set; }
        public DateTime ModifiedDate { get; set; }

        public ICollection<CustomerAddress> CustomerAddres { get; set; }
        public ICollection<SalesOrderHeader> SalesOrderHeaders { get; set; }
    }
}
